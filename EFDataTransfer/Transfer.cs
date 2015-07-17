using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataTransfer
{
    public class Transfer
    {
        private DataAccess _dataAccess;
        private string dbCurrentDB;

        public Transfer()
        {
            //_dataAccess = new DataAccess("Data Source=tcp:x4erdsx1dl.database.windows.net,1433;Initial Catalog=putsa_db;User ID=efits@x4erdsx1dl;Password=Kn4ck3br0d");
            _dataAccess = new DataAccess("Data Source=server01.eriksfonsterputs.net;Initial Catalog=master;User ID=sa;Password=VNbNAQHbK8TDdeMuDXdv");
            //Proddatabasen
            dbCurrentDB = "eriks_test_db";

            //Testdatabasen. Just nu är de omvända, så försiktighet tack!!
            //dbCurrentDB = "putsa_db";

            SqlStrings.dbToUse = dbCurrentDB;


        }

        private void Addresses()
        {
            var twAddresses = _dataAccess.SelectIntoTable(SqlStrings.SelectTWAddresses);
            var postalCodeModels = _dataAccess.SelectIntoTable(SqlStrings.SelectAllPostalCodeModels);
            var clients = _dataAccess.SelectIntoTable("SELECT id, mother_id FROM eriks_migration.dbo.TW_clients");

            var cleaningObjects = new DataTable();
            cleaningObjects.Columns.AddRange(new DataColumn[] {
                new DataColumn("Id", typeof(int)),
                //new DataColumn("AppartmentNo", typeof(string)),
                new DataColumn("PostalAddressModelId", typeof(int)),
                new DataColumn("IsActive", typeof(bool)),
                new DataColumn("RouteIndex", typeof(int)),
                new DataColumn("Invoicable", typeof(bool)),
                new DataColumn("IsNew", typeof(bool))
            });
            var coMappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("Id", "Id"),
                new SqlBulkCopyColumnMapping("PostalAddressModelId", "PostalAddressModelId"),
                new SqlBulkCopyColumnMapping("IsActive", "IsActive"),
                new SqlBulkCopyColumnMapping("RouteIndex", "RouteIndex"),
                new SqlBulkCopyColumnMapping("Invoicable", "Invoicable"),
                new SqlBulkCopyColumnMapping("IsNew", "IsNew")
            };

            var persons = new DataTable();
            persons.Columns.AddRange(new DataColumn[] {
                new DataColumn("PersonId", typeof(int)),
                new DataColumn("PostalAddressModelId", typeof(int)),
                new DataColumn("Type", typeof(int))
            });
            var pMappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("PersonId", "PersonId"),
                new SqlBulkCopyColumnMapping("PostalAddressModelId", "PostalAddressModelId"),
                new SqlBulkCopyColumnMapping("Type", "Type")
            };

            int checkInt = 0;
            foreach (DataRow row in twAddresses.Rows)
            {
                int idTest = Convert.ToInt32(row["id"]);
                if (idTest == 1 || idTest == 2 || idTest == 3 || idTest == 4 || idTest == 2587 || idTest == 16323)
                    Console.WriteLine();

                checkInt++;
                if (checkInt % 1000 == 0)
                    Console.WriteLine(checkInt + " of " + twAddresses.Rows.Count + " rows finished...");

                int postalCodeModelId = 0;

                string postalCodeStr = Convert.ToString(row["postalcode_fixed"]);
                if (string.IsNullOrEmpty(postalCodeStr)) continue;

                string city = Convert.ToString(row["city"]).ToUpper().Trim();
                string[] addrStrArr = Convert.ToString(row["address"]).Split(' ');
                string address = addrStrArr[0];

                int dummy = 0;
                int streetInt = 0;

                if (addrStrArr.Length > 2)
                    for (int i = 1; i < addrStrArr.Length - 1; i++)
                        if (!int.TryParse(addrStrArr[i], out dummy))
                            address += " " + addrStrArr[i];
                        else break;

                address = address.ToUpper().Trim();
                string streetNo = string.Empty;

                bool startReg = false;

                for (int i = 1; i < addrStrArr.Length; i++)
                {
                    if (!startReg && int.TryParse(addrStrArr[i], out dummy))
                        startReg = true;

                    if (startReg)
                        streetNo += addrStrArr[i] + " ";
                }

                var postalCodeRows = postalCodeModels.Select("PostalCode = " + postalCodeStr);

                bool even = false;

                if (int.TryParse(streetNo, out streetInt))
                    even = streetInt % 2 == 0;
                else
                {
                    string evalStreetNo = string.Empty;

                    for (int i = 0; i < streetNo.Length; i++)
                    {
                        if (int.TryParse(streetNo[i].ToString(), out dummy))
                            evalStreetNo += streetNo[i];
                        else break;
                    }

                    if (int.TryParse(evalStreetNo, out streetInt))
                        even = streetInt % 2 == 0;
                }

                if (postalCodeRows.Count() > 0)
                {
                    var postalCodes = postalCodeRows.Where(x => Convert.ToString(x["City"]) == city);

                    if (postalCodes.Count() > 0)
                    {
                        postalCodes = postalCodeRows.Where(x => Convert.ToString(x["PostalAddress"]) == address);

                        if (postalCodes.Count() > 0)
                        {
                            DataRow postalCode = null;
                            try
                            {
                                postalCode = postalCodes.FirstOrDefault(x => Convert.ToInt32(x["StreetNoLowest"]) <= streetInt && Convert.ToInt32(x["StreetNoHighest"]) >= streetInt &&
                                    even ? Convert.ToString(x["TypeOfPlacement"]) == "NJ" : Convert.ToString(x["TypeOfPlacement"]) == "NU"
                                );
                            }
                            catch(FormatException fex)
                            {
                                if (postalCodes.Count() == 1)
                                    postalCode = postalCodes.FirstOrDefault();
                                else
                                {
                                    var pam = _dataAccess.SelectIntoTable(string.Format(SqlStrings.SelectPostalAddressModelsBy(streetNo, postalCodes.Select(x => Convert.ToInt32(x["Id"])))));
                                    try
                                    {
                                        postalCode = postalCodes.FirstOrDefault(x => Convert.ToInt32(x["Id"]) == Convert.ToInt32(pam.Rows[0]["PostalCodeModelId"]));
                                    }
                                    catch(IndexOutOfRangeException iorex)
                                    {
                                        
                                    }
                                }
                            }
                            catch(InvalidCastException icex)
                            {
                                if (Convert.IsDBNull(postalCodes.ElementAt(0)["StreetNoLowest"]))
                                    postalCode = postalCodes.FirstOrDefault();
                            }

                            if (postalCode != null)
                                postalCodeModelId = Convert.ToInt32(postalCode["Id"]);         
                        }
                    }
                }

                if (postalCodeModelId == 0)
                {
                    postalCodeModelId = _dataAccess.InsertSingle(SqlStrings.InsertIntoPostalCodeModels(postalCodeStr, "AT", address, streetNo, streetNo, city, even ? "NJ" : "NU"));
                }

                var postalAddressModels = _dataAccess.SelectIntoTable(string.Format(
                    "SELECT Id FROM " + dbCurrentDB + ".dbo.PostalAddressModels WHERE PostalCodeModelId = '{0}' AND StreetNo = '{1}'", postalCodeModelId, streetNo));

                bool isDelivery = (Convert.ToString(row["is_delivery"]) == "Y") || (Convert.ToInt32(row["route_num"]) > 0 && Convert.ToInt32(row["workarea_id"]) > 0);
                bool isInvoice = Convert.ToString(row["is_invoice"]) == "Y";

                int postalAddressModelId = 0;

                if (postalAddressModels.Rows.Count > 0)
                {
                    postalAddressModelId = Convert.ToInt32(postalAddressModels.Rows[0]["Id"]);
                }
                else
                {
                    string address2 = Convert.ToString(row["co_address"]);
                    if (string.IsNullOrEmpty(address2))
                        address2 = " ";

                    float longitude;
                    float latitude;

                    if (!Convert.IsDBNull(row["longitude"]))
                        longitude = (float)Convert.ToDouble(row["longitude"]);
                    else
                        longitude = 0.0F;
                    if (!Convert.IsDBNull(row["latitude"]))
                        latitude = (float)Convert.ToDouble(row["latitude"]);
                    else
                        latitude = 0.0F;

                    postalAddressModelId = _dataAccess.InsertSingle(SqlStrings.InsertPostalAddressModel(streetNo, postalCodeModelId, "AT", address2, longitude, latitude));
                }

                //int addressType = isDelivery && isInvoice ? 3 : isDelivery ? 1 : 2;

                foreach (DataRow person in clients.Select("id = " + row["client_id"]))
                {
                    if (isDelivery)
                        persons.Rows.Add(new object[] { person["id"], postalAddressModelId, 1 });

                    if (isInvoice)
                    {
                        persons.Rows.Add(new object[] { person["id"], postalAddressModelId, 2 });
                        // Inte helt säker på detta
                        persons.Rows.Add(new object[] { person["id"], postalAddressModelId, 4 });
                    }
                }

                if (isDelivery)
                    cleaningObjects.Rows.Add(new object[] { row["id"], postalAddressModelId, true, row["route_num"], true, false });
            }

            //Console.WriteLine("Inserting " + cleaningObjects.Rows.Count + " cleaning objects...");
            _dataAccess.InsertMany("" + dbCurrentDB + ".dbo.CleaningObjects", cleaningObjects, true, coMappings);

            //Console.WriteLine("Inserting " + persons.Rows.Count + " person to address connections...");
            _dataAccess.InsertMany("" + dbCurrentDB + ".dbo.PersonPostalAddressModels", persons, false, pMappings);
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
                    Console.WriteLine("Start of correcting postalcodes in old table...");
                    FixPostalCodes();
                    Console.WriteLine("End of correcting postalcodes.");
                    Console.WriteLine("Transferring addresses...");
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
                    break;
                case "ISSUES":
                    NotesAndIssues();
                    break;
                case "USERS":
                    Employees();
                    Console.WriteLine("Creating team users...");
                    CreateTeamUsers();
                    break;
                default:
                    break;
            }
        }

 

        private void ConnectCustomersToCleaningObjects()
        {
            
            _dataAccess.NonQuery(SqlStrings.ConnectCustomersToCleaningObjects);
            _dataAccess.NonQuery(SqlStrings.InsertContactsWhereNeeded);
        }

        private void ConnectTeamsToCleaningObjects()
        {
            var twWorkAreas = _dataAccess.SelectIntoTable(SqlStrings.SelectTWWorkAreas);
            var schedules = _dataAccess.SelectIntoTable("SELECT Id, Name FROM " + dbCurrentDB + ".dbo.Schedules");
            int checkInt = 0;

            foreach (DataRow row in twWorkAreas.Rows)
            {
                int teamId = Convert.ToInt32(_dataAccess.SelectIntoTable(
                    string.Format("SELECT t.Id AS Id FROM " + dbCurrentDB + ".dbo.Teams t JOIN " + dbCurrentDB + ".dbo.Vehicles v ON v.Id = t.VehicleId WHERE v.Notes = '{0}'", row["name"])).Rows[0]["Id"]);

                //int tempTeamId = 0;

                //switch (Convert.ToInt32(row["resource_id"]))
                //{
                //    case 1: tempTeamId = 57; break;
                //    case 2: tempTeamId = 58; break;
                //    case 3: tempTeamId = 59; break;
                //    case 4: tempTeamId = 60; break;
                //    case 5: tempTeamId = 61; break;
                //    case 6: tempTeamId = 62; break;
                //    case 7: tempTeamId = 63; break;
                //    case 8: tempTeamId = 64; break;
                //    case 9: tempTeamId = 65; break;
                //    case 10: tempTeamId = 66; break;
                //    case 11: tempTeamId = 67; break;
                //    case 12: tempTeamId = 68; break;
                //    case 13: tempTeamId = 69; break;
                //    case 14: tempTeamId = 70; break;
                //    case 15: tempTeamId = 71; break;
                //    case 16: tempTeamId = 72; break;
                //    case 17: tempTeamId = 73; break;
                //}

                //string[] addrStr = Convert.ToString(row["address"]).Split(' ');
                //string address = addrStr[0].ToUpper();

                //if (addrStr.Length > 2)
                //    for (int i = 0; i < addrStr.Length - 1; i++)
                //        address += " " + addrStr[i];

                //string streetNo = string.Empty;

                //bool startReg = false;
                //int dummy = 0;

                //for (int i = 1; i < addrStr.Length; i++)
                //{
                //    if (!startReg && int.TryParse(addrStr[i], out dummy))
                //        startReg = true;

                //    if (startReg)
                //        streetNo += addrStr[i] + " ";
                //}

                //var coIds = _dataAccess.SelectIntoTable(string.Format("SELECT Id FROM CleaningObjects WHERE 
                    //SqlStrings.SelectCleaningObjectBy(
                    //Convert.ToInt32(row["clientnbr"]), address, Convert.ToInt32(row["postalcode_fixed"]), Convert.ToString(row["city"]).ToUpper(), streetNo));

                //foreach (DataRow idRow in coIds.Rows)
                //{
                _dataAccess.NonQuery(string.Format("UPDATE " + dbCurrentDB + ".dbo.CleaningObjects SET TeamId = {0} WHERE Id = {1}", teamId, row["Id"])); //idRow["Id"]));
                //}

                //var pcmIds = _dataAccess.SelectIntoTable(SqlStrings.SelectPostalCodeModelIdsBy(address, Convert.ToInt32(row["postalcode_fixed"]), Convert.ToString(row["city"]).ToUpper()));

                //foreach (DataRow idRow in pcmIds.Rows)
                //{
                    var srt = string.Format("Name LIKE '% {0}'", Convert.ToString(row["interlude_num"]));
                    var schedule = schedules.Select(string.Format("Name LIKE '% {0}'", Convert.ToString(row["interlude_num"])));

                    //_dataAccess.NonQuery(string.Format("UPDATE PostalCodeModels SET ScheduleId = {0} WHERE Id = {1}", schedule[0]["Id"], idRow["Id"]));
                //}

                    _dataAccess.NonQuery(SqlStrings.UpdatePostalCodeScheduleIds(Convert.ToInt32(schedule[0]["Id"]), Convert.ToInt32(row["Id"])));
                    checkInt++;
                    if (checkInt % 1000 == 0)
                        Console.WriteLine(checkInt + " of " + twWorkAreas.Rows.Count + " rows finished...");

            }

            // Make sure no postal codes contain more than 1 schedule - set schedule to the one with the most entries
            //var pcmsWithMultiScheds = _dataAccess.SelectIntoTable(SqlStrings.SelectPostalCodesWithMultipleSchedules);
            //foreach (DataRow row in pcmsWithMultiScheds.Rows)
            //{
            //    string postalCode = Convert.ToString(row["PostalCode"]);
            //    int majorityScheduleId = Convert.ToInt32(_dataAccess.SelectIntoTable(SqlStrings.SelectMajorityScheduleId(postalCode)).Rows[0]["ScheduleId"]);

            //    _dataAccess.NonQuery(SqlStrings.UpdatePostalCodeScheduleIds(majorityScheduleId, postalCode));
            //}
        }

 

        public void CreateTeamUsers()
        {
            _dataAccess.NonQuery(SqlStrings.CreateUsersForTeams);
        }

        private void SetRUT()
        {
            _dataAccess.NonQuery(SqlStrings.SetRUT);
            _dataAccess.NonQuery("UPDATE " + dbCurrentDB + ".dbo.Contacts SET RUT = 0 WHERE PersonId IN (SELECT Id FROM " + dbCurrentDB + ".dbo.Persons WHERE NoPersonalNoValidation = 1)");
        }

        public void Employees()
        {
            _dataAccess.NonQuery(SqlStrings.TransferEmployees);
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

            _dataAccess.InsertMany("" + dbCurrentDB + ".dbo.Periods", periods, true, mapping);
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

            _dataAccess.InsertMany("" + dbCurrentDB + ".dbo.Settings", settings, false, mapping);
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

            _dataAccess.InsertMany("" + dbCurrentDB + ".dbo.Prices", prices, false, mapping);

            var twWorkOrders = _dataAccess.SelectIntoTable(SqlStrings.SelectTWWorkOrders);

            var subscriptions = new DataTable();
            subscriptions.Columns.AddRange(new DataColumn[] {
                new DataColumn("Id", typeof(int)),
                new DataColumn("CleaningObjectId", typeof(int)),
                new DataColumn("IsInactive", typeof(bool))
            });

            
            foreach (DataRow row in twWorkOrders.Rows)
            {
                //int caId = Convert.ToInt32(row["caId"]);

                //if (caId != prevId)
                    subscriptions.Rows.Add(new object[] { row["woId"], row["caId"], row["woIsInactive"] });

                //prevId = caId;
            }

            _dataAccess.InsertMany("" + dbCurrentDB + ".dbo.Subscriptions", subscriptions, true, null);
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

                coPrices.Rows.Add(new object[] { coId, Convert.ToInt32(row["ServiceId"]), mod, Convert.ToString(row["wolDesc"]) });
            }

            _dataAccess.InsertMany("" + dbCurrentDB + ".dbo.CleaningObjectPrices", coPrices, false, mappings);
        }

        private void SubscriptionServices()
        {
            var twWorkOrderLines = _dataAccess.SelectIntoTable(SqlStrings.SelectTWWorkOrderLines);
            var subscriptionServices = new DataTable();
            subscriptionServices.Columns.AddRange(new DataColumn[] {
                new DataColumn("SetOrChanged", typeof(int)),
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
                                        subscriptionServices.Rows.Add(new object[] { 0, row["workorder_id"], row["sId"], j });

                                    break;
                                }
                                else
                                {
                                    int periodNo = int.Parse(occasion[occasion.Length - 1].ToString());

                                    subscriptionServices.Rows.Add(new object[] { 0, row["workorder_id"], row["sId"], periodNo });
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < 8; j++)
                        subscriptionServices.Rows.Add(new object[] { 4, row["workorder_id"], row["sId"], j });
                }
            }

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("SetOrChanged", "SetOrChanged"),
                new SqlBulkCopyColumnMapping("SubscriptionId", "SubscriptionId"),
                new SqlBulkCopyColumnMapping("ServiceId", "ServiceId"),
                new SqlBulkCopyColumnMapping("PeriodNo", "PeriodNo")
            };

            _dataAccess.InsertMany("" + dbCurrentDB + ".dbo.SubscriptionServices", subscriptionServices, false, mappings);
        }

    
        private void FixPostalCodes()
        {
            var twClientAddresses = _dataAccess.SelectIntoTable(SqlStrings.SelectTWClientAddresses);
            
            foreach (DataRow row in twClientAddresses.Rows)
            {
                string postalCode = string.Empty;
                string orgPostalCode = Convert.ToString(row["postalcode"]).Trim();

                if (orgPostalCode.Length > 5)
                {
                    string[] postalCodeArr = orgPostalCode.Split(' ');
                    if (postalCodeArr.Length == 2)
                        postalCode = postalCodeArr[0] + postalCodeArr[1];
                    else if (postalCodeArr.Length == 3)
                        postalCode = postalCodeArr[0] + postalCodeArr[2];
                }
                else if (orgPostalCode.Length == 5)
                    postalCode = orgPostalCode;
                else
                {
                    Console.WriteLine("Kunde inte korrigera rad med id: " + row["id"]);
                }


                int pcInt = 0;
                if (int.TryParse(postalCode, out pcInt))
                {
                    _dataAccess.NonQuery(SqlStrings.PostalCodeFixUpdate(Convert.ToInt32(row["id"]), int.Parse(postalCode)));
                }
                else
                    Console.WriteLine("Kunde inte korrigera rad med id: " + row["id"]);
            }
        }

 

        public void UtilityTables()
        {
            string sqlTemp;

            sqlTemp="SET IDENTITY_INSERT " + dbCurrentDB + ".dbo.TableLocks ON INSERT INTO " + dbCurrentDB + ".dbo.TableLocks (Id, NextInvoiceNumberTable) VALUES (1, 0) SET IDENTITY_INSERT " + dbCurrentDB + ".dbo.TableLocks OFF";

            if (_dataAccess.SelectIntoTable("SELECT Id FROM " + dbCurrentDB + ".dbo.TableLocks").Rows.Count == 0)
                _dataAccess.NonQuery(
                    @sqlTemp);

            sqlTemp = "SET IDENTITY_INSERT " + dbCurrentDB + ".dbo.NextInvoiceNumbers ON INSERT INTO " + dbCurrentDB + ".dbo.NextInvoiceNumbers (Id, NextAvailableInvoiceNumber) VALUES (1, 1) SET IDENTITY_INSERT " + dbCurrentDB + ".dbo.NextInvoiceNumbers OFF";

            if (_dataAccess.SelectIntoTable("SELECT Id FROM " + dbCurrentDB + ".dbo.NextInvoiceNumbers").Rows.Count == 0)
                _dataAccess.NonQuery(
                    @sqlTemp);
        }

        private void NotesAndIssues()
        {
            string sqlTemp;

           /* sqlTemp= "SELECT n.id AS Id, header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority], "
                     + "CASE WHEN ISDATE(content) = 1 THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, "
                     + "CASE WHEN ISDATE(content) = 1  THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS FinishedDate, "
                     + "clientnbr AS CustomerId, "
                     + "7 AS IssueType, n.created_by_id AS CreatorId, 0 AS Private "
                     + "FROM eriks_migration.dbo.TW_notes n "
                     + "JOIN eriks_migration.dbo.TW_clients cli ON n.table_id = cli.id "
                     + "JOIN " + dbCurrentDB + ".dbo.Customers c on c.Id = cli.clientnbr "
                     + "WHERE table_name = 'clients' "
                     + "AND header LIKE 'Inlagddatum' "
                     + "AND n.deleted = 'N' "
                     + "AND cli.deleted = 'N' "
                     + "AND clientnbr IS NOT NULL";
            * */

            sqlTemp = "SELECT n.id AS Id, header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority], "
         + "CASE WHEN ISDATE(content) = 1 THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, "
         + "CASE WHEN ISDATE(content) = 1  THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS FinishedDate, "
         + "clientnbr AS CustomerId, "
         + "7 AS IssueType, n.created_by_id AS CreatorId, 0 AS Private "
         + "FROM eriks_migration.dbo.TW_notes n "
         + "JOIN eriks_migration.dbo.TW_clients cli ON n.table_id = cli.id "
         + "JOIN " + dbCurrentDB + @".dbo.Customers c on c.Id = cli.clientnbr "
         + "WHERE tag_home = 'kund' AND tag_type = 'beställning'";


            Console.WriteLine("'Första beställningsdatum': " + _dataAccess.SelectIntoTable(@sqlTemp).Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferCustomerAddedNotes);

            Console.WriteLine("'Ekonomianteckning objekt'" + _dataAccess.SelectIntoTable(@"SELECT n.id AS Id, header AS Title, content AS [Description], 
                        CASE WHEN n.deleted = 'N' AND n.closed = 'N' THEN 2 ELSE 4 END AS [Status], 
                        0 AS [Priority], 
                        CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, 
                        5 AS IssueType, wo.delivery_clientaddress_id AS CleaningObjectId, n.created_by_id AS CreatorId, 0 AS Private
                    FROM eriks_migration.dbo.TW_notes n
                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                    WHERE table_name = 'workorders' --AND tag_type IS NULL 
                    AND notetype_id = '5'
                    AND n.deleted = 'N'").Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferEconomyNotes);

            sqlTemp = "SELECT n.id AS Id, header AS Title, content AS [Description], CASE WHEN n.deleted = 'N' AND n.closed = 'N' THEN 2 ELSE 4 END "
                    + "AS [Status], 0 AS [Priority], CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, "
                    + "5 AS IssueType, clientnbr AS CustomerId, "
                    + "CASE WHEN n.created_by_id <> -1 THEN n.created_by_id ELSE NULL END AS CreatorId, 0 AS Private "
                    + "FROM eriks_migration.dbo.TW_notes n "
                    + "JOIN eriks_migration.dbo.TW_clients cli ON n.table_id = cli.id "
                    + "JOIN " + dbCurrentDB + ".dbo.Customers c ON c.Id = cli.clientnbr "
                    + "WHERE table_name = 'clients' "
                    + "AND notetype_id = '5' "
                    + "AND n.deleted = 'N'";

            Console.WriteLine("'Ekonomianteckning kund: " + _dataAccess.SelectIntoTable(@sqlTemp).Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferEconomyCustomerNotes);

            Console.WriteLine("Setting 'InfoBeforeCleaning'");
            // *** Update CleaningObjectInfo *** //
            // Before
            var twInfos = _dataAccess.SelectIntoTable(SqlStrings.SelectTWCleaningObjectInfoBefore);

            int currentCleaningObjectId = 0;
            int checkInt=0;
            string info = string.Empty;

            checkInt = 0;

            foreach (DataRow row in twInfos.Rows)
            {


                int nextCleaningObjectId = Convert.ToInt32(row["CleaningObjectId"]);

                if (nextCleaningObjectId != currentCleaningObjectId)
                {
                    info = Convert.ToString(row["Info"]).Replace("'", "''");
                    currentCleaningObjectId = nextCleaningObjectId;
                }
                else
                {
                    info = info + "\r\n" + Convert.ToString(row["Info"]).Replace("'", "''");
                    currentCleaningObjectId = nextCleaningObjectId;
                }

                _dataAccess.NonQuery(SqlStrings.UpdateCleaningObjectInfoBefore(currentCleaningObjectId, info));

                checkInt++;
                if (checkInt % 1000 == 0)
                    Console.WriteLine(checkInt + " of " + twInfos.Rows.Count + " rows finished...");

            }

            Console.WriteLine("Setting 'InfoDuringCleaning'");
            // During
            var twInfosDuring = _dataAccess.SelectIntoTable(SqlStrings.SelectTWCleaningObjectInfoDuring);

            currentCleaningObjectId = 0;
            info = string.Empty;
            checkInt = 0;

            foreach (DataRow row in twInfosDuring.Rows)
            {
                int nextCleaningObjectId = Convert.ToInt32(row["CleaningObjectId"]);

                if (nextCleaningObjectId != currentCleaningObjectId)
                {
                    info = Convert.ToString(row["Info"]).Replace("'", "''");
                    currentCleaningObjectId = nextCleaningObjectId;
                }
                else
                {
                    info = info + "\r\n" + Convert.ToString(row["Info"]).Replace("'", "''");
                    currentCleaningObjectId = nextCleaningObjectId;
                }

                _dataAccess.NonQuery(SqlStrings.UpdateCleaningObjectInfoDuring(currentCleaningObjectId, info));

                checkInt++;
                if (checkInt % 1000 == 0)
                    Console.WriteLine(checkInt + " of " + twInfosDuring.Rows.Count + " rows finished...");

            }

            Console.WriteLine("Setting remaining 'InfoBeforeCleaning'");
            _dataAccess.NonQuery(SqlStrings.UpdateEmptyCleaningObjectInfoBefore);

            ////// *** Info updated *** //

            ////Console.WriteLine("Transferring notes and issues");
            ////////////////_dataAccess.NonQuery(SqlStrings.TransferCleaningObjectNotes);

            sqlTemp = " SELECT notes.id AS Id, header AS Title, content AS [Description], 2 AS [Status], "
	                + "CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,"
	                + "wo.delivery_clientaddress_id AS CleaningObjectId,"
	                + "7 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority "
                    + "FROM eriks_migration.dbo.TW_notes AS notes " 
                    + "JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id "
                    + "JOIN " + dbCurrentDB + ".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id "
                    + "WHERE table_name = 'workorders' AND notes.header LIKE '%NDRING%' AND notes.header LIKE '%TILLVAL%' "
                    + "AND notes.deleted = 'N'";

            Console.WriteLine("Vanliga anteckningar Objekt Tillval: " + _dataAccess.SelectIntoTable(sqlTemp).Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferMoreCleaningObjectNotes);

            Console.WriteLine("Vanliga anteckningar Kund Tillval: " + _dataAccess.SelectIntoTable(@"  SELECT notes.id AS Id,
                        CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    header AS Title, content AS [Description], 4 AS [Status], 7 AS IssueType, notes.created_by_id AS CreatorId,
	                    cli.clientnbr AS CustomerId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes  
                    JOIN eriks_migration.dbo.TW_clients AS cli on notes.table_id = cli.id
                    WHERE table_name = 'clients' AND notes.header LIKE '%NDRING%' AND notes.header LIKE '%TILLVAL%' AND notes.deleted = 'N'
                        AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + ".dbo.Issues)  ").Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferCustomerNotes);

            sqlTemp = "SELECT notes.id AS Id, notes.header AS Title, content AS [Description], 2 AS [Status], "
	                 + "CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,"
	                 + "created_by_id AS CreatorId, 2 AS IssueType, wo.delivery_clientaddress_id AS CleaningObjectId, 0 AS Private, 0 AS Priority "
                     + "FROM eriks_migration.dbo.TW_notes AS notes " 
                     + "JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id "
                     + "JOIN " + dbCurrentDB + ".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id "
                     + "WHERE table_name = 'workorders' AND notes.header LIKE 'BESTÄLLNING' AND notes.deleted = 'N' "
                     + "AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + ".dbo.Issues) ";

            Console.WriteLine("Vanliga anteckningar Beställning: " + _dataAccess.SelectIntoTable(sqlTemp).Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectOrders);

            sqlTemp = "SELECT notes.id AS Id, header AS Title, content AS [Description], "
	                + "CASE WHEN notes.deleted = 'N' THEN 4 ELSE 2 END AS [Status], "  
	                + "CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate, "
	                + "wo.delivery_clientaddress_id AS CleaningObjectId, 7 AS IssueType, 0 AS Private, 0 AS Priority "
                    + "FROM eriks_migration.dbo.TW_notes AS notes  "
                    + "JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id "
                    + "JOIN " + dbCurrentDB + ".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id "
                    + "WHERE table_name = 'workorders' AND notes.header LIKE 'telefonhistorik' AND notes.deleted = 'N' "
                    + "AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + ".dbo.Issues)  ";

            Console.WriteLine("Vanliga anteckningar Telefonhistorik: " + _dataAccess.SelectIntoTable(sqlTemp).Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferCustomerTelephoneNotes);

             sqlTemp ="SELECT notes.id AS Id, notes.header AS Title, content AS [Description], "
	                + "CASE WHEN notes.deleted ='N' THEN 2 ELSE 4 END AS [Status], "
	                + "CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate, "
	                + "created_by_id AS CreatorId, 7 AS IssueType, wo.delivery_clientaddress_id AS CleaningObjectId, 0 AS Private, 0 AS Priority "
                    + "FROM eriks_migration.dbo.TW_notes AS notes  "
                    + "JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id "
                    + "JOIN " + dbCurrentDB + ".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id "
                    + "WHERE table_name = 'workorders' AND notes.header LIKE 'HUSBESKRIVNING' AND notes.deleted = 'N' "
                    + "AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + ".dbo.Issues)  ";

            Console.WriteLine("Vanliga anteckningar Husbeskrivning: " + _dataAccess.SelectIntoTable(sqlTemp).Rows.Count);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectDescriptions);

            Console.WriteLine("Uppsägningar kund: " + _dataAccess.SelectIntoTable(@" SELECT DISTINCT notes.id AS Id, 'Uppsägning' AS Title, content AS [Description], 4 AS [Status],
	                    CASE WHEN ISDATE(content) = 1 THEN content WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    cli.clientnbr AS CustomerId, 3 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_clients AS cli ON cli.id = notes.table_id
                    WHERE table_name = 'clients' 
                    AND content <> ''
                    AND notes.header IN ('Uppsagddatum', 'UPPSÄGNING', 'UPPSAGD') AND notes.deleted = 'N'
                        AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + ".dbo.Issues)").Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferCustomerCancellations);

            Console.WriteLine("Uppsägningar: " + _dataAccess.SelectIntoTable(@" SELECT n.Id AS Id, content AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority], 
                        CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, 
                        0 AS [Private], wo.delivery_clientaddress_id AS CleaningObjectId, 3 AS IssueType
                    FROM eriks_migration.dbo.TW_notes n 
                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                    JOIN " + dbCurrentDB + @".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' AND notetype_id = 1 AND n.deleted = 'N' AND important = 'Y' AND 
                    n.content LIKE '%UPPSAGD%'     AND n.id NOT IN (SELECT Id FROM " + dbCurrentDB + @".dbo.Issues)  
                    ORDER BY table_id").Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferMoreCancellations);

            Console.WriteLine("Uppsägningar objekt: " + _dataAccess.SelectIntoTable(@"SELECT DISTINCT notes.id AS Id, 'Uppsägning' AS Title, content AS [Description], 4 AS [Status],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    wo.delivery_clientaddress_id AS CleaningObjectId, 3 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_workorders wo ON wo.id = notes.table_id
                    JOIN " + dbCurrentDB + @".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' 
                    AND content <> ''
                    AND notes.header IN ('Uppsagddatum', 'UPPSÄGNING', 'UPPSAGD') AND notes.deleted = 'N'
                    AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + ".dbo.Issues)").Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectCancellations);

            Console.WriteLine("Anteckningar objekt: " + _dataAccess.SelectIntoTable(@" SELECT  DISTINCT notes.id AS Id, header AS Title, content AS [Description], 2 AS [Status], 
                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
                    wo.delivery_clientaddress_id AS CleaningObjectId, 7 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                FROM eriks_migration.dbo.TW_notes AS notes 
                JOIN eriks_migration.dbo.TW_workorders AS wo on wo.id = notes.table_id
                JOIN " + dbCurrentDB + @".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                WHERE table_name = 'workorders' AND notes.deleted = 'N'
                AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + @".dbo.Issues)
                AND notes.header IN ('%TELEFON%','%Kundhistorik%','%info_BV%','%MAIL%','%info tillval%','%info faktura%','%special%','%Anm%','%Info%')").Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferEvenMoreCleaningObjectNotes);

            Console.WriteLine("Anteckningar kund: " + _dataAccess.SelectIntoTable(@" SELECT notes.id AS Id, header AS Title, content AS [Description], 2 AS [Status], 
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    cli.clientnbr AS CustomerId, 7 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_clients AS cli ON cli.id = notes.table_id
                    JOIN " + dbCurrentDB + @".dbo.Customers c ON c.Id = cli.clientnbr
                    WHERE table_name = 'clients'
                    AND notes.deleted = 'N'
                    AND notes.header IN ('TELEFON','Kundhistorik','info_BV','MAIL','info tillval','info faktura','special','Anm', 'Info')
                    AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + ".dbo.Issues)").Rows.Count);

            _dataAccess.NonQuery(SqlStrings.TransferMoreCustomerNotes);
        }

  
  

        internal void MergeSubscriptions()
        {
            var subscriptions = _dataAccess.SelectIntoTable("SELECT * FROM " + dbCurrentDB + ".dbo.Subscriptions");

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
                        _dataAccess.NonQuery("DELETE FROM " + dbCurrentDB + ".dbo.Subscriptions WHERE Id = " + subIdToChange);
                    }
                }
                checkInt++;
                if (checkInt % 1000 == 0)
                    Console.WriteLine(checkInt + " of " + subscriptions.Rows.Count + " rows finished...");

            }
        }

        public void DeleteTable(string refTable,string refFieldToClean,string table)
        {
            if (refFieldToClean.Length>1)
            {
                if(refTable.Length>1)
                {
                    _dataAccess.NonQuery("UPDATE " + dbCurrentDB + ".dbo." + refTable + " SET " + refFieldToClean + " = Null");
                }
            }
            _dataAccess.NonQuery("DELETE FROM " + dbCurrentDB + ".dbo." + table);
        }

        private void TransferAndConnectBanks()
        {

        }

        internal void TruncateTable(string table)
        {
            _dataAccess.NonQuery("TRUNCATE TABLE " + dbCurrentDB + ".dbo." + table);
        }
    }
}
