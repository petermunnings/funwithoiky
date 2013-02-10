CREATE TABLE nccb.Groupcategory(
	SmallGroupCategoryID [int] NOT NULL,
	Name nvarchar(255) NOT NULL,
	Description nvarchar(255) NOT NULL,
	RatingSetID [int] NOT NULL,
	Active bit NOT NULL,
	UserUpdated int NULL,
	DateUpdated DateTime NULL
 CONSTRAINT [PK_NCCB_Groupcategory] PRIMARY KEY CLUSTERED 
(
	SmallGroupCategoryID ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO nccb.Groupcategory (SmallGroupCategoryID,Name,Description,RatingSetID,Active,UserUpdated,DateUpdated) VALUES 
 (1,'Stage/Age Based','Community, Study, Prayer',4,'False',844,'2009-01-26 10:31:03'),
 (2,'Needs Based','Healing, Comfort, Connection',4,'False',844,'2009-01-26 10:31:11'),
 (3,'Special Interest','Connection, Community, Study',4,'True',844,'2009-01-26 10:13:04'),
 (5,'Task Based','Service, Community, Prayer',4,'False',844,'2009-01-26 10:30:54'),
 (6,'HomeGroups','home groups',4,'True',844,'2009-01-26 10:13:41');