exec sp_executesql N'
/****** Object:  Table [dbo].[PersonRelationship]    Script Date: 04/09/2011 14:26:14 ******/
SET ANSI_NULLS ON


SET QUOTED_IDENTIFIER ON


CREATE TABLE [dbo].[PersonRelationship](
	[PersonRelationshipId] [int] NOT NULL,
	[PersonId] [int] NOT NULL,
	[PersonRelatedToId] [int] NOT NULL,
	[RelationshipId] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
 CONSTRAINT [PK_PersonRelationship] PRIMARY KEY CLUSTERED 
(
	[PersonRelationshipId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[PersonRelationship]  WITH CHECK ADD  CONSTRAINT [FK_PersonRelationship_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([PersonId])


ALTER TABLE [dbo].[PersonRelationship] CHECK CONSTRAINT [FK_PersonRelationship_Person]


ALTER TABLE [dbo].[PersonRelationship]  WITH CHECK ADD  CONSTRAINT [FK_PersonRelationship_PersonRelatedTo] FOREIGN KEY([PersonRelatedToId])
REFERENCES [dbo].[Person] ([PersonId])


ALTER TABLE [dbo].[PersonRelationship] CHECK CONSTRAINT [FK_PersonRelationship_PersonRelatedTo]


ALTER TABLE [dbo].[PersonRelationship]  WITH CHECK ADD  CONSTRAINT [FK_PersonRelationship_Relationship] FOREIGN KEY([RelationshipId])
REFERENCES [dbo].[Relationship] ([RelationshipId])


ALTER TABLE [dbo].[PersonRelationship] CHECK CONSTRAINT [FK_PersonRelationship_Relationship]


ALTER TABLE [dbo].[PersonRelationship] ADD  CONSTRAINT [DF_PersonRelationship_Created]  DEFAULT (getdate()) FOR [Created]


ALTER TABLE [dbo].[PersonRelationship] ADD  CONSTRAINT [DF_PersonRelationship_Changed]  DEFAULT (getdate()) FOR [Changed]


'