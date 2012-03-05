exec sp_executesql N'
/****** Object:  Table [dbo].[Table]    Script Date: 04/11/2011 00:13:56 ******/
SET ANSI_NULLS ON


SET QUOTED_IDENTIFIER ON


CREATE TABLE [dbo].[Table](
	[TableId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED 
(
	[TableId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[Table] ADD  CONSTRAINT [DF_Table_Created]  DEFAULT (getdate()) FOR [Created]
'

