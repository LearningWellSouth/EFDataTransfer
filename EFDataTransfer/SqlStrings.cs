using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataTransfer
{
    public static class SqlStrings
    {
        #region Insert

        public static string CreateTeamsAndConnectToVehicles
        {
            get
            {
                return @"
                    INSERT INTO putsa_db.dbo.Teams (Name, WorkerLimit, VehicleId)
                    SELECT name, 2, id FROM eriks_migration.dbo.TW_resources
                ";
            }
        }

        public static string InsertAccounts
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Accounts ON
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (1, 'Intäkter abonnemang', 3001, 'Intäkter abonnemang', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (2, 'Kundfordran', 1510, 'Kundfordran', 1);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (3, 'Bank', 1930, 'Bank', 2);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (4, 'Fordran skatteverket', 1513, 'Fordran skatteverket', 1);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (5, 'Utgående moms', 2611, 'Utgående moms', 3);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (6, 'Öres- och kronutjämning', 3740, 'Öres- och kronutjämning', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (7, 'Intäkter engångsputs', 3003, 'Intäkter engångsputs', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (8, 'Intäkter alla sidor', 3002, 'Intäkter alla sidor', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (9, 'Intäkt körersättning', 3009, 'Intäkt körersättning', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (10, 'Administrativa', 3540, 'Administrativa', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (11, 'Lämnade rabatter', 3730, 'Lämnade rabatter', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (12, 'Påminnelseavgift', 3930, 'Påminnelseavgift', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (14, 'Special', 0, NULL, 0);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (15, 'Kassa', 1910, 'Kassa', 2);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (16, 'PlusGiro', 1920, 'PlusGiro', 2);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (17, 'Förskott (skuld)', 2420, 'Förskott (skuld)', 2);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (18, 'Presentkort (skuld)', 2421, 'Presentkort (skuld)', 2);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (19, 'Övrig försäljning', 3051, 'Övrig försäljning', 4);
                    INSERT INTO putsa_db.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
                        VALUES (20, 'Lämnade Kampanjrabatter', 3732, NULL, 4);
                    SET IDENTITY_INSERT putsa_db.dbo.Accounts OFF
                ";
            }
        }

        public static string InsertIntoPostalCodeModels(
            string postalCode, string postalCodeType, string address, string streetNoLowest, string streetNoHighest, string city, string typeOfPlacement)
        {
            return string.Format(@"INSERT INTO putsa_db.dbo.PostalCodeModels (PostalCode, PostalCodeType, PostalAddress, StreetNoLowest, StreetNoHighest, City, TypeOfPlacement, IsNotValid)
                OUTPUT INSERTED.Id VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', 1)", 
                postalCode, postalCodeType, address, streetNoLowest, streetNoHighest, city, typeOfPlacement);
        }

        public static string InsertIntoSchedules(int num)
        {
            return string.Format("INSERT INTO putsa_db.dbo.Schedules (Name, _ValidFrom, _ValidTo) OUTPUT INSERTED.Id VALUES ('Schema {0}', GETDATE(), GETDATE())", num);
        }

        public static string InsertIntoServiceGroups
        {
            get
            {
                return string.Format("INSERT INTO putsa_db.dbo.ServiceGroups ([From], [To], Name) OUTPUT INSERTED.Id VALUES ('{0}', '{1}', '2015')", 
                    DateTime.Now.Year.ToString() + "-1-1", DateTime.Now + new TimeSpan(365, 0, 0, 0));
            }
        }

        public static string InsertPostalAddressModel(string streetNo, int postalCodeModelId, string addressType, string address2, float longitude, float latitude)
        {
            return string.Format(new System.Globalization.CultureInfo("en-US"),
                "INSERT INTO putsa_db.dbo.PostalAddressModels (StreetNo, PostalCodeModelId, AddressType, Address2, Longitude, Latitude) OUTPUT INSERTED.Id VALUES ('{0}', {1}, '{2}', '{3}', {4}, {5})",
                streetNo, postalCodeModelId, addressType, address2, longitude, latitude);
        }

        public static string InsertSubCategories
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.SubCategories ON
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(1, 'Tillägg', 'Övriga', 6);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(2, 'Tillval', 'Generella Tillval', 2);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(3, 'Tillval', 'Uterum', 2);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(4, 'Tillval', 'Övervåning', 2);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(5, 'Tillval', 'Källarvåning', 2);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(6,'Övrigt', 'Extra', 4);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(7, 'Övrigt', 'Alla sidor', 4);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(8, 'Övrigt', 'Övrig Ekonomi', 4);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(9, 'Övrigt', 'Textrad', 4);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(10, 'Grundpris', 'Grundputsning', 1);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(11, 'Övrigt', 'Admin o utkörning', 7);
                    INSERT INTO putsa_db.dbo.SubCategories (Id, [Type], Name, Category)
                        VALUES(12, 'Övrigt', 'Övrigt', 4);
                    SET IDENTITY_INSERT putsa_db.dbo.SubCategories OFF
                ";
            }
        }

        public static string TransferClients
        {
            get
            {
                return @"SET IDENTITY_INSERT putsa_db.dbo.Persons ON
                    INSERT INTO putsa_db.dbo.Persons (Id, PersonalNo, FirstName, LastName, WorkPhone, PrivatePhone, Email, MobilePhone, PersonType, CompanyName, NoPersonalNoValidation)
                    SELECT DISTINCT id, persnbr, firstname, lastname, workphone, phone, email, mobile, 
                        CASE WHEN companyname IS NOT NULL OR clienttype_id = 2 THEN 2 ELSE 1 END,
                        companyname,
                        CASE WHEN persnbr = '' THEN 1 ELSE 0 END
                    FROM eriks_migration.dbo.TW_clients WHERE deleted = 'N'
                    SET IDENTITY_INSERT putsa_db.dbo.Persons OFF";
            }
        }

        public static string TransferContacts
        {
            get
            {
                return @"
                    INSERT INTO putsa_db.dbo.Contacts (PersonId, CleaningObjectId, RUT, InvoiceReference, Notify)
                    SELECT clo.id AS PersonId, cla.id AS CleaningObjectId, 1 AS RUT, 0 AS InvoiceReference, 1 AS Notify
                    FROM eriks_migration.dbo.TW_clients cli
                    JOIN eriks_migration.dbo.TW_clients clo ON cli.id = clo.mother_id
                    JOIN eriks_migration.dbo.TW_clientaddresses cla ON cla.client_id = cli.id
                    WHERE --cla.is_delivery = 'Y' AND 
                    cli.deleted = 'N'
                    AND clo.deleted = 'N'
                    AND cla.deleted = 'N'
                    AND EXISTS (SELECT Id FROM putsa_db.dbo.CleaningObjects WHERE Id = cla.id)";
            }
        }

        public static string InsertContactsWhereNeeded
        {
            get
            {
                return @"
                    INSERT INTO putsa_db.dbo.Contacts (RUT, InvoiceReference, Notify, PersonId, CleaningObjectId)
                    SELECT 1.0, 0, 1, p.Id, co.Id
                    FROM putsa_db.dbo.Persons p
                    JOIN putsa_db.dbo.Customers cust ON cust.PersonId = p.Id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.CustomerId = cust.Id
                    WHERE co.Id NOT IN (
	                    SELECT CleaningObjectId FROM putsa_db.dbo.Contacts
                )";
            }
        }

        public static string TransferCustomers
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Customers ON
                    INSERT INTO putsa_db.dbo.Customers (Id, PersonId, IsInactive, IsInvoicable, InvoiceMethod, IsCreditBlocked, PaymentTerms)
                    SELECT DISTINCT clientnbr AS Id, TW_clients.Id AS PersonId, 0 AS IsInactive, 1 AS IsInvoicable, 
                        CASE WHEN TW_clients.paymenttype = 4 THEN 1 WHEN TW_clients.paymenttype = 7 THEN 2 WHEN TW_clients.paymenttype = 2 THEN 0 ELSE 3 END AS InvoiceMethod, 
                        0 AS IsCreditBlocked, paymentterms
                    FROM eriks_migration.dbo.TW_clients
                    INNER JOIN eriks_migration.dbo.TW_clientaddresses cli ON TW_clients.id = cli.client_id
                    WHERE is_invoice = 'Y' AND eriks_migration.dbo.TW_clients.deleted = 'N' AND mother_id = 0
                    SET IDENTITY_INSERT putsa_db.dbo.Customers OFF
                ";
            }
        }

        public static string TransferEmployees
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Users ON
                    INSERT INTO putsa_db.dbo.Users (Id, Username, Permissions)
                    SELECT id, CONCAT(firstname, ' ', lastname), CASE WHEN role_id = 1 THEN 96 ELSE 63 END
                    FROM eriks_migration.dbo.TW_employees
                    WHERE deleted = 'N'
                    SET IDENTITY_INSERT putsa_db.dbo.Users OFF
                ";
            }
        }

        public static string TransferServices
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.[Services] ON
                    INSERT INTO putsa_db.dbo.[Services] (Id, Category, Name, AccountId, RUT, Vat, CalcHourly, CalcPercentage, CalcArea, CalcNone, [From], Circa,
                        VisibleOnInvoice, IsSalarySetting, SortOrder, IsDefault, 
                        SubCategoryId)
                    SELECT id AS Id, 
	                    CASE category_id
		                    WHEN 1 THEN 1
		                    WHEN 2 THEN 2
		                    WHEN 3 THEN 2
		                    WHEN 4 THEN 2
		                    WHEN 5 THEN 4
		                    WHEN 6 THEN 4
		                    WHEN 8 THEN 4 
		                    WHEN 9 THEN 7
		                    WHEN 10 THEN 5
		                    WHEN 11 THEN 7
		                    WHEN 12 THEN 6
	                    END AS Category, 
	                    name AS Name, 
	                    CASE account_id
		                    WHEN 1 THEN 1
		                    WHEN 2 THEN 8
		                    WHEN 3 THEN 10
		                    WHEN 4 THEN 7
		                    WHEN 6 THEN 9
		                    WHEN 13 THEN 20
		                    END AS AccountId,
	                    CASE WHEN deductible = 2 THEN 1 ELSE 0 END AS RUT,
	                    CASE WHEN vat = 25 THEN 1 ELSE 0 END AS Vat,
	                    CASE WHEN unit_id = 3 THEN 1 ELSE 0 END AS CalcHourly,
                        0 AS CalcPercentage,
                        0 AS CalcArea,
                        0 AS CalcNone,
                        0 AS [From],
                        0 AS Circa,
	                    CASE WHEN visible_invoice = 'Y' THEN 1 ELSE 0 END AS VisibleOnInvoice,
	                    CASE WHEN servicetype_id IN (1, 3) THEN 1 ELSE 0 END AS IsSalarySetting,
	                    id AS SortOrder,
	                    CASE WHEN name = 'BV - Grundpris' THEN 1 ELSE 0 END AS IsDefault,
	                    CASE 
		                    WHEN name = 'BV - Grundpris' THEN 10
		                    WHEN category_id = 1 THEN 2
		                    WHEN category_id = 2 THEN 1
		                    WHEN category_id = 3 THEN 3
		                    WHEN category_id = 4 THEN 4
		                    WHEN category_id = 5 THEN 12
		                    WHEN category_id = 6 THEN 6
		                    WHEN category_id = 8 THEN 11
		                    WHEN category_id = 9 THEN 8
		                    WHEN category_id = 10 THEN 7
		                    WHEN name = 'Textrad' THEN 9
		                    WHEN category_id = 11 THEN 8
		                    WHEN category_id = 12 THEN 8
		                    END AS SubCategoryId
	                    FROM eriks_migration.dbo.TW_services WHERE deleted = 'N'
                    SET IDENTITY_INSERT putsa_db.dbo.[Services] OFF
                ";
            }
        }

        public static string TransferVehicles
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Vehicles ON
                    INSERT INTO putsa_db.dbo.Vehicles (Id, Notes) 
                    SELECT id, name FROM eriks_migration.dbo.TW_resources
                    SET IDENTITY_INSERT putsa_db.dbo.Vehicles OFF
                ";
            }
        }

        public static string TransferWorkers
        {
            get
            {
                return @"
                    INSERT INTO putsa_db.dbo.Workers (UserId, PersonalNo, FirstName, LastName, WorkPhone, [Address], City, Zip, Email, StartDate, CurActive)
                    SELECT id, persnbr, firstname, lastname, phone, [address], city, postalcode, email, CASE WHEN ctime IS NOT NULL THEN ctime ELSE GETDATE() END, 1
                    FROM eriks_migration.dbo.TW_employees WHERE deleted = 'N' AND role_id = 1
                ";
            }
        }

        public static string TransferCustomerAddedNotes
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], [Priority], StartDate, FinishedDate, CustomerId, IssueType, CreatorId, Private)
                    SELECT n.id AS Id, header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority],
                        CASE WHEN ISDATE(content) = 1 THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate,
                        CASE WHEN ISDATE(content) = 1  THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS FinishedDate, 
                        clientnbr AS CustomerId, 
                        7 AS IssueType, n.created_by_id AS CreatorId, 0 AS Private
                    FROM eriks_migration.dbo.TW_notes n
                    JOIN eriks_migration.dbo.TW_clients cli ON n.table_id = cli.id
                    JOIN putsa_db.dbo.Customers c on c.Id = cli.clientnbr
                    WHERE table_name = 'clients'
                    AND header LIKE 'Inlagddatum'
                    AND n.deleted = 'N'
                    AND cli.deleted = 'N'
                    AND clientnbr IS NOT NULL
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF";
            }
        }

        public static string TransferEconomyNotes
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], [Priority], StartDate, IssueType, CleaningObjectId, CreatorId, Private)
                    SELECT n.id AS Id, header AS Title, content AS [Description], 
                        CASE WHEN n.deleted = 'N' AND n.closed = 'N' THEN 2 ELSE 4 END AS [Status], 
                        0 AS [Priority], 
                        CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, 
                        5 AS IssueType, wo.delivery_clientaddress_id AS CleaningObjectId, n.created_by_id AS CreatorId, 0 AS Private
                    FROM eriks_migration.dbo.TW_notes n
                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                    WHERE table_name = 'workorders' --AND tag_type IS NULL 
                    AND notetype_id = '5'
                    AND n.deleted = 'N'
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                ";
            }
        }

        public static string TransferEconomyCustomerNotes
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], [Priority], StartDate, IssueType, CustomerId, CreatorId, Private)
                    SELECT n.id AS Id, header AS Title, content AS [Description], 
	                    CASE WHEN n.deleted = 'N' AND n.closed = 'N' THEN 2 ELSE 4 END 
	                    AS [Status], 0 AS [Priority], CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, 
                        5 AS IssueType, clientnbr AS CustomerId, 
                        CASE WHEN n.created_by_id <> -1 THEN n.created_by_id ELSE NULL END AS CreatorId, 0 AS Private
                    FROM eriks_migration.dbo.TW_notes n
                    JOIN eriks_migration.dbo.TW_clients cli ON n.table_id = cli.id
                    JOIN putsa_db.dbo.Customers c ON c.Id = cli.clientnbr
                    WHERE table_name = 'clients'
                    AND notetype_id = '5'
                    AND n.deleted = 'N'
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF";
            }
        }

