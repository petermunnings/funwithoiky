exec sp_executesql N'/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.Person
	DROP CONSTRAINT FK_Person_Address

ALTER TABLE dbo.Address SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.Person
	DROP COLUMN Surname, AddressId

ALTER TABLE dbo.Person SET (LOCK_ESCALATION = TABLE)

COMMIT
'
