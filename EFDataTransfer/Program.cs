﻿using System;
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
        // Dropbox/CV/EF Arbetskatalog/Migrering/App
        static void Main(string[] args)
        {
            var transferrer = new Transfer();

            try
            {

                List<tableProperty> allTables = new List<tableProperty>();

                //tables with no transfer or subtables for transfers
                /*allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "BGMAXPaymentInvoiceConnections", truncFlag = true, transferData = false });
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
                allTables.Add(new tableProperty() {refTable="", refFieldToClean = "", tableName = "UsedTaxReductionRequestNumbers", truncFlag=true, transferData=false });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Periods", truncFlag = true, transferData = false }); //Connected to Schedules
                allTables.Add(new tableProperty() { refTable = "PostalCodeModels", refFieldToClean = "ScheduleId", tableName = "Schedules", truncFlag = false, transferData = true }); // Kontrollera mot postnummer-område.xlsx
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Workers", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "VehicleHistories", truncFlag = true, transferData = false });
                allTables.Add(new tableProperty() { refTable = "Teams", refFieldToClean = "VehicleId", tableName = "Vehicles", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "Users", refFieldToClean = "TeamId", tableName = "Teams", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Accounts", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SubCategories", truncFlag = false, transferData = true });

                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Services", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "ServiceGroups", truncFlag = false, transferData = false });//Connected to Subscriptions
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Prices", truncFlag = true, transferData = false });//Connected to Subscriptions
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Subscriptions", truncFlag = false, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "SubscriptionServices", truncFlag = true, transferData = true });
                allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "CleaningObjectPrices", truncFlag = true, transferData = true });*/
                //allTables.Add(new tableProperty() { refTable = "Issues", refFieldToClean = "CreatorId", tableName = "Users", truncFlag = false, transferData = true });

                //PÅ väg.. vi tar inte den i första rundan
                //allTables.Add(new tableProperty() { refTable = "", refFieldToClean = "", tableName = "Issues", truncFlag = true, transferData = true });
              
 
               

                foreach (tableProperty curTable in allTables)
                {
                    //Töm tabellen först
                    if (curTable.truncFlag==true)
                    {
                        Console.WriteLine("truncating {0}...",curTable.tableName);
                        transferrer.TruncateTable(curTable.tableName);

                    }
                    else
                    {
                        Console.WriteLine("Deleting all records in {0}...", curTable.tableName);
                        transferrer.DeleteTable(curTable.refTable,curTable.refFieldToClean, curTable.tableName);
                    }
                    //Kör transfer
                    
                    if(curTable.transferData==true)
                    {
                        Console.WriteLine("Transferring {0}...", curTable.tableName);
                        transferrer.TransferData(curTable.tableName);
                    }
                }

                ////Console.WriteLine("TODO: fixa så att kunder får hemadresser!!!");
                ////return;

                ////Console.WriteLine("Waiting for manual transfer of postal code file to database. When ready, copy, modify and run this script: ");
                ////Console.WriteLine("SET IDENTITY_INSERT PostalCodeModels ON");
                ////Console.WriteLine("INSERT INTO PostalCodeModels (Id, PostalCode, PostalCodeType, StreetNoLowest, StreetNoHighest, City, TypeOfPlacement, StateCode,");
                ////Console.WriteLine("[State], MunicipalityCode, Municipality, ParishCode, Parish, City2, GateLowest,GateHighest,IsNotValid, PostalAddress)");
                ////Console.WriteLine("SELECT DISTINCT [Column 17], [Column 6], [Column 0], [Column 2], [Column 4], [Column 7], [Column 8], [Column 9], ");
                ////Console.WriteLine("[Column 10], [Column 11], [Column 12], [Column 13], [Column 14], [Column 15], [Column 3], [Column 5], 0, [Column 1]");
                ////Console.WriteLine("FROM [2013-09-18 Gatuadresser-postnummer]");
                ////Console.WriteLine("SET IDENTITY_INSERT PostalCodeModels OFF");
                ////Console.WriteLine("Press any key to continue.");

 
                ////Console.ReadKey();

 
                ////// Om hemadresser inte kommit över, kör detta:
                //////--insert into " + dbCurrentDB + ".dbo.PersonPostalAddressModels (PostalAddressModelId, PersonId, [Type])
                //////--select distinct PostalAddressModelId, PersonId, 4
                //////--from eriks_migration.dbo.TW_clientaddresses twcad
                //////--JOIN " + dbCurrentDB + ".dbo.PostalCodeModels pcm ON twcad.postalcode_fixed = pcm.PostalCode
                //////--JOIN " + dbCurrentDB + ".dbo.PostalAddressModels pam ON pcm.Id = pam.PostalCodeModelId
                //////--JOIN " + dbCurrentDB + ".dbo.PersonPostalAddressModels ppam ON ppam.PostalAddressModelId = pam.Id
                //////--join " + dbCurrentDB + ".dbo.Persons p on p.Id = ppam.PersonId
                //////--WHERE deleted = 'N' and is_invoice = 'Y'

 
                //// Verkar som att kontakter för objekt utan kontakter inte kommer över, trots att frågan körs... Om detta är fallet, kör följande manuellt:
                ////    INSERT INTO " + dbCurrentDB + ".dbo.Contacts (RUT, InvoiceReference, Notify, PersonId, CleaningObjectId)
                ////    SELECT 1.0, 0, 1, p.Id, co.Id
                ////    FROM " + dbCurrentDB + ".dbo.Persons p
                ////    JOIN " + dbCurrentDB + ".dbo.Customers cust ON cust.PersonId = p.Id
                ////    JOIN " + dbCurrentDB + ".dbo.CleaningObjects co ON co.CustomerId = cust.Id
                ////    WHERE co.Id NOT IN (
                ////        SELECT CleaningObjectId FROM " + dbCurrentDB + ".dbo.Contacts
                ////)

 
                /* Har inte tagit med användare i min förändrade lösning
                 * Tänker att de ska vara oförändrade som de är idag eller läggas upp manuellt
                 * i det nya systemet
                 * 
                 */

 

                Console.WriteLine("To set RUT, check TW_clients.full_reduction_pot and TW_clients.taxreduction_percentage and update manually. ");
                Console.WriteLine("If full_reduction_pot == 0 then check percentage, if percentage == 0 then RUT == 100%");
                Console.WriteLine("Else if full_reduction_pot == 2 then RUT == 0");
                Console.WriteLine("Else if full_reduction_pot == 1 then RUT should be activated after years end (new feature)");

                //// Om alla putsobjekt inte får arbetslag kopplade: 
                ////UPDATE CleaningObjects SET TeamId = t.Id
                ////FROM eriks_migration.dbo.TW_workareas wa 
                ////JOIN eriks_migration.dbo.TW_clientaddresses ca on wa.id = ca.workarea_id 
                ////JOIN eriks_migration.dbo.TW_resources res ON res.id = wa.resource_id
                ////JOIN " + dbCurrentDB + ".dbo.Vehicles v ON v.Notes = res.name
                ////JOIN " + dbCurrentDB + ".dbo.Teams t ON t.VehicleId = v.Id
                ////JOIN CleaningObjects co ON co.Id = ca.id
                ////WHERE ca.deleted = 'N'
                ////AND wa.deleted = 'N'
                ////AND ca.postalcode_fixed IS NOT NULL

                //transferrer.UtilityTables();

                // TODO:
                // Ta med kunder med deleted = 'Y' och sätt som inactive

 
                Console.WriteLine("Merging subscriptions");

                transferrer.MergeSubscriptions();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done. Press any key to quit.");
            Console.ReadKey();
        }
    }
}