//        public static string TransferCleaningObjectNotes
//        {
//            get
//            {
//                return @"
//                    INSERT INTO putsa_db.dbo.Issues (Title, [Description], [Status], StartDate, CleaningObjectId, IssueType, Private, Priority)
//                    SELECT 'Anteckning' AS Title, content AS [Description], 
//                    CASE WHEN closed = 'N' THEN 2 ELSE 4 END AS [Status],
//                    CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate,
//                    wo.delivery_clientaddress_id AS CleaningObjectId,
//                    7 AS IssueType, 0 AS Private, 0 AS Priority
//                    FROM eriks_migration.dbo.TW_notes n 
//                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
//                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
//                    WHERE table_name = 'workorders' AND n.header LIKE '' AND n.important = 'N' AND n.deleted = 'N' AND content <> ''                
//                ";
//            }
//        }

        public static string TransferMoreCleaningObjectNotes
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], StartDate, CleaningObjectId, IssueType, CreatorId, Private, Priority)
                    SELECT notes.id AS Id, header AS Title, content AS [Description], 2 AS [Status], 
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    wo.delivery_clientaddress_id AS CleaningObjectId,
	                    7 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' AND notes.header LIKE '%NDRING%' AND notes.header LIKE '%TILLVAL%'
                    AND notes.deleted = 'N'
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                ";
            }
        }

        public static string TransferCustomerNotes
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, StartDate, Title, [Description], [Status], IssueType, CreatorId, CustomerId, Private, Priority)
                    SELECT notes.id AS Id,
                        CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    header AS Title, content AS [Description], 4 AS [Status], 7 AS IssueType, notes.created_by_id AS CreatorId,
	                    cli.clientnbr AS CustomerId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes  
                    JOIN eriks_migration.dbo.TW_clients AS cli on notes.table_id = cli.id
                    WHERE table_name = 'clients' AND notes.header LIKE '%NDRING%' AND notes.header LIKE '%TILLVAL%' AND notes.deleted = 'N'
                        AND notes.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)                    
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                ";
            }
        }

        public static string TransferCustomerTelephoneNotes
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], StartDate, CleaningObjectId, IssueType, Private, Priority)
                    SELECT notes.id AS Id, header AS Title, content AS [Description], 
	                    CASE WHEN notes.deleted = 'N' THEN 4 ELSE 2 END AS [Status],  
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    wo.delivery_clientaddress_id AS CleaningObjectId, 7 AS IssueType, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' AND notes.header LIKE 'telefonhistorik' AND notes.deleted = 'N'
                        AND notes.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)  
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                    ";
            }
        }

        public static string TransferCleaningObjectOrders
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], StartDate, CreatorId, IssueType, CleaningObjectId, Private, Priority)
                    SELECT notes.id AS Id, notes.header AS Title, content AS [Description], 2 AS [Status],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    created_by_id AS CreatorId, 2 AS IssueType, wo.delivery_clientaddress_id AS CleaningObjectId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' AND notes.header LIKE 'BESTÄLLNING' AND notes.deleted = 'N'
                        AND notes.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)  
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                ";
            }
        }

        public static string TransferCleaningObjectDescriptions
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], StartDate, CreatorId, IssueType, CleaningObjectId, Private, Priority)
                    SELECT notes.id AS Id, notes.header AS Title, content AS [Description], 
	                    CASE WHEN notes.deleted ='N' THEN 2 ELSE 4 END AS [Status],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    created_by_id AS CreatorId, 7 AS IssueType, wo.delivery_clientaddress_id AS CleaningObjectId, 0 AS Private, 0 AS Priority
                     FROM eriks_migration.dbo.TW_notes AS notes 
                     JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                     WHERE table_name = 'workorders' AND notes.header LIKE 'HUSBESKRIVNING' AND notes.deleted = 'N'
                    AND notes.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)  
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                ";
             
            }
        }

        public static string TransferCustomerCancellations
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], StartDate, CustomerId, IssueType, CreatorId, Private, Priority)
                    SELECT DISTINCT notes.id AS Id, 'Uppsägning' AS Title, content AS [Description], 4 AS [Status],
	                    CASE WHEN ISDATE(content) = 1 THEN content WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    cli.clientnbr AS CustomerId, 3 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_clients AS cli ON cli.id = notes.table_id
                    WHERE table_name = 'clients' 
                    AND content <> ''
                    AND notes.header IN ('Uppsagddatum', 'UPPSÄGNING', 'UPPSAGD') AND notes.deleted = 'N'
                        AND notes.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                    ";
            }
        }

        public static string TransferCleaningObjectCancellations
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], StartDate, CleaningObjectId, IssueType, CreatorId, Private, Priority)
                    SELECT DISTINCT notes.id AS Id, 'Uppsägning' AS Title, content AS [Description], 4 AS [Status],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    wo.delivery_clientaddress_id AS CleaningObjectId, 3 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_workorders wo ON wo.id = notes.table_id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' 
                    AND content <> ''
                    AND notes.header IN ('Uppsagddatum', 'UPPSÄGNING', 'UPPSAGD') AND notes.deleted = 'N'
                    AND notes.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                ";
            }
        }

        public static string TransferEvenMoreCleaningObjectNotes
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], StartDate, CleaningObjectId, IssueType, CreatorId, Private, Priority)
                    SELECT  DISTINCT notes.id AS Id, header AS Title, content AS [Description], 2 AS [Status], 
                        CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
                        wo.delivery_clientaddress_id AS CleaningObjectId, 7 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_workorders AS wo on wo.id = notes.table_id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' AND notes.deleted = 'N'
                    AND notes.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)
                    AND notes.header IN ('%TELEFON%','%Kundhistorik%','%info_BV%','%MAIL%','%info tillval%','%info faktura%','%special%','%Anm%','%Info%')
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                    ";
            }
        }

        public static string TransferMoreCustomerNotes
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], StartDate, CustomerId, IssueType, CreatorId, Private, Priority)
                    SELECT notes.id AS Id, header AS Title, content AS [Description], 2 AS [Status], 
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    cli.clientnbr AS CustomerId, 7 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_clients AS cli ON cli.id = notes.table_id
                    JOIN putsa_db.dbo.Customers c ON c.Id = cli.clientnbr
                    WHERE table_name = 'clients'
                    AND notes.deleted = 'N'
                    AND notes.header IN ('TELEFON','Kundhistorik','info_BV','MAIL','info tillval','info faktura|special','Anm', 'Info')
                    AND notes.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                ";
            }
        }

        public static string TransferMoreCancellations
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Issues ON
                    INSERT INTO putsa_db.dbo.Issues (Id, Title, [Description], [Status], [Priority], StartDate, [Private], CleaningObjectId, IssueType)
                    SELECT n.Id AS Id, content AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority], 
                        CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, 
                        0 AS [Private], wo.delivery_clientaddress_id AS CleaningObjectId, 3 AS IssueType
                    FROM eriks_migration.dbo.TW_notes n 
                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' AND notetype_id = 1 AND n.deleted = 'N' AND important = 'Y' AND 
                    n.content LIKE '%UPPSAGD%'     AND n.id NOT IN (SELECT Id FROM putsa_db.dbo.Issues)  
                    ORDER BY table_id
                    SET IDENTITY_INSERT putsa_db.dbo.Issues OFF
                    ";
            }
        }

        public static string TransferBanks 
        { 
            get
            {
                return @"
                    SET IDENTITY_INSERT putsa_db.dbo.Banks ON
                    INSERT INTO putsa_db.dbo.Banks (Id, BankId, Name)
                    SELECT b.id AS Id, enar AS BankId, name AS Name FROM eriks_migration.dbo.TW_banks b
                    SET IDENTITY_INSERT putsa_db.dbo.Banks OFF
                ";
            }
        }

        #endregion

        #region select

        public static string SelectPostalAddressModelsBy(string streetNo, IEnumerable<int> pcmIds)
        {
            string pcmIdStr = "(";
            for (int i = 0; i < pcmIds.Count(); i++)
            {
                pcmIdStr += pcmIds.ElementAt(i).ToString();
                if (i < pcmIds.Count() - 1)
                    pcmIdStr += ", ";
                else
                    pcmIdStr += ")";
            }

            return string.Format("SELECT PostalCodeModelId FROM putsa_db.dbo.PostalAddressModels WHERE StreetNo LIKE '{0}' AND PostalCodeModelId IN {1}", streetNo, pcmIdStr);
        }

        public static string SelectAllPostalCodeModels
        {
            get
            {
                return @"SELECT Id, PostalCode, PostalAddress, StreetNoLowest, StreetNoHighest, City, TypeOfPlacement FROM putsa_db.dbo.PostalCodeModels 
                    WHERE PostalCode IN (SELECT postalcode_fixed FROM eriks_migration.dbo.TW_clientaddresses)";
            }
        }

