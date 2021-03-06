
CREATE TABLE [nccb].[FamilyMemberType](
	[FMTypeID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Active] [bit] NULL,
	[ParentalRole] [bit] NULL,
	[UserUpdated] [int] NULL,
	[DateUpdated] [datetime] NULL,
	[LastSync] [nvarchar](50) NULL,
 CONSTRAINT [PK_FamilyMemberType] PRIMARY KEY CLUSTERED 
(
	[FMTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (1, N'Father', 1, 1, 1, CAST(0x00009490015EF8D4 AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (2, N'Mother', 1, 1, 1, CAST(0x00009490015EFEB0 AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (3, N'Daughter', 1, 0, 1, CAST(0x00009490015F2EBC AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (4, N'Son', 1, 0, 1, CAST(0x00009490015F336C AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (5, N'Step Father', 1, 1, 1, CAST(0x000094B00175AC28 AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (6, N'Only member', 1, 1, 149, CAST(0x0000950D0106DEEC AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (7, N'Husband', 1, 1, 1, CAST(0x00009AF400D12950 AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (8, N'Wife', 1, 1, 1, CAST(0x00009AF400D13184 AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (9, N'Single Mother', 1, 1, 1, CAST(0x00009AF400D13E68 AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (10, N'Single Father', 1, 1, 1, CAST(0x00009AF400D14570 AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (11, N'Sister', 1, 0, 1, CAST(0x00009AF400D15128 AS DateTime), NULL)
INSERT [nccb].[FamilyMemberType] ([FMTypeID], [Name], [Active], [ParentalRole], [UserUpdated], [DateUpdated], [LastSync]) VALUES (12, N'Brother', 1, 0, 1, CAST(0x00009AF400D15830 AS DateTime), NULL)
