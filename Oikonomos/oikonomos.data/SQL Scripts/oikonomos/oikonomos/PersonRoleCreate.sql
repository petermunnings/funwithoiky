exec sp_executesql N'
/****** Object:  Table [dbo].[PersonRole]    Script Date: 03/26/2011 14:03:53 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[PersonRole](
	[RoleId] [int] NOT NULL,
	[PersonId] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
 CONSTRAINT [PK_PersonRole] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[PersonId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[PersonRole]  WITH CHECK ADD  CONSTRAINT [FK_PersonRole_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([PersonId])

ALTER TABLE [dbo].[PersonRole] CHECK CONSTRAINT [FK_PersonRole_Person]

ALTER TABLE [dbo].[PersonRole]  WITH CHECK ADD  CONSTRAINT [FK_PersonRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])

ALTER TABLE [dbo].[PersonRole] CHECK CONSTRAINT [FK_PersonRole_Role]

ALTER TABLE [dbo].[PersonRole] ADD  CONSTRAINT [DF_PersonRole_Created]  DEFAULT (getdate()) FOR [Created]

ALTER TABLE [dbo].[PersonRole] ADD  CONSTRAINT [DF_PersonRole_Changed]  DEFAULT (getdate()) FOR [Changed]
'

