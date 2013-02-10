CREATE TABLE nccb.Groupmembertype(
	MemberTypeID [int] NOT NULL,
	MemberTypeName nvarchar(255) NOT NULL,
	MemberTypeDesc nvarchar(255) NOT NULL,
	LeaderRole bit NOT NULL,
	UserUpdated int NULL,
	DateUpdated DateTime NULL
 CONSTRAINT [PK_NCCB_Groupmembertype] PRIMARY KEY CLUSTERED 
(
	MemberTypeID ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


INSERT INTO nccb.Groupmembertype (MemberTypeID,MemberTypeName,MemberTypeDesc,LeaderRole,UserUpdated,DateUpdated) VALUES 
 (2,'Leader','Leader of a particular small group','True',1,'2004-02-17 21:25:26'),
 (5,'Member','A normal member of the group','False',1,'2004-02-17 21:26:12');