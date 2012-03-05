exec sp_executesql N'
/****** Object:  Table [dbo].[Address]    Script Date: 03/16/2011 20:20:54 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Address](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[Line1] [nvarchar](255) NOT NULL,
	[Line2] [nvarchar](255) NOT NULL,
	[Line3] [nvarchar](255) NOT NULL,
	[Line4] [nvarchar](255) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Changed] [datetime] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[Address] ADD  CONSTRAINT [DF_Address_Line1]  DEFAULT ('''') FOR [Line1]

ALTER TABLE [dbo].[Address] ADD  CONSTRAINT [DF_Address_Line2]  DEFAULT ('''') FOR [Line2]

ALTER TABLE [dbo].[Address] ADD  CONSTRAINT [DF_Address_Line3]  DEFAULT ('''') FOR [Line3]

ALTER TABLE [dbo].[Address] ADD  CONSTRAINT [DF_Address_Line4]  DEFAULT ('''') FOR [Line4]


ALTER TABLE [dbo].[Address] ADD  CONSTRAINT [DF_Address_Created]  DEFAULT (getdate()) FOR [Created]


ALTER TABLE [dbo].[Address] ADD  CONSTRAINT [DF_Address_Changed]  DEFAULT (getdate()) FOR [Changed]
'

