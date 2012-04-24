select * 
into OldRole
from [Role]

select * from OldRole


/****** Object:  Table [dbo].[Permission]    Script Date: 04/22/2012 21:17:46 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Permission](
	[PermissionId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[Catery] [nvarchar](50) NOT NULL,
	[IsVisible] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
	[DependentOn] [int] NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (0, N'Login', N'Login to Oiky', N'Basic', 1, CAST(0x0000A00E01068137 AS DateTime), CAST(0x0000A00E01068137 AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (1, N'AllocateSecurityRole', N'AllocateSecurityRole', N'Basic', 1, CAST(0x0000A0340138C1C3 AS DateTime), CAST(0x0000A0340138C1C3 AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (2, N'EditOwnDetails', N'Show Own Contact List', N'Basic', 1, CAST(0x0000A00E0109953B AS DateTime), CAST(0x0000A00E0109953B AS DateTime), 0)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (3, N'EditOwnGroups', N'EditOwnGroups', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (4, N'EditAllGroups', N'EditAllGroups', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (5, N'EditGroupPersonalDetails', N'EditGroupPersonalDetails', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (6, N'ViewGroupContactDetails', N'ViewGroupContactDetails', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (7, N'EditChurchPersonalDetails', N'EditChurchPersonalDetails', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (8, N'ViewChurchContactDetails', N'ViewChurchContactDetails', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (9, N'AddGroup', N'AddGroup', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (10, N'DeleteGroup', N'DeleteGroup', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (11, N'RemovePersonFromGroup', N'RemovePersonFromGroup', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (12, N'ViewLists', N'ViewLists', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (13, N'ViewAdminReports', N'ViewAdminReports', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (14, N'AddComment', N'AddComment', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (15, N'AddEvent', N'AddEvent', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (16, N'DeleteEvent', N'DeleteEvent', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (17, N'EditSettings', N'EditSettings', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (18, N'SendSms', N'SendSms', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (19, N'ViewGeneralComments', N'ViewGeneralComments', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (20, N'ViewPersonalComments', N'ViewPersonalComments', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (21, N'AddSite', N'AddSite', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (22, N'EditSite', N'EditSite', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (23, N'DeleteSite', N'DeleteSite', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (24, N'AddSuburb', N'AddSuburb', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (25, N'DeleteSuburb', N'DeleteSuburb', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (26, N'AddGroupClassification', N'AddGroupClassification', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (27, N'DeleteGroupClassification', N'DeleteGroupClassification', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (28, N'EditBulkSmsDetails', N'EditBulkSmsDetails', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (29, N'EditChurchContactDetails', N'EditChurchContactDetails', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (30, N'SendEmailAndPassword', N'SendEmailAndPassword', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (31, N'SetGroupLeaderOrAdministrator', N'SetGroupLeaderOrAdministrator', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (32, N'ViewPeopleNotInAnyGroup', N'ViewPeopleNotInAnyGroup', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (33, N'ViewGroupAttendance', N'ViewGroupAttendance', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (34, N'EmailGroupMembers', N'EmailGroupMembers', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (35, N'SmsGroupMembers', N'SmsGroupMembers', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (36, N'EmailGroupLeaders', N'EmailGroupLeaders', N'Basic', 1, CAST(0x0000A00E0109953C AS DateTime), CAST(0x0000A00E0109953C AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (37, N'SmsGroupLeaders', N'SmsGroupLeaders', N'Basic', 1, CAST(0x0000A00E0109953D AS DateTime), CAST(0x0000A00E0109953D AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (38, N'EmailChurch', N'EmailChurch', N'Basic', 1, CAST(0x0000A00E0109953D AS DateTime), CAST(0x0000A00E0109953D AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (39, N'SmsChurch', N'SmsChurch', N'Basic', 1, CAST(0x0000A00E0109953D AS DateTime), CAST(0x0000A00E0109953D AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (40, N'SystemAdministrator', N'SystemAdministrator', N'Basic', 0, CAST(0x0000A00E0109953D AS DateTime), CAST(0x0000A00E0109953D AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (41, N'NotifyGroupLeaderOfVisit', N'NotifyGroupLeaderOfVisit', N'Basic', 1, CAST(0x0000A00E0109953D AS DateTime), CAST(0x0000A00E0109953D AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (42, N'SendWelcomeLetter', N'SendWelcomeLetter', N'Basic', 1, CAST(0x0000A00E0109953D AS DateTime), CAST(0x0000A00E0109953D AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (43, N'AddMember', N'AddMember', N'Basic', 1, CAST(0x0000A00E0109953D AS DateTime), CAST(0x0000A00E0109953D AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (44, N'EditGroups', N'EditGroups', N'Basic', 1, CAST(0x0000A033015A7EEF AS DateTime), CAST(0x0000A033015A7EEF AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (45, N'EditGroupLeader', N'EditGroupLeader', N'Basic', 1, CAST(0x0000A033015A91B0 AS DateTime), CAST(0x0000A033015A91B0 AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (46, N'EditGroupAdministrator', N'EditGroupAdministrator', N'Basic', 1, CAST(0x0000A033015AA555 AS DateTime), CAST(0x0000A033015AA555 AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (47, N'DeletePerson', N'DeletePerson', N'Basic', 1, CAST(0x0000A0330169A3AB AS DateTime), CAST(0x0000A0330169A3AB AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (48, N'IncludeInGroupAttendanceStats', N'IncludeInGroupAttendanceStats', N'Basic', 1, CAST(0x0000A0330169C0BC AS DateTime), CAST(0x0000A0330169C0BC AS DateTime), NULL)
INSERT [dbo].[Permission] ([PermissionId], [Name], [Description], [Catery], [IsVisible], [Created], [Changed], [DependentOn]) VALUES (49, N'EditPermissions', N'EditPermissions', N'Basic', 1, CAST(0x0000A0340157A218 AS DateTime), CAST(0x0000A0340157A218 AS DateTime), NULL)
/****** Object:  Default [DF_Permission_IsVisible]    Script Date: 04/22/2012 21:17:46 ******/
ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_Permission_IsVisible]  DEFAULT ((1)) FOR [IsVisible]

