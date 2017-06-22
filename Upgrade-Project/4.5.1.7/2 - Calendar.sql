/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Calendar ADD
	Image nvarchar(150) NULL
GO
ALTER TABLE dbo.Calendar SET (LOCK_ESCALATION = TABLE)
GO
COMMIT




/****** Object:  View [dbo].[CalendarView]    Script Date: 5/2/2015 4:25:55 PM ******/
DROP VIEW [dbo].[CalendarView]
GO

/****** Object:  View [dbo].[CalendarView]    Script Date: 5/2/2015 4:25:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CalendarView]
AS
SELECT        Id, DateFrom, DateTo, Time, Title, Location, Status, Description, CategoryId, UserId, LastModified, Language, Image
FROM            dbo.Calendar AS c

GO



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Calendar
	(
	Id int NOT NULL IDENTITY (1, 1),
	DateFrom datetime NULL,
	DateTo datetime NULL,
	Time varchar(50) NULL,
	Title nvarchar(150) NULL,
	Location varchar(150) NULL,
	Status int NULL,
	Description ntext NULL,
	CategoryId int NULL,
	UserId int NULL,
	Language smallint NULL,
	LastModified datetime NULL,
	Image nvarchar(150) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Calendar SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Calendar ON
GO
IF EXISTS(SELECT * FROM dbo.Calendar)
	 EXEC('INSERT INTO dbo.Tmp_Calendar (Id, DateFrom, DateTo, Time, Title, Location, Status, Description, CategoryId, UserId, Language, LastModified, Image)
		SELECT Id, DateFrom, DateTo, Time, CONVERT(nvarchar(150), Title), Location, Status, Description, CategoryId, UserId, Language, LastModified, Image FROM dbo.Calendar WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Calendar OFF
GO
DROP TABLE dbo.Calendar
GO
EXECUTE sp_rename N'dbo.Tmp_Calendar', N'Calendar', 'OBJECT' 
GO
ALTER TABLE dbo.Calendar ADD CONSTRAINT
	PK_Calendar PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
CREATE FULLTEXT INDEX ON dbo.Calendar
( 
	Time LANGUAGE 1033, 
	Title LANGUAGE 1033, 
	Location LANGUAGE 1033, 
	Description LANGUAGE 1033
 )
KEY INDEX PK_Calendar
ON Search_Calendar
 WITH  CHANGE_TRACKING  AUTO 
GO
ALTER FULLTEXT INDEX ON dbo.Calendar
ENABLE 
GO


