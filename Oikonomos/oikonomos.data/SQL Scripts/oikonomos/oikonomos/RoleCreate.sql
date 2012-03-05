exec sp_executesql N'
/****** Object:  Table [dbo].[Role]    Script Date: 03/26/2011 14:01:48 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Role](
	[RoleId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_Created]  DEFAULT (getdate()) FOR [Created]
'