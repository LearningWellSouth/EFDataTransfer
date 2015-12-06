-- A contact is a person that is not a customer. 
-- That means: they are only used as contacts, they don't pay the bill
-- and they are not the "primary" as seen from the cleaning object
INSERT INTO @TARGET_DATABASE.dbo.Contacts(
	PersonId, CleaningObjectId, RUT, InvoiceReference, Notify)
	SELECT child.id, address.id, 1, 0, 1
		FROM @SOURCE_DATABASE.dbo.TW_clients mother
		INNER JOIN @SOURCE_DATABASE.dbo.TW_clients child ON child.mother_id = mother.id AND child.deleted = 'N'
		INNER JOIN @SOURCE_DATABASE.dbo.TW_clientaddresses address ON address.client_id = mother.id AND address.deleted = 'N'
		INNER JOIN @TARGET_DATABASE.dbo.CleaningObjects cleaningObj WHERE cleaningObj.Id = address.id
		WHERE mother.deleted = 'N'
GO
UPDATE @TARGET_DATABASE@.dbo.Contacts SET RUT = 
	CASE 
		WHEN twc.full_reduction_pot = 0 AND twc.persnbr IS NOT NULL AND twc.persnbr <> ''
		THEN 
			CASE
				WHEN twc.taxreduction_percentage = 0 
				THEN 1
				ELSE twc.taxreduction_percentage / 100 
			END
		ELSE 0 
	END
FROM @TARGET_DATABASE@.dbo.Contacts c
INNER JOIN @SOURCE_DATABASE.dbo.TW_clients twc ON twc.id = c.PersonId
GO
UPDATE @TARGET_DATABASE.dbo.Contacts SET RUT = 0 WHERE PersonId IN (SELECT Id FROM @TARGET_DATABASE.dbo.Persons WHERE NoPersonalNoValidation = 1)
GO
UPDATE @TARGET_DATABASE.dbo.Contacts SET RUT = 0 WHERE PersonId IN (SELECT Id FROM @TARGET_DATABASE.dbo.Persons WHERE PersonType = 2)
GO