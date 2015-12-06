-- Users
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Users ON
INSERT INTO @TARGET_DATABASE.dbo.Users (Id, Username, Permissions)
	SELECT id, CONCAT(firstname, ' ', lastname), CASE WHEN role_id = 1 THEN 96 ELSE 63 END
	FROM @SOURCE_DATABASE.dbo.TW_employees e
	WHERE deleted = 'N'
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Users OFF;
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

-- Persons linked to postal codes and Cleaning Objects created (after postal codes are linked). This is done in the application