/****** Object:  Default [DF_Permission_Created]    Script Date: 04/22/2012 21:17:46 ******/
ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_Permission_Created]  DEFAULT (getdate()) FOR [Created]

/****** Object:  Default [DF_Permission_Changed]    Script Date: 04/22/2012 21:17:46 ******/
ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_Permission_Changed]  DEFAULT (getdate()) FOR [Changed]



ALTER TABLE dbo.[Role] ADD
	ChurchId int NOT NULL CONSTRAINT DF_Role_ChurchId DEFAULT 1,
	Changed datetime NOT NULL CONSTRAINT DF_Role_Changed DEFAULT GetDate()

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonRole_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonRole]'))
ALTER TABLE [dbo].[PersonRole] DROP CONSTRAINT [FK_PersonRole_Role]

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND name = N'PK_Role')
ALTER TABLE [dbo].[Role] DROP CONSTRAINT [PK_Role]

Alter Table dbo.[Role]
Add Id_new Int Identity(1, 1)


Alter Table dbo.[Role] Drop Column RoleID


Exec sp_rename 'Role.Id_new', 'RoleId', 'Column'


DECLARE @ChurchId int;

--BEGIN TRANSACTION
SET @ChurchId = 2;
WHILE @ChurchId <=21
BEGIN
	INSERT INTO [Role] (ChurchId, Name)
	SELECT @ChurchId, Name from OldRole

	UPDATE pr SET RoleId = r.RoleId
	--SELECT p.FirstName, oldR.Name, r.Name, @ChurchId
	FROM PersonRole pr
	join Person p
	on pr.PersonId = p.PersonId
	JOIN [Role] oldR
	on pr.RoleId = oldR.RoleId
	JOIN [Role] r
	on oldR.Name = r.Name
    where oldR.ChurchId = @ChurchId
	and r.ChurchId = @ChurchId
	and p.ChurchId = @ChurchId
	
	SET @ChurchId = @ChurchId + 1
	SELECT @ChurchId			 
END

SELECT p.Firstname, f.FamilyName, nr.Name as NewRole
FROM PersonRole pr
JOIN [Role] nr
on pr.RoleId = nr.RoleId
join Person p
on pr.PersonId = p.PersonId
join Family f
on p.FamilyId = f.FamilyId

select * from [Role]

delete from [Role] where ChurchId > 1 and Name = 'System Administrator'

select * from [church]


