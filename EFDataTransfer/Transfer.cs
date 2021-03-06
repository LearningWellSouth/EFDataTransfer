﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EFDataTransfer
{
    public class Transfer
    {
        public const string POSTAL_CODE_PLACEMENT_TYPE = "TypeOfPlacement";
        private readonly DataAccess _dataAccess;
        private readonly string _dbCurrentDb;
        private readonly Logger _logger;

        public Transfer(Logger logger)
        {
            _logger = logger;

            #if DEBUG
                _dbCurrentDb = "putsa_db";
                _dataAccess = new DataAccess(@"Server=WIN-HIA2DJPDOTQ\SQLEXPRESS;Integrated Security=true;Initial Catalog=" + _dbCurrentDb);
                _dataAccess.ValidateConnectionSettings();
            #else
                _dataAccess = new DataAccess("Data Source=server01.eriksfonsterputs.net;Initial Catalog=master;User ID=sa;Password=VNbNAQHbK8TDdeMuDXdv");
                //_dbCurrentDb = "eriks_dev_db";
                _dbCurrentDb = "putsa_db";
            #endif

                SqlStrings.dbToUse = _dbCurrentDb;
        }

        class PostalCodeArea
        {
            public int From,To;
            public string PostalCode;
            public int Id;

            public PostalCodeArea(Address addr, int id)
            {
                Id = id;
                PostalCode = addr.PostalNumber;
            }
        }

        public void Addresses()
        {
            var twAddresses = _dataAccess.SelectIntoTable(SqlStrings.SelectTWAddresses);
            var postalCodeModels = _dataAccess.SelectIntoTable(SqlStrings.SelectAllPostalCodeModels);
            var clients = _dataAccess.SelectIntoTable("SELECT id, mother_id FROM eriks_migration.dbo.TW_clients");
            var cache = new Dictionary<string,PostalCodeArea>();

            var cleaningObjects = new DataTable();
            cleaningObjects.Columns.AddRange(new DataColumn[] {
                new DataColumn("Id", typeof(int)),
                new DataColumn("PostalAddressModelId", typeof(int)),
                new DataColumn("IsActive", typeof(bool)),
                new DataColumn("RouteIndex", typeof(int)),
                new DataColumn("Invoicable", typeof(bool)),
                new DataColumn("IsNew", typeof(bool))
            });
            var coMappings = MakeSimpleFieldMapping(new[]
                {"Id", "PostalAddressModelId", "IsActive", "RouteIndex", "Invoicable", "IsNew"});

            var personPostalAddressModel = new DataTable();
            personPostalAddressModel.Columns.AddRange(new DataColumn[] {
                new DataColumn("PersonId", typeof(int)),
                new DataColumn("PostalAddressModelId", typeof(int)),
                new DataColumn("Type", typeof(int))
            });
            var pMappings = MakeSimpleFieldMapping(new[]{"PersonId","PostalAddressModelId","Type"});

            var recordNumber = 0;
            var parser = new AddressParser(_logger);
            foreach (DataRow clientRecord in twAddresses.Rows)
            {
                recordNumber++;
                if (recordNumber%1000 == 0)
                    Console.WriteLine(recordNumber + " of " + twAddresses.Rows.Count + " rows finished...");

                var parsedAddress = parser.ParseAddress(clientRecord["address"], clientRecord["postalcode"],clientRecord["city"]);

                //"postalcode_fixed" is used by other "fixers" in late migration
                if(parsedAddress.GetPostalCodeAsInteger() > 0)
                    _dataAccess.NonQuery(SqlStrings.PostalCodeFixUpdate(Convert.ToInt32(clientRecord["id"]), parsedAddress.GetPostalCodeAsInteger())); ;
                if(Convert.ToString(clientRecord["deleted"]) == "Y") continue;

                int postalCodeModelId = 0;
                if (cache.ContainsKey(parsedAddress.GetKeyString()))
                {
                    var area = cache[parsedAddress.GetKeyString()];
                    postalCodeModelId = area.Id;
                }
                else
                {
                    var possiblePostalCodeModels = postalCodeModels.Select(
                        string.Format("City = '{0}' AND PostalAddress ='{1}'", parsedAddress.City,parsedAddress.StreetName));

                    if (possiblePostalCodeModels.Any())
                    {
                        var postalCode = possiblePostalCodeModels.FirstOrDefault(
                            x =>
                                isStreetNumberWithinMaxAndMin(x, parsedAddress.StreetNumber)
                                && hasCorrectPostalNumberType(x, parsedAddress));

                        if (postalCode != null)
                            postalCodeModelId = Convert.ToInt32(postalCode["Id"]);
                    }
                }
                if (postalCodeModelId == 0)
                {
                    postalCodeModelId = InsertNewPostalCodeModelRow(parsedAddress);
                    cache.Add(parsedAddress.GetKeyString(), new PostalCodeArea(parsedAddress,postalCodeModelId));
                }

                var postalAddressModels = fetchAddressEntriesForPostalCodeAndStreetNumber(postalCodeModelId, parsedAddress);

                int postalAddressModelId;

                if (postalAddressModels.Rows.Count > 0)
                {
                    postalAddressModelId = Convert.ToInt32(postalAddressModels.Rows[0]["Id"]);
                }
                else
                {
                    var address2 = Convert.IsDBNull(clientRecord["co_address"]) ? "" : Convert.ToString(clientRecord["co_address"]);
                    var longitude = extractFloatOrDefault(clientRecord, "longitude");
                    var latitude = extractFloatOrDefault(clientRecord, "latitude");

                    postalAddressModelId = InsertWithKeyReturn(SqlStrings.InsertPostalAddressModel(parsedAddress.StreetNumberFull, postalCodeModelId, "AT", address2, longitude, latitude));
                }

                bool isDelivery = (Convert.ToString(clientRecord["is_delivery"]) == "Y") || (Convert.ToInt32(clientRecord["route_num"]) > 0 && Convert.ToInt32(clientRecord["workarea_id"]) > 0);
                bool isInvoice = Convert.ToString(clientRecord["is_invoice"]) == "Y";

                foreach (var person in clients.Select("id = " + clientRecord["client_id"]))
                {
                    if (isDelivery)
                        personPostalAddressModel.Rows.Add(person["id"], postalAddressModelId, 1);

                    if (isInvoice)
                    {
                        personPostalAddressModel.Rows.Add(person["id"], postalAddressModelId, 2);
                        // Inte helt säker på detta
                        // TODO : now thats an interesting comment! :) "i'm not sure..."
                        personPostalAddressModel.Rows.Add(person["id"], postalAddressModelId, 4);
                    }
                }

                if (isDelivery) // TODO : modify this to bind to customers. Now the customers are created before any way so we may do this. See the script "AfterCreatingCleaningObjects"
                    cleaningObjects.Rows.Add(clientRecord["id"], postalAddressModelId, true, clientRecord["route_num"], true, false);
                }

            _dataAccess.InsertMany(string.Format("{0}.dbo.CleaningObjects", _dbCurrentDb), cleaningObjects, true, coMappings);
            _dataAccess.InsertMany(string.Format("{0}.dbo.PersonPostalAddressModels", _dbCurrentDb), personPostalAddressModel, false, pMappings);
        }

        private static SqlBulkCopyColumnMapping[] MakeSimpleFieldMapping(IEnumerable<string> fields)
                    {
            return fields.Select(field => new SqlBulkCopyColumnMapping(field, field)).ToArray();
        }

        public static bool hasCorrectPostalNumberType(DataRow dataRow, Address parsedAddress)
                        {
            const string EVEN = "NJ";
            const string ODD = "NU";

            var type = Convert.ToString(dataRow[POSTAL_CODE_PLACEMENT_TYPE]);
            if (!(type == EVEN || type == ODD)) return true;
            return (parsedAddress.isEvenStreetNumber() ^ type == ODD);
                            }

        private int InsertWithKeyReturn(string statement)
                            {
                                    try
                                    {
              return _dataAccess.InsertSingle(statement);
                                    }
            catch (Exception exc)
                                    {
              _logger.PostError("Exception while inserting "+RecurseOverExceptionMessages(exc)+exc.StackTrace);
              throw;
                                    }
                                }

        private string RecurseOverExceptionMessages(Exception exc)
                            {
            if (exc == null) return "";
            return exc.Message + RecurseOverExceptionMessages(exc.InnerException);
                            }

        private static float extractFloatOrDefault(DataRow row, string columnName, float def = 0.0F)
        {
            return !Convert.IsDBNull(row[columnName]) ? (float) Convert.ToDouble(row[columnName]) : def;
                        }

        private DataTable fetchAddressEntriesForPostalCodeAndStreetNumber(int postalCodeModelId, Address parsedAddress)
                {
            return _dataAccess.SelectIntoTable(string.Format(
              "SELECT Id FROM " + _dbCurrentDb + ".dbo.PostalAddressModels WHERE PostalCodeModelId = '{0}' AND StreetNo = '{1}'", postalCodeModelId, parsedAddress.StreetNumberFull));
                }

        private int InsertNewPostalCodeModelRow(Address addr)
                {
            _logger.PostInfo(string.Format("Creating new postal code {0} {1} {2}",addr.StreetName,addr.City,addr.PostalNumber));
            return InsertWithKeyReturn(SqlStrings.InsertIntoPostalCodeModels(addr.PostalNumber, "AT", addr.StreetName, 1, addr.StreetNumber, addr.City, addr.isEvenStreetNumber() ? "NJ" : "NU"));
                }

        public static bool isStreetNumberWithinMaxAndMin(DataRow x, int streetNumber)
        {
            if (streetNumber <= 0) return true;
            return ExtractBeginingAsIntegerValue(x["StreetNoLowest"]) <= streetNumber && ExtractBeginingAsIntegerValue(x["StreetNoHighest"]) >= streetNumber;
                }

        private static int ExtractBeginingAsIntegerValue(object data)
                {
            return AddressParser.ExtractBeginingOfStringAsInteger(Convert.ToString(data));
                    }

        /* Main routine for doing the actual transfer */
        public void TransferData(string tableName)
        {
            switch (tableName.ToUpper())
            {
                case "PERSONS":
                    _dataAccess.NonQuery(SqlStrings.TransferClients);
                    break;
                case "SETTINGS":
                    Settings();
                    break;
                case "POSTALADDRESSMODELS":
                    Addresses();
                    break;
                case "CONTACTS":
                    _dataAccess.NonQuery(SqlStrings.TransferContacts);
                    Console.WriteLine("Updating Contacts with RUT...");
                    SetRUT();
                    break;
                case "CUSTOMERS":
                    _dataAccess.NonQuery(SqlStrings.TransferCustomers);
                    Console.WriteLine("Connecting Customers to CleaningObjects...");
                    ConnectCustomersToCleaningObjects();
                    break;
                case "BANKS":
                    _dataAccess.NonQuery(SqlStrings.TransferBanks);
                    Console.WriteLine("Connecting Banks to customers...");
                    _dataAccess.NonQuery(SqlStrings.ConnectBanksToCustomers);
                    break;
                case "SCHEDULES":
                    SchedulesAndPeriods();
                    break;
                case "WORKERS":
                    Console.WriteLine("Creating Workers...");
                    _dataAccess.NonQuery(SqlStrings.TransferWorkers);
                    break;
                case "VEHICLES":
                    _dataAccess.NonQuery(SqlStrings.TransferVehicles);
                    Console.WriteLine("Updating Vehicles with hardcoded values...");
                    _dataAccess.NonQuery(SqlStrings.UpdateVehicles);
                    break;
                case "TEAMS":
                    _dataAccess.NonQuery(SqlStrings.CreateTeamsAndConnectToVehicles);
                    Console.WriteLine("Connect Workers with Team...");
                    _dataAccess.NonQuery(SqlStrings.ConnectWorkersToTeams);
                    Console.WriteLine("Connecting teams to cleaning objects...");
                    ConnectTeamsToCleaningObjects();
                // Samtliga putsobjekt som inte får teamId av ovanstående har i TW-tabellen workarea_id = 0, vilket jag antar betyder att de inte är kopplade
                    break;
                case "ACCOUNTS":
                    _dataAccess.NonQuery(SqlStrings.InsertAccounts);
                    break;
                case "SUBCATEGORIES":
                    _dataAccess.NonQuery(SqlStrings.InsertSubCategories);
                    break;
                case "SERVICES":
                    _dataAccess.NonQuery(SqlStrings.TransferServices);
                    break;
                case "SUBSCRIPTIONS":
                    Subscriptions();
                    break;
                case "SUBSCRIPTIONSERVICES": //SubscriptionServices
                    SubscriptionServices();
                    break;
                case "CLEANINGOBJECTPRICES":
                    SubscriptionPrices();
                    _dataAccess.NonQuery(string.Format("UPDATE {0}.dbo.CleaningObjectPrices SET ServiceGroupId = (SELECT Max(Id) FROM {0}.dbo.ServiceGroups)", _dbCurrentDb));
                    break;
                case "ISSUES":

                    NotesAndIssues();
                    break;
                case "USERS":
                    Console.WriteLine("Creating team users...");
                    CreateTeamUsers();
                    break;
                default:
                    throw new ArgumentException("No migration for "+tableName);
            }
        }

        public void AddSchedulesToCleaningObjectsWithout()
        {
            Console.WriteLine("Adding schedules to those addresses that lacks them...");
            _dataAccess.NonQuery(SqlStrings.UpdateAllPostalCodeScheduleIds);
        }
 
        private void ConnectCustomersToCleaningObjects()
        {
            
            _dataAccess.NonQuery(SqlStrings.ConnectCustomersToCleaningObjects);
            _dataAccess.NonQuery(SqlStrings.InsertContactsWhereNeeded);
        }

        public void ConnectTeamsToCleaningObjects()
        {
            var twWorkAreas = _dataAccess.SelectIntoTable(SqlStrings.SelectTWWorkAreas);
            var schedules = _dataAccess.SelectIntoTable("SELECT Id, Name FROM " + _dbCurrentDb + ".dbo.Schedules");
            int checkInt = 0;

            foreach (DataRow row in twWorkAreas.Rows)
            {
                int teamId = Convert.ToInt32(_dataAccess.SelectIntoTable(
                    string.Format("SELECT t.Id AS Id FROM " + _dbCurrentDb + ".dbo.Teams t JOIN " + _dbCurrentDb + ".dbo.Vehicles v ON v.Id = t.VehicleId WHERE v.Notes = '{0}'", row["name"])).Rows[0]["Id"]);
                _dataAccess.NonQuery(string.Format("UPDATE " + _dbCurrentDb + ".dbo.CleaningObjects SET TeamId = {0} WHERE Id = {1}", teamId, row["Id"])); //idRow["Id"]));

                    checkInt++;
                    if (checkInt % 1000 == 0)
                        Console.WriteLine(checkInt + " of " + twWorkAreas.Rows.Count + " rows finished...");

            }
        }

        public void SetPostalCodeScheduleIds()
        {
            Console.WriteLine("Fixing schedulesIds on PostalCodeModels");
            _dataAccess.NonQuery(SqlStrings.SetPostalCodeScheduleIds);
            }

        public void CreateTeamUsers()
        {
            _dataAccess.NonQuery(string.Format(@"INSERT INTO {0}.dbo.Users (Username, [Permissions], TeamId) SELECT CONCAT(REPLACE(Name, ' ', ''), '@eriksfonsterputs.se'), 128, Id FROM {0}.dbo.Teams", _dbCurrentDb));
        }

        private void SetRUT()
        {
            _dataAccess.NonQuery(SqlStrings.SetRUT);
            _dataAccess.NonQuery("UPDATE " + _dbCurrentDb + ".dbo.Contacts SET RUT = 0 WHERE PersonId IN (SELECT Id FROM " + _dbCurrentDb + ".dbo.Persons WHERE NoPersonalNoValidation = 1)");
            _dataAccess.NonQuery(string.Format("UPDATE {0}.dbo.Contacts SET RUT = 0 WHERE PersonId IN (SELECT Id FROM {0}.dbo.Persons WHERE PersonType = 2)", _dbCurrentDb));
        }

        public void CreateUsersForEmployees()
        {
            _dataAccess.NonQuery(string.Format(@"
                    SET IDENTITY_INSERT {0}.dbo.Users ON
                    INSERT INTO {0}.dbo.Users (Id, Username, Permissions)
                    SELECT id, CONCAT(firstname, ' ', lastname), CASE WHEN role_id = 1 THEN 96 ELSE 63 END
                    FROM eriks_migration.dbo.TW_employees
                    WHERE deleted = 'N'
                    SET IDENTITY_INSERT {0}.dbo.Users OFF", _dbCurrentDb));
        }
  
        private void SchedulesAndPeriods()
        {
            var interludes = _dataAccess.SelectIntoTable(SqlStrings.SelectTWInterludes);

            var periods = new DataTable();
            periods.Columns.AddRange(new DataColumn[] {
                new DataColumn("Id", typeof(int)), 
                new DataColumn("ScheduleId", typeof(int)), 
                new DataColumn("WeekFrom", typeof(int)), 
                new DataColumn("WeekTo", typeof(int)),
                new DataColumn("ValidFrom", typeof(DateTime)),
                new DataColumn("ValidForYear", typeof(int))
            });

            var mapping = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("Id", "Id"),
                new SqlBulkCopyColumnMapping("ScheduleId", "ScheduleId"),
                new SqlBulkCopyColumnMapping("WeekFrom", "WeekFrom"),
                new SqlBulkCopyColumnMapping("WeekTo", "WeekTo"),
                new SqlBulkCopyColumnMapping("ValidFrom", "ValidFrom"),
                new SqlBulkCopyColumnMapping("ValidForYear", "ValidForYear")
            };

            int num = 0;
            int scheduleId = 0;

            foreach (DataRow interlude in interludes.Rows)
            {
                int thisNum = Convert.ToInt32(interlude["num"]);

                if (num != thisNum)
                {
                    num = thisNum;
                    scheduleId = _dataAccess.InsertSingle(SqlStrings.InsertIntoSchedules(num));
                }
                var test = Convert.ToDateTime(interlude["startdate"]);
                periods.Rows.Add(new object[] { 
                    interlude["id"], scheduleId, interlude["weekFrom"], Convert.ToInt32(interlude["weekFrom"]) + 1, Convert.ToDateTime(interlude["startdate"]), interlude["period"] });
            }

            _dataAccess.InsertMany("" + _dbCurrentDb + ".dbo.Periods", periods, true, mapping);
        }

        private void Settings()
        {
            var settings = new DataTable();
            settings.Columns.AddRange(new DataColumn[] {
                new DataColumn("Key", typeof(string)),
                new DataColumn("Value", typeof(string))
            });

            var mapping = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("Key", "Key"),
                new SqlBulkCopyColumnMapping("Value", "Value")
            };

            settings.Rows.Add(new object[] { "SMSURL", "https://api.beepsend.com/2/sms/" });
            settings.Rows.Add(new object[] { "SMSToken", "4027747559efb58a31aeb06606371e4f1e8b08c47a9fa5b15fcfd1aac099f858" });
            settings.Rows.Add(new object[] { "SMSMessage", "Fönsterputsningen för {0} {1} i {2} är färdigt.\nMed vänlig hälsning\nEriks fönsterputs" });
            settings.Rows.Add(new object[] { "SMSDefaultCountryCode", "46" });
            settings.Rows.Add(new object[] { "SMSDebugPhoneNumber", "+46739105500" /*+46702307254"*/ });
            settings.Rows.Add(new object[] { "MOMS", "0.25" });
            settings.Rows.Add(new object[] { "ReminderText", "Påminnelse\nRad1\nRad2" });
            settings.Rows.Add(new object[] { "ReminderTaxReductionText", "Text om att skattereduktion avvisas\nRad1\nRad2\nRad3" });
            settings.Rows.Add(new object[] { "ReminderDebtCollectionText", "Inkassotext\nRad1\nRad2" });
            settings.Rows.Add(new object[] { "SMTPUsername", "noreply@eriksfonsterputs.se" });
            settings.Rows.Add(new object[] { "SMTPPassword", "svarainte" });
            settings.Rows.Add(new object[] { "SMTPHost", "smtp.gmail.com" });
            settings.Rows.Add(new object[] { "SMTPPortTLS", "465" });
            settings.Rows.Add(new object[] { "SMTPPortSSL", "587" });
            settings.Rows.Add(new object[] { "ConfirmationMailSubject", "Fönsterputs" });
            settings.Rows.Add(new object[] { "ConfirmationMailContent", "Bästa kund!<br/><br/><br/><br/>Vi har idag putsat fönster på {0}. Vill du kontakta oss gällande putsningen, vänligen ring kundtjänst på 0771-424242 och ange refnr {1}.<br /><br />Miljövän? Tänk på att du kan anmäla dig för e-faktura hos din bank! <br /><br />Med vänliga hälsningar, {2}<br />Eriks Fönsterputs" });
            settings.Rows.Add(new object[] { "DebugMailAddress", "msp@learningwell.se" });
            settings.Rows.Add(new object[] { "ProductionMode", "false" });
            settings.Rows.Add(new object[] { "InvoiceMailSubject", "Faktura" });
            settings.Rows.Add(new object[] { "InvoiceMailContent", "Bästa kund!<br/><br/><br/><br/>I medföljande bilaga finner du en faktura från Eriks Fönsterputs.<br /><br /> Vill du kontakta oss gällande frågor kring fakturan, vänligen ring kundtjänst på 0771-424242.<br /><br />Med vänliga hälsningar<br />Eriks Fönsterputs" });
            settings.Rows.Add(new object[] { "SubscriptionMailSubject", "Bekräftelse" });
            settings.Rows.Add(new object[] { "SubscriptionMailMessage", "Meddelande 1" });
            settings.Rows.Add(new object[] { "SubscriptionMailMessage2", "Meddelande 2" });
            settings.Rows.Add(new object[] { "SubscriptionMailMessage3", "Meddelande 3" });
            settings.Rows.Add(new object[] { "SubscriptionMailMessage4", "Meddelande 4" });
            settings.Rows.Add(new object[] { "SubscriptionMailMessage5", "Meddelande 5" });

            _dataAccess.InsertMany("" + _dbCurrentDb + ".dbo.Settings", settings, false, mapping);
        }

        private void Subscriptions()
        {
            var twServices = _dataAccess.SelectIntoTable(SqlStrings.SelectTWServices);
            int serviceGroupId = _dataAccess.InsertSingle(SqlStrings.InsertIntoServiceGroups);

            var prices = new DataTable();
            prices.Columns.AddRange(new DataColumn[] {
                new DataColumn("ServiceGroupId", typeof(int), serviceGroupId.ToString()),
                new DataColumn("ServiceId", typeof(int)),
                new DataColumn("Cost", typeof(float))
            });

            twServices.AsEnumerable().CopyToDataTable(prices, LoadOption.Upsert); 

            var mapping = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("Cost", "Cost"),
                new SqlBulkCopyColumnMapping("ServiceId", "ServiceId"),
                new SqlBulkCopyColumnMapping("ServiceGroupId", "ServiceGroupId")
            };

            _dataAccess.InsertMany("" + _dbCurrentDb + ".dbo.Prices", prices, false, mapping);

            var twWorkOrders = _dataAccess.SelectIntoTable(SqlStrings.SelectTWWorkOrders);

            var subscriptions = new DataTable();
            subscriptions.Columns.AddRange(new DataColumn[] {
                new DataColumn("Id", typeof(int)),
                new DataColumn("CleaningObjectId", typeof(int)),
                new DataColumn("IsInactive", typeof(bool)),
                new DataColumn("ActiveChangedDate", typeof(DateTime))
            });

            
            foreach (DataRow row in twWorkOrders.Rows)
            {
                subscriptions.Rows.Add(new object[] { row["woId"], row["caId"], row["woIsInactive"], DateTime.Now });
            }

            _dataAccess.InsertMany("" + _dbCurrentDb + ".dbo.Subscriptions", subscriptions, true, null);
            _dataAccess.NonQuery(SqlStrings.SetSubscriptionsInActive);
        }

        private void SubscriptionPrices()
        {
            var twWorkOrderPrices = _dataAccess.SelectIntoTable(SqlStrings.SelectTWWorkOrderPrices);

            var coPrices = new DataTable();
            coPrices.Columns.AddRange(new DataColumn[] { 
                new DataColumn("CleaningObjectId", typeof(int)), 
                new DataColumn("ServiceId", typeof(int)), 
                new DataColumn("Modification", typeof(float)),
                new DataColumn("FreeText", typeof(string))
            });

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("CleaningObjectId", "CleaningObjectId"),
                new SqlBulkCopyColumnMapping("ServiceId", "ServiceId"),
                new SqlBulkCopyColumnMapping("Modification", "Modification"),
                new SqlBulkCopyColumnMapping("FreeText", "FreeText")
            };

            foreach(DataRow row in twWorkOrderPrices.Rows)
            {
                if (Convert.ToInt32(row["caId"]) == 15335)
                    Console.WriteLine(" - ");

                int coId = Convert.ToInt32(row["caId"]);
                decimal woPrice = Convert.ToDecimal(row["unit_price"]);
                decimal sPrice = Convert.ToDecimal(row["price_per_unit"]);
                float mod = 0.0f;
                if (woPrice != 0 && sPrice != 0)
                    mod = (float)Math.Round((decimal)(woPrice / sPrice), 4);
                else if (sPrice == 0 && woPrice != 0)
                {
                    mod = (float)woPrice;

                    if (mod > 0)
                        mod = (float)Math.Round(mod * 1.25f, 4);
                }

                coPrices.Rows.Add(new object[] { coId, Convert.ToInt32(row["ServiceId"]), mod, Convert.ToString(row["wolDesc"]) });
            }

            _dataAccess.InsertMany("" + _dbCurrentDb + ".dbo.CleaningObjectPrices", coPrices, false, mappings);
        }

        private void SubscriptionServices()
        {
            var twWorkOrderLines = _dataAccess.SelectIntoTable(SqlStrings.SelectTWWorkOrderLines);
            var subscriptionServices = new DataTable();
            subscriptionServices.Columns.AddRange(new DataColumn[] {
                new DataColumn("IsPermanent", typeof(int)),
                new DataColumn("SubscriptionId", typeof(int)),
                new DataColumn("ServiceId", typeof(int)),
                new DataColumn("PeriodNo", typeof(int))
            });

            foreach (DataRow row in twWorkOrderLines.Rows)
            {
                string interludeOccasion = Convert.ToString(row["interlude_occasion"]);

                if (!string.IsNullOrEmpty(interludeOccasion))
                {
                    string interludeStr = interludeOccasion.Trim('}').Split('{')[1];

                    string[] interludeArr = interludeStr.Split(';');
                    bool[] periods = new bool[7];

                    for (int i = 0; i < interludeArr.Length; i++)
                    {
                        string[] occasionArr = interludeArr[i].Split(':');

                        if (occasionArr[0] != "i")
                        {
                            string occasion = occasionArr[occasionArr.Length - 1].Trim('"');

                            if (occasion != "o9999" && !string.IsNullOrEmpty(occasion))
                            {
                                if (occasion == "o10000")
                                {
                                    for (int j = 1; j < 8; j++)
                                        subscriptionServices.Rows.Add(new object[] { 1, row["workorder_id"], row["sId"], j });

                                    break;
                                }
                                else
                                {
                                    int periodNo = int.Parse(occasion[occasion.Length - 1].ToString());
                                    if (periods[periodNo - 1]) _logger.PostError("duplicate schedule occation= " + occasion + ", workorder_id= " + row["workorder_id"] + ", sid=" + row["sId"]);
                                    //else
                                    periods[periodNo - 1] = true;
                                }
                            }
                                }
                            }

                    if (periods.Contains(true))
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            subscriptionServices.Rows.Add(new object[] { periods[j] ? 1:0, row["workorder_id"], row["sId"], j + 1 });
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < 8; j++)
                        subscriptionServices.Rows.Add(new object[] { 0, row["workorder_id"], row["sId"], j });
                }
            }

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("IsPermanent", "IsPermanent"),
                new SqlBulkCopyColumnMapping("SubscriptionId", "SubscriptionId"),
                new SqlBulkCopyColumnMapping("ServiceId", "ServiceId"),
                new SqlBulkCopyColumnMapping("PeriodNo", "PeriodNo")
            };

            _dataAccess.InsertMany("" + _dbCurrentDb + ".dbo.SubscriptionServices", subscriptionServices, false, mappings);
        }

        public void SetBasePriceAndInactive()
        {
            _dataAccess.NonQuery(SqlStrings.SetEmptySubscriptionsInactive);
    
            var cleaningObjectPrices = new DataTable();
            cleaningObjectPrices.Columns.AddRange(new DataColumn[]
        {
                new DataColumn("Modification", typeof(float)),
                new DataColumn("CleaningObjectId", typeof(int)),
                new DataColumn("ServiceId", typeof(int)),
                new DataColumn("ServiceGroupId", typeof(int))
            });

            var periods = _dataAccess.SelectIntoTable(SqlStrings.SelectAllPeriodsForEmptySubs(DateTime.Now.Year));

            var subscriptionServices = new DataTable();
            subscriptionServices.Columns.AddRange(new DataColumn[] {
                new DataColumn("IsPermanent", typeof(int)),
                new DataColumn("SubscriptionId", typeof(int)),
                new DataColumn("ServiceId", typeof(int)),
                new DataColumn("PeriodNo", typeof(int))
            });

            int periodNo = 0;
            int subscriptionId = 0;

            int serviceGroupId = Convert.ToInt32(_dataAccess.SelectIntoTable(string.Format("SELECT Max(Id) AS Id FROM {0}.dbo.ServiceGroups", _dbCurrentDb)).Rows[0]["Id"]);
            
            foreach (DataRow row in periods.Rows)
            {
                int rowSubscriptionId = Convert.ToInt32(row["SubscriptionId"]);

                if (subscriptionId != rowSubscriptionId)
                {
                    cleaningObjectPrices.Rows.Add(new object[] { 1.0, row["CleaningObjectId"], 1, serviceGroupId });

                    periodNo = 1;
                    subscriptionId = rowSubscriptionId;
                }

                subscriptionServices.Rows.Add(new object[] { 1, rowSubscriptionId, 28, periodNo });

                periodNo++;
                }

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("IsPermanent", "IsPermanent"),
                new SqlBulkCopyColumnMapping("SubscriptionId", "SubscriptionId"),
                new SqlBulkCopyColumnMapping("ServiceId", "ServiceId"),
                new SqlBulkCopyColumnMapping("PeriodNo", "PeriodNo")
            };

            _dataAccess.InsertMany(_dbCurrentDb + ".dbo.SubscriptionServices", subscriptionServices, false, mappings);

            mappings = new SqlBulkCopyColumnMapping[]
                {
                new SqlBulkCopyColumnMapping("Modification", "Modification"),
                new SqlBulkCopyColumnMapping("CleaningObjectId", "CleaningObjectId"),
                new SqlBulkCopyColumnMapping("ServiceId", "ServiceId"),
                new SqlBulkCopyColumnMapping("ServiceGroupId", "ServiceGroupId")
            };

            _dataAccess.InsertMany(_dbCurrentDb + ".dbo.CleaningObjectPrices", cleaningObjectPrices, false, mappings);
                }

        // Nytt 2015-11-13
        public void SetAdminFees()
        {
            var periods = _dataAccess.SelectIntoTable(SqlStrings.SelectAllPeriods(DateTime.Now.Year));

            var subscriptionServices = new DataTable();
            subscriptionServices.Columns.AddRange(new DataColumn[] {
                new DataColumn("IsPermanent", typeof(int)),
                new DataColumn("SubscriptionId", typeof(int)),
                new DataColumn("ServiceId", typeof(int)),
                new DataColumn("PeriodNo", typeof(int))
            });
            
            int periodNo = 1;
            int subscriptionId = Convert.ToInt32(periods.Rows[0]["SubscriptionId"]);

            foreach (DataRow row in periods.Rows)
            {
                int rowSubscriptionId = Convert.ToInt32(row["SubscriptionId"]);

                if (subscriptionId != rowSubscriptionId)
                {
                    periodNo = 1;
                    subscriptionId = rowSubscriptionId;
            }
                
                subscriptionServices.Rows.Add(new object[] { 1, rowSubscriptionId, 28, periodNo });

                periodNo++;
        }

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("IsPermanent", "IsPermanent"),
                new SqlBulkCopyColumnMapping("SubscriptionId", "SubscriptionId"),
                new SqlBulkCopyColumnMapping("ServiceId", "ServiceId"),
                new SqlBulkCopyColumnMapping("PeriodNo", "PeriodNo")
            };

            _dataAccess.InsertMany(_dbCurrentDb + ".dbo.SubscriptionServices", subscriptionServices, false, mappings);
 
            _dataAccess.NonQuery(SqlStrings.InsertAdminFeePriceMods);
        }

        public void UtilityTables()
        {
            string sqlTemp;

            sqlTemp = string.Format(@"SET IDENTITY_INSERT {0}.dbo.TableLocks ON 
                INSERT INTO {0}.dbo.TableLocks (Id, [Table], IsLocked) VALUES (1, 'NextInvoiceNumber', 0);
                INSERT INTO {0}.dbo.TableLocks (Id, [Table], IsLocked) VALUES (2, 'WorkOrderGenerator', 0);
                SET IDENTITY_INSERT {0}.dbo.TableLocks OFF", _dbCurrentDb);

            if (_dataAccess.SelectIntoTable("SELECT Id FROM " + _dbCurrentDb + ".dbo.TableLocks").Rows.Count == 0)
                _dataAccess.NonQuery(@sqlTemp);
            else
                _dataAccess.NonQuery(string.Format(@"UPDATE {0}.dbo.TableLocks SET IsLocked = 0", _dbCurrentDb));

            sqlTemp = "SET IDENTITY_INSERT " + _dbCurrentDb + ".dbo.NextInvoiceNumbers ON INSERT INTO " + _dbCurrentDb + 
                ".dbo.NextInvoiceNumbers (Id, NextAvailableInvoiceNumber) VALUES (1, 1000000) SET IDENTITY_INSERT " + _dbCurrentDb + ".dbo.NextInvoiceNumbers OFF";

            if (_dataAccess.SelectIntoTable("SELECT Id FROM " + _dbCurrentDb + ".dbo.NextInvoiceNumbers").Rows.Count == 0)
                _dataAccess.NonQuery(@sqlTemp);
            else _dataAccess.NonQuery(string.Format("UPDATE {0}.dbo.NextInvoiceNumbers SET NextAvailableInvoiceNumber = 1000000", _dbCurrentDb));
        }

        public void NotesAndIssues()
        {
            _dataAccess.NonQuery(SqlStrings.TransferCustomerNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectOrderNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCustomerEconomyNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectEconomyNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectFieldNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCustomerFieldNotes);
            _dataAccess.NonQuery(SqlStrings.TransferReclamationNotes);
            _dataAccess.NonQuery(SqlStrings.TransferDamageReports);
            _dataAccess.NonQuery(SqlStrings.TransferTerminationNotes);

            _dataAccess.NonQuery(SqlStrings.TransferInfoBeforeCleaning);
            _dataAccess.NonQuery(SqlStrings.TransferHeaderAndContentInfoBeforeCleaning);

            _dataAccess.NonQuery(SqlStrings.TransferInfoDuringCleaning);
            _dataAccess.NonQuery(SqlStrings.TransferHeaderAndContentInfoDuringCleaning);

            Console.WriteLine("Transferring and merging infoBeforeCleaning...");

            TransferCleaningObjectInfo(_dataAccess.SelectIntoTable(SqlStrings.SelectDuplicateTwNoteTableIds), "InfoBeforeCleaning", false);
            TransferCleaningObjectInfo(_dataAccess.SelectIntoTable(SqlStrings.SelectHeaderAndContentIdsFromDuplicateTWNotes), "InfoBeforeCleaning", true);

            Console.WriteLine("Transferring and merging infoDuringCleaning...");

            TransferCleaningObjectInfo(_dataAccess.SelectIntoTable(SqlStrings.SelectDuplicateTwNoteIdsForDuring), "InfoDuringCleaning", false);
            TransferCleaningObjectInfo(_dataAccess.SelectIntoTable(SqlStrings.SelectHeaderAndContentIdsFromDuplicateTWNotesForDuring), "InfoDuringCleaning", true);

            Console.WriteLine("Transferring Issues");
            var twIssues = _dataAccess.SelectIntoTable(SqlStrings.SelectTwIssues);
            var issues = new DataTable();
            issues.Columns.AddRange(new DataColumn[] {
                new DataColumn("Title", typeof(string)),
                new DataColumn("Description", typeof(string)),
                new DataColumn("Status", typeof(int)), 
                new DataColumn("Priority", typeof(int)),
                new DataColumn("StartDate", typeof(DateTime)),
                new DataColumn("FinishedDate", typeof(DateTime)),
                new DataColumn("IssueType", typeof(int)),
                new DataColumn("Private", typeof(bool)),
                new DataColumn("CustomerId", typeof(int))
            });

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("Title", "Title"),
                new SqlBulkCopyColumnMapping("Description", "Description"),
                new SqlBulkCopyColumnMapping("Status", "Status"),
                new SqlBulkCopyColumnMapping("Priority", "Priority"),
                new SqlBulkCopyColumnMapping("StartDate", "StartDate"),
                new SqlBulkCopyColumnMapping("FinishedDate", "FinishedDate"),
                new SqlBulkCopyColumnMapping("IssueType", "IssueType"),
                new SqlBulkCopyColumnMapping("Private", "Private"),
                new SqlBulkCopyColumnMapping("CustomerId", "CustomerId")
            };

            //foreach (DataRow row in twIssues.Rows)
            //{
            //    issues.Rows.Add(new object[] {
            //        row["Title"], Regex.Replace(Convert.ToString(row["Description"]), @"<[^>]+>|&nbsp;", "").Trim(), row["Status"], row["Priority"], row["StartDate"], row["FinishedDate"],
            //        row["IssueType"], row["Private"], row["CustomerId"]
            //    });
            //}

            _dataAccess.InsertMany(_dbCurrentDb + ".dbo.Issues", issues, false, mappings);
        }

        private void TransferCleaningObjectInfo(DataTable noteTableIds, string infoField, bool includeHeader)
        {
            foreach (DataRow noteRow in noteTableIds.Rows)
            {
                int noteTableId = Convert.ToInt32(noteRow["table_id"]);
                var contentTable = _dataAccess.SelectIntoTable(SqlStrings.SelectTwNoteAndCleaningObjectId(noteTableId, infoField == "InfoBeforeCleaning"));

                int coId = 0;
                string content = string.Empty;

                foreach (DataRow contentRow in contentTable.Rows)
                {
                    var bla = contentRow["header"];
                    bla = contentRow["content"];

                    if (includeHeader)
                        content +=
                            Convert.ToString(contentRow["header"]) + " - " +
                            Convert.ToString(contentRow["content"]) + Environment.NewLine;
                    else
                        content += Convert.ToString(contentRow["content"]) + Environment.NewLine;

                    coId = Convert.ToInt32(contentRow["coId"]);
                }

                _dataAccess.NonQuery(string.Format("UPDATE {0}.dbo.CleaningObjects SET {1} = '{2}' WHERE Id = {3}", _dbCurrentDb, infoField, content.Replace("'", "''"), coId));
            }
        }
  
        internal void MergeSubscriptionsOld()
        {
            var subscriptions = _dataAccess.SelectIntoTable("SELECT * FROM " + _dbCurrentDb + ".dbo.Subscriptions");

            int subIdToChange = 0;
            int checkInt = 0;

            foreach(DataRow row in subscriptions.Rows)
            {
                int subIdToSet = Convert.ToInt32(row["Id"]);
                
                if (subIdToSet != subIdToChange)
                {
                    var subIdToChangeTbl = _dataAccess.SelectIntoTable(SqlStrings.SelectSubscriptionId(subIdToSet, Convert.ToInt32(row["CleaningObjectId"])));

                    if (subIdToChangeTbl.Rows.Count != 0)
                    {
                        subIdToChange = Convert.ToInt32(subIdToChangeTbl.Rows[0]["Id"]);
                        _dataAccess.NonQuery(SqlStrings.UpdateSubscriptionServiceSubscriptionIds(subIdToSet, subIdToChange));
                        _dataAccess.NonQuery("DELETE FROM " + _dbCurrentDb + ".dbo.Subscriptions WHERE Id = " + subIdToChange);
                    }
                }
                checkInt++;
                if (checkInt % 1000 == 0)
                    Console.WriteLine(checkInt + " of " + subscriptions.Rows.Count + " rows finished...");

            }
        }

        internal void MergeSubscriptions()
        {
            var cleaningObjectsIds = _dataAccess.SelectIntoTable("SELECT count(CleaningObjectId) As NoOfSubscriptions,CleaningObjectId FROM " + _dbCurrentDb + ".dbo.Subscriptions Group By CleaningObjectId HAVING count(CleaningObjectId) >1");

            int subIdToKeep = 0;
            string collectedIds = "";

            foreach (DataRow row in cleaningObjectsIds.Rows)
            {
                int cleaningObjectId = Convert.ToInt32(row["CleaningObjectId"]);

                var subIdToChangeTbl = _dataAccess.SelectIntoTable("Select Id from " + _dbCurrentDb + ".dbo.subscriptions where CleaningObjectId = " + cleaningObjectId.ToString());
                
                collectedIds = "0";
                subIdToKeep = Convert.ToInt32(subIdToChangeTbl.Rows[0]["Id"]);

                foreach (DataRow SubId in subIdToChangeTbl.Rows)
                {
                    collectedIds = collectedIds + "," + Convert.ToString(SubId["Id"]);
                }

                _dataAccess.NonQuery("UPDATE " + _dbCurrentDb + ".dbo.SubscriptionServices SET SubscriptionId = " + subIdToKeep + " WHERE SubscriptionId IN(" + collectedIds + ")");
                _dataAccess.NonQuery("DELETE FROM " + _dbCurrentDb + ".dbo.Subscriptions WHERE Id!=" + subIdToKeep.ToString() + " AND CleaningObjectId = " + cleaningObjectId.ToString());
            }
        }
        
        public void FixCleaningObjectsWithUnconnectedTeams()
        {
            var unconnected = _dataAccess.SelectIntoTable(SqlStrings.GetCleaningObjectsUnconnectedToTeams);
            if (unconnected.Rows.Count > 0)
                _dataAccess.NonQuery(SqlStrings.ConnectUnconnectedCleaningObjectsToTeams);
        }

        public void FixMoreCleaningObjectsWithUnconnectedTeams()
        {
            var unconnected = _dataAccess.SelectIntoTable(SqlStrings.GetCleaningObjectsStillUnconnectedToTeam);

            foreach (DataRow row in unconnected.Rows)
            {
                var possibleTeamId = _dataAccess.SelectIntoTable(SqlStrings.GetTeamIdForUnconnectedCleaningObject(Convert.ToInt32(row["PostalAddressModelId"])));
                if (possibleTeamId.Rows.Count == 0)
                    possibleTeamId = _dataAccess.SelectIntoTable(SqlStrings.GetTeamIdFromPcmsForUnconnectedCleaningObject(Convert.ToInt32(row["PostalAddressModelId"])));
                if (possibleTeamId.Rows.Count == 0)
                    possibleTeamId = _dataAccess.SelectIntoTable(SqlStrings.GetTeamIdFromSamePostalCodeForUnconnectedCo(Convert.ToInt32(row["Id"])));
                if (possibleTeamId.Rows.Count == 0)
                    possibleTeamId = _dataAccess.SelectIntoTable(SqlStrings.GetTeamIdFromSameCityForUnconnCo(Convert.ToInt32(row["Id"])));

                _dataAccess.NonQuery(SqlStrings.SetTeamIdOnCleaningObject(Convert.ToInt32(row["Id"]), Convert.ToInt32(possibleTeamId.Rows[0]["TeamId"])));
            }
        }

        public void AddContactsWhereMissing()
        {
            _dataAccess.NonQuery(SqlStrings.AddContactsToCustomersWithout);
            _dataAccess.NonQuery(SqlStrings.AddContactsToCleaningObjectsWithout);
        }

        public void FixRUT()
        {
            _dataAccess.NonQuery(SqlStrings.UpdateRUTByMainTWContacts);
            _dataAccess.NonQuery(SqlStrings.UpdateRUTOnContacts);
            
            _dataAccess.NonQuery(string.Format("UPDATE {0}.dbo.Contacts SET RUT = 0 WHERE PersonId IN(SELECT Id FROM {0}.dbo.Persons WHERE NoPersonalNoValidation = 1)", _dbCurrentDb));
            _dataAccess.NonQuery(string.Format("UPDATE {0}.dbo.Contacts SET RUT = 0 WHERE PersonId IN(SELECT Id FROM {0}.dbo.Persons WHERE PersonType = 2)", _dbCurrentDb));
        }

        public void ConvertDuplicateServicesToExtrasOnCleaningObjectsAndSubscriptions()
        {
            const int UNIT_PRICE_FOR_EXTRAS = 50;
            _dataAccess.NonQuery(
                string.Format(
                @"SET IDENTITY_INSERT {0}.dbo.Services ON;
                IF NOT EXISTS(SELECT * FROM {0}.dbo.Services WHERE Name = 'Extra 5')
                BEGIN 
	                INSERT INTO {0}.[dbo].[Services]
			                   ([Id]
			                   ,[Name]
			                   ,[Category]
			                   ,[RUT]
			                   ,[Vat]
			                   ,[AccountId]
			                   ,[CalcPercentage]
			                   ,[CalcHourly]
			                   ,[CalcArea]
			                   ,[CalcNone]
			                   ,[From]
			                   ,[Circa]
			                   ,[VisibleOnInvoice]
			                   ,[IsSalarySetting]
			                   ,[SortOrder]
			                   ,[IsDefault]
			                   ,[SubCategoryId]
			                   ,[SettableByCleaners])
		                 VALUES
			                   (75,'Extra 5',4,1,1,1,0,0,0,0,0,1,1,1,75,0,12,1)
			                   ,(76,'Extra 6',4,1,1,1,0,0,0,0,0,1,1,1,76,0,12,1)
			                   ,(77,'Extra 7',4,1,1,1,0,0,0,0,0,1,1,1,77,0,12,1)
                                ,(78,'Extra 8',4,1,1,1,0,0,0,0,0,1,1,1,78,0,12,1)
                                ,(79,'Extra 9',4,1,1,1,0,0,0,0,0,1,1,1,79,0,12,1)
                                ,(80,'Extra 10',4,1,1,1,0,0,0,0,0,1,1,1,80,0,12,1)
                                ,(81,'Extra 11',4,1,1,1,0,0,0,0,0,1,1,1,81,0,12,1);
						INSERT INTO {0}.[dbo].[Prices]
								([Cost]
								,[ServiceGroupId]
								,[ServiceId])
							VALUES
								({1},(SELECT 1 FROM {0}.dbo.ServiceGroups),75)
								,({1},(SELECT 1 FROM {0}.dbo.ServiceGroups),76)
								,({1},(SELECT 1 FROM {0}.dbo.ServiceGroups),77)
								,({1},(SELECT 1 FROM {0}.dbo.ServiceGroups),78)
								,({1},(SELECT 1 FROM {0}.dbo.ServiceGroups),79)
								,({1},(SELECT 1 FROM {0}.dbo.ServiceGroups),80)
								,({1},(SELECT 1 FROM {0}.dbo.ServiceGroups),81)
								,({1},(SELECT 2 FROM {0}.dbo.ServiceGroups),75)
								,({1},(SELECT 2 FROM {0}.dbo.ServiceGroups),76)
								,({1},(SELECT 2 FROM {0}.dbo.ServiceGroups),77)
								,({1},(SELECT 2 FROM {0}.dbo.ServiceGroups),78)
								,({1},(SELECT 2 FROM {0}.dbo.ServiceGroups),79)
								,({1},(SELECT 2 FROM {0}.dbo.ServiceGroups),80)
								,({1},(SELECT 2 FROM {0}.dbo.ServiceGroups),81);
                END;
                SET IDENTITY_INSERT {0}.dbo.Services OFF;", _dbCurrentDb, UNIT_PRICE_FOR_EXTRAS)
            );
            var duplicates = _dataAccess.SelectIntoTable(
                string.Format(
                    @"WITH duplicate_service as (
	                    select CleaningObjectId, ServiceId from {0}.dbo.CleaningObjectPrices
	                    group by CleaningObjectId, ServiceId
	                    having count(*) > 1
                    )
                    SELECT co_prices.Id AS SubscriptionPriceId, co_prices.Modification, co_prices.CleaningObjectId, co_prices.ServiceId,co_prices.[FreeText]
                        ,(SELECT Cost FROM {0}.dbo.Prices prices WHERE ServiceId = co_prices.ServiceId AND prices.ServiceGroupId = 1) as ServicePrice
                        , (SELECT Name FROM {0}.dbo.Services WHERE Id = co_prices.ServiceId) AS ServiceName
                    FROM {0}.dbo.CleaningObjectPrices co_prices
                    INNER JOIN duplicate_service on duplicate_service.CleaningObjectId = co_prices.CleaningObjectId and duplicate_service.ServiceId = co_prices.ServiceId
                    ORDER BY CleaningObjectId,ServiceId"
                    , _dbCurrentDb)
            );
            var cleaningObject = 0;
            var servicesOnObject = new List<int>();
            var extras = new[]{75,76,77,78,79,80,81};

            foreach (DataRow duplicate in duplicates.Rows)
            {
                if (cleaningObject != extractInteger(duplicate,"CleaningObjectId"))
                {
                    servicesOnObject.Clear();
                    cleaningObject = extractInteger(duplicate,"CleaningObjectId");
                }
                var service = extractInteger(duplicate, "ServiceId");
                if (servicesOnObject.Contains(service))
                {
                    var availableExtra = extras.First(extra => !servicesOnObject.Contains(extra));
                    service = availableExtra;
                    var modification = (extractFloatOrDefault(duplicate, "Modification")*extractFloatOrDefault(duplicate, "ServicePrice")) / UNIT_PRICE_FOR_EXTRAS;
                    _dataAccess.NonQuery(
                        string.Format(
                        @"with duplicates as (
	                        select CleaningObjectId, ServiceId from {0}.dbo.CleaningObjectPrices
	                        group by CleaningObjectId, ServiceId
	                        having count(*) > 1
                        ), ordered_duplicates as (
	                        select ss.id as sub_serv_id,subs.CleaningObjectId,ss.ServiceId,subs.id as subscription,
		                        ss.PeriodNo,
		                        row_number() over (partition by SubscriptionId,ss.ServiceId, PeriodNo order by ss.ServiceId) as duplication_rank 
	                        from {0}.dbo.SubscriptionServices ss
	                        inner join {0}.dbo.Subscriptions subs ON subs.Id = ss.SubscriptionId
	                        inner join duplicates on duplicates.CleaningObjectId = subs.CleaningObjectId AND duplicates.ServiceId = ss.ServiceId
                        ), filtered_first_duplicate as (
	                        select * 
	                        from ordered_duplicates
	                        where duplication_rank = 2 and CleaningObjectId = {1:D}
                        )
                        UPDATE {0}.dbo.SubscriptionServices set ServiceId = 25 WHERE id IN (select sub_serv_id from filtered_first_duplicate)",
                            _dbCurrentDb, extractInteger(duplicate, "CleaningObjectId"))
                    );
                    _dataAccess.NonQuery(string.Format(CultureInfo.InvariantCulture, "UPDATE {0}.dbo.CleaningObjectPrices SET ServiceId = {1}, [FreeText]='{2}-{3}', Modification = {4:F8} WHERE Id = {5}"
                        , _dbCurrentDb, service, duplicate["ServiceName"], duplicate["FreeText"], modification, extractInteger(duplicate, "SubscriptionPriceId")));
                }
                servicesOnObject.Add(service);
            }
        }

        private int extractInteger(DataRow record, string field)
        {
            return Convert.ToInt32(record[field]);
        }
    }
}
