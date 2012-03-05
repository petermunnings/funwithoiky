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

ALTER TABLE dbo.Person
	DROP CONSTRAINT FK_Person_Church

ALTER TABLE dbo.Church SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.Person
	DROP CONSTRAINT FK_Person_Site

ALTER TABLE dbo.Site SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.Person
	DROP CONSTRAINT FK_Person_Family

ALTER TABLE dbo.Family SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.Person
	DROP CONSTRAINT FK_Person_Title

ALTER TABLE dbo.Title SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.Person
	DROP CONSTRAINT DF_Person_Created

ALTER TABLE dbo.Person
	DROP CONSTRAINT DF_Person_Changed

CREATE TABLE dbo.Tmp_Person
	(
	PersonId int NOT NULL IDENTITY (1, 1),
	ChurchId int NOT NULL,
	SiteId int NULL,
	TitleId int NULL,
	Firstname nvarchar(255) NOT NULL,
	Email nvarchar(255) NULL,
	FamilyId int NOT NULL,
	Created datetime NOT NULL,
	Changed datetime NOT NULL,
	DateOfBirth datetime NULL,
	Occupation nvarchar(255) NULL,
	Username nvarchar(50) NULL,
	PasswordHash nvarchar(50) NULL
	)  ON [PRIMARY]

ALTER TABLE dbo.Tmp_Person SET (LOCK_ESCALATION = TABLE)

ALTER TABLE dbo.Tmp_Person ADD CONSTRAINT
	DF_Person_Created DEFAULT (getdate()) FOR Created

ALTER TABLE dbo.Tmp_Person ADD CONSTRAINT
	DF_Person_Changed DEFAULT (getdate()) FOR Changed

SET IDENTITY_INSERT dbo.Tmp_Person ON

IF EXISTS(SELECT * FROM dbo.Person)
	 EXEC(''INSERT INTO dbo.Tmp_Person (PersonId, ChurchId, SiteId, TitleId, Firstname, Email, FamilyId, Created, Changed, DateOfBirth, Occupation, Username, PasswordHash)
		SELECT PersonId, ChurchId, SiteId, TitleId, Firstname, Email, FamilyId, Created, Changed, DateOfBirth, Occupation, Username, PasswordHash FROM dbo.Person WITH (HOLDLOCK TABLOCKX)'')

SET IDENTITY_INSERT dbo.Tmp_Person OFF

ALTER TABLE dbo.PersonNews
	DROP CONSTRAINT FK_PersonNews_Person

ALTER TABLE dbo.PersonGroup
	DROP CONSTRAINT FK_PersonGroup_Person

ALTER TABLE dbo.[Group]
	DROP CONSTRAINT FK_Group_Leader

ALTER TABLE dbo.[Group]
	DROP CONSTRAINT FK_Group_Administrator

ALTER TABLE dbo.PersonContact
	DROP CONSTRAINT FK_PersonContact_Person

ALTER TABLE dbo.PersonRole
	DROP CONSTRAINT FK_PersonRole_Person

DROP TABLE dbo.Person

EXECUTE sp_rename N''dbo.Tmp_Person'', N''Person'', ''OBJECT'' 

ALTER TABLE dbo.Person ADD CONSTRAINT
	PK_Person PRIMARY KEY CLUSTERED 
	(
	PersonId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


ALTER TABLE dbo.Person ADD CONSTRAINT
	FK_Person_Title FOREIGN KEY
	(
	TitleId
	) REFERENCES dbo.Title
	(
	TitleId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.Person ADD CONSTRAINT
	FK_Person_Family FOREIGN KEY
	(
	FamilyId
	) REFERENCES dbo.Family
	(
	FamilyId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.Person ADD CONSTRAINT
	FK_Person_Site FOREIGN KEY
	(
	SiteId
	) REFERENCES dbo.Site
	(
	SiteId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.Person ADD CONSTRAINT
	FK_Person_Church FOREIGN KEY
	(
	ChurchId
	) REFERENCES dbo.Church
	(
	ChurchId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.PersonRole ADD CONSTRAINT
	FK_PersonRole_Person FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.Person
	(
	PersonId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.PersonRole SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.PersonContact ADD CONSTRAINT
	FK_PersonContact_Person FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.Person
	(
	PersonId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.PersonContact SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.[Group] ADD CONSTRAINT
	FK_Group_Leader FOREIGN KEY
	(
	LeaderId
	) REFERENCES dbo.Person
	(
	PersonId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.[Group] ADD CONSTRAINT
	FK_Group_Administrator FOREIGN KEY
	(
	AdministratorId
	) REFERENCES dbo.Person
	(
	PersonId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.[Group] SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.PersonGroup ADD CONSTRAINT
	FK_PersonGroup_Person FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.Person
	(
	PersonId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.PersonGroup SET (LOCK_ESCALATION = TABLE)

COMMIT
BEGIN TRANSACTION

ALTER TABLE dbo.PersonNews ADD CONSTRAINT
	FK_PersonNews_Person FOREIGN KEY
	(
	PersonId
	) REFERENCES dbo.Person
	(
	PersonId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.PersonNews SET (LOCK_ESCALATION = TABLE)

COMMIT
'
