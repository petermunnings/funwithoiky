CREATE TABLE nccb.[Role](
	RoleID [int] NOT NULL,
	RoleName nvarchar(50) NOT NULL,
	RoleType nvarchar(50) NOT NULL,
	Active bit NULL,
	UserUpdated int NULL,
	DateUpdated DateTime NULL,
	LeaderRole bit NULL,
	LastSync [nvarchar](50) NULL
 CONSTRAINT [PK_NCCB_Role] PRIMARY KEY CLUSTERED 
(
	RoleID ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

delete from nccb.[Role]

INSERT INTO nccb.[Role] (RoleID,RoleName,RoleType,Active,UserUpdated,DateUpdated,LeaderRole,LastSync) VALUES 
 (1,'Visitor','System',1,1,'2003-01-01 00:00:00',0,NULL),
 (2,'Member','System',1,1,'2003-01-01 00:00:00',0,NULL),
 (3,'Staff','System',1,1,'2003-01-01 00:00:00',0,NULL),
 (4,'Pastor','System',1,1,'2003-01-01 00:00:00',1,NULL),
 (5,'Counselor','System',1,1,'2003-01-01 00:00:00',0,NULL),
 (6,'SmallGroupLeader','System',1,1,'2003-01-01 00:00:00',1,NULL),
 (7,'Administrator','System',1,1,'2003-01-01 00:00:00',0,NULL),
 (8,'Elder','User',1,1,'2009-02-04 17:39:49',1,NULL),
 (9,'Youth Leader','User',1,1,'2009-02-04 17:41:44',1,NULL),
 (10,'Worship Team','User',1,1,'2004-03-14 18:15:56',0,NULL),
 (11,'Area Leader','User',1,1,'2009-02-04 17:39:31',1,NULL),
 (100,'Meeting Coordinator','System',1,1,'2005-01-01 00:00:00',0,NULL),
 (101,'Resource Manager','System',1,1,'2005-01-01 00:00:00',0,NULL),
 (102,'Counseling Coordinator','System',1,1,'2005-01-01 00:00:00',0,NULL),
 (103,'Follow Up Administrator','System',1,1,'2005-01-01 00:00:00',0,NULL),
 (104,'Deacon','User',1,1,'2009-02-04 17:39:45',1,NULL);