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

ALTER TABLE dbo.Address SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.Family ADD
	HomePhone nvarchar(50) NULL,
	AddressId int NULL,
	Anniversary datetime NULL,
	ChurchId int NULL

ALTER TABLE dbo.Family ADD CONSTRAINT
	FK_Family_Address FOREIGN KEY
	(
	AddressId
	) REFERENCES dbo.Address
	(
	AddressId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.Family SET (LOCK_ESCALATION = TABLE)

COMMIT
'