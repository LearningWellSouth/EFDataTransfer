﻿using System;
using System.Collections.Generic;
using System.IO;
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
        public string refTable { get; set; }
        public string refFieldToClean { get; set; }
    }

    public class Program
    {
      private const string PATH_TO_SQL_SCRIPTS = @"..\..\..\";
        static void Main(string[] args)
        {
          var logger = new Logger();
            try
            {
              var transferHandler = new Transfer(logger);
              var tablesToMigrate = new List<tableProperty>();

              // TODO : these tables have no meaning since no data is transfered from old system to new. After the change in filosophy of actually recreating the database on migration. The delete is no longer needed
              /*
               * allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "BGMAXPaymentInvoiceConnections",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderResources",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderResourceWorkers",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderTimeReports",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "Invoices", refFieldToClean = "WorkOrderId", tableName = "Workorders",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "InvoiceRows", refFieldToClean = "TransactionId", tableName = "Transactions",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "TransactionValues",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "IssueHistories",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SingleCleanings",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SingleCleaningServices",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Deviations",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "InvoiceContacts",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "InvoiceRows",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "InvoiceId", tableName = "Invoices",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "CleaningObjectId", tableName = "CleaningObjects",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "PersonPostalAddressModels",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Periods",  transferData = false }); //Connected to Schedules
            allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "WorkOrderServices",  transferData = false });
            allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Prices",  transferData = false });//Connected to Subscriptions
            allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "ServiceGroups",  transferData = false });       //Connected to Subscriptions
            allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "VehicleHistories",  transferData = false });
            allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "UsedTaxReductionRequestNumbers",  transferData = false });
               allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SystemLogs",  transferData = false });*/
              tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Settings" });
            tablesToMigrate.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "CustomerId", tableName = "Persons" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "PostalAddressModels" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Contacts" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Customers" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Banks" });
            tablesToMigrate.Add(new tableProperty() { refTable = "PostalCodeModels", refFieldToClean = "ScheduleId", tableName = "Schedules" });  // Kontrollera mot postnummer-område.xlsx
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Workers" });
            tablesToMigrate.Add(new tableProperty() { refTable = "Teams", refFieldToClean = "VehicleId", tableName = "Vehicles" });
            tablesToMigrate.Add(new tableProperty() { refTable = "Users", refFieldToClean = "TeamId", tableName = "Teams" }); // Also connects schedules to postalcodes
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Accounts" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SubCategories" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Services" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Subscriptions" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SubscriptionServices" });
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "CleaningObjectPrices" });
            ////////////allTables.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "CreatorId", tableName = "Users" }); // Connected to workers
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////Kopplingen av arbetslag till användare funkar inte, löses manuellt
            tablesToMigrate.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Issues" });

            transferHandler.ExecuteScriptFile(PATH_TO_SQL_SCRIPTS + "baseline_architecture.sql");
            transferHandler.ExecuteScriptFile(PATH_TO_SQL_SCRIPTS + "InsertPostalNumbers.sql");
            transferHandler.ExecuteScriptFile(PATH_TO_SQL_SCRIPTS + "InitialMigration.sql");


            foreach (var table in tablesToMigrate)
            {
                logger.PostNote(string.Format("Transferring {0}...",table.tableName));
                transferHandler.TransferData(table.tableName);
            }
                
            //// Om alla putsobjekt inte får arbetslag kopplade: 
            transferHandler.FixCleaningObjectsWithUnconnectedTeams();
            transferHandler.FixMoreCleaningObjectsWithUnconnectedTeams();
 
            // TODO : Schedules to cleaning object, so literally the note below says "have lost track of the sequence in which data is added". More the reason to create separate script files to move sql code out of the application
            ////// Denna ligger utanför resten för att:
            ////// A: Det verkar som att den inte körts när SchedulesAndPeriods() körts, eller behöver data som tillkommer senare i flödet, samt
            ////// 2: Vi vill kunna köra den utan att också behöva kommentera in Schedules i denna fil, kommentera ut SchedulesAndPeriods() i andra filen, och till sist kommentera ut delete/trunc-partierna i denna fil
            transferHandler.AddSchedulesToCleaningObjectsWithout();

                ////// Om hemadresser inte kommit över, kör detta:
                //////--insert into " + dbCurrentDB + ".dbo.PersonPostalAddressModels (PostalAddressModelId, PersonId, [Type])
                //////--select distinct PostalAddressModelId, PersonId, 4
                //////--from eriks_migration.dbo.TW_clientaddresses twcad
                //////--JOIN " + dbCurrentDB + ".dbo.PostalCodeModels pcm ON twcad.postalcode_fixed = pcm.PostalCode
                //////--JOIN " + dbCurrentDB + ".dbo.PostalAddressModels pam ON pcm.Id = pam.PostalCodeModelId
                //////--JOIN " + dbCurrentDB + ".dbo.PersonPostalAddressModels ppam ON ppam.PostalAddressModelId = pam.Id
                //////--join " + dbCurrentDB + ".dbo.Persons p on p.Id = ppam.PersonId
                //////--WHERE deleted = 'N' and is_invoice = 'Y'
 
                //// Om kontakter för objekt utan kontakter inte kommer över, kör följande manuellt:
                ////    INSERT INTO " + dbCurrentDB + ".dbo.Contacts (RUT, InvoiceReference, Notify, PersonId, CleaningObjectId)
                ////    SELECT 1.0, 0, 1, p.Id, co.Id
                ////    FROM Persons p
                ////    JOIN Customers cust ON cust.PersonId = p.Id
                ////    JOIN CleaningObjects co ON co.CustomerId = cust.Id
                ////    WHERE co.Id NOT IN (
                ////        SELECT CleaningObjectId FROM Contacts
                ////    )
 
                /* Har inte tagit med användare i min förändrade lösning
                 * Tänker att de ska vara oförändrade som de är idag eller läggas upp manuellt
                 * i det nya systemet
                 */
                //Console.WriteLine("To set RUT, check TW_clients.full_reduction_pot and TW_clients.taxreduction_percentage and update manually. ");
                //Console.WriteLine("If full_reduction_pot == 0 then check percentage, if percentage == 0 then RUT == 100%");
                //Console.WriteLine("Else if full_reduction_pot == 2 then RUT == 0");
                //Console.WriteLine("Else if full_reduction_pot == 1 then RUT should be activated after years end (new feature)");
                

            transferHandler.FixRUT();

            transferHandler.AddContactsWhereMissing();
            transferHandler.SetPostalCodeScheduleIds();

            transferHandler.UtilityTables();

            Console.WriteLine("Merging subscriptions");

            transferHandler.MergeSubscriptions();

            transferHandler.SetBasePriceAndInactive();

            transferHandler.SetAdminFees();
            }
            catch (Exception ex)
            {
              var errorMessage = ex.Message+@"\n\r"+ex.StackTrace;
              logger.PostError(errorMessage);
              Console.WriteLine(errorMessage);
            }

            Console.WriteLine("Done. Press any key to quit.");
            Console.ReadKey();
            logger.WriteToFile("migration error log.txt");
        }
    }
}
