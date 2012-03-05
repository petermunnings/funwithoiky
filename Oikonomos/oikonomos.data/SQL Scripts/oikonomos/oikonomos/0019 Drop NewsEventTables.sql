exec sp_executesql N'
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[dbo].[FK_PersonNews_NewsEvent]'') AND parent_object_id = OBJECT_ID(N''[dbo].[PersonNews]''))
ALTER TABLE [dbo].[PersonNews] DROP CONSTRAINT [FK_PersonNews_NewsEvent]


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''[dbo].[FK_PersonNews_Person]'') AND parent_object_id = OBJECT_ID(N''[dbo].[PersonNews]''))
ALTER TABLE [dbo].[PersonNews] DROP CONSTRAINT [FK_PersonNews_Person]


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N''[DF_PersonNews_Created]'') AND type = ''D'')
BEGIN
ALTER TABLE [dbo].[PersonNews] DROP CONSTRAINT [DF_PersonNews_Created]
END


/****** Object:  Table [dbo].[PersonNews]    Script Date: 04/10/2011 12:08:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[dbo].[PersonNews]'') AND type in (N''U''))
DROP TABLE [dbo].[PersonNews]


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N''[DF_NewsEvent_Created]'') AND type = ''D'')
BEGIN
ALTER TABLE [dbo].[NewsEvent] DROP CONSTRAINT [DF_NewsEvent_Created]
END


/****** Object:  Table [dbo].[NewsEvent]    Script Date: 04/10/2011 12:06:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[dbo].[NewsEvent]'') AND type in (N''U''))
DROP TABLE [dbo].[NewsEvent]
'

