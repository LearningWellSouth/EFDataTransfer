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
	COALESCE(ctime,GETDATE),
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
SET IDENTITY_INSERT  @TARGET_DATABASE @.dbo.Persons ON
INSERT INTO @TARGET_DATABASE @.dbo.Persons (
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
SET IDENTITY_INSERT  @TARGET_DATABASE @.dbo.Persons OFF
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

-- Reset locking table
UPDATE @TARGET_DATABASE.dbo.TableLocks SET NextInvoiceNumberTable = 0
GO


-- Persons are linked to postal codes and Cleaning Objects are beeing created (after postal codes are linked). 
-- This is done in the application after termination of this script