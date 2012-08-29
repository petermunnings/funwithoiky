DROP TABLE [dbo].[EventType]
GO

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



EXEC sp_rename 'Event', 'OldEvent'
GO

CREATE TABLE [dbo].[Event](
	[EventId] [int] IDENTITY(1,1) NOT NULL,
	[ChurchId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[ShowInGroupScreen] [bit] NOT NULL,
	[EventOrder] [int] NOT NULL,
 CONSTRAINT [PK_Event_1] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Church1] FOREIGN KEY([ChurchId])
REFERENCES [dbo].[Church] ([ChurchId])
GO

ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Church1]
GO

ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_ShowInGroupScreen]  DEFAULT ((0)) FOR [ShowInGroupScreen]
GO

ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_EventOrder]  DEFAULT ((0)) FOR [EventOrder]
GO

CREATE TABLE [dbo].[CanViewComment](
	[CommentRoleId] [int] NOT NULL,
	[CanViewByRoleId] [int] NOT NULL,
 CONSTRAINT [PK_CanViewComment] PRIMARY KEY CLUSTERED 
(
	[CommentRoleId] ASC,
	[CanViewByRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CanViewComment]  WITH CHECK ADD  CONSTRAINT [FK_CanViewComment_CanViewRole] FOREIGN KEY([CanViewByRoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO

ALTER TABLE [dbo].[CanViewComment] CHECK CONSTRAINT [FK_CanViewComment_CanViewRole]
GO

ALTER TABLE [dbo].[CanViewComment]  WITH CHECK ADD  CONSTRAINT [FK_CanViewComment_CommentRole] FOREIGN KEY([CommentRoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO

ALTER TABLE [dbo].[CanViewComment] CHECK CONSTRAINT [FK_CanViewComment_CommentRole]
GO

CREATE TABLE [dbo].[Comment](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[AboutPersonId] [int] NOT NULL,
	[MadeByPersonId] [int] NOT NULL,
	[MadeByRoleId] [int] NOT NULL,
	[CommentDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_AboutPerson] FOREIGN KEY([AboutPersonId])
REFERENCES [dbo].[Person] ([PersonId])
GO

ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_AboutPerson]
GO

ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_MadeByPerson] FOREIGN KEY([MadeByPersonId])
REFERENCES [dbo].[Person] ([PersonId])
GO

ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_MadeByPerson]
GO

ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Role] FOREIGN KEY([MadeByRoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO

ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Role]
GO

delete from PermissionRole
where PermissionId = 20

insert into PermissionRole (PermissionId, RoleId)
select 20, roleid from permissionrole
where PermissionId = 19

insert into Permission(PermissionId, Name, Description, Category)
values (56, 'ViewPersonGroups', 'Can view the groups a person is in', 'Basic')

delete from PermissionRole
where PermissionId = 56

insert into PermissionRole (PermissionId, RoleId)
select 56, roleid from permissionrole
where PermissionId = 19

update Permission set Name = 'ViewComments'
where PermissionId = 19

update Permission set Name = 'ViewEvents'
where PermissionId = 20

insert into Comment (AboutPersonId, Comment, CommentDate, MadeByPersonId, MadeByRoleId)
select e.Reference, e.Comments, e.Created, e.CreatedByPersonId, pc.RoleId
from [OldEvent] e
join [PersonChurch] pc
on e.CreatedByPersonId = pc.PersonId
and e.ChurchId = pc.ChurchId
where e.Description = 'Comment'
and e.TableId = 1

delete from OldEvent
where Description = 'Comment'
and TableId = 1

select * from [Role]
where Name in ('Church Administrator', 'Group Administrator', 'Elder', 'System Administrator')

insert into CanViewComment (CommentRoleId, CanViewByRoleId)
select commentRole.RoleId, viewRole.RoleId
from [Role] commentRole 
join [Role] viewRole
on commentRole.ChurchId = viewRole.ChurchId
where commentRole.Name in ('Church Administrator', 'Group Administrator', 'Elder', 'System Administrator')
and viewRole.Name in ('Elder')

insert into CanViewComment (CommentRoleId, CanViewByRoleId)
select commentRole.RoleId, viewRole.RoleId
from [Role] commentRole 
join [Role] viewRole
on commentRole.ChurchId = viewRole.ChurchId
where commentRole.Name in ('Church Administrator', 'Group Administrator')
and viewRole.Name in ('Group Administrator')

insert into CanViewComment (CommentRoleId, CanViewByRoleId)
select commentRole.RoleId, viewRole.RoleId
from [Role] commentRole 
join [Role] viewRole
on commentRole.ChurchId = viewRole.ChurchId
where commentRole.Name in ('Church Administrator', 'Group Administrator')
and viewRole.Name in ('Church Administrator')

insert into CanViewComment (CommentRoleId, CanViewByRoleId)
select commentRole.RoleId, viewRole.RoleId
from [Role] commentRole 
join [Role] viewRole
on commentRole.ChurchId = viewRole.ChurchId
where commentRole.Name in ('Church Administrator', 'Group Administrator', 'System Administrator')
and viewRole.Name in ('System Administrator')


