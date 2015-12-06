-- Users & privileges
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Users ON
INSERT INTO @TARGET_DATABASE.dbo.Users (Id, Username, Permissions)
	SELECT id, CONCAT(firstname, ' ', lastname), CASE WHEN role_id = 1 THEN 96 ELSE 63 END
	FROM @SOURCE_DATABASE.dbo.TW_employees e
	WHERE deleted = 'N'
		AND e.id NOT IN (SELECT Id FROM @TARGET_DATABASE.dbo.Users)
SET IDENTITY_INSERT @TARGET_DATABASE.dbo.Users OFF;