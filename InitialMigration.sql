-- Users
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Users ON
INSERT INTO @TARGET_DATABASE.dbo.Users (
	Id,
	Username, 
	[Permissions])
SELECT 
	id, 
	CONCAT(firstname, ' ', lastname), 
	CASE 
		WHEN role_id = 1 
			THEN 96 
		ELSE 63 
	END
FROM @SOURCE_DATABASE.dbo.TW_employees e
WHERE deleted = 'N'
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Users OFF;
GO

-- Workers
INSERT INTO @TARGET_DATABASE.dbo.Workers (
	UserId, PersonalNo, FirstName, 
	LastName, PrivatePhone, [Address], 
	City, Zip, Email, 
	StartDate, CurActive)
SELECT 
	id, persnbr, firstname, 
	lastname, phone, [address], 
	city, postalcode, email,
	COALESCE(ctime,GETDATE()),
	1
FROM @SOURCE_DATABASE.dbo.TW_employees WHERE deleted = 'N' AND role_id = 1
GO

-- Banks
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Banks ON
INSERT INTO @TARGET_DATABASE.dbo.Banks (Id, BankId, Name)
	SELECT b.id AS Id, enar AS BankId, name AS Name 
	FROM @SOURCE_DATABASE.dbo.TW_banks b
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Banks OFF
GO

-- Persons
SET IDENTITY_INSERT  @TARGET_DATABASE.dbo.Persons ON
INSERT INTO @TARGET_DATABASE.dbo.Persons (
	Id, PersonalNo, 
	FirstName, 
	LastName, 
	WorkPhone, PrivatePhone, Email, MobilePhone, PersonType, 
	CompanyName, 
	NoPersonalNoValidation)
SELECT id, persnbr, 
	CASE WHEN clienttype_id = 1 THEN firstname ELSE Null END,
	CASE WHEN clienttype_id = 1 THEN lastname ELSE Null END, 
	workphone, phone, email, mobile, clienttype_id,
	CASE WHEN clienttype_id = 2 THEN companyname ELSE Null END,
	CASE WHEN (persnbr = '' or persnbr is null) THEN 1 ELSE 0 END
FROM @SOURCE_DATABASE.dbo.TW_clients WHERE deleted = 'N'
SET IDENTITY_INSERT  @TARGET_DATABASE.dbo.Persons OFF
GO

-- Customers
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Customers ON
INSERT INTO @TARGET_DATABASE.dbo.Customers (
	Id, PersonId, IsInactive, 
	IsInvoicable, 
	InvoiceMethod, 
	IsCreditBlocked,
	PaymentTerms,
	BankId)
SELECT DISTINCT client.clientnbr AS Id, client.Id AS PersonId, 0 AS IsInactive,
	1 AS IsInvoicable, 
	CASE 
		WHEN client.paymenttype = 4 
			THEN 1 
		WHEN client.paymenttype = 7 
			THEN 2 
		WHEN client.paymenttype = 2 
			THEN 0 
		ELSE 3 
	END AS InvoiceMethod, 
	0 AS IsCreditBlocked, 
	paymentterms,
	CASE
		WHEN client.bank_id = 0 
			THEN NULL 
		ELSE client.bank_id 
	END AS BankId 
FROM @SOURCE_DATABASE.dbo.TW_clients client
INNER JOIN @SOURCE_DATABASE.dbo.TW_clientaddresses address ON client.id = address.client_id
WHERE address.is_invoice = 'Y' AND client.deleted = 'N' AND mother_id = 0
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Customers OFF
GO

-- Settings
INSERT INTO @TARGET_DATABASE.dbo.Settings([Key],[Value])
VALUES
	 ( 'SMSURL', 'https://api.beepsend.com/2/sms/' ),
	 ( 'SMSToken', '4027747559efb58a31aeb06606371e4f1e8b08c47a9fa5b15fcfd1aac099f858' ),
	 ( 'SMSMessage', 'Fönsterputsningen för {0} {1} i {2} är färdigt.\nMed vänlig hälsning\nEriks fönsterputs' ),
	 ( 'SMSDefaultCountryCode', '46' ),
	 ( 'SMSDebugPhoneNumber', '+46739105500'),--+46702307254
	 ( 'MOMS', '0.25' ),
	 ( 'ReminderText', 'Påminnelse\nRad1\nRad2' ),
	 ( 'ReminderTaxReductionText', 'Text om att skattereduktion avvisas\nRad1\nRad2\nRad3' ),
	 ( 'ReminderDebtCollectionText', 'Inkassotext\nRad1\nRad2' ),
	 ( 'SMTPUsername', 'noreply@eriksfonsterputs.se' ),
	 ( 'SMTPPassword', 'svarainte' ),
	 ( 'SMTPHost', 'smtp.gmail.com' ),
	 ( 'SMTPPortTLS', '465' ),
	 ( 'SMTPPortSSL', '587' ),
	 ( 'ConfirmationMailSubject', 'Fönsterputs' ),
	 ( 'ConfirmationMailContent', 'Bästa kund!<br/><br/><br/><br/>Vi har idag putsat fönster på {0}. Vill du kontakta oss gällande putsningen, vänligen ring kundtjänst på 0771-424242 och ange refnr {1}.<br /><br />Miljövän? Tänk på att du kan anmäla dig för e-faktura hos din bank! <br /><br />Med vänliga hälsningar, {2}<br />Eriks Fönsterputs' ),
	 ( 'DebugMailAddress', 'msp@learningwell.se' ),
	 ( 'ProductionMode', 'false' ),
	 ( 'InvoiceMailSubject', 'Faktura' ),
	 ( 'InvoiceMailContent', 'Bästa kund!<br/><br/><br/><br/>I medföljande bilaga finner du en faktura från Eriks Fönsterputs.<br /><br /> Vill du kontakta oss gällande frågor kring fakturan, vänligen ring kundtjänst på 0771-424242.<br /><br />Med vänliga hälsningar<br />Eriks Fönsterputs' ),
	 ( 'SubscriptionMailSubject', 'Bekräftelse' ),
	 ( 'SubscriptionMailMessage', 'Meddelande 1' ),
	 ( 'SubscriptionMailMessage2', 'Meddelande 2' ),
	 ( 'SubscriptionMailMessage3', 'Meddelande 3' ),
	 ( 'SubscriptionMailMessage4', 'Meddelande 4' ),
	 ( 'SubscriptionMailMessage5', 'Meddelande 5' );
