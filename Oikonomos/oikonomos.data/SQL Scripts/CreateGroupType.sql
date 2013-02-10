CREATE TABLE nccb.Grouptype(
	GroupTypeID [int] NOT NULL,
	TypeName nvarchar(255) NOT NULL,
	SmallGroupCategoryID [int] NOT NULL,
	Active bit NOT NULL,
	UserUpdated int NULL,
	DateUpdated DateTime NULL
 CONSTRAINT [PK_NCCB_Grouptype] PRIMARY KEY CLUSTERED 
(
	GroupTypeID ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO nccb.Grouptype (GroupTypeID,TypeName,SmallGroupCategoryID,Active,UserUpdated,DateUpdated) VALUES 
 (7,'Task Groups',5,'False',844,'2009-01-26 10:28:55'),
 (8,'Relational',1,'False',844,'2009-01-26 10:29:21'),
 (9,'Youth',1,'False',844,'2009-01-26 10:29:24'),
 (10,'Home Group',6,'True',844,'2009-02-12 08:30:55'),
 (11,'Music and Lyrics',3,'True',844,'2009-02-11 08:04:28'),
 (12,'Social Upliftment',3,'True',844,'2009-02-11 08:04:43'),
 (13,'Dance',3,'True',844,'2009-02-11 08:04:50'),
 (14,'Hiking - Drakensberg',3,'True',844,'2009-02-11 08:05:19'),
 (15,'Theology Focus',3,'True',844,'2009-02-11 08:05:37'),
 (16,'Photography',3,'True',844,'2009-02-11 08:05:45'),
 (17,'Outdoor adventure and Nature',3,'True',844,'2009-02-11 08:06:00'),
 (18,'Running',3,'True',844,'2009-02-11 08:06:10'),
 (19,'Cycling',3,'True',844,'2009-02-11 08:06:15'),
 (20,'Business',3,'True',844,'2009-02-11 08:06:20'),
 (21,'Golf',3,'True',844,'2009-02-11 08:06:26'),
 (22,'Scrapbooking',3,'True',844,'2009-02-11 08:06:37'),
 (23,'Design Workshops',3,'True',844,'2009-02-11 08:06:46'),
 (24,'Rock Climbing',3,'True',844,'2009-02-11 08:06:55'),
 (25,'Cricket',3,'True',844,'2009-02-11 08:07:04'),
 (26,'Book Club',3,'True',844,'2009-02-11 08:07:12'),
 (27,'Painting/Art',3,'True',844,'2009-02-11 08:07:22'),
 (28,'Biking/Motorcycling',3,'True',844,'2009-02-11 08:07:51'),
 (29,'Movies',3,'True',844,'2009-02-11 08:08:05'),
 (30,'Arts and Crafts',3,'True',844,'2009-02-11 09:04:14'),
 (31,'Ladies Group',6,'True',844,'2009-02-12 08:30:41'),
 (32,'',6,'False',651,'2009-11-04 15:20:15'),
 (33,'Worship Team',6,'True',651,'2009-11-17 11:32:58');