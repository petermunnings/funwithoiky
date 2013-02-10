CREATE TABLE nccb.Area(
	AreaID [int] NOT NULL,
	AreaName [nvarchar](255) NULL,
	Active bit NULL,
	DefaultItem bit NULL,
	UserUpdated int NULL,
	DateUpdated DateTime NULL,
	LastSync [nvarchar](50) NULL
 CONSTRAINT [PK_Area] PRIMARY KEY CLUSTERED 
(
	AreaID ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

delete from nccb.Area

INSERT INTO nccb.Area (AreaID,AreaName,Active,DefaultItem,UserUpdated,DateUpdated,LastSync) VALUES 
 (1,'Bryanston',1,0,844,'2009-01-23 11:58:42',NULL),
 (2,'Lonehill',1,0,844,'2009-01-26 11:15:38',NULL),
 (3,'Douglasdale',1,0,844,'2009-01-26 11:29:34',NULL),
 (4,'Olivedale',1,0,844,'2009-01-26 11:29:39',NULL),
 (5,'Sandhurst',1,0,844,'2009-01-26 11:29:50',NULL),
 (6,'Northriding',1,0,844,'2009-01-26 11:29:55',NULL),
 (7,'Bordeaux',1,0,844,'2009-01-26 11:30:16',NULL),
 (8,'Ferndale',1,0,844,'2009-01-26 11:30:21',NULL),
 (9,'Morningside',1,0,844,'2009-01-26 11:30:29',NULL),
 (10,'Rivonia',1,0,844,'2009-01-26 11:30:34',NULL),
 (11,'Hyde Park',1,0,844,'2009-01-26 11:30:41',NULL),
 (12,'Northcliff',1,0,844,'2009-01-26 11:30:47',NULL),
 (13,'Sharonlea',1,0,844,'2009-01-26 11:31:08',NULL),
 (14,'Linden',1,0,844,'2009-01-26 11:31:18',NULL),
 (15,'Illovo',1,0,844,'2009-01-26 11:31:32',NULL),
 (16,'Jukskei Park',1,0,844,'2009-01-26 11:31:51',NULL),
 (17,'Kya Sands',1,0,844,'2009-01-26 11:32:00',NULL),
 (18,'Dainfern',1,0,844,'2009-01-26 11:32:04',NULL),
 (19,'Fourways',1,0,844,'2009-01-26 11:32:11',NULL),
 (20,'Magaliessig',1,0,844,'2009-01-26 11:32:26',NULL),
 (21,'Parkmore',1,0,844,'2009-01-26 11:32:34',NULL),
 (22,'Sandown',1,0,844,'2009-01-26 11:32:50',NULL),
 (23,'Randburg',1,0,844,'2009-01-26 11:33:04',NULL),
 (24,'Randpark Ridge',1,0,844,'2009-01-26 12:00:01',NULL),
 (25,'Sunninghill',1,0,844,'2009-01-26 11:33:39',NULL),
 (26,'Paulshof',1,0,844,'2009-01-26 11:33:49',NULL),
 (27,'Muldersdrift',1,0,844,'2009-01-26 11:56:56',NULL),
 (28,'Parktown',1,0,844,'2009-01-26 11:57:12',NULL),
 (29,'Robindale',1,0,844,'2009-01-26 11:59:03',NULL),
 (30,'Woodmead',1,0,844,'2009-01-26 12:00:26',NULL),
 (31,'Fountainebleau',1,0,844,'2009-01-26 12:02:16',NULL),
 (32,'Robinhills',1,0,844,'2009-01-26 12:02:50',NULL),
 (33,'Wilgeheuwel',1,0,844,'2009-01-26 12:04:31',NULL),
 (34,'Craigavon',1,0,844,'2009-01-26 12:05:09',NULL),
 (35,'Kensington B',1,0,844,'2009-01-26 12:05:32',NULL),
 (36,'Malanshof',1,0,844,'2009-01-26 12:06:42',NULL),
 (37,'Blackheath',1,0,844,'2009-01-26 12:06:58',NULL),
 (38,'Sundowner',1,0,844,'2009-01-26 12:08:43',NULL),
 (39,'Weltevreden Park',1,0,844,'2009-01-26 12:09:07',NULL),
 (40,'Broadacres',1,0,844,'2009-01-26 12:10:02',NULL),
 (41,'Bloubos Rand',1,0,844,'2009-01-26 12:10:49',NULL),
 (42,'Bromhof',1,0,844,'2009-01-26 12:12:22',NULL),
 (43,'Other',1,0,844,'2009-02-25 11:37:46',NULL);