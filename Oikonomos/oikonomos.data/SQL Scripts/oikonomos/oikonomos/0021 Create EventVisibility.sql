exec sp_executesql N'
/****** Object:  Table [dbo].[EventVisibility]    Script Date: 04/11/2011 00:15:44 ******/
SET ANSI_NULLS ON


SET QUOTED_IDENTIFIER ON


CREATE TABLE [dbo].[EventVisibility](
	[EventVisibilityId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_EventVisibility] PRIMARY KEY CLUSTERED 
(
	[EventVisibilityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[EventVisibility] ADD  CONSTRAINT [DF_EventVisibility_Created]  DEFAULT (getdate()) FOR [Created]

/****** Object:  Table [dbo].[EventVisibility]    Script Date: 04/11/2011 00:17:12 ******/
INSERT [dbo].[EventVisibility] ([EventVisibilityId], [Name], [Created]) VALUES (1, N''Elders'', CAST(0x00009EC100C6D02F AS DateTime))
INSERT [dbo].[EventVisibility] ([EventVisibilityId], [Name], [Created]) VALUES (2, N''Group'', CAST(0x00009EC100C6DA33 AS DateTime))
INSERT [dbo].[EventVisibility] ([EventVisibilityId], [Name], [Created]) VALUES (3, N''Site'', CAST(0x00009EC100C6E121 AS DateTime))
INSERT [dbo].[EventVisibility] ([EventVisibilityId], [Name], [Created]) VALUES (4, N''Church'', CAST(0x00009EC100C6E48B AS DateTime))
INSERT [dbo].[EventVisibility] ([EventVisibilityId], [Name], [Created]) VALUES (5, N''Public'', CAST(0x00009EC100C6E885 AS DateTime))

'

