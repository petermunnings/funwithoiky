exec sp_executesql N'
/****** Object:  Table [dbo].[Relationship]    Script Date: 04/09/2011 14:23:50 ******/
SET ANSI_NULLS ON


SET QUOTED_IDENTIFIER ON


CREATE TABLE [dbo].[Relationship](
	[RelationshipId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Relationship] PRIMARY KEY CLUSTERED 
(
	[RelationshipId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[Relationship] ADD  CONSTRAINT [DF_Relationship_Created]  DEFAULT (getdate()) FOR [Created]

'