//        public static string SelectCleaningObjectBy(int customerId, string address, int postalCode, string city, string streetNo)
//        {
//            return string.Format(
//                @"SELECT co.Id FROM CleaningObjects co
//                JOIN PostalAddressModels pam ON co.PostalAddressModelId = pam.Id
//                JOIN PostalCodeModels pcm ON pam.PostalCodeModelId = pcm.Id
//                WHERE co.CustomerId = {0}
//                AND pcm.PostalAddress = '{1}'
//                AND pcm.PostalCode = {2}
//                AND pcm.City = '{3}'
//                AND pam.StreetNo = '{4}'",
//            customerId, address, postalCode, city, streetNo);
//        }

        public static string SelectCustomerBy(int personId)
        {
            return string.Format("SELECT * FROM Customers WHERE PersonId = {0}", personId);
        }

        public static string SelectPostalCodeModelIdsBy(string address, int postalCode, string city)
        {
            return string.Format("SELECT Id FROM PostalCodeModels WHERE PostalAddress = '{0}' AND PostalCode = {1} AND City = '{2}'", address, postalCode, city);
        }

        public static string SelectSubscriptionId(int subToSetId, int cleaningObjectId)
        {
            return "SELECT Id FROM putsa_db.dbo.Subscriptions WHERE Id != " + subToSetId + " AND CleaningObjectId = " + cleaningObjectId;
        }

        public static string SelectTWAddresses
        {
            get
            {
                return "SELECT id, [address], co_address, postalcode_fixed, city, latitude, longitude, route_num, is_delivery, is_invoice, client_id, workarea_id FROM eriks_migration.dbo.TW_clientaddresses WHERE deleted = 'N'";
            }
        }

        public static string SelectTWCleaningObjectInfoBefore
        {
            get
            {
//                return @"
//                SELECT n.content AS Info, wo.delivery_clientaddress_id AS CleaningObjectId FROM eriks_migration.dbo.TW_notes n 
//                JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
//                WHERE table_name = 'workorders' AND notetype_id = 1 AND n.deleted = 'N' AND important = 'Y' AND 
//                n.header LIKE 'statusINFO' OR 
//                n.header LIKE '%KONTAKT%'
//                ORDER BY table_id
//                ";
                return @"
                    SELECT n.content AS Info, wo.delivery_clientaddress_id AS CleaningObjectId
                    FROM eriks_migration.dbo.TW_notes n 
                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' AND n.important = 'Y' AND n.deleted = 'N' AND content <> '' AND notetype_id = 1 AND n.header LIKE '' 
                        OR n.header LIKE 'uppdragsbeskrivning%'
                    ORDER BY wo.delivery_clientaddress_id
                ";
            }
        }

        public static string SelectTWCleaningObjectInfoDuring
        {
            get
            {
                return @"
                    SELECT n.content AS Info, wo.delivery_clientaddress_id AS CleaningObjectId
                    FROM eriks_migration.dbo.TW_notes n 
                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' AND n.header LIKE '' AND n.important = 'N' AND n.deleted = 'N' AND content <> '' AND notetype_id = 1
                    ORDER BY wo.delivery_clientaddress_id
                ";
            }
        }
        
        public static string SelectTWClientAddresses
        {
            get
            {
                return "SELECT id, postalcode FROM eriks_migration.dbo.TW_clientaddresses WHERE postalcode_fixed IS NULL";
            }
        }

        public static string SelectTWServices
        {
            get
            {
                return @"SELECT tws.id AS twId, s.Id AS ServiceId, price_per_unit * 1.25 AS Cost FROM eriks_migration.dbo.TW_services tws 
                        JOIN [putsa_db].[dbo].[Services] s ON s.Name = tws.name
                        WHERE tws.active = 'Y'
                        AND tws.deleted = 'N'";
            }
        }

        public static string SelectTWInterludes
        {
            get
            {
                return @"SELECT id, num, DATEPART(wk, CAST(startdate AS DATETIME)) AS weekFrom, startdate , period
                        FROM eriks_migration.dbo.TW_interludes WHERE deleted = 'N' AND occasion <> 0 ORDER BY num";
            }
        }

        public static string SelectTWWorkAreas
        {
            get
            {
                return "SELECT wa.resource_id, ca.id AS Id, interlude_num, res.name AS name FROM eriks_migration.dbo.TW_workareas wa " + 
//                return @"SELECT wa.resource_id, ca.[address], ca.postalcode_fixed, ca.city, cli.clientnbr, interlude_num FROM TW_workareas wa
                    "JOIN eriks_migration.dbo.TW_clientaddresses ca on wa.id = ca.workarea_id " +
                    //JOIN TW_clients cli ON ca.client_id = cli.id
                    @"JOIN eriks_migration.dbo.TW_resources res ON res.id = wa.resource_id
                    WHERE ca.deleted = 'N' " +
                    //AND ca.is_delivery = 'Y' " + 
                    //AND cli.deleted = 'N'
                    @"AND wa.deleted = 'N'
                    AND ca.postalcode_fixed IS NOT NULL";
            }
        }

        public static string SelectTWWorkOrders
        {
            get
            {
//                return @"
//                    SELECT wo.id AS woId, ca.id AS caId, wo.[status] FROM TW_workorders wo
//                    JOIN TW_clientaddresses ca ON wo.delivery_clientaddress_id = ca.id
//                    WHERE ca.is_delivery = 'Y'
//                    AND ca.deleted = 'N'
//                    AND wo.deleted = 'N'
//                    AND wo.[status] <> 6";
                return @"
                    SELECT wo.id AS woId, co.Id AS caId, CASE WHEN wo.[status] IN (1, 3) THEN 0 ELSE 1 END AS woIsInactive
                    FROM eriks_migration.dbo.TW_workorders wo
                    JOIN putsa_db.dbo.CleaningObjects co ON wo.delivery_clientaddress_id = co.Id
                    WHERE wo.deleted = 'N'
                    AND wo.[status] <> 6
                    ORDER BY co.Id DESC, wo.id DESC";
            }
        }

        public static string SelectTWWorkOrderLines
        {
            get
            {
                return @"SELECT wol.id AS wolId, interlude_occasion, workorder_id, s.Id AS sId FROM eriks_migration.dbo.TW_workorderlines wol
                    JOIN eriks_migration.dbo.TW_services twS ON twS.id = wol.service_id
                    JOIN putsa_db.dbo.[Services] s ON s.Id = twS.id
                    WHERE wol.deleted = 'N'
                    AND tws.deleted = 'N'
                    AND wol.interlude_num IS NOT NULL
                ";
            }
        }

        public static string SelectTWWorkOrderPrices
        {
            get
            {
                return @"SELECT s.Id AS ServiceId, wol.unit_price, twS.price_per_unit, ca.id AS caId, wol.[description] AS wolDesc FROM eriks_migration.dbo.TW_workorderlines wol
                        JOIN eriks_migration.dbo.TW_services twS ON wol.service_id = twS.id
                        JOIN putsa_db.dbo.[Services] s ON twS.id = s.Id
                        JOIN eriks_migration.dbo.TW_workorders wo ON wol.workorder_id = wo.id
                        JOIN eriks_migration.dbo.TW_clientaddresses ca ON wo.delivery_clientaddress_id = ca.id
                        WHERE --ca.is_delivery = 'Y' AND 
                        ca.deleted = 'N'
                        AND wo.deleted = 'N'
                        AND wo.[status] <> 6
                        AND postalcode_fixed IS NOT NULL
                        AND wol.interlude_num IS NOT NULL
                    ";

//                return @"SELECT cli.clientnbr, ca.[address], ca.postalcode_fixed, ca.city, s.Id AS ServiceId, wol.unit_price, twS.price_per_unit FROM TW_workorderlines wol
//                    JOIN TW_services twS ON wol.service_id = twS.id
//                    JOIN [Services] s ON twS.Name = s.Name
//                    JOIN TW_workorders wo ON wol.workorder_id = wo.id
//                    JOIN TW_clientaddresses ca ON wo.delivery_clientaddress_id = ca.id
//                    JOIN TW_clients cli ON ca.client_id = cli.id
//                    WHERE ca.is_delivery = 'Y'
//                    AND cli.deleted = 'N'
//                    AND ca.deleted = 'N'
//                    AND wo.deleted = 'N'
//                    AND wo.[status] <> 6
//                    AND postalcode_fixed IS NOT NULL";
            }
        }

        #endregion

        #region update

        public static string ConnectBanksToCustomers
        {
            get
            {
                return @"
                    UPDATE putsa_db.dbo.Customers SET BankId = X.BankId FROM (
	                    SELECT clientnbr, CASE WHEN bank_id = 0 THEN NULL ELSE bank_id END AS BankId FROM eriks_migration.dbo.TW_clients
                    ) X
                    WHERE Id = clientnbr
                ";
            }
        }

        public static string ConnectCustomersToCleaningObjects
        {
            get
            {
                return
                    @"UPDATE putsa_db.dbo.CleaningObjects SET CustomerId = cust.Id
                    FROM putsa_db.dbo.CleaningObjects co 
                    LEFT JOIN eriks_migration.dbo.TW_clientaddresses cla ON co.Id = cla.id 
                    RIGHT JOIN putsa_db.dbo.Customers cust ON cla.client_id = cust.PersonId";
                    //"
//                    @"UPDATE CleaningObjects SET CustomerId = cust.Id 
//                    FROM CleaningObjects co
//                    RIGHT JOIN PersonPostalAddressModels ppam ON co.PostalAddressModelId = ppam.PostalAddressModelId
//                    JOIN Customers cust ON cust.PersonId = ppam.PersonId";
//                    FROM CleaningObjects co
//                    RIGHT JOIN PostalAddressModels pam ON co.PostalAddressModelId = ppam.PostalAddressModelId
//                    RIGHT JOIN PersonPostalAddressModels ppam ON pam.Id = ppam.PostalAddressModelId
//                    RIGHT JOIN Persons pers ON ppam.PersonId = pers.Id
                //";
            }
        }

        public static string ConnectWorkersToTeams
        {
            get
            {
                return @"
                    UPDATE putsa_db.dbo.Workers SET TeamId = t.Id
                    FROM putsa_db.dbo.Workers w
					JOIN eriks_migration.dbo.TW_resources_employees emp ON w.UserId = emp.employee_id
					JOIN putsa_db.dbo.Teams t ON t.VehicleId = emp.resource_id
                ";
            }
        }

        public static string CreateUsersForTeams
        {
            get
            {
                return @"INSERT INTO putsa_db.dbo.Users (Username, [Permissions], TeamId) SELECT Name, 128, Id FROM putsa_db.dbo.Teams";
            }
        }

        public static string SetSubscriptionsInActive
        {
            get
            {
                return @"
                    UPDATE putsa_db.dbo.Subscriptions SET IsInactive = CASE WHEN [status] <> 3 THEN 1 ELSE 0 END
                    FROM putsa_db.dbo.Subscriptions s
                    JOIN eriks_migration.dbo.TW_workorders wo ON wo.delivery_clientaddress_id = s.CleaningObjectId
                ";
            }
        }

        public static string SetRUT
        {
            get
            {
                return @"
                    UPDATE putsa_db.dbo.Contacts SET RUT = CASE 
                    WHEN twc.full_reduction_pot = 0 AND twc.persnbr IS NOT NULL AND twc.persnbr <> ''
	                    THEN CASE WHEN twc.taxreduction_percentage = 0 THEN 1 ELSE twc.taxreduction_percentage / 100 END
                    ELSE 0 END
                    FROM putsa_db.dbo.Contacts c
                    JOIN eriks_migration.dbo.TW_clients twc ON twc.id = c.PersonId
                ";
            }
        }

        public static string UpdateCleaningObjectInfoBefore(int id, string info)
        {
            return "UPDATE putsa_db.dbo.CleaningObjects SET InfoBeforeCleaning = '" + info + "' WHERE Id = " + id;
        }

        public static string UpdateCleaningObjectInfoDuring(int id, string info)
        {
            return "UPDATE putsa_db.dbo.CleaningObjects SET InfoDuringCleaning = '" + info + "' WHERE Id = " + id;
        }

        public static string UpdateEmptyCleaningObjectInfoBefore
        {
            get
            {
                return @"
                UPDATE putsa_db.dbo.CleaningObjects SET InfoBeforeCleaning = n.content
                FROM eriks_migration.dbo.TW_notes n 
                JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                WHERE table_name = 'workorders' AND n.header LIKE '' AND n.important = 'Y' AND n.deleted = 'N' AND content <> '' AND notetype_id = 1
                AND n.header LIKE '%KONTAKT%'
                AND InfoBeforeCleaning IS NULL
            ";
            }
        }

        public static string UpdatePostalCodeTeamIds(int postalCodeId, int teamId)
        {
            return string.Format("UPDATE PostalCodes SET TeamId = {0} WHERE Id = {1}", teamId, postalCodeId);
        }

        public static string UpdatePostalCodeScheduleIds(int scheduleId, int coId)
        {
            return string.Format(@"UPDATE putsa_db.dbo.PostalCodeModels SET ScheduleId = {0} 
                FROM putsa_db.dbo.PostalCodeModels pcm
                JOIN putsa_db.dbo.PostalAddressModels pam ON pcm.Id = pam.PostalCodeModelId
                JOIN putsa_db.dbo.CleaningObjects co ON pam.Id = co.PostalAddressModelId
                WHERE co.Id = {1}", scheduleId, coId);
        }

        public static string UpdateSubscriptionServiceSubscriptionIds(int subscriptionIdToSet, int subscriptionIdToChange)
        {
            return "UPDATE putsa_db.dbo.SubscriptionServices SET SubscriptionId = " + subscriptionIdToSet + " WHERE SubscriptionId = " + subscriptionIdToChange;
        }

        public static string UpdateVehicles
        {
            get
            {
                return @"
update putsa_db.dbo.Vehicles set RegNo = 'MET 811', Phone = '073-9105501', AgreementStartDate = '2014-11-05', AgreementEndDate = '2014-11-04', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 1;
update putsa_db.dbo.Vehicles set RegNo = 'GXF 831', Phone = '073-9105502', AgreementStartDate = '2014-09-01', AgreementEndDate = '2014-09-28', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2013 where Id = 2;
update putsa_db.dbo.Vehicles set RegNo = 'LTW 037', Phone = '073-9105503', AgreementStartDate = '2014-09-03', AgreementEndDate = '2015-05-27', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 3;
update putsa_db.dbo.Vehicles set RegNo = 'MET 804', Phone = '073-9105504', AgreementStartDate = '2014-12-04', AgreementEndDate = '2014-12-03', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 4;
update putsa_db.dbo.Vehicles set RegNo = 'KUH 840', Phone = '073-9105505', AgreementStartDate = '2014-12-01', AgreementEndDate = '2019-04-01', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2010 where Id = 5;
update putsa_db.dbo.Vehicles set RegNo = 'HBZ 959', Phone = '073-9105506', AgreementStartDate = '2014-09-01', AgreementEndDate = '2014-09-28', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2013 where Id = 6;
update putsa_db.dbo.Vehicles set RegNo = 'OSW 662', Phone = '073-9105507', AgreementStartDate = '2014-12-01', AgreementEndDate = '2019-12-20', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 7;
update putsa_db.dbo.Vehicles set RegNo = 'PZP 763', Phone = '073-9105508', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 8;
update putsa_db.dbo.Vehicles set RegNo = 'MET 795', Phone = '073-9105509', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 9;
update putsa_db.dbo.Vehicles set RegNo = 'LWB 564', Phone = '073-9105510', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2013 where Id = 10;
update putsa_db.dbo.Vehicles set RegNo = 'GDZ 634', Phone = '073-9105511', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2010 where Id = 11;
update putsa_db.dbo.Vehicles set RegNo = 'KXG 587', Phone = '073-9105512', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 12;
update putsa_db.dbo.Vehicles set RegNo = 'DYC 836', Phone = '073-9105513', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 13;
update putsa_db.dbo.Vehicles set RegNo = 'HZR 182', Phone = '073-9105514', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 14;
update putsa_db.dbo.Vehicles set RegNo = 'NTL 687', Phone = '073-9105515', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2011 where Id = 15;
update putsa_db.dbo.Vehicles set RegNo = 'GXF 796', Phone = '-', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2013 where Id = 16;
update putsa_db.dbo.Vehicles set RegNo = 'HBZ 891', Phone = '-', Brand = 'Ford', VehicleModel = 'Transit', ManufacturingYear = 2013 where Id = 17;
";
            }
        }

        public static string PostalCodeFixUpdate(int id, int postalCode)
        {
            return string.Format("UPDATE eriks_migration.dbo.TW_clientaddresses SET postalcode_fixed = {0} WHERE id = {1}", postalCode, id);
        }

        #endregion

        public static string SelectPostalCodesWithMultipleSchedules
        {
            get
            {
                return @"
	                SELECT PostalCode
	                FROM (
		                SELECT PostalCode FROM putsa_db.dbo.PostalCodeModels pcm
		                RIGHT JOIN putsa_db.dbo.Schedules sched ON sched.Id = pcm.ScheduleId
		                GROUP BY PostalCode
		                HAVING (COUNT(DISTINCT sched.Id) > 1)
	                ) X";
            }
        }

        public static string SelectMajorityScheduleId(string postalCode)
        {
            return 
                "SELECT TOP(1) ScheduleId, COUNT(ScheduleId) AS ScheduleCount FROM putsa_db.dbo.PostalCodeModels " +
		        "WHERE PostalCode = '" + postalCode + "' AND ScheduleId IS NOT NULL " + 
		        "GROUP BY ScheduleId ORDER BY ScheduleCount DESC";
        }

        public static string UpdatePostalCodeScheduleIds(int scheduleId, string postalCode)
        {
            return "UPDATE putsa_db.dbo.PostalCodeModels SET ScheduleId = " + scheduleId + " WHERE PostalCode = '" + postalCode + "'";
        }
    }
}
