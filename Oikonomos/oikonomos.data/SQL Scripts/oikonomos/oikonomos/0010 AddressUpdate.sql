exec sp_executesql N'
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
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

ALTER TABLE dbo.Address ADD
	Lat numeric(18, 6) NOT NULL CONSTRAINT DF_Address_Lat DEFAULT 0,
	Long numeric(18, 6) NOT NULL CONSTRAINT DF_Address_Long DEFAULT 0

ALTER TABLE dbo.Address SET (LOCK_ESCALATION = TABLE)

COMMIT
'