exec sp_executesql N'
/****** Object:  Table [dbo].[PersonContact]    Script Date: 03/13/2011 17:25:20 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[PersonContact](
	[PersonContactId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[ContactTypeId] [int] NOT NULL,
	[Contact] [nvarchar](255) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
 CONSTRAINT [PK_PersonContact] PRIMARY KEY CLUSTERED 
(
	[PersonContactId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[PersonContact]  WITH CHECK ADD  CONSTRAINT [FK_PersonContact_ContactType] FOREIGN KEY([ContactTypeId])
REFERENCES [dbo].[ContactType] ([ContactTypeId])

ALTER TABLE [dbo].[PersonContact] CHECK CONSTRAINT [FK_PersonContact_ContactType]

ALTER TABLE [dbo].[PersonContact]  WITH CHECK ADD  CONSTRAINT [FK_PersonContact_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([PersonId])

ALTER TABLE [dbo].[PersonContact] CHECK CONSTRAINT [FK_PersonContact_Person]

ALTER TABLE [dbo].[PersonContact] ADD  CONSTRAINT [DF_PersonContact_Created]  DEFAULT (getdate()) FOR [Created]

ALTER TABLE [dbo].[PersonContact] ADD  CONSTRAINT [DF_PersonContact_Changed]  DEFAULT (getdate()) FOR [Changed]
'

