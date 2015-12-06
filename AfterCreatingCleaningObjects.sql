-- A contact is a person that is not a customer. 
-- That means: they are only used as contacts, they don't pay the bill
-- and they are not the "primary" as seen from the cleaning object
-- have a dependency towards the CleaningObjects in the target
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

-- bind customers to cleaning objects TODO these joins are just weird! 
-- The final right join could cause us to try and update the (null) element (the empty set) in CleaningObjects
-- better is to do this in the code (when cleaning objects are created in the first place)
UPDATE @TARGET_DATABASE.dbo.CleaningObjects SET CustomerId = cust.Id
FROM @TARGET_DATABASE.dbo.CleaningObjects co 
LEFT JOIN eriks_migration.dbo.TW_clientaddresses cla ON co.Id = cla.id 
RIGHT JOIN @TARGET_DATABASE.dbo.Customers cust ON cla.client_id = cust.PersonId
GO

-- TODO
-- This seems to try and find the conjugate of the intersection between the set of cleaning objects 
-- with existing "contacts" (discribed above) and those who are yet to get one. T-SQL statement is named "except"
INSERT INTO @TARGET_DATABASE.dbo.Contacts (RUT, InvoiceReference, Notify, PersonId, CleaningObjectId)
SELECT 1.0, 0, 1, p.Id, co.Id
FROM @TARGET_DATABASE.dbo.Persons p
JOIN @TARGET_DATABASE.dbo.Customers cust ON cust.PersonId = p.Id
JOIN @TARGET_DATABASE.dbo.CleaningObjects co ON co.CustomerId = cust.Id
WHERE co.Id NOT IN (
	SELECT CleaningObjectId FROM @TARGET_DATABASE.dbo.Contacts)
GO