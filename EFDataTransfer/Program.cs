using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataTransfer
{

    /* Instruktioner
   
               ////// Om Azure SQL:
                //////// Kör Backup genom Azures dashboard -
                //////// a. Gå in på databasen
                //////// b. Välj "Export" från den nedre menyn
                //////// c. Fyll i uppgifter
                //////// d. Kör
                //////// Info om SQL Azure DB-export/import: http://msdn.microsoft.com/en-us/library/azure/hh335292.aspx

                // ////a. Packa upp dumpfilen *.sql.bz2 (mha tex 7-zip)
                // ////b. Importera till en MySQL-server (mha MySQL Workbench) 
                // ////     Starta en server (med MySQL Notifier), tryck sedan "Start Server" under panel Navigator/Instance/"Startup / Shutdown" i Workbench
                // ////c. Om ny SQL-server:
                // ////c.1. Kör Sql Server Migration Assistant for MySQL för att lägga över db-schemat från MySql-db till MSSQL
                // ////c.2. Högerklicka på resulterande schema i SQL Server Metadata Explorer-fönstret i SSMA, välj "Save as script"
                // ////c.3. Modifiera scriptet så att det använder rätt databas och har rätt kolumnnamn (ska börja på TW_) - gör med "Find and replace", scriptet är väldigt långt
                // ////I clients, invoicelines kan ctime behöva sättas till NULL där det har värdet '0000-00-00 00:00:00'.
                ////// Kolla även så att eriks_migrations-databasen på glesys har alla fält som finns i dumpfilen.
                // ////d. Kör EF_DataMigration (MySQL -> SQL)

                //// Om nya TW-tabeller, kör: alter table TW_clientaddresses add postalcode_fixed int null
      
      
      
    */

    public class tableProperty
    {
        public string tableName { get; set; }
        public bool truncFlag { get; set; }
        public bool transferData { get; set; }
        public string refTable { get; set; }
        public string refFieldToClean { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
          var ErrorLogger = new ErrorLogger();
            try
            {
              var transferrer = new Transfer(ErrorLogger);
              List<tableProperty> allTables = new List<tableProperty>();

                //tables with no transfer or subtables for transfers
              // TODO : is this the complete list of tables?
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "BGMAXPaymentInvoiceConnections", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderResources", truncFlag = false, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderResourceWorkers", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderTimeReports", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "Invoices", refFieldToClean = "WorkOrderId", tableName = "Workorders", truncFlag = false, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "InvoiceRows", refFieldToClean = "TransactionId", tableName = "Transactions", truncFlag = false, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "TransactionValues", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "IssueHistories", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SingleCleanings", truncFlag = false, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SingleCleaningServices", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Deviations", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "InvoiceContacts", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "InvoiceRows", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "InvoiceId", tableName = "Invoices", truncFlag = false, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "CleaningObjectId", tableName = "CleaningObjects", truncFlag = false, transferData = false });

                 //Tables with transfers
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SystemLogs", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Settings", truncFlag = true, transferData = true });
                 allTables.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "CustomerId", tableName = "Persons", truncFlag = false, transferData = true });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "PersonPostalAddressModels", truncFlag = false, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "PostalAddressModels", truncFlag = false, transferData = true });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Contacts", truncFlag = true, transferData = true });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Customers", truncFlag = false, transferData = true });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Banks", truncFlag = false, transferData = true });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "UsedTaxReductionRequestNumbers", truncFlag = true, transferData = false });
                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Periods", truncFlag = true, transferData = false }); //Connected to Schedules
                 allTables.Add(new tableProperty() { refTable = "PostalCodeModels", refFieldToClean = "ScheduleId", tableName = "Schedules", truncFlag = false, transferData = true });  // Kontrollera mot postnummer-område.xlsx

                 allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Workers", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "VehicleHistories", truncFlag = true, transferData = false });
                allTables.Add(new tableProperty() { refTable = "Teams", refFieldToClean = "VehicleId", tableName = "Vehicles", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "Users", refFieldToClean = "TeamId", tableName = "Teams", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Accounts", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SubCategories", truncFlag = false, transferData = true });


                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Services", truncFlag = false, transferData = true });
                 
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "ServiceGroups", truncFlag = false, transferData = false });       //Connected to Subscriptions
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Prices", truncFlag = true, transferData = false });//Connected to Subscriptions
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Subscriptions", truncFlag = false, transferData = true }); 
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderServices", truncFlag = true, transferData = false });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SubscriptionServices", truncFlag = false, transferData = true });


                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "CleaningObjectPrices", truncFlag = true, transferData = true });
                allTables.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "CreatorId", tableName = "Users", truncFlag = false, transferData = true });

                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Issues", truncFlag = true, transferData = true });
              



                foreach (tableProperty curTable in allTables)
                {
                    if (curTable.tableName == "CleaningObjects" && allTables.FirstOrDefault(x => x.tableName == "PostalAddressModels") == null)
                        continue;// TODO : WTF?! I sincerely hope that allTables is NOT modified within the itteration

                    //Töm tabellen först
                    if (curTable.truncFlag == true) // TODO : this switching is unneccessary. Use delete statement for all or "truncate cascaded"
                    {
                        Console.WriteLine("truncating {0}...", curTable.tableName);
                        transferrer.TruncateTable(curTable.tableName);
                    }
                    else
                    {
                        Console.WriteLine("Deleting all records in {0}...", curTable.tableName);
                        transferrer.DeleteAllRowsInTable(curTable.refTable, curTable.refFieldToClean, curTable.tableName);
                    }
                    //Kör transfer
                    
                    if(curTable.transferData==true)
                    {
                        Console.WriteLine("Transferring {0}...", curTable.tableName);
                        transferrer.TransferData(curTable.tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+@"\n\r"+ex.StackTrace);
            }

            Console.WriteLine("Done. Press any key to quit.");
            Console.ReadKey();
            ErrorLogger.WriteToFile("migration error log.txt");
        }
    }
}
