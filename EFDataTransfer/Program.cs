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
                // ////     Management/Import / Restore, välj "self-enclosed file"(andra alternativet), inte från mapp 
                // ////     Välj rätt schema i dropdownen
                // ////c. Om ny SQL-server:
                // ////c.1. Kör Sql Server Migration Assistant for MySQL för att lägga över db-schemat från MySql-db till MSSQL
                // ////c.2. Högerklicka på resulterande schema i SQL Server Metadata Explorer-fönstret i SSMA, välj "Save as script"
                // ////c.3. Modifiera scriptet så att det använder rätt databas och har rätt kolumnnamn (ska börja på TW_) - gör med "Find and replace", scriptet är väldigt långt

                // ////I clients, invoicelines, issues kan ctime behöva sättas till NULL där det har värdet '0000-00-00 00:00:00'.
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
          var ErrorLogger = new Logger();
            try
            {
              var transferrer = new Transfer(ErrorLogger);
              var allTables = new List<tableProperty>();
                
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

                ////Tables with transfers
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
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Teams", truncFlag = false, transferData = true }); // Also connects schedules to postalcodes
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Accounts", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SubCategories", truncFlag = false, transferData = true });


                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Services", truncFlag = false, transferData = true });

                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "ServiceGroups", truncFlag = false, transferData = false });       //Connected to Subscriptions
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Prices", truncFlag = true, transferData = false });//Connected to Subscriptions
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Subscriptions", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderServices", truncFlag = true, transferData = false });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SubscriptionServices", truncFlag = false, transferData = true });


                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "CleaningObjectPrices", truncFlag = true, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Users", truncFlag = false, transferData = true }); // Connected to workers

                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Issues", truncFlag = true, transferData = true });


                transferrer.CreateUsersForEmployees();
                foreach (tableProperty curTable in allTables)
                {
                    //Töm tabellen först
                        if (curTable.truncFlag == true) // TODO : this switching is unneccessary. Use delete statement for all or "truncate cascaded"
                    {
                        ErrorLogger.PostInfo(string.Format("truncating {0}...", curTable.tableName));
                        transferrer.TruncateTable(curTable.tableName);
                    }
                    else
                    {
                        ErrorLogger.PostInfo(string.Format("Deleting all records in {0}...", curTable.tableName));
                        transferrer.DeleteAllRowsInTable(curTable.refTable, curTable.refFieldToClean, curTable.tableName);
                    }
                    //Kör transfer
    
                if (curTable.transferData == true)
                    {
                        Console.WriteLine("Transferring {0}...", curTable.tableName);
                        transferrer.TransferData(curTable.tableName);
                    }
                }

                // Om alla putsobjekt inte får arbetslag kopplade: 
                transferrer.FixCleaningObjectsWithUnconnectedTeams();
                transferrer.FixMoreCleaningObjectsWithUnconnectedTeams();

                //TODO: Schedules to cleaning object, so literally the note below says "have lost track of the sequence in which data is added".More the reason to create separate script files to move sql code out of the application
                //// Denna ligger utanför resten för att:
                //// A: Det verkar som att den inte körts när SchedulesAndPeriods() körts, eller behöver data som tillkommer senare i flödet, samt
                //// 2: Vi vill kunna köra den utan att också behöva kommentera in Schedules i denna fil, kommentera ut SchedulesAndPeriods() i andra filen, och till sist kommentera ut delete/trunc-partierna i denna fil
                transferrer.AddSchedulesToCleaningObjectsWithout();


                //// Om hemadresser inte kommit över, kör detta:
                ////--insert into " + dbCurrentDB + ".dbo.PersonPostalAddressModels (PostalAddressModelId, PersonId, [Type])
                ////--select distinct PostalAddressModelId, PersonId, 4
                ////--from eriks_migration.dbo.TW_clientaddresses twcad
                ////--JOIN " + dbCurrentDB + ".dbo.PostalCodeModels pcm ON twcad.postalcode_fixed = pcm.PostalCode
                ////--JOIN " + dbCurrentDB + ".dbo.PostalAddressModels pam ON pcm.Id = pam.PostalCodeModelId
                ////--JOIN " + dbCurrentDB + ".dbo.PersonPostalAddressModels ppam ON ppam.PostalAddressModelId = pam.Id
                ////--join " + dbCurrentDB + ".dbo.Persons p on p.Id = ppam.PersonId
                ////--WHERE deleted = 'N' and is_invoice = 'Y'

                // Om kontakter för objekt utan kontakter inte kommer över, kör följande manuellt:
                //    INSERT INTO " + dbCurrentDB + ".dbo.Contacts (RUT, InvoiceReference, Notify, PersonId, CleaningObjectId)
                //    SELECT 1.0, 0, 1, p.Id, co.Id
                //    FROM Persons p
                //    JOIN Customers cust ON cust.PersonId = p.Id
                //    JOIN CleaningObjects co ON co.CustomerId = cust.Id
                //    WHERE co.Id NOT IN (
                //        SELECT CleaningObjectId FROM Contacts
                //    )

                /* Har inte tagit med användare i min förändrade lösning
                 * Tänker att de ska vara oförändrade som de är idag eller läggas upp manuellt
                 * i det nya systemet
                 */
                /*
               Console.WriteLine("To set RUT, check TW_clients.full_reduction_pot and TW_clients.taxreduction_percentage and update manually. ");
               Console.WriteLine("If full_reduction_pot == 0 then check percentage, if percentage == 0 then RUT == 100%");
               Console.WriteLine("Else if full_reduction_pot == 2 then RUT == 0");
               Console.WriteLine("Else if full_reduction_pot == 1 then RUT should be activated after years end (new feature)");
               */


                transferrer.FixRUT();

                transferrer.AddContactsWhereMissing();
                transferrer.SetPostalCodeScheduleIds();

                transferrer.UtilityTables();

                Console.WriteLine("Merging subscriptions");

                transferrer.MergeSubscriptions();

                transferrer.SetBasePriceAndInactive();

                /////// Nytt 2015-11-13
                transferrer.SetAdminFees();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message+"\n\r"+ex.StackTrace;
                ErrorLogger.PostError(errorMessage);
                Console.WriteLine(errorMessage);
            }

            Console.WriteLine("Done. Press any key to quit.");
            Console.ReadKey();
            ErrorLogger.WriteToFile("migration error log.txt");
        }
    }
}
