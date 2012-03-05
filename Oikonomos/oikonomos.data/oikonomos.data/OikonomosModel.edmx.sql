
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/30/2011 08:57:12
-- Generated from EDMX file: C:\Users\peter\Documents\Sandton City Church\SCC SVN\trunk\oikonomos.data\oikonomos.data\OikonomosModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [oikonomos];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Address_ChurchSuburb]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Address] DROP CONSTRAINT [FK_Address_ChurchSuburb];
GO
IF OBJECT_ID(N'[dbo].[FK_Church_Address]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Church] DROP CONSTRAINT [FK_Church_Address];
GO
IF OBJECT_ID(N'[dbo].[FK_ChurchOptionalField_Church]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChurchOptionalField] DROP CONSTRAINT [FK_ChurchOptionalField_Church];
GO
IF OBJECT_ID(N'[dbo].[FK_ChurchOptionalField_OptionalField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChurchOptionalField] DROP CONSTRAINT [FK_ChurchOptionalField_OptionalField];
GO
IF OBJECT_ID(N'[dbo].[FK_ChurchSuburb_Church]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChurchSuburb] DROP CONSTRAINT [FK_ChurchSuburb_Church];
GO
IF OBJECT_ID(N'[dbo].[FK_Event_ChangedByPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Event] DROP CONSTRAINT [FK_Event_ChangedByPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_Event_CreatedByPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Event] DROP CONSTRAINT [FK_Event_CreatedByPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_Event_EventVisibility]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Event] DROP CONSTRAINT [FK_Event_EventVisibility];
GO
IF OBJECT_ID(N'[dbo].[FK_Event_Table]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Event] DROP CONSTRAINT [FK_Event_Table];
GO
IF OBJECT_ID(N'[dbo].[FK_EventType_Church]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventType] DROP CONSTRAINT [FK_EventType_Church];
GO
IF OBJECT_ID(N'[dbo].[FK_EventType_Table]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EventType] DROP CONSTRAINT [FK_EventType_Table];
GO
IF OBJECT_ID(N'[dbo].[FK_Family_Address]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Family] DROP CONSTRAINT [FK_Family_Address];
GO
IF OBJECT_ID(N'[dbo].[FK_Group_Address]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_Address];
GO
IF OBJECT_ID(N'[dbo].[FK_Group_Administrator]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_Administrator];
GO
IF OBJECT_ID(N'[dbo].[FK_Group_Church]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_Church];
GO
IF OBJECT_ID(N'[dbo].[FK_Group_GroupClassification]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_GroupClassification];
GO
IF OBJECT_ID(N'[dbo].[FK_Group_GroupType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_GroupType];
GO
IF OBJECT_ID(N'[dbo].[FK_Group_Leader]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_Leader];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupClassification_Church]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupClassification] DROP CONSTRAINT [FK_GroupClassification_Church];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupClassification_GroupType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupClassification] DROP CONSTRAINT [FK_GroupClassification_GroupType];
GO
IF OBJECT_ID(N'[dbo].[FK_Person_Church]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person] DROP CONSTRAINT [FK_Person_Church];
GO
IF OBJECT_ID(N'[dbo].[FK_Person_Family]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person] DROP CONSTRAINT [FK_Person_Family];
GO
IF OBJECT_ID(N'[dbo].[FK_Person_Site]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person] DROP CONSTRAINT [FK_Person_Site];
GO
IF OBJECT_ID(N'[dbo].[FK_Person_Title]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person] DROP CONSTRAINT [FK_Person_Title];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonGroup_Group]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonGroup] DROP CONSTRAINT [FK_PersonGroup_Group];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonGroup_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonGroup] DROP CONSTRAINT [FK_PersonGroup_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonOptionalField_OptionalField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonOptionalField] DROP CONSTRAINT [FK_PersonOptionalField_OptionalField];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonOptionField_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonOptionalField] DROP CONSTRAINT [FK_PersonOptionField_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonRelationship_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonRelationship] DROP CONSTRAINT [FK_PersonRelationship_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonRelationship_PersonRelatedTo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonRelationship] DROP CONSTRAINT [FK_PersonRelationship_PersonRelatedTo];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonRelationship_Relationship]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonRelationship] DROP CONSTRAINT [FK_PersonRelationship_Relationship];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonRole_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonRole] DROP CONSTRAINT [FK_PersonRole_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonRole_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonRole] DROP CONSTRAINT [FK_PersonRole_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_Site_Address]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Site] DROP CONSTRAINT [FK_Site_Address];
GO
IF OBJECT_ID(N'[dbo].[FK_Site_Church]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Site] DROP CONSTRAINT [FK_Site_Church];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Address];
GO
IF OBJECT_ID(N'[dbo].[Church]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Church];
GO
IF OBJECT_ID(N'[dbo].[ChurchOptionalField]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChurchOptionalField];
GO
IF OBJECT_ID(N'[dbo].[ChurchSuburb]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChurchSuburb];
GO
IF OBJECT_ID(N'[dbo].[Event]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Event];
GO
IF OBJECT_ID(N'[dbo].[EventType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventType];
GO
IF OBJECT_ID(N'[dbo].[EventVisibility]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EventVisibility];
GO
IF OBJECT_ID(N'[dbo].[Family]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Family];
GO
IF OBJECT_ID(N'[dbo].[Group]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Group];
GO
IF OBJECT_ID(N'[dbo].[GroupClassification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupClassification];
GO
IF OBJECT_ID(N'[dbo].[GroupType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupType];
GO
IF OBJECT_ID(N'[dbo].[OptionalField]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OptionalField];
GO
IF OBJECT_ID(N'[dbo].[Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person];
GO
IF OBJECT_ID(N'[dbo].[PersonGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonGroup];
GO
IF OBJECT_ID(N'[dbo].[PersonOptionalField]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonOptionalField];
GO
IF OBJECT_ID(N'[dbo].[PersonRelationship]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonRelationship];
GO
IF OBJECT_ID(N'[dbo].[PersonRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonRole];
GO
IF OBJECT_ID(N'[dbo].[Relationship]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Relationship];
GO
IF OBJECT_ID(N'[dbo].[Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Role];
GO
IF OBJECT_ID(N'[dbo].[Site]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Site];
GO
IF OBJECT_ID(N'[dbo].[Table]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Table];
GO
IF OBJECT_ID(N'[dbo].[Title]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Title];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Churches'
CREATE TABLE [dbo].[Churches] (
    [ChurchId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL,
    [SiteHeader] nvarchar(255)  NULL,
    [SiteDescription] nvarchar(255)  NULL,
    [BackgroundImage] nvarchar(50)  NULL,
    [UITheme] nvarchar(50)  NULL,
    [AddressId] int  NULL,
    [SendWelcome] bit  NOT NULL,
    [Url] nvarchar(255)  NULL,
    [OfficePhone] nvarchar(50)  NULL,
    [OfficeEmail] nvarchar(255)  NULL,
    [Province] nvarchar(50)  NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [GroupId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [LeaderId] int  NULL,
    [GroupTypeId] int  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL,
    [ChurchId] int  NOT NULL,
    [AdministratorId] int  NULL,
    [AddressId] int  NULL,
    [GroupClassificationId] int  NULL
);
GO

-- Creating table 'GroupTypes'
CREATE TABLE [dbo].[GroupTypes] (
    [GroupTypeId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL
);
GO

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [PersonId] int IDENTITY(1,1) NOT NULL,
    [ChurchId] int  NOT NULL,
    [SiteId] int  NULL,
    [TitleId] int  NULL,
    [Firstname] nvarchar(255)  NOT NULL,
    [Email] nvarchar(255)  NULL,
    [FamilyId] int  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL,
    [DateOfBirth] datetime  NULL,
    [Occupation] nvarchar(255)  NULL,
    [Username] nvarchar(50)  NULL,
    [PasswordHash] nvarchar(50)  NULL,
    [Anniversary] datetime  NULL,
    [PublicId] nvarchar(50)  NULL
);
GO

-- Creating table 'PersonGroups'
CREATE TABLE [dbo].[PersonGroups] (
    [PersonGroupId] int IDENTITY(1,1) NOT NULL,
    [PersonId] int  NOT NULL,
    [GroupId] int  NOT NULL,
    [Joined] datetime  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL
);
GO

-- Creating table 'Sites'
CREATE TABLE [dbo].[Sites] (
    [SiteId] int IDENTITY(1,1) NOT NULL,
    [ChurchId] int  NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL,
    [AddressId] int  NULL
);
GO

-- Creating table 'Titles'
CREATE TABLE [dbo].[Titles] (
    [TitleId] int  NOT NULL,
    [Title1] nvarchar(50)  NOT NULL,
    [Created] datetime  NOT NULL
);
GO

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [AddressId] int IDENTITY(1,1) NOT NULL,
    [Line1] nvarchar(255)  NOT NULL,
    [Line2] nvarchar(255)  NOT NULL,
    [Line3] nvarchar(255)  NOT NULL,
    [Line4] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL,
    [Lat] decimal(18,6)  NOT NULL,
    [Long] decimal(18,6)  NOT NULL,
    [AddressType] nvarchar(255)  NULL,
    [ChurchSuburbId] int  NULL
);
GO

-- Creating table 'PersonRoles'
CREATE TABLE [dbo].[PersonRoles] (
    [RoleId] int  NOT NULL,
    [PersonId] int  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Created] datetime  NOT NULL
);
GO

-- Creating table 'Families'
CREATE TABLE [dbo].[Families] (
    [FamilyId] int IDENTITY(1,1) NOT NULL,
    [FamilyName] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL,
    [HomePhone] nvarchar(50)  NULL,
    [AddressId] int  NULL,
    [Anniversary] datetime  NULL,
    [ChurchId] int  NULL
);
GO

-- Creating table 'PersonRelationships'
CREATE TABLE [dbo].[PersonRelationships] (
    [PersonRelationshipId] int IDENTITY(1,1) NOT NULL,
    [PersonRelatedToId] int  NOT NULL,
    [PersonId] int  NOT NULL,
    [RelationshipId] int  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL
);
GO

-- Creating table 'Relationships'
CREATE TABLE [dbo].[Relationships] (
    [RelationshipId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Created] datetime  NOT NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [EventId] int IDENTITY(1,1) NOT NULL,
    [TableId] int  NOT NULL,
    [Reference] int  NOT NULL,
    [Description] nvarchar(255)  NOT NULL,
    [Comments] nvarchar(max)  NULL,
    [FollowUpDate] datetime  NULL,
    [EventVisibilityId] int  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL,
    [CreatedByPersonId] int  NOT NULL,
    [ChangedByPersonId] int  NOT NULL,
    [EventDate] datetime  NOT NULL,
    [Value] nvarchar(255)  NULL
);
GO

-- Creating table 'EventVisibilities'
CREATE TABLE [dbo].[EventVisibilities] (
    [EventVisibilityId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Created] datetime  NOT NULL
);
GO

-- Creating table 'Tables'
CREATE TABLE [dbo].[Tables] (
    [TableId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Created] datetime  NOT NULL
);
GO

-- Creating table 'OptionalFields'
CREATE TABLE [dbo].[OptionalFields] (
    [OptionalFieldId] int  NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Regex] nvarchar(255)  NULL,
    [Created] datetime  NOT NULL
);
GO

-- Creating table 'ChurchOptionalFields'
CREATE TABLE [dbo].[ChurchOptionalFields] (
    [ChurchOptionalFieldId] int IDENTITY(1,1) NOT NULL,
    [ChurchId] int  NOT NULL,
    [OptionalFieldId] int  NOT NULL,
    [Visible] bit  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL
);
GO

-- Creating table 'PersonOptionalFields'
CREATE TABLE [dbo].[PersonOptionalFields] (
    [PersonOptionalFieldId] int IDENTITY(1,1) NOT NULL,
    [PersonId] int  NOT NULL,
    [OptionalFieldId] int  NOT NULL,
    [Value] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL
);
GO

-- Creating table 'ChurchSuburbs'
CREATE TABLE [dbo].[ChurchSuburbs] (
    [ChurchSuburbId] int IDENTITY(1,1) NOT NULL,
    [ChurchId] int  NOT NULL,
    [Suburb] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL
);
GO

-- Creating table 'GroupClassifications'
CREATE TABLE [dbo].[GroupClassifications] (
    [GroupClassificationId] int IDENTITY(1,1) NOT NULL,
    [ChurchId] int  NOT NULL,
    [GroupTypeId] int  NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL,
    [Changed] datetime  NOT NULL
);
GO

-- Creating table 'EventTypes'
CREATE TABLE [dbo].[EventTypes] (
    [EventTypeId] int  NOT NULL,
    [ChurchId] int IDENTITY(1,1) NOT NULL,
    [TableId] int  NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Created] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ChurchId] in table 'Churches'
ALTER TABLE [dbo].[Churches]
ADD CONSTRAINT [PK_Churches]
    PRIMARY KEY CLUSTERED ([ChurchId] ASC);
GO

-- Creating primary key on [GroupId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([GroupId] ASC);
GO

-- Creating primary key on [GroupTypeId] in table 'GroupTypes'
ALTER TABLE [dbo].[GroupTypes]
ADD CONSTRAINT [PK_GroupTypes]
    PRIMARY KEY CLUSTERED ([GroupTypeId] ASC);
GO

-- Creating primary key on [PersonId] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([PersonId] ASC);
GO

-- Creating primary key on [PersonGroupId] in table 'PersonGroups'
ALTER TABLE [dbo].[PersonGroups]
ADD CONSTRAINT [PK_PersonGroups]
    PRIMARY KEY CLUSTERED ([PersonGroupId] ASC);
GO

-- Creating primary key on [SiteId] in table 'Sites'
ALTER TABLE [dbo].[Sites]
ADD CONSTRAINT [PK_Sites]
    PRIMARY KEY CLUSTERED ([SiteId] ASC);
GO

-- Creating primary key on [TitleId] in table 'Titles'
ALTER TABLE [dbo].[Titles]
ADD CONSTRAINT [PK_Titles]
    PRIMARY KEY CLUSTERED ([TitleId] ASC);
GO

-- Creating primary key on [AddressId] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([AddressId] ASC);
GO

-- Creating primary key on [RoleId], [PersonId] in table 'PersonRoles'
ALTER TABLE [dbo].[PersonRoles]
ADD CONSTRAINT [PK_PersonRoles]
    PRIMARY KEY CLUSTERED ([RoleId], [PersonId] ASC);
GO

-- Creating primary key on [RoleId] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleId] ASC);
GO

-- Creating primary key on [FamilyId] in table 'Families'
ALTER TABLE [dbo].[Families]
ADD CONSTRAINT [PK_Families]
    PRIMARY KEY CLUSTERED ([FamilyId] ASC);
GO

-- Creating primary key on [PersonRelationshipId] in table 'PersonRelationships'
ALTER TABLE [dbo].[PersonRelationships]
ADD CONSTRAINT [PK_PersonRelationships]
    PRIMARY KEY CLUSTERED ([PersonRelationshipId] ASC);
GO

-- Creating primary key on [RelationshipId] in table 'Relationships'
ALTER TABLE [dbo].[Relationships]
ADD CONSTRAINT [PK_Relationships]
    PRIMARY KEY CLUSTERED ([RelationshipId] ASC);
GO

-- Creating primary key on [EventId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([EventId] ASC);
GO

-- Creating primary key on [EventVisibilityId] in table 'EventVisibilities'
ALTER TABLE [dbo].[EventVisibilities]
ADD CONSTRAINT [PK_EventVisibilities]
    PRIMARY KEY CLUSTERED ([EventVisibilityId] ASC);
GO

-- Creating primary key on [TableId] in table 'Tables'
ALTER TABLE [dbo].[Tables]
ADD CONSTRAINT [PK_Tables]
    PRIMARY KEY CLUSTERED ([TableId] ASC);
GO

-- Creating primary key on [OptionalFieldId] in table 'OptionalFields'
ALTER TABLE [dbo].[OptionalFields]
ADD CONSTRAINT [PK_OptionalFields]
    PRIMARY KEY CLUSTERED ([OptionalFieldId] ASC);
GO

-- Creating primary key on [ChurchOptionalFieldId] in table 'ChurchOptionalFields'
ALTER TABLE [dbo].[ChurchOptionalFields]
ADD CONSTRAINT [PK_ChurchOptionalFields]
    PRIMARY KEY CLUSTERED ([ChurchOptionalFieldId] ASC);
GO

-- Creating primary key on [PersonOptionalFieldId] in table 'PersonOptionalFields'
ALTER TABLE [dbo].[PersonOptionalFields]
ADD CONSTRAINT [PK_PersonOptionalFields]
    PRIMARY KEY CLUSTERED ([PersonOptionalFieldId] ASC);
GO

-- Creating primary key on [ChurchSuburbId] in table 'ChurchSuburbs'
ALTER TABLE [dbo].[ChurchSuburbs]
ADD CONSTRAINT [PK_ChurchSuburbs]
    PRIMARY KEY CLUSTERED ([ChurchSuburbId] ASC);
GO

-- Creating primary key on [GroupClassificationId] in table 'GroupClassifications'
ALTER TABLE [dbo].[GroupClassifications]
ADD CONSTRAINT [PK_GroupClassifications]
    PRIMARY KEY CLUSTERED ([GroupClassificationId] ASC);
GO

-- Creating primary key on [EventTypeId] in table 'EventTypes'
ALTER TABLE [dbo].[EventTypes]
ADD CONSTRAINT [PK_EventTypes]
    PRIMARY KEY CLUSTERED ([EventTypeId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ChurchId] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [FK_Person_Church]
    FOREIGN KEY ([ChurchId])
    REFERENCES [dbo].[Churches]
        ([ChurchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Person_Church'
CREATE INDEX [IX_FK_Person_Church]
ON [dbo].[People]
    ([ChurchId]);
GO

-- Creating foreign key on [ChurchId] in table 'Sites'
ALTER TABLE [dbo].[Sites]
ADD CONSTRAINT [FK_Site_Church]
    FOREIGN KEY ([ChurchId])
    REFERENCES [dbo].[Churches]
        ([ChurchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Site_Church'
CREATE INDEX [IX_FK_Site_Church]
ON [dbo].[Sites]
    ([ChurchId]);
GO

-- Creating foreign key on [GroupTypeId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Group_GroupType]
    FOREIGN KEY ([GroupTypeId])
    REFERENCES [dbo].[GroupTypes]
        ([GroupTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Group_GroupType'
CREATE INDEX [IX_FK_Group_GroupType]
ON [dbo].[Groups]
    ([GroupTypeId]);
GO

-- Creating foreign key on [GroupId] in table 'PersonGroups'
ALTER TABLE [dbo].[PersonGroups]
ADD CONSTRAINT [FK_PersonGroup_Group]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Groups]
        ([GroupId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonGroup_Group'
CREATE INDEX [IX_FK_PersonGroup_Group]
ON [dbo].[PersonGroups]
    ([GroupId]);
GO

-- Creating foreign key on [SiteId] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [FK_Person_Site]
    FOREIGN KEY ([SiteId])
    REFERENCES [dbo].[Sites]
        ([SiteId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Person_Site'
CREATE INDEX [IX_FK_Person_Site]
ON [dbo].[People]
    ([SiteId]);
GO

-- Creating foreign key on [TitleId] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [FK_Person_Title]
    FOREIGN KEY ([TitleId])
    REFERENCES [dbo].[Titles]
        ([TitleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Person_Title'
CREATE INDEX [IX_FK_Person_Title]
ON [dbo].[People]
    ([TitleId]);
GO

-- Creating foreign key on [PersonId] in table 'PersonGroups'
ALTER TABLE [dbo].[PersonGroups]
ADD CONSTRAINT [FK_PersonGroup_Person]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonGroup_Person'
CREATE INDEX [IX_FK_PersonGroup_Person]
ON [dbo].[PersonGroups]
    ([PersonId]);
GO

-- Creating foreign key on [ChurchId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Group_Church]
    FOREIGN KEY ([ChurchId])
    REFERENCES [dbo].[Churches]
        ([ChurchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Group_Church'
CREATE INDEX [IX_FK_Group_Church]
ON [dbo].[Groups]
    ([ChurchId]);
GO

-- Creating foreign key on [AdministratorId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Group_Administrator]
    FOREIGN KEY ([AdministratorId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Group_Administrator'
CREATE INDEX [IX_FK_Group_Administrator]
ON [dbo].[Groups]
    ([AdministratorId]);
GO

-- Creating foreign key on [LeaderId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Group_Leader]
    FOREIGN KEY ([LeaderId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Group_Leader'
CREATE INDEX [IX_FK_Group_Leader]
ON [dbo].[Groups]
    ([LeaderId]);
GO

-- Creating foreign key on [PersonId] in table 'PersonRoles'
ALTER TABLE [dbo].[PersonRoles]
ADD CONSTRAINT [FK_PersonRole_Person]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonRole_Person'
CREATE INDEX [IX_FK_PersonRole_Person]
ON [dbo].[PersonRoles]
    ([PersonId]);
GO

-- Creating foreign key on [RoleId] in table 'PersonRoles'
ALTER TABLE [dbo].[PersonRoles]
ADD CONSTRAINT [FK_PersonRole_Role]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AddressId] in table 'Churches'
ALTER TABLE [dbo].[Churches]
ADD CONSTRAINT [FK_Church_Address]
    FOREIGN KEY ([AddressId])
    REFERENCES [dbo].[Addresses]
        ([AddressId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Church_Address'
CREATE INDEX [IX_FK_Church_Address]
ON [dbo].[Churches]
    ([AddressId]);
GO

-- Creating foreign key on [AddressId] in table 'Families'
ALTER TABLE [dbo].[Families]
ADD CONSTRAINT [FK_Family_Address]
    FOREIGN KEY ([AddressId])
    REFERENCES [dbo].[Addresses]
        ([AddressId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Family_Address'
CREATE INDEX [IX_FK_Family_Address]
ON [dbo].[Families]
    ([AddressId]);
GO

-- Creating foreign key on [FamilyId] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [FK_Person_Family]
    FOREIGN KEY ([FamilyId])
    REFERENCES [dbo].[Families]
        ([FamilyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Person_Family'
CREATE INDEX [IX_FK_Person_Family]
ON [dbo].[People]
    ([FamilyId]);
GO

-- Creating foreign key on [PersonId] in table 'PersonRelationships'
ALTER TABLE [dbo].[PersonRelationships]
ADD CONSTRAINT [FK_PersonRelationship_Person]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonRelationship_Person'
CREATE INDEX [IX_FK_PersonRelationship_Person]
ON [dbo].[PersonRelationships]
    ([PersonId]);
GO

-- Creating foreign key on [PersonRelatedToId] in table 'PersonRelationships'
ALTER TABLE [dbo].[PersonRelationships]
ADD CONSTRAINT [FK_PersonRelationship_PersonRelatedTo]
    FOREIGN KEY ([PersonRelatedToId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonRelationship_PersonRelatedTo'
CREATE INDEX [IX_FK_PersonRelationship_PersonRelatedTo]
ON [dbo].[PersonRelationships]
    ([PersonRelatedToId]);
GO

-- Creating foreign key on [RelationshipId] in table 'PersonRelationships'
ALTER TABLE [dbo].[PersonRelationships]
ADD CONSTRAINT [FK_PersonRelationship_Relationship]
    FOREIGN KEY ([RelationshipId])
    REFERENCES [dbo].[Relationships]
        ([RelationshipId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonRelationship_Relationship'
CREATE INDEX [IX_FK_PersonRelationship_Relationship]
ON [dbo].[PersonRelationships]
    ([RelationshipId]);
GO

-- Creating foreign key on [ChangedByPersonId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_Event_ChangedByPerson]
    FOREIGN KEY ([ChangedByPersonId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Event_ChangedByPerson'
CREATE INDEX [IX_FK_Event_ChangedByPerson]
ON [dbo].[Events]
    ([ChangedByPersonId]);
GO

-- Creating foreign key on [CreatedByPersonId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_Event_CreatedByPerson]
    FOREIGN KEY ([CreatedByPersonId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Event_CreatedByPerson'
CREATE INDEX [IX_FK_Event_CreatedByPerson]
ON [dbo].[Events]
    ([CreatedByPersonId]);
GO

-- Creating foreign key on [EventVisibilityId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_Event_EventVisibility]
    FOREIGN KEY ([EventVisibilityId])
    REFERENCES [dbo].[EventVisibilities]
        ([EventVisibilityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Event_EventVisibility'
CREATE INDEX [IX_FK_Event_EventVisibility]
ON [dbo].[Events]
    ([EventVisibilityId]);
GO

-- Creating foreign key on [TableId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_Event_Table]
    FOREIGN KEY ([TableId])
    REFERENCES [dbo].[Tables]
        ([TableId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Event_Table'
CREATE INDEX [IX_FK_Event_Table]
ON [dbo].[Events]
    ([TableId]);
GO

-- Creating foreign key on [AddressId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Group_Address]
    FOREIGN KEY ([AddressId])
    REFERENCES [dbo].[Addresses]
        ([AddressId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Group_Address'
CREATE INDEX [IX_FK_Group_Address]
ON [dbo].[Groups]
    ([AddressId]);
GO

-- Creating foreign key on [AddressId] in table 'Sites'
ALTER TABLE [dbo].[Sites]
ADD CONSTRAINT [FK_Site_Address]
    FOREIGN KEY ([AddressId])
    REFERENCES [dbo].[Addresses]
        ([AddressId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Site_Address'
CREATE INDEX [IX_FK_Site_Address]
ON [dbo].[Sites]
    ([AddressId]);
GO

-- Creating foreign key on [ChurchId] in table 'ChurchOptionalFields'
ALTER TABLE [dbo].[ChurchOptionalFields]
ADD CONSTRAINT [FK_ChurchOptionalField_Church]
    FOREIGN KEY ([ChurchId])
    REFERENCES [dbo].[Churches]
        ([ChurchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChurchOptionalField_Church'
CREATE INDEX [IX_FK_ChurchOptionalField_Church]
ON [dbo].[ChurchOptionalFields]
    ([ChurchId]);
GO

-- Creating foreign key on [OptionalFieldId] in table 'ChurchOptionalFields'
ALTER TABLE [dbo].[ChurchOptionalFields]
ADD CONSTRAINT [FK_ChurchOptionalField_OptionalField]
    FOREIGN KEY ([OptionalFieldId])
    REFERENCES [dbo].[OptionalFields]
        ([OptionalFieldId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChurchOptionalField_OptionalField'
CREATE INDEX [IX_FK_ChurchOptionalField_OptionalField]
ON [dbo].[ChurchOptionalFields]
    ([OptionalFieldId]);
GO

-- Creating foreign key on [OptionalFieldId] in table 'PersonOptionalFields'
ALTER TABLE [dbo].[PersonOptionalFields]
ADD CONSTRAINT [FK_PersonOptionalField_OptionalField]
    FOREIGN KEY ([OptionalFieldId])
    REFERENCES [dbo].[OptionalFields]
        ([OptionalFieldId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonOptionalField_OptionalField'
CREATE INDEX [IX_FK_PersonOptionalField_OptionalField]
ON [dbo].[PersonOptionalFields]
    ([OptionalFieldId]);
GO

-- Creating foreign key on [PersonId] in table 'PersonOptionalFields'
ALTER TABLE [dbo].[PersonOptionalFields]
ADD CONSTRAINT [FK_PersonOptionField_Person]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonOptionField_Person'
CREATE INDEX [IX_FK_PersonOptionField_Person]
ON [dbo].[PersonOptionalFields]
    ([PersonId]);
GO

-- Creating foreign key on [ChurchSuburbId] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [FK_Address_ChurchSuburb]
    FOREIGN KEY ([ChurchSuburbId])
    REFERENCES [dbo].[ChurchSuburbs]
        ([ChurchSuburbId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Address_ChurchSuburb'
CREATE INDEX [IX_FK_Address_ChurchSuburb]
ON [dbo].[Addresses]
    ([ChurchSuburbId]);
GO

-- Creating foreign key on [ChurchId] in table 'ChurchSuburbs'
ALTER TABLE [dbo].[ChurchSuburbs]
ADD CONSTRAINT [FK_ChurchSuburb_Church]
    FOREIGN KEY ([ChurchId])
    REFERENCES [dbo].[Churches]
        ([ChurchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChurchSuburb_Church'
CREATE INDEX [IX_FK_ChurchSuburb_Church]
ON [dbo].[ChurchSuburbs]
    ([ChurchId]);
GO

-- Creating foreign key on [ChurchId] in table 'GroupClassifications'
ALTER TABLE [dbo].[GroupClassifications]
ADD CONSTRAINT [FK_GroupClassification_Church]
    FOREIGN KEY ([ChurchId])
    REFERENCES [dbo].[Churches]
        ([ChurchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupClassification_Church'
CREATE INDEX [IX_FK_GroupClassification_Church]
ON [dbo].[GroupClassifications]
    ([ChurchId]);
GO

-- Creating foreign key on [GroupClassificationId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [FK_Group_GroupClassification]
    FOREIGN KEY ([GroupClassificationId])
    REFERENCES [dbo].[GroupClassifications]
        ([GroupClassificationId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Group_GroupClassification'
CREATE INDEX [IX_FK_Group_GroupClassification]
ON [dbo].[Groups]
    ([GroupClassificationId]);
GO

-- Creating foreign key on [GroupTypeId] in table 'GroupClassifications'
ALTER TABLE [dbo].[GroupClassifications]
ADD CONSTRAINT [FK_GroupClassification_GroupType]
    FOREIGN KEY ([GroupTypeId])
    REFERENCES [dbo].[GroupTypes]
        ([GroupTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupClassification_GroupType'
CREATE INDEX [IX_FK_GroupClassification_GroupType]
ON [dbo].[GroupClassifications]
    ([GroupTypeId]);
GO

-- Creating foreign key on [ChurchId] in table 'EventTypes'
ALTER TABLE [dbo].[EventTypes]
ADD CONSTRAINT [FK_EventType_Church]
    FOREIGN KEY ([ChurchId])
    REFERENCES [dbo].[Churches]
        ([ChurchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EventType_Church'
CREATE INDEX [IX_FK_EventType_Church]
ON [dbo].[EventTypes]
    ([ChurchId]);
GO

-- Creating foreign key on [TableId] in table 'EventTypes'
ALTER TABLE [dbo].[EventTypes]
ADD CONSTRAINT [FK_EventType_Table]
    FOREIGN KEY ([TableId])
    REFERENCES [dbo].[Tables]
        ([TableId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EventType_Table'
CREATE INDEX [IX_FK_EventType_Table]
ON [dbo].[EventTypes]
    ([TableId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------