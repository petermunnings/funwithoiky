
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

