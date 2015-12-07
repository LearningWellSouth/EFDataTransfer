﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EFDataTransfer
{
    public class Transfer
    {
      public const string POSTALCODEMODEL_CODETYPE = "PostalCodeType";
      private readonly IDataAccess _dataAccess;
        private readonly string _dbCurrentDb;
      private readonly Logger _logger;

      public Transfer(Logger logger)
        {
        _logger = logger;

#if DEBUG
          _dataAccess = new DataAccess(@"Server=WIN-HIA2DJPDOTQ\SQLEXPRESS;Integrated Security=true;Initial Catalog=master");
          _dbCurrentDb = "debug_migration_db";
          _dataAccess.ValidateConnectionSettings();
#else
            _dataAccess = new DataAccess("Data Source=server01.eriksfonsterputs.net;Initial Catalog=master;User ID=sa;Password=VNbNAQHbK8TDdeMuDXdv");
            //dbCurrentDB = "eriks_dev_db";
            dbCurrentDB = "putsa_db";
#endif
            SqlStrings.dbToUse = _dbCurrentDb;
        }

        public void FindClientPostalCodeMapping_CreateAddressAndCleaningObjectEntriesForClient()
        {
            var twAddresses = _dataAccess.SelectIntoTable(SqlStrings.SelectTWAddresses);
            var postalCodeModels = _dataAccess.SelectIntoTable(SqlStrings.SelectAllPostalCodeModels);
            var clients = _dataAccess.SelectIntoTable("SELECT id, mother_id FROM eriks_migration.dbo.TW_clients");

            var cleaningObjects = new DataTable();
            cleaningObjects.Columns.AddRange(new DataColumn[] {
                new DataColumn("Id", typeof(int)),
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

            var personPostalAddressModel = new DataTable();
            personPostalAddressModel.Columns.AddRange(new DataColumn[] {
                new DataColumn("PersonId", typeof(int)),
                new DataColumn("PostalAddressModelId", typeof(int)),
                new DataColumn("Type", typeof(int))
            });
            var pMappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("PersonId", "PersonId"),
                new SqlBulkCopyColumnMapping("PostalAddressModelId", "PostalAddressModelId"),
                new SqlBulkCopyColumnMapping("Type", "Type")
            };

            var recordNumber = 0;
          var parser = new AddressParser(_logger);
            foreach (DataRow addressRecord in twAddresses.Rows)
            {

                recordNumber++;
                if (recordNumber % 1000 == 0)
                    Console.WriteLine(recordNumber + " of " + twAddresses.Rows.Count + " rows finished...");

              var parsedAddress = parser.ParseAddress(addressRecord["address"], addressRecord["postalcode"], addressRecord["city"]);

                 var possiblePostalCodeModels = postalCodeModels.Select(string.Format("City = '{0}' AND PostalAddress ='{1}'", parsedAddress.City, parsedAddress.StreetName));

                  int postalCodeModelId = 0;
                  if (possiblePostalCodeModels.Any())
                  {
                    var postalCode = possiblePostalCodeModels.FirstOrDefault(
                      x => isStreetNumberWithinMaxAndMin(x, parsedAddress.StreetNumber) && hasCorrectPostalNumberType(x,parsedAddress));

                      if (postalCode != null)
                          postalCodeModelId = Convert.ToInt32(postalCode["Id"]);
                  }

                if (postalCodeModelId == 0)
                {
                  postalCodeModelId = InsertNewPostalCodeModelRow(parsedAddress.PostalNumber, parsedAddress, parsedAddress.City);
                }

                var postalAddressModels = fetchAddressEntriesForPostalCodeAndStreetNumber(postalCodeModelId, parsedAddress);

                int postalAddressModelId;

                if (postalAddressModels.Rows.Count > 0)
                {
                    postalAddressModelId = Convert.ToInt32(postalAddressModels.Rows[0]["Id"]);
                }
                else
                {
                  var address2 = Convert.IsDBNull(addressRecord["co_address"]) ? "" : Convert.ToString(addressRecord["co_address"]);
                  var longitude = extractFloatOrDefault(addressRecord, "longitude");
                  var latitude = extractFloatOrDefault(addressRecord, "latitude");

                  postalAddressModelId = InsertWithKeyReturn(SqlStrings.InsertPostalAddressModel(parsedAddress.StreetNumberFull, postalCodeModelId, "AT", address2, longitude, latitude));
                }

                bool isDelivery = (Convert.ToString(addressRecord["is_delivery"]) == "Y") || (Convert.ToInt32(addressRecord["route_num"]) > 0 && Convert.ToInt32(addressRecord["workarea_id"]) > 0);
                bool isInvoice = Convert.ToString(addressRecord["is_invoice"]) == "Y";

                foreach (var person in clients.Select("id = " + addressRecord["client_id"]))
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
                    cleaningObjects.Rows.Add(addressRecord["id"], postalAddressModelId, true, addressRecord["route_num"], true, false);
            }

            _dataAccess.InsertMany(string.Format("{0}.dbo.CleaningObjects", _dbCurrentDb), cleaningObjects, true, coMappings);
            _dataAccess.InsertMany(string.Format("{0}.dbo.PersonPostalAddressModels", _dbCurrentDb), personPostalAddressModel, false, pMappings);
        }

      public static bool hasCorrectPostalNumberType(DataRow dataRow, Address parsedAddress)
      {
        const string EVEN = "NJ";
        const string ODD = "NU";

        var type = Convert.ToString(dataRow[POSTALCODEMODEL_CODETYPE]);
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
          _logger.PostError("Exception while inserting "+recurseExceptionMessages(exc)+exc.StackTrace);
          throw;
        }
      }

      private string recurseExceptionMessages(Exception exc)
      {
        if (exc == null) return "";
        return exc.Message + recurseExceptionMessages(exc.InnerException);
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

      private int InsertNewPostalCodeModelRow(string postalCodeStr, Address parsedAddress, string city)
      {
        return InsertWithKeyReturn(SqlStrings.InsertIntoPostalCodeModels(postalCodeStr, "AT", parsedAddress.StreetName, parsedAddress.StreetNumberFull, parsedAddress.StreetNumberFull, city, parsedAddress.isEvenStreetNumber() ? "NJ" : "NU"));
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
                case "SCHEDULES":
                    SchedulesAndPeriods();
                    break;
                case "TEAMS":
                    Console.WriteLine("Connecting teams to cleaning objects...");
                    ConnectTeamsToCleaningObjects();
                // Samtliga putsobjekt som inte får teamId av ovanstående har i TW-tabellen workarea_id = 0, vilket jag antar betyder att de inte är kopplade
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
                    Employees();
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

        public void ConnectTeamsToCleaningObjects()
        {
            var twWorkAreas = _dataAccess.SelectIntoTable(SqlStrings.SelectTWWorkAreas);
            var schedules = _dataAccess.SelectIntoTable("SELECT Id, Name FROM " + _dbCurrentDb + ".dbo.Schedules");
            int checkInt = 0;

            foreach (DataRow row in twWorkAreas.Rows)
            {
                int teamId = Convert.ToInt32(_dataAccess.SelectIntoTable(
                    string.Format("SELECT t.Id AS Id FROM " + _dbCurrentDb + ".dbo.Teams t JOIN " + _dbCurrentDb + ".dbo.Vehicles v ON v.Id = t.VehicleId WHERE v.Notes = '{0}'", row["name"])).Rows[0]["Id"]);

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
                _dataAccess.NonQuery(string.Format("UPDATE " + _dbCurrentDb + ".dbo.CleaningObjects SET TeamId = {0} WHERE Id = {1}", teamId, row["Id"])); //idRow["Id"]));
                //}

                //var pcmIds = _dataAccess.SelectIntoTable(SqlStrings.SelectPostalCodeModelIdsBy(address, Convert.ToInt32(row["postalcode_fixed"]), Convert.ToString(row["city"]).ToUpper()));

                //foreach (DataRow idRow in pcmIds.Rows)
                //{
                    
                    //var schedule = schedules.Select(string.Format("Name LIKE '% {0}'", Convert.ToString(row["interlude_num"])));
               
                    //_dataAccess.NonQuery(string.Format("UPDATE PostalCodeModels SET ScheduleId = {0} WHERE Id = {1}", schedule[0]["Id"], idRow["Id"]));
                //}

                    //_dataAccess.NonQuery(SqlStrings.UpdatePostalCodeScheduleIds(Convert.ToInt32(schedule[0]["Id"]), Convert.ToInt32(row["Id"])));
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

      // TODO : Schedules, this means there is an ambiguation in the schedules data. Also there are notes about "should this be done or not"
        // Make sure no postal codes contain more than 1 schedule - set schedule to the one with the most entries
        public void MergePostalCodeSchedules()
        {
            var pcmsWithMultiScheds = _dataAccess.SelectIntoTable(SqlStrings.SelectPostalCodesWithMultipleSchedules);
            foreach (DataRow row in pcmsWithMultiScheds.Rows)
            {
                string postalCode = Convert.ToString(row["PostalCode"]);
                int majorityScheduleId = Convert.ToInt32(_dataAccess.SelectIntoTable(SqlStrings.SelectMajorityScheduleId(postalCode)).Rows[0]["ScheduleId"]);

                _dataAccess.NonQuery(SqlStrings.UpdatePostalCodeScheduleIds(majorityScheduleId, postalCode));
            }
        }

      public void ExecuteScriptFile(string pathToScript)
      {
          _logger.PostInfo("Started running script "+pathToScript);
          using(var script = File.OpenRead(pathToScript))
          using(var stringReader = new StreamReader(script))
          {            
            var batch = "";
            while (!stringReader.EndOfStream)
            {
              var line = ReadLineFromScript(stringReader);

              if (line == "GO")
              {
                try
                {
                  _dataAccess.NonQuery(batch);
                }
                catch (Exception ex)
                {
                  _logger.PostError(string.Format("{0} while running batch \"{1}\"", ex.Message, batch));
                  throw;
                }       
                batch = "";              
              }
              else
              {
                batch += " " + line;
              }
            }
          };
      }

      public string ReadLineFromScript(StreamReader stringReader)
      {
        var line = stringReader.ReadLine();
        if (string.IsNullOrEmpty(line)) return "";
        line = Regex.Replace(line.Trim(), "--.*", "");
        return line
          .Replace("DATABASE_NAME", _dbCurrentDb)
          .Replace("@TARGET_DATABASE", _dbCurrentDb)
          .Replace("@SOURCE_DATABASE", "eriks_migration");
      }

      public void CreateTeamUsers()
        {
            _dataAccess.NonQuery(SqlStrings.CreateUsersForTeams);
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
                periods.Rows.Add(new object[] { 
                    interlude["id"], scheduleId, interlude["weekFrom"], Convert.ToInt32(interlude["weekFrom"]) + 1, Convert.ToDateTime(interlude["startdate"]), interlude["period"] });
            }

            _dataAccess.InsertMany("" + _dbCurrentDb + ".dbo.Periods", periods, true, mapping);
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
                new DataColumn("IsInactive", typeof(bool))
            });

            
            foreach (DataRow row in twWorkOrders.Rows)
            {
                //int caId = Convert.ToInt32(row["caId"]);

                //if (caId != prevId)
                    subscriptions.Rows.Add(new object[] { row["woId"], row["caId"], row["woIsInactive"] });

                //prevId = caId;
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
                    mod = (float)woPrice;

                if (mod > 0)
                    mod = (float)Math.Round(mod * 1.25f, 4);

                coPrices.Rows.Add(new object[] { coId, Convert.ToInt32(row["ServiceId"]), mod, Convert.ToString(row["wolDesc"]) });
            }

            _dataAccess.InsertMany("" + _dbCurrentDb + ".dbo.CleaningObjectPrices", coPrices, false, mappings);
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
                        subscriptionServices.Rows.Add(new object[] { 3, row["workorder_id"], row["sId"], j });
                }
            }

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("SetOrChanged", "SetOrChanged"),
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
                new DataColumn("SetOrChanged", typeof(int)),
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

                subscriptionServices.Rows.Add(new object[] { 0, rowSubscriptionId, 28, periodNo });

                periodNo++;
            }

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("SetOrChanged", "SetOrChanged"),
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
                new DataColumn("SetOrChanged", typeof(int)),
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
                
                subscriptionServices.Rows.Add(new object[] { 0, rowSubscriptionId, 28, periodNo });

                periodNo++;
            }

            var mappings = new SqlBulkCopyColumnMapping[] {
                new SqlBulkCopyColumnMapping("SetOrChanged", "SetOrChanged"),
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

            sqlTemp="SET IDENTITY_INSERT " + _dbCurrentDb + ".dbo.TableLocks ON INSERT INTO " + _dbCurrentDb + ".dbo.TableLocks (Id, NextInvoiceNumberTable) VALUES (1, 0) SET IDENTITY_INSERT " + _dbCurrentDb + ".dbo.TableLocks OFF";

            if (_dataAccess.SelectIntoTable("SELECT Id FROM " + _dbCurrentDb + ".dbo.TableLocks").Rows.Count == 0)
                _dataAccess.NonQuery(
                    @sqlTemp);

            sqlTemp = "SET IDENTITY_INSERT " + _dbCurrentDb + ".dbo.NextInvoiceNumbers ON INSERT INTO " + _dbCurrentDb + ".dbo.NextInvoiceNumbers (Id, NextAvailableInvoiceNumber) VALUES (1, 1) SET IDENTITY_INSERT " + _dbCurrentDb + ".dbo.NextInvoiceNumbers OFF";

            if (_dataAccess.SelectIntoTable("SELECT Id FROM " + _dbCurrentDb + ".dbo.NextInvoiceNumbers").Rows.Count == 0)
                _dataAccess.NonQuery(
                    @sqlTemp);
        }

        private void NotesAndIssues()
        {
            _dataAccess.NonQuery(SqlStrings.TransferCustomerNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectOrderNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCustomerEconomyNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectEconomyNotes);
            _dataAccess.NonQuery(SqlStrings.TransferCleaningObjectFieldNotes);
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
                var contentTable = _dataAccess.SelectIntoTable(SqlStrings.SelectTwNoteAndCleaningObjectId(noteTableId));

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

        public void DeleteAllRowsInTable(string refTable,string refFieldToClean,string table)
        {
            if (refFieldToClean.Length>1)
            {
                if(refTable.Length>1)
                {
                    _dataAccess.NonQuery("UPDATE " + _dbCurrentDb + ".dbo." + refTable + " SET " + refFieldToClean + " = Null");
                }
            }
            _dataAccess.NonQuery("DELETE FROM " + _dbCurrentDb + ".dbo." + table);
        }

        private void TransferAndConnectBanks()
        {

        }

        internal void TruncateTable(string table)
        {
            _dataAccess.NonQuery("TRUNCATE TABLE " + _dbCurrentDb + ".dbo." + table);
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
    }
}
