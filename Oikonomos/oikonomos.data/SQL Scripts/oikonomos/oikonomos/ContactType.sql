exec sp_executesql N'
/****** Object:  Table [dbo].[ContactType]    Script Date: 03/13/2011 17:10:04 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ContactType](
	[ContactTypeId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Regex] [nvarchar](255) NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED 
(
	[ContactTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[ContactType] ADD  CONSTRAINT [DF_ContactType_Created]  DEFAULT (getdate()) FOR [Created]

INSERT INTO ContactType (ContactTypeId, [Name], Regex)
VALUES (1, ''Home Phone'', ''[0](\d{9})|([0](\d{2})( |-)((\d{3}))( |-)(\d{4}))|[0](\d{2})( |-)(\d{7})'')

INSERT INTO ContactType (ContactTypeId, [Name], Regex)
VALUES (2, ''Work Phone'', ''[0](\d{9})|([0](\d{2})( |-)((\d{3}))( |-)(\d{4}))|[0](\d{2})( |-)(\d{7})'')

INSERT INTO ContactType (ContactTypeId, [Name], Regex)
VALUES (3, ''Cell Phone'', ''(^0[87][23467]((\d{7})|( |-)((\d{3}))( |-)(\d{4})|( |-)(\d{7})))'')

INSERT INTO ContactType (ContactTypeId, [Name], Regex)
VALUES (4, ''Skype'', null)

INSERT INTO ContactType (ContactTypeId, [Name], Regex)
VALUES (5, ''Twitter'', null)

INSERT INTO ContactType (ContactTypeId, [Name], Regex)
VALUES (6, ''Facebook'', null)'