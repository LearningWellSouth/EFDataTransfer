using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataTransfer
{
    public class Program
    {
        // Dropbox/CV/EF Arbetskatalog/Migrering/App
        static void Main(string[] args)
        {
            var transferrer = new Transfer();

            try
            {
                //transferrer.DeleteTable("Issues");
                //transferrer.DeleteTable("Invoices");

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

                ////Console.ReadKey();

                //transferrer.Settings();

                //Console.WriteLine("Updating postalcodes in old table");
                //transferrer.FixPostalCodes();

                //transferrer.DeleteTable("CleaningObjects");
                ////transferrer.TruncateTable("CleaningObjects");
                //transferrer.DeleteTable("Customers");
                //transferrer.DeleteTable("PersonPostalAddressModels");
                //transferrer.DeleteTable("PostalAddressModels");

                //Console.WriteLine("Transferring persons...");
                //transferrer.DeleteTable("Persons");
                //transferrer.Persons();

                //Console.WriteLine("Transferring addresses...");
                //transferrer.Addresses();

                ////// Om hemadresser inte kommit över, kör detta:
                //////--insert into putsa_db.dbo.PersonPostalAddressModels (PostalAddressModelId, PersonId, [Type])
                //////--select distinct PostalAddressModelId, PersonId, 4
                //////--from eriks_migration.dbo.TW_clientaddresses twcad
                //////--JOIN putsa_db.dbo.PostalCodeModels pcm ON twcad.postalcode_fixed = pcm.PostalCode
                //////--JOIN putsa_db.dbo.PostalAddressModels pam ON pcm.Id = pam.PostalCodeModelId
                //////--JOIN putsa_db.dbo.PersonPostalAddressModels ppam ON ppam.PostalAddressModelId = pam.Id
                //////--join putsa_db.dbo.Persons p on p.Id = ppam.PersonId
                //////--WHERE deleted = 'N' and is_invoice = 'Y'

                //Console.WriteLine("Transferring contacts...");
                //transferrer.DeleteTable("Contacts");
                //transferrer.Contacts();

                //// Verkar som att kontakter för objekt utan kontakter inte kommer över, trots att frågan körs... Om detta är fallet, kör följande manuellt:
                ////    INSERT INTO putsa_db.dbo.Contacts (RUT, InvoiceReference, Notify, PersonId, CleaningObjectId)
                ////    SELECT 1.0, 0, 1, p.Id, co.Id
                ////    FROM putsa_db.dbo.Persons p
                ////    JOIN putsa_db.dbo.Customers cust ON cust.PersonId = p.Id
                ////    JOIN putsa_db.dbo.CleaningObjects co ON co.CustomerId = cust.Id
                ////    WHERE co.Id NOT IN (
                ////        SELECT CleaningObjectId FROM putsa_db.dbo.Contacts
                ////)

                //Console.WriteLine("Setting RUT");
                //transferrer.SetRUT();

                //Console.WriteLine("Connecting customers and cleaning objects...");
                //transferrer.ConnectCustomersToCleaningObjects();

                //Console.WriteLine("Transferring banks and connecting to customers");
                //transferrer.DeleteTable("Banks");
                //transferrer.TransferAndConnectBanks();

                //Console.WriteLine("Transferring schedules and periods...");
                //transferrer.DeleteTable("Periods");
                //transferrer.SchedulesAndPeriods();

                // Kontrollera ovanstående mot postnummer-område.xlsx

                //Console.WriteLine("Transferring users...");
                //transferrer.Employees();

                //Console.WriteLine("Transferring workers...");
                //transferrer.Workers();

                //Console.WriteLine("Transferring vehicles...");
                //transferrer.DeleteTable("Teams");
                //transferrer.DeleteTable("Vehicles");
                //transferrer.Vehicles();

                //Console.WriteLine("Updating vehicles...");
                //transferrer.UpdateVehicles();

                //Console.WriteLine("Creating teams and connecting to Vehicles...");
                //transferrer.CreateTeamsAndConnectToVehicles();

                //Console.WriteLine("Connecting workers to teams...");
                //transferrer.ConnectWorkersToTeams();

                //Console.WriteLine("Creating team users...");
                //transferrer.CreateTeamUsers();

                //Console.WriteLine("Connecting teams to cleaning objects...");
                //transferrer.ConnectTeamsToCleaningObjects();

                // Samtliga putsobjekt som inte får teamId av ovanstående har i TW-tabellen workarea_id = 0, vilket jag antar betyder att de inte är kopplade

                //transferrer.DeleteTable("Accounts");
                //Console.WriteLine("Transferring accounts");
                //transferrer.Accounts();

                //Console.WriteLine("Transferring subcategories");
                //transferrer.DeleteTable("SubCategories");
                //transferrer.SubCategories();
                //Console.WriteLine("Transferring services");
                //transferrer.DeleteTable("Services");
                //transferrer.Services();
                ////////////Console.ReadKey();

                //transferrer.DeleteTable("Subscriptions");
                //Console.WriteLine("Transferring subscriptions...");
                //transferrer.Subscriptions();

                //Console.WriteLine("Transferring occasions...");
                //transferrer.DeleteTable("SubscriptionServices");
                //transferrer.SubscriptionServices();

                //Console.WriteLine("Transferring subscription prices...");
                //transferrer.DeleteTable("CleaningObjectPrices");
                //transferrer.SubscriptionPrices();

                //Console.WriteLine("To set RUT, check TW_clients.full_reduction_pot and TW_clients.taxreduction_percentage and update manually. ");
                //Console.WriteLine("If full_reduction_pot == 0 then check percentage, if percentage == 0 then RUT == 100%");
                //Console.WriteLine("Else if full_reduction_pot == 2 then RUT == 0");
                //Console.WriteLine("Else if full_reduction_pot == 1 then RUT should be activated after years end (new feature)");

                //// Om alla putsobjekt inte får arbetslag kopplade: 
                ////UPDATE CleaningObjects SET TeamId = t.Id
                ////FROM eriks_migration.dbo.TW_workareas wa 
                ////JOIN eriks_migration.dbo.TW_clientaddresses ca on wa.id = ca.workarea_id 
                ////JOIN eriks_migration.dbo.TW_resources res ON res.id = wa.resource_id
                ////JOIN putsa_db.dbo.Vehicles v ON v.Notes = res.name
                ////JOIN putsa_db.dbo.Teams t ON t.VehicleId = v.Id
                ////JOIN CleaningObjects co ON co.Id = ca.id
                ////WHERE ca.deleted = 'N'
                ////AND wa.deleted = 'N'
                ////AND ca.postalcode_fixed IS NOT NULL

                //transferrer.UtilityTables();

                // TODO:
                // Ta med kunder med deleted = 'Y' och sätt som inactive
                //transferrer.DeleteTable("Issues");
                //Console.WriteLine("Transferring notes and issues");
                //transferrer.NotesAndIssues();

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
