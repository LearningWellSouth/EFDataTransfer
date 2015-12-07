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

        private static string dbCurrentDB;

        public static string dbToUse
        {
            set 
            {
               dbCurrentDB = value; 
            }
        }

        public static string ConnectUnconnectedCleaningObjectsToTeams
        {
            get
            {
                return string.Format(@"UPDATE {0}.dbo.CleaningObjects SET TeamId = t.Id
                    FROM eriks_migration.dbo.TW_workareas wa
                    JOIN eriks_migration.dbo.TW_clientaddresses ca on wa.id = ca.workarea_id
                    JOIN eriks_migration.dbo.TW_resources res ON res.id = wa.resource_id
                    JOIN " + dbCurrentDB + ".dbo.Vehicles v ON v.Notes = res.name " +
                    "JOIN " + dbCurrentDB + @".dbo.Teams t ON t.VehicleId = v.Id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = ca.id
                    WHERE ca.deleted = 'N'
                    AND wa.deleted = 'N'
                    AND ca.postalcode_fixed IS NOT NULL", dbCurrentDB);
            }
        }

        public static string GetCleaningObjectsUnconnectedToTeams
        {
            get
            {
                return string.Format(@"
                        SELECT t.*, wa.*
                        FROM eriks_migration.dbo.TW_workareas wa
                        JOIN eriks_migration.dbo.TW_clientaddresses ca on wa.id = ca.workarea_id
                        JOIN eriks_migration.dbo.TW_resources res ON res.id = wa.resource_id
                        JOIN {0}.dbo.Vehicles v ON v.Notes = res.name
                        JOIN {0}.dbo.Teams t ON t.VehicleId = v.Id
                        JOIN {0}.dbo.CleaningObjects co ON co.Id = ca.id
                        WHERE ca.postalcode_fixed IS NOT NULL
                        AND co.TeamId IS NULL
                    ", dbCurrentDB);
            }                
        }

        public static string GetCleaningObjectsStillUnconnectedToTeam
        {
            get
            {
                return string.Format(@"SELECT Id, PostalAddressModelId FROM {0}.dbo.CleaningObjects WHERE TeamId IS NULL", dbCurrentDB);
            }
        }

        public static string GetTeamIdForUnconnectedCleaningObject(int pamId)
        {
            return string.Format(@"
                SELECT TOP(1) TeamId FROM {0}.dbo.CleaningObjects 
                WHERE PostalAddressModelId = {1}
                AND TeamId IS NOT NULL", dbCurrentDB, pamId);
        }

        public static string SetTeamIdOnCleaningObject(int coId, int teamId)
        {
            return string.Format(@"UPDATE {0}.dbo.CleaningObjects SET TeamId = {1} WHERE Id = {2}", dbCurrentDB, teamId, coId);
        }

        public static string GetTeamIdFromPcmsForUnconnectedCleaningObject(int postalAddressModelId)
        {
            return string.Format(@"
                SELECT TOP (1) co.TeamId FROM {0}.dbo.PostalCodeModels pcm
                JOIN {0}.dbo.PostalAddressModels pam1 ON pam1.PostalCodeModelId = pcm.Id
                JOIN {0}.dbo.PostalAddressModels pam2 ON pam2.PostalCodeModelId = pcm.Id
                JOIN {0}.dbo.CleaningObjects co ON co.PostalAddressModelId = pam2.Id
                WHERE pam1.Id = {1} 
                AND co.TeamId IS NOT NULL
            ", dbCurrentDB, postalAddressModelId);
        }

        public static string GetTeamIdFromSamePostalCodeForUnconnectedCo(int coId)
        {
            return string.Format(@"
                SELECT TOP(1) co.TeamId FROM {0}.dbo.CleaningObjects co WHERE PostalAddressModelId IN(
                    SELECT Id FROM {0}.dbo.PostalAddressModels WHERE PostalCodeModelId IN (
                        SELECT pcm1.Id FROM {0}.dbo.PostalCodeModels pcm1
                        JOIN {0}.dbo.PostalCodeModels pcm2 ON pcm1.PostalCode = pcm2.PostalCode
                        JOIN {0}.dbo.PostalAddressModels pam1 ON pam1.PostalCodeModelId = pcm2.Id
                        JOIN {0}.dbo.CleaningObjects co ON co.PostalAddressModelId = pam1.Id
                        WHERE co.Id = {1}
                    )
                ) AND co.TeamId IS NOT NULL", dbCurrentDB, coId);
        }

        public static string GetTeamIdFromSameCityForUnconnCo(int coId)
        {
            return string.Format(@"
                SELECT DISTINCT co.TeamId FROM {0}.dbo.CleaningObjects co WHERE PostalAddressModelId IN (		
	                SELECT Id FROM {0}.dbo.PostalAddressModels WHERE PostalCodeModelId IN (
		                SELECT pcm1.Id FROM {0}.dbo.PostalCodeModels pcm1
		                JOIN {0}.dbo.PostalCodeModels pcm2 ON pcm1.City = pcm2.City
		                JOIN {0}.dbo.PostalAddressModels pam1 ON pam1.PostalCodeModelId = pcm2.Id
		                JOIN {0}.dbo.CleaningObjects co ON co.PostalAddressModelId = pam1.Id
		                WHERE co.Id = 2601
	                )
                ) AND TeamId IS NOT NULL
            ", dbCurrentDB, coId);
        }

        public static string InsertIntoPostalCodeModels(
            string postalCode, string postalCodeType, string address, string streetNoLowest, string streetNoHighest, string city, string typeOfPlacement)
        {
            return string.Format(@"INSERT INTO " + dbCurrentDB + @".dbo.PostalCodeModels (PostalCode, PostalCodeType, PostalAddress, StreetNoLowest, StreetNoHighest, City, TypeOfPlacement, IsNotValid)
                OUTPUT INSERTED.Id VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', 1)", 
                postalCode, postalCodeType, address, streetNoLowest, streetNoHighest, city, typeOfPlacement);
        }

        public static string InsertIntoSchedules(int num)
        {
            return string.Format("INSERT INTO " + dbCurrentDB + ".dbo.Schedules (Name, _ValidFrom, _ValidTo) OUTPUT INSERTED.Id VALUES ('Schema {0}', GETDATE()-200, GETDATE()+365)", num);
        }

        public static string InsertIntoServiceGroups
        {
            get
            {
                return string.Format("INSERT INTO " + dbCurrentDB + ".dbo.ServiceGroups ([From], [To], Name) OUTPUT INSERTED.Id VALUES ('{0}', '{1}', '{2}')", 
                    DateTime.Now.Year.ToString() + "-1-1", DateTime.Now.Year.ToString() + "-12-31", DateTime.Now.Year);
            }
        }

        public static string InsertPostalAddressModel(string streetNo, int postalCodeModelId, string addressType, string address2, float longitude, float latitude)
        {
            return string.Format(new System.Globalization.CultureInfo("en-US"),
                "INSERT INTO " + dbCurrentDB + ".dbo.PostalAddressModels (StreetNo, PostalCodeModelId, AddressType, Address2, Longitude, Latitude) OUTPUT INSERTED.Id VALUES ('{0}', {1}, '{2}', '{3}', {4}, {5})",
                streetNo, postalCodeModelId, addressType, address2, longitude, latitude);
        }

        public static string TransferEmployees
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT " + dbCurrentDB + @".dbo.Users ON
                    INSERT INTO " + dbCurrentDB + @".dbo.Users (Id, Username, Permissions)
                    SELECT id, CONCAT(firstname, ' ', lastname), CASE WHEN role_id = 1 THEN 96 ELSE 63 END
                    FROM eriks_migration.dbo.TW_employees
                    WHERE deleted = 'N'
                    SET IDENTITY_INSERT " + dbCurrentDB + ".dbo.Users OFF";
            }
        }


        public static string TransferCustomerNotes
        {
            get
            {
                return string.Format(@"
                    
                    INSERT INTO {0}.dbo.Issues ( Title, [Description], [Status], [Priority], StartDate, FinishedDate, CustomerId, IssueType, [Private])
                    SELECT header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority],
                        CASE WHEN ISDATE(content) = 1 AND CONVERT(DATETIME, content) > n.ctime THEN content 
		                    WHEN n.ctime IS NOT NULL THEN n.ctime ELSE n.ctime END AS StartDate,
                        CASE WHEN ISDATE(content) = 1  THEN content 
		                    WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS FinishedDate, 
                        c.Id AS CustomerId, 7 AS IssueType, 0 AS [Private]
                    FROM eriks_migration.dbo.TW_notes n
                    JOIN eriks_migration.dbo.TW_clients cli ON n.table_id = cli.id
                    JOIN {0}.dbo.Customers c on c.Id = cli.clientnbr
                    WHERE n.tag_type LIKE '%anteckning%' AND n.table_name LIKE '%clients%'
                    
                ", dbCurrentDB);
            }
        }

//        public static string TransferCustomerAddedNotes
//        {
//            get
//            {
//                return @"
//                    SET IDENTITY_INSERT " + dbCurrentDB + @".dbo.Issues ON
//                    INSERT INTO " + dbCurrentDB + @".dbo.Issues (Id, Title, [Description], [Status], [Priority], StartDate, FinishedDate, CustomerId, IssueType, CreatorId, Private)
//                    SELECT n.id AS Id, header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority],
//                        CASE WHEN ISDATE(content) = 1 THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate,
//                        CASE WHEN ISDATE(content) = 1  THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS FinishedDate, 
//                        clientnbr AS CustomerId, 
//                        7 AS IssueType, n.created_by_id AS CreatorId, 0 AS Private
//                    FROM eriks_migration.dbo.TW_notes n
//                    JOIN eriks_migration.dbo.TW_clients cli ON n.table_id = cli.id
//                    JOIN " + dbCurrentDB + @".dbo.Customers c on c.Id = cli.clientnbr
//                    WHERE tag_home = 'kund' AND tag_type = 'anteckning'
//                    SET IDENTITY_INSERT " + dbCurrentDB + @".dbo.Issues OFF";

//                //WHERE table_name = 'clients'
//                //AND header LIKE 'Inlagddatum'
//                //AND n.deleted = 'N'
//                //AND cli.deleted = 'N'
//                //AND clientnbr IS NOT NULL

//            }
//        }

        public static string TransferCleaningObjectEconomyNotes
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.Issues (Title, [Description], [Status], [Priority], StartDate, FinishedDate, IssueType, CleaningObjectId, CreatorId, Private)
                    SELECT header AS Title, content AS [Description], 
                        CASE WHEN n.closed = 'N' THEN 2 ELSE 4 END AS [Status], 
                        0 AS [Priority], 
                        CASE WHEN ISDATE(content) = 1 THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, 
						CASE WHEN n.closed = 'N' THEN NULL WHEN ISDATE(content) = 1 THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS FinishedDate, 
                        5 AS IssueType, wo.delivery_clientaddress_id AS CleaningObjectId, n.created_by_id AS CreatorId, 0 AS Private
                    FROM eriks_migration.dbo.TW_notes n
                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                    WHERE table_name LIKE '%workorders%' AND tag_type LIKE '%ekonomi%'
                ", dbCurrentDB);
            }
        }

        public static string TransferCleaningObjectFieldNotes
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.Issues (Title, [Description], [Status], [Priority], StartDate, FinishedDate, [Private], CleaningObjectId, IssueType)
                    SELECT notes.header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS FinishedDate,
	                    0 AS [Private], wo.delivery_clientaddress_id AS CleaningObjectId, 10 AS IssueType
                    FROM eriks_migration.dbo.TW_notes AS notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%falt%' AND table_name LIKE '%workorders%'
                    SET IDENTITY_INSERT {0}.dbo.Issues OFF
                ", dbCurrentDB);
            }
        }

        public static string TransferCustomerEconomyNotes
        {
            get
            {
                return string.Format(@"
                    
                    INSERT INTO {0}.dbo.Issues ( Title, [Description], [Status], [Priority], StartDate, FinishedDate, IssueType, CustomerId, CreatorId, Private)
                    SELECT  header AS Title, content AS [Description], 
	                    CASE WHEN n.closed = 'N' THEN 2 ELSE 4 END AS [Status], 0 AS [Priority], 
						CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate, 
                        CASE WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS FinishedDate, 
                        5 AS IssueType, clientnbr AS CustomerId, 
                        CASE WHEN n.created_by_id <> -1 THEN n.created_by_id ELSE NULL END AS CreatorId, 0 AS Private
                    FROM eriks_migration.dbo.TW_notes n
                    JOIN eriks_migration.dbo.TW_clients cli ON n.table_id = cli.id
                    JOIN {0}.dbo.Customers c ON c.Id = cli.clientnbr
                    WHERE table_name LIKE '%clients%' AND n.tag_type LIKE '%ekonomi%' ", dbCurrentDB);
            }
        }

        public static string TransferCleaningObjectNotes
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.Issues (Title, [Description], [Status], StartDate, FinishedDate, CleaningObjectId, IssueType, Private, Priority)
                    SELECT n.header AS Title, content AS [Description], 4 AS [Status],
                        CASE WHEN ISDATE(content) = 1 THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS StartDate,
                        CASE WHEN ISDATE(content) = 1 THEN content WHEN n.mtime IS NOT NULL THEN n.mtime ELSE n.ctime END AS FinishedDate,
                        wo.delivery_clientaddress_id AS CleaningObjectId,
                        7 AS IssueType, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes n 
                    JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE n.tag_type LIKE '%anteckning%' AND n.table_name LIKE '%workorders%'
                ", dbCurrentDB);
            }
        }

        public static string TransferCleaningObjectOrderNotes
        {
            get
            {
                return string.Format(@"
                                     INSERT INTO {0}.dbo.Issues ( Title, [Description], [Status], StartDate, FinishedDate, CreatorId, IssueType, CleaningObjectId, Private, Priority)
                    SELECT  notes.header AS Title, content AS [Description], 4 AS [Status],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
						CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS FinishedDate,
	                    created_by_id AS CreatorId, 2 AS IssueType, wo.delivery_clientaddress_id AS CleaningObjectId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name LIKE '%workorders%' AND notes.tag_type LIKE '%bestallning%'
                    
                   ",
                dbCurrentDB);
            }
        }

        public static string TransferReclamationNotes
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.Issues (Title, [Description], [Status], [Priority], StartDate, FinishedDate, [Private], CleaningObjectId, IssueType)
                    SELECT notes.header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS FinishedDate,
	                    0 AS [Private], wo.delivery_clientaddress_id AS CleaningObjectId, 1 AS IssueType
                    FROM eriks_migration.dbo.TW_notes AS notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%reklamation%' AND table_name LIKE '%workorders%'
                ", dbCurrentDB);
            }
        }

        public static string TransferDamageReports
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.Issues (Title, [Description], [Status], [Priority], StartDate, FinishedDate, [Private], CleaningObjectId, IssueType)
                    SELECT notes.header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS FinishedDate,
	                    0 AS [Private], wo.delivery_clientaddress_id AS CleaningObjectId, 4 AS IssueType
                    FROM eriks_migration.dbo.TW_notes AS notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%skadeanmalan%' AND table_name LIKE '%workorders%'
                ", dbCurrentDB);
            }
        }

        public static string TransferTerminationNotes
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.Issues (Title, [Description], [Status], [Priority], StartDate, FinishedDate, [Private], CleaningObjectId, IssueType)
                    SELECT notes.header AS Title, content AS [Description], 4 AS [Status], 0 AS [Priority],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS FinishedDate,
	                    0 AS [Private], wo.delivery_clientaddress_id AS CleaningObjectId, 3 AS IssueType
                    FROM eriks_migration.dbo.TW_notes AS notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%uppsagning%' AND table_name LIKE '%workorders%'
                ", dbCurrentDB);
            }
        }

        public static string TransferCleaningObjectCancellations
        {
            get
            {
                return @"
                    SET IDENTITY_INSERT " + dbCurrentDB + @".dbo.Issues ON
                    INSERT INTO " + dbCurrentDB + @".dbo.Issues (Id, Title, [Description], [Status], StartDate, CleaningObjectId, IssueType, CreatorId, Private, Priority)
                    SELECT DISTINCT notes.id AS Id, 'Uppsägning' AS Title, content AS [Description], 4 AS [Status],
	                    CASE WHEN notes.mtime IS NOT NULL THEN notes.mtime ELSE notes.ctime END AS StartDate,
	                    wo.delivery_clientaddress_id AS CleaningObjectId, 3 AS IssueType, notes.created_by_id AS CreatorId, 0 AS Private, 0 AS Priority
                    FROM eriks_migration.dbo.TW_notes AS notes 
                    JOIN eriks_migration.dbo.TW_workorders wo ON wo.id = notes.table_id
                    JOIN " + dbCurrentDB + @".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE table_name = 'workorders' 
                    AND content <> ''
                    AND notes.header IN ('Uppsagddatum', 'UPPSÄGNING', 'UPPSAGD') AND notes.deleted = 'N'
                    AND notes.id NOT IN (SELECT Id FROM " + dbCurrentDB + @".dbo.Issues)
                    SET IDENTITY_INSERT " + dbCurrentDB + ".dbo.Issues OFF";
            }
        }

        // Nytt 2015-11-13
        public static string InsertAdminFeePriceMods
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.CleaningObjectPrices (Modification, CleaningObjectId, ServiceId, ServiceGroupId)
                    SELECT CASE WHEN twc.invoicefee = 28 OR twc.invoicefee = 1 THEN 1 ELSE 0 END AS Modification, 
	                    co.Id AS CleaningObjectId, 
                        28 AS ServiceId, 
	                    (SELECT Max(Id) FROM {0}.dbo.ServiceGroups) AS ServiceGroupId
                    FROM eriks_migration.dbo.TW_clients AS twc
                    JOIN {0}.dbo.CleaningObjects co ON co.CustomerId = twc.clientnbr
                    WHERE twc.deleted = 'N'
                    AND twc.mother_id = 0
                ", dbCurrentDB);
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

            return string.Format("SELECT PostalCodeModelId FROM " + dbCurrentDB + ".dbo.PostalAddressModels WHERE StreetNo LIKE '{0}' AND PostalCodeModelId IN {1}", streetNo, pcmIdStr);
        }

        public static string SelectAllPostalCodeModels
        {
            get
            {
                return @"SELECT Id, PostalCode, PostalAddress, StreetNoLowest, StreetNoHighest, City, TypeOfPlacement FROM " + dbCurrentDB + @".dbo.PostalCodeModels";
                    // WHERE PostalCode IN (SELECT postalcode_fixed FROM eriks_migration.dbo.TW_clientaddresses)"; Kan inte köra detta pga att landskoder tillkommit i postnr
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
            return "SELECT Id FROM " + dbCurrentDB + ".dbo.Subscriptions WHERE Id != " + subToSetId + " AND CleaningObjectId = " + cleaningObjectId;
        }

        public static string SelectSubscriptionIds(int cleaningObjectId)
        {
            return "SELECT Id FROM " + dbCurrentDB + ".dbo.Subscriptions WHERE CleaningObjectId = " + cleaningObjectId;
        }


        public const string SelectTWAddresses
                = "SELECT id, [address], co_address, postalcode_fixed, postalcode, city, latitude, longitude, route_num, is_delivery, is_invoice, client_id, workarea_id " + 
                    "FROM eriks_migration.dbo.TW_clientaddresses WHERE deleted = 'N'";

      public static string SelectTWServices
        {
            get
            {
                return @"SELECT tws.id AS twId, s.Id AS ServiceId, price_per_unit * 1.25 AS Cost FROM eriks_migration.dbo.TW_services tws 
                        JOIN [" + dbCurrentDB + @"].[dbo].[Services] s ON s.Name = tws.name
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
                return @"SELECT wa.resource_id, ca.id AS Id, interlude_num, res.name AS name FROM eriks_migration.dbo.TW_workareas wa 
                    JOIN eriks_migration.dbo.TW_clientaddresses ca on wa.id = ca.workarea_id 
                    JOIN eriks_migration.dbo.TW_resources res ON res.id = wa.resource_id
                    WHERE ca.deleted = 'N' 
                    AND wa.deleted = 'N'
                    AND ca.postalcode_fixed IS NOT NULL";
                //                return @"SELECT wa.resource_id, ca.[address], ca.postalcode_fixed, ca.city, cli.clientnbr, interlude_num FROM TW_workareas wa
                //JOIN TW_clients cli ON ca.client_id = cli.id
                //AND ca.is_delivery = 'Y' " + 
                //AND cli.deleted = 'N'
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
                    JOIN " + dbCurrentDB + @".dbo.CleaningObjects co ON wo.delivery_clientaddress_id = co.Id
                    WHERE wo.deleted = 'N'
                    AND wo.[status] <> 6
                    ORDER BY co.Id DESC, wo.id DESC";
            }
        }

        // Nytt 2015-11-13
        public static string SelectAllPeriods(int validForYear)
        {
            return string.Format(@"
                SELECT subs.Id AS SubscriptionId, p.Id
                FROM {0}.dbo.CleaningObjects co
                JOIN {0}.dbo.Subscriptions subs ON subs.CleaningObjectId = co.Id
                JOIN {0}.dbo.PostalAddressModels pam ON pam.Id = co.PostalAddressModelId
                JOIN {0}.dbo.PostalCodeModels pcm ON pcm.Id = pam.PostalCodeModelId
                JOIN {0}.dbo.Periods p ON p.ScheduleId = pcm.ScheduleId
                WHERE p.ValidForYear = {1}
                ORDER BY subs.Id
            ", dbCurrentDB, validForYear);
        }

        public static string SelectAllPeriodsForEmptySubs(int validForYear)
        {
            return string.Format(@"
                SELECT subs.Id AS SubscriptionId, p.Id, co.Id AS CleaningObjectId
                FROM {0}.dbo.CleaningObjects co
                JOIN {0}.dbo.Subscriptions subs ON subs.CleaningObjectId = co.Id
                LEFT JOIN {0}.dbo.SubscriptionServices ss ON ss.SubscriptionId = subs.Id
                JOIN {0}.dbo.PostalAddressModels pam ON pam.Id = co.PostalAddressModelId
                JOIN {0}.dbo.PostalCodeModels pcm ON pcm.Id = pam.PostalCodeModelId
                JOIN {0}.dbo.Periods p ON p.ScheduleId = pcm.ScheduleId
                WHERE p.ValidForYear = {1}
                AND ss.Id IS NULL
                ORDER BY subs.Id
            ", dbCurrentDB, validForYear);
        }

        public static string SelectTWWorkOrderLines
        {
            get
            {
                return @"SELECT wol.id AS wolId, interlude_occasion, workorder_id, s.Id AS sId FROM eriks_migration.dbo.TW_workorderlines wol
                    JOIN eriks_migration.dbo.TW_services twS ON twS.id = wol.service_id
                    JOIN " + dbCurrentDB + @".dbo.[Services] s ON s.Id = twS.id
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
                // wol.deleted = 'N' tillagt 20151111
                return @"SELECT s.Id AS ServiceId, wol.unit_price, twS.price_per_unit, ca.id AS caId, wol.[description] AS wolDesc FROM eriks_migration.dbo.TW_workorderlines wol
                        JOIN eriks_migration.dbo.TW_services twS ON wol.service_id = twS.id
                        JOIN " + dbCurrentDB + @".dbo.[Services] s ON twS.id = s.Id
                        JOIN eriks_migration.dbo.TW_workorders wo ON wol.workorder_id = wo.id
                        JOIN eriks_migration.dbo.TW_clientaddresses ca ON wo.delivery_clientaddress_id = ca.id
                        WHERE --ca.is_delivery = 'Y' AND 
                        ca.deleted = 'N'
                        AND wo.deleted = 'N'
                        AND wo.[status] <> 6
                        AND postalcode_fixed IS NOT NULL
                        AND wol.interlude_num IS NOT NULL
                        AND wol.deleted = 'N'
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

        public static string SelectDuplicateTwNoteTableIds
        {
            get
            {
                return string.Format(@"                    
                    SELECT table_id
                    FROM eriks_migration.dbo.TW_notes notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%uppdrag_fore%' AND table_name LIKE '%workorders%'
                    AND header IN ('statusINFO', 'Uppdragsbeskrivning', '', 'INFO', 'ANTECKNING', 'HUSBESKRIVNING', 'VIKTIGT') 
                    AND header NOT IN ('OBS!!!', 'UPPSÄGNING', 'REKLAMATION', 'BESTÄLLNING', 'UPPSAGD', 'ALLA SIDOR', 'KONTAKTA')
                    AND table_id IN (
	                    SELECT table_id FROM eriks_migration.dbo.TW_notes 
	                    WHERE tag_type LIKE '%uppdrag_fore%' 
	                    AND table_name LIKE '%workorders%' 
	                    AND header IN ('statusINFO', 'Uppdragsbeskrivning', '', 'INFO', 'ANTECKNING', 'HUSBESKRIVNING', 'VIKTIGT') 
	                    AND header NOT IN ('OBS!!!', 'UPPSÄGNING', 'REKLAMATION', 'BESTÄLLNING', 'UPPSAGD', 'ALLA SIDOR', 'KONTAKTA')
	                    GROUP BY table_id
	                    HAVING COUNT (table_id) > 1 
                    )
                ", dbCurrentDB);
            }
        }

        public static string SelectDuplicateTwNoteIdsForDuring
        {
            get
            {
                return string.Format(@"
                    SELECT table_id
                    FROM eriks_migration.dbo.TW_notes notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%uppdrag_under%' AND table_name LIKE '%workorders%'
                    AND header IN ('Uppdragsbeskrivning', '', 'INFO', 'statusInfo')
                    AND table_id IN (
	                    SELECT table_id FROM eriks_migration.dbo.TW_notes notes
	                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
	                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
	                    WHERE tag_type LIKE '%uppdrag_under%' AND table_name LIKE '%workorders%'
	                    AND header NOT IN ('Uppdragsbeskrivning', '')
	                    GROUP BY table_id
	                    HAVING COUNT (table_id) > 1
                    )
                ", dbCurrentDB);
            }
        }

        public static string SelectHeaderAndContentIdsFromDuplicateTWNotes
        {
            get
            {
                return @"
                    SELECT table_id FROM eriks_migration.dbo.TW_notes notes 
                    WHERE tag_type LIKE '%uppdrag_fore%' AND table_name LIKE '%workorders%'
                    --AND header NOT IN ('statusINFO', 'Uppdragsbeskrivning', '', 'INFO', 'ANTECKNING', 'HUSBESKRIVNING', 'VIKTIGT') 
                    AND table_id IN (
	                    SELECT table_id FROM eriks_migration.dbo.TW_notes 
	                    WHERE tag_type LIKE '%uppdrag_fore%' AND table_name LIKE '%workorders%' 
	                    --AND header NOT IN ('statusINFO', 'Uppdragsbeskrivning', '', 'INFO', 'ANTECKNING', 'HUSBESKRIVNING', 'VIKTIGT') 
	                    GROUP BY table_id
	                    HAVING COUNT(table_id) > 1 
                    )
                ";
            }
        }

        public static string SelectHeaderAndContentIdsFromDuplicateTWNotesForDuring
        {
            get {
                return @"
                    SELECT table_id
                    FROM eriks_migration.dbo.TW_notes notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    WHERE tag_type LIKE '%uppdrag_under%' AND table_name LIKE '%workorders%'
                    --AND header NOT IN ('Uppdragsbeskrivning', '', 'INFO', 'statusInfo')
                    AND table_id IN (
	                    SELECT table_id FROM eriks_migration.dbo.TW_notes notes
	                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
	                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
	                    WHERE tag_type LIKE '%uppdrag_under%' AND table_name LIKE '%workorders%'
	                  --  AND header NOT IN ('Uppdragsbeskrivning', '')
	                    GROUP BY table_id
	                    HAVING COUNT (table_id) > 1
                    )";
            }
        }            

        public static string SelectTwNoteAndCleaningObjectId(int id)
        {
            return string.Format(@"
                SELECT content, header, co.Id AS coId
                FROM eriks_migration.dbo.TW_notes notes
                JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                WHERE notes.table_id = {1} AND notes.tag_type LIKE '%uppdrag_under%'
            ", dbCurrentDB, id);
        }

        public static string SelectTwIssues
        {
            get {
                return @"
                    select [subject] as Title, content as [Description], 4 as [Status], 1 as [Priority], creation_date as StartDate, i.ctime as FinishedDate, 7 as IssueType, 
                        0 as [Private], c.clientnbr as CustomerId
                    from eriks_migration.dbo.TW_issues i 
                    join eriks_migration.dbo.TW_clients c on i.client_id = c.id
                ";
            }
        }

        #endregion

        #region update

        public static string TransferInfoBeforeCleaning
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.CleaningObjects SET InfoBeforeCleaning = content
                    FROM eriks_migration.dbo.TW_notes notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%uppdrag_fore%' AND table_name LIKE '%workorders%'
                    AND header IN ('statusINFO', 'Uppdragsbeskrivning', '', 'INFO', 'ANTECKNING', 'HUSBESKRIVNING', 'VIKTIGT') 
                    AND header NOT IN ('OBS!!!', 'UPPSÄGNING', 'REKLAMATION', 'BESTÄLLNING', 'UPPSAGD', 'ALLA SIDOR', 'KONTAKTA')
                    AND table_id IN (
	                    SELECT table_id FROM eriks_migration.dbo.TW_notes 
	                    WHERE tag_type LIKE '%uppdrag_fore%' 
	                    AND table_name LIKE '%workorders%' 
	                    AND header IN ('statusINFO', 'Uppdragsbeskrivning', '', 'INFO', 'ANTECKNING', 'HUSBESKRIVNING', 'VIKTIGT') 
	                    AND header NOT IN ('OBS!!!', 'UPPSÄGNING', 'REKLAMATION', 'BESTÄLLNING', 'UPPSAGD', 'ALLA SIDOR', 'KONTAKTA')
	                    GROUP BY table_id
	                    HAVING COUNT (table_id) = 1 
                    )
                ", dbCurrentDB);
            }
        }

        public static string TransferInfoDuringCleaning
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.CleaningObjects SET InfoDuringCleaning = REPLACE(content, '''', ':')
                    FROM eriks_migration.dbo.TW_notes notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%uppdrag_under%' AND table_name LIKE '%workorders%'
                    AND table_id IN (
	                    SELECT table_id FROM eriks_migration.dbo.TW_notes notes
	                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
	                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
	                    WHERE tag_type LIKE '%uppdrag_under%' AND table_name LIKE '%workorders%' 
	                    GROUP BY table_id
	                    HAVING COUNT (table_id) = 1
                    )
                ", dbCurrentDB);
            }
        }

        public static string TransferHeaderAndContentInfoBeforeCleaning
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.CleaningObjects SET InfoBeforeCleaning = CONCAT(header, ' - ', content)
                    FROM eriks_migration.dbo.TW_notes notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%uppdrag_fore%' AND table_name LIKE '%workorders%'
                    AND table_id IN (
	                    SELECT table_id FROM eriks_migration.dbo.TW_notes notes
	                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
	                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
	                    WHERE tag_type LIKE '%uppdrag_fore%' AND table_name LIKE '%workorders%'
	                    GROUP BY table_id
	                    HAVING COUNT (table_id) = 1
                    )             
                ", dbCurrentDB);
            }
        }

        public static string TransferHeaderAndContentInfoDuringCleaning
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.CleaningObjects SET InfoDuringCleaning = CONCAT(header, ' - ', REPLACE(content, '''', ':'))
                    FROM eriks_migration.dbo.TW_notes notes
                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                    WHERE tag_type LIKE '%uppdrag_under%' AND table_name LIKE '%workorders%'
                    AND header NOT IN ('Uppdragsbeskrivning', '', 'INFO', 'statusInfo')
                    AND table_id IN (
	                    SELECT table_id FROM eriks_migration.dbo.TW_notes notes
	                    JOIN eriks_migration.dbo.TW_workorders wo ON notes.table_id = wo.id
	                    JOIN putsa_db.dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
	                    WHERE tag_type LIKE '%uppdrag_under%' AND table_name LIKE '%workorders%' 
	                    GROUP BY table_id
	                    HAVING COUNT (table_id) = 1
                    )
                ", dbCurrentDB);
            }
        }

        public static string CreateUsersForTeams
        {
            get
            {
                return @"INSERT INTO " + dbCurrentDB + ".dbo.Users (Username, [Permissions], TeamId) SELECT CONCAT(REPLACE(Name, ' ', ''), '@eriksfonsterputs.se'), 128, Id FROM " + 
                    dbCurrentDB + ".dbo.Teams";
            }
        }

        public static string SetSubscriptionsInActive
        {
            get
            {
                return @"
                    UPDATE " + dbCurrentDB + @".dbo.Subscriptions SET IsInactive = CASE WHEN [status] <> 3 THEN 1 ELSE 0 END
                    FROM " + dbCurrentDB + @".dbo.Subscriptions s
                    JOIN eriks_migration.dbo.TW_workorders wo ON wo.delivery_clientaddress_id = s.CleaningObjectId
                ";
            }
        }

        public static string SetSubscriptionsInActiveByClientsDeleted
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.Subscriptions SET IsInactive = 1
                    WHERE Id IN (
	                    SELECT s.Id FROM {0}.dbo.Subscriptions s
	                    JOIN {0}.dbo.CleaningObjects co on co.Id = s.CleaningObjectId
	                    JOIN eriks_migration.dbo.TW_clients twc on twc.clientnbr = co.CustomerId
	                    WHERE twc.deleted = 'Y'
                    )
                ", dbCurrentDB);
            }
        }

        public static string SetEmptySubscriptionsInactive
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.Subscriptions SET IsInactive = 1
                    WHERE Id IN (
                        SELECT s.Id FROM {0}.dbo.Subscriptions s
                        LEFT JOIN {0}.dbo.SubscriptionServices ss ON ss.SubscriptionId = s.Id
                        WHERE ss.Id IS NULL
                    )
                ", dbCurrentDB);
            }
        }

        public static string InsertBaseFees
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.SubscriptionServices (
                ", dbCurrentDB);
            }
        }


        public static string UpdateCleaningObjectInfoBefore(int id, string info)
        {
            return "UPDATE " + dbCurrentDB + ".dbo.CleaningObjects SET InfoBeforeCleaning = '" + info + "' WHERE Id = " + id;
        }

        public static string UpdateCleaningObjectInfoDuring(int id, string info)
        {
            return "UPDATE " + dbCurrentDB + ".dbo.CleaningObjects SET InfoDuringCleaning = '" + info + "' WHERE Id = " + id;
        }

        public static string UpdateEmptyCleaningObjectInfoBefore
        {
            get
            {
                return @"
                UPDATE " + dbCurrentDB + @".dbo.CleaningObjects SET InfoBeforeCleaning = n.content
                FROM eriks_migration.dbo.TW_notes n 
                JOIN eriks_migration.dbo.TW_workorders wo ON n.table_id = wo.id
                JOIN " + dbCurrentDB + @".dbo.CleaningObjects co ON co.Id = wo.delivery_clientaddress_id
                WHERE table_name = 'workorders' AND n.header LIKE '' AND n.important = 'Y' AND n.deleted = 'N' AND content <> '' AND notetype_id = 1
                AND n.header LIKE '%KONTAKT%'
                AND InfoBeforeCleaning IS NULL
            ";
            }
        }

        public static string SetPostalCodeScheduleIds
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.PostalCodeModels SET ScheduleId = sched.Id 
                    FROM eriks_migration.dbo.TW_workareas wa
                    JOIN eriks_migration.dbo.TW_clientaddresses ca ON ca.workarea_id = wa.id
                    JOIN {0}.dbo.CleaningObjects co ON co.Id = ca.id
                    JOIN {0}.dbo.PostalAddressModels pam ON pam.Id = co.PostalAddressModelId
                    JOIN {0}.dbo.PostalCodeModels pcm ON pcm.Id = pam.PostalCodeModelId
                    JOIN {0}.dbo.Schedules sched ON sched.Name LIKE '% ' + wa.interlude_num
                ", dbCurrentDB);
            }
        }

        public static string UpdatePostalCodeTeamIds(int postalCodeId, int teamId)
        {
            return string.Format("UPDATE PostalCodes SET TeamId = {0} WHERE Id = {1}", teamId, postalCodeId);
        }

        public static string UpdatePostalCodeScheduleIds(int scheduleId, int coId)
        {
            return string.Format(@"UPDATE " + dbCurrentDB + @".dbo.PostalCodeModels SET ScheduleId = {0} 
                FROM " + dbCurrentDB + @".dbo.PostalCodeModels pcm
                JOIN " + dbCurrentDB + @".dbo.PostalAddressModels pam ON pcm.Id = pam.PostalCodeModelId
                JOIN " + dbCurrentDB + @".dbo.CleaningObjects co ON pam.Id = co.PostalAddressModelId
                WHERE co.Id = {1}", scheduleId, coId);
        }

        public static string UpdateSubscriptionServiceSubscriptionIds(int subscriptionIdToSet, int subscriptionIdToChange)
        {
            return "UPDATE " + dbCurrentDB + ".dbo.SubscriptionServices SET SubscriptionId = " + subscriptionIdToSet + " WHERE SubscriptionId = " + subscriptionIdToChange;
        }

        public static string PostalCodeFixUpdate(int id, int postalCode)
        {
            return string.Format("UPDATE eriks_migration.dbo.TW_clientaddresses SET postalcode_fixed = {0} WHERE id = {1}", postalCode, id);
        }

        public static string UpdateAllPostalCodeScheduleIds
        {
            get
            {
                return @"UPDATE X SET X.ToSchedule = X.FromSchedule
                    FROM (SELECT PCM1.PostalCode,PCM1.ScheduleId As ToSchedule, PCM2.ScheduleId As FromSchedule FROM " + dbCurrentDB + @".dbo.PostalCodeModels PCM1
                    INNER JOIN  " + dbCurrentDB + @".dbo.PostalCodeModels PCM2 ON PCM1.PostalCode = PCM2.PostalCode
                    WHERE PCM1.ScheduleId is null AND PCM2.ScheduleId>0) X";
            }


        }

        #endregion
        public static string SelectPostalCodesWithMultipleSchedules
        {
            get
            {
                return @"
	                SELECT PostalCode
	                FROM (
		                SELECT PostalCode FROM " + dbCurrentDB + @".dbo.PostalCodeModels pcm
		                RIGHT JOIN " + dbCurrentDB + @".dbo.Schedules sched ON sched.Id = pcm.ScheduleId
		                GROUP BY PostalCode
		                HAVING (COUNT(DISTINCT sched.Id) > 1)
	                ) X";
            }
        }

        public static string SelectMajorityScheduleId(string postalCode)
        {
            return 
                "SELECT TOP(1) ScheduleId, COUNT(ScheduleId) AS ScheduleCount FROM " + dbCurrentDB + ".dbo.PostalCodeModels " +
		        "WHERE PostalCode = '" + postalCode + "' AND ScheduleId IS NOT NULL " + 
		        "GROUP BY ScheduleId ORDER BY ScheduleCount DESC";
        }

        public static string UpdatePostalCodeScheduleIds(int scheduleId, string postalCode)
        {
            return "UPDATE " + dbCurrentDB + ".dbo.PostalCodeModels SET ScheduleId = " + scheduleId + " WHERE PostalCode = '" + postalCode + "'";
        }
        public static string AddContactsToCustomersWithout
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.Contacts (RUT, InvoiceReference, Notify, PersonId, CleaningObjectId)
                    SELECT 0.0, 0, 1, c.PersonId, co.Id
                    FROM {0}.dbo.Customers c
                    JOIN {0}.dbo.CleaningObjects co ON c.Id = co.CustomerId
                    WHERE c.PersonId NOT IN (
                        SELECT PersonId FROM {0}.dbo.Contacts
                    )
                ", dbCurrentDB);
            }
        }
        public static string AddContactsToCleaningObjectsWithout
        {
            get
            {
                return string.Format(@"
                    INSERT INTO {0}.dbo.Contacts (RUT, InvoiceReference, Notify, PersonId, CleaningObjectId)
                    SELECT 0.0, 0, 1, p.Id, co.Id
                    FROM {0}.dbo.Persons p
                    JOIN {0}.dbo.Customers cust ON cust.PersonId = p.Id
                    JOIN {0}.dbo.CleaningObjects co ON co.CustomerId = cust.Id
                    WHERE co.Id NOT IN(
                        SELECT CleaningObjectId FROM {0}.dbo.Contacts
                    )
                ", dbCurrentDB);
            }
        }

        public static string UpdateRUTOnContacts
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.Contacts SET RUT = CASE WHEN twc.full_reduction_pot = 0 THEN
                            CASE WHEN twc.taxreduction_percentage = 0 THEN 1 ELSE twc.taxreduction_percentage / 100 END
                        ELSE 0 END
                    FROM {0}.dbo.Customers c
                    JOIN {0}.dbo.Persons p ON c.PersonId = p.Id
                    JOIN eriks_migration.dbo.TW_clients twc ON twc.clientnbr = c.Id
                    JOIN {0}.dbo.Contacts con ON con.PersonId = p.Id
                ", dbCurrentDB);
            }
        }

        public static string UpdateRUTByMainTWContacts
        {
            get
            {
                return string.Format(@"
                    UPDATE {0}.dbo.Contacts SET RUT = 0
                    WHERE PersonId IN (
	                    SELECT distinct twcons.id
	                    FROM {0}.dbo.Customers c
	                    JOIN {0}.dbo.Persons p ON c.PersonId = p.Id
	                    JOIN eriks_migration.dbo.TW_clients twc ON twc.clientnbr = c.Id
	                    JOIN eriks_migration.dbo.TW_clients twcons ON twcons.mother_id = twc.id
	                    JOIN {0}.dbo.Contacts con ON con.PersonId = p.Id
	                    WHERE twc.full_reduction_pot = 0
                    )
                ", dbCurrentDB);
            }
        }

        public static string GetCoIdsFromContactsWithTooMuchRut
        {
            get
            {
                return string.Format(@"
                    SELECT DISTINCT c.CleaningObjectId FROM {0}.dbo.Contacts c
                    JOIN (
                        SELECT CleaningObjectId, SUM(RUT) AS rutSum FROM {0}.dbo.Contacts
                        GROUP BY CleaningObjectId
                    ) x
                    ON x.CleaningObjectId = c.CleaningObjectId
                    WHERE rutSum > 1                    
                ", dbCurrentDB);
            }
        }
    }
}
