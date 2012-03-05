exec sp_executesql N'
/****** Object:  Table [dbo].[ChurchContactType]    Script Date: 03/22/2011 18:23:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON


CREATE TABLE [dbo].[ChurchContactType](
	[ChurchContactTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ChurchId] [int] NOT NULL,
	[ContactTypeId] [int] NOT NULL,
	[Visible] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
 CONSTRAINT [PK_ChurchContactType] PRIMARY KEY CLUSTERED 
(
	[ChurchContactTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[ChurchContactType]  WITH CHECK ADD  CONSTRAINT [FK_ChurchContactType_Church] FOREIGN KEY([ChurchId])
REFERENCES [dbo].[Church] ([ChurchId])

ALTER TABLE [dbo].[ChurchContactType] CHECK CONSTRAINT [FK_ChurchContactType_Church]

ALTER TABLE [dbo].[ChurchContactType]  WITH CHECK ADD  CONSTRAINT [FK_ChurchContactType_ContactType] FOREIGN KEY([ContactTypeId])
REFERENCES [dbo].[ContactType] ([ContactTypeId])

ALTER TABLE [dbo].[ChurchContactType] CHECK CONSTRAINT [FK_ChurchContactType_ContactType]

ALTER TABLE [dbo].[ChurchContactType] ADD  CONSTRAINT [DF_ChurchContactType_Visible]  DEFAULT ((1)) FOR [Visible]

ALTER TABLE [dbo].[ChurchContactType] ADD  CONSTRAINT [DF_ChurchContactType_Created]  DEFAULT (getdate()) FOR [Created]

ALTER TABLE [dbo].[ChurchContactType] ADD  CONSTRAINT [DF_ChurchContactType_Changed]  DEFAULT (getdate()) FOR [Changed]
'