GO

-- Accounts
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Accounts ON
INSERT INTO @TARGET_DATABASE.dbo.Accounts (Id, Name, AccountNumber, Designation, AccountType)
VALUES 
	 (1, 'Intäkter abonnemang', 3001, 'Intäkter abonnemang', 4),
	 (2, 'Kundfordran', 1510, 'Kundfordran', 1),
	 (3, 'Bank', 1930, 'Bank', 2),
	 (4, 'Fordran skatteverket', 1513, 'Fordran skatteverket', 1),
	 (5, 'Utgående moms', 2611, 'Utgående moms', 3),
	 (6, 'Öres- och kronutjämning', 3740, 'Öres- och kronutjämning', 4),
	 (7, 'Intäkter engångsputs', 3003, 'Intäkter engångsputs', 4),
	 (8, 'Intäkter alla sidor', 3002, 'Intäkter alla sidor', 4),
	 (9, 'Intäkt körersättning', 3009, 'Intäkt körersättning', 4),
	 (10, 'Administrativa', 3540, 'Administrativa', 4),
	 (11, 'Lämnade rabatter', 3730, 'Lämnade rabatter', 4),
	 (12, 'Påminnelseavgift', 3930, 'Påminnelseavgift', 4),
	 (14, 'Special', 0, NULL, 0),
	 (15, 'Kassa', 1910, 'Kassa', 2),
	 (16, 'PlusGiro', 1920, 'PlusGiro', 2),
	 (17, 'Förskott (skuld)', 2420, 'Förskott (skuld)', 2),
	 (18, 'Presentkort (skuld)', 2421, 'Presentkort (skuld)', 2),
	 (19, 'Övrig försäljning', 3051, 'Övrig försäljning', 4),
	 (20, 'Lämnade Kampanjrabatter', 3732, NULL, 4),
	 (21, 'Putsskada', 4602, 'Putsskada', 4);
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Accounts OFF
GO

-- SubCategories
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.SubCategories ON
INSERT INTO @TARGET_DATABASE.dbo.SubCategories (Id, [Type], Name, Category)
VALUES
	(1, 'Tillägg', 'Övriga', 6),
	(2, 'Tillval', 'Generella Tillval', 2),
	(3, 'Tillval', 'Uterum', 2),
	(4, 'Tillval', 'Övervåning', 2),
	(5, 'Tillval', 'Källarvåning', 2),
	(6,'Övrigt', 'Extra', 4),
	(7, 'Övrigt', 'Alla sidor', 4),
	(8, 'Övrigt', 'Övrig Ekonomi', 4),
	(9, 'Övrigt', 'Textrad', 4),
	(10, 'Grundpris', 'Grundputsning', 1),
	(11, 'Övrigt', 'Admin o utkörning', 7),
	(12, 'Övrigt', 'Övrigt', 4);
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.SubCategories OFF

-- Services
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.[Services] ON
INSERT INTO @TARGET_DATABASE.dbo.[Services] (
	Id, Category, Name, 
	AccountId, RUT, Vat, 
	CalcHourly, CalcPercentage, CalcArea, 
	CalcNone, [From], Circa,
	VisibleOnInvoice, IsSalarySetting, 
	SortOrder, IsDefault, SubCategoryId)
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
		WHEN 14 THEN 21
		END AS AccountId,
	CASE WHEN deductible = 2 THEN 1 ELSE 0 END AS RUT,
	CASE WHEN vat = 25 THEN 1 ELSE 0 END AS Vat,
	CASE WHEN unit_id = 3 THEN 1 ELSE 0 END AS CalcHourly,
	0 AS CalcPercentage,
	0 AS CalcArea,
	0 AS CalcNone,
	0 AS [From],
	1 AS Circa,
	CASE WHEN visible_invoice = 'Y' THEN 1 ELSE 0 END AS VisibleOnInvoice,
	CASE WHEN servicetype_id IN (1, 3) THEN 1 ELSE 0 END AS IsSalarySetting,
	id AS SortOrder,
	CASE WHEN name = 'BV - Grundpris' OR name = 'Inställelse- o adm.avgift' THEN 1 ELSE 0 END AS IsDefault,
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
	FROM @SOURCE_DATABASE.dbo.TW_services WHERE deleted = 'N'
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.[Services] OFF

-- Reset locking table
UPDATE @TARGET_DATABASE.dbo.TableLocks SET NextInvoiceNumberTable = 0
GO


-- Persons are linked to postal codes and Cleaning Objects are beeing created (after postal codes are linked). 
-- This is done in the application after termination of this script