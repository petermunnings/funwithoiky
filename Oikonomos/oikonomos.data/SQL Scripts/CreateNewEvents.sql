
CREATE TABLE [dbo].[StandardComment](
	[StandardCommentId] [int] IDENTITY(1,1) NOT NULL,
	[StandardComment] [nvarchar](255) NOT NULL,
	[ChurchId] [int] NOT NULL,
 CONSTRAINT [PK_StandardComment] PRIMARY KEY CLUSTERED 
(
	[StandardCommentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[StandardComment]  WITH CHECK ADD  CONSTRAINT [FK_StandardComment_Church] FOREIGN KEY([ChurchId])
REFERENCES [dbo].[Church] ([ChurchId])
GO

ALTER TABLE [dbo].[StandardComment] CHECK CONSTRAINT [FK_StandardComment_Church]
GO

insert into standardcomment(churchid, standardcomment)
SELECT [ChurchId]
      ,[Name]
  FROM [EventType]
  
GO
  
DROP TABLE [dbo].[EventType]

Update OptionalField set Name = 'Standard Comments'
where OptionalFieldId = 11


ALTER TABLE dbo.PersonChurch ADD
	RoleId int NOT NULL CONSTRAINT DF_PersonChurch_RoleId DEFAULT 1
GO
ALTER TABLE dbo.PersonChurch ADD CONSTRAINT
	FK_PersonChurch_Role FOREIGN KEY
	(
	RoleId
	) REFERENCES dbo.Role
	(
	RoleId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO

update pc set roleid = pr.roleid
from PersonChurch pc
join PersonRole pr
on pc.PersonId = pr.PersonId
join [Role] r
on pc.ChurchId = r.ChurchId
and pr.RoleId = r.RoleId

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FamilyChurch_Church]') AND parent_object_id = OBJECT_ID(N'[dbo].[FamilyChurch]'))
ALTER TABLE [dbo].[FamilyChurch] DROP CONSTRAINT [FK_FamilyChurch_Church]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FamilyChurch_Family]') AND parent_object_id = OBJECT_ID(N'[dbo].[FamilyChurch]'))
ALTER TABLE [dbo].[FamilyChurch] DROP CONSTRAINT [FK_FamilyChurch_Family]
GO


/****** Object:  Table [dbo].[FamilyChurch]    Script Date: 08/19/2012 19:47:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FamilyChurch]') AND type in (N'U'))
DROP TABLE [dbo].[FamilyChurch]
GO




IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonRole_Person]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonRole]'))
ALTER TABLE [dbo].[PersonRole] DROP CONSTRAINT [FK_PersonRole_Person]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonRole_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonRole]'))
ALTER TABLE [dbo].[PersonRole] DROP CONSTRAINT [FK_PersonRole_Role]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PersonRole_Created]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PersonRole] DROP CONSTRAINT [DF_PersonRole_Created]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PersonRole_Changed]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PersonRole] DROP CONSTRAINT [DF_PersonRole_Changed]
END

GO


/****** Object:  Table [dbo].[PersonRole]    Script Date: 08/05/2012 19:24:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonRole]') AND type in (N'U'))
DROP TABLE [dbo].[PersonRole]
GO


