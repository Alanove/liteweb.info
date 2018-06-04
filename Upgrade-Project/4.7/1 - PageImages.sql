
GO

/****** Object:  Table [dbo].[PhotoAlbumImages]    Script Date: 5/3/2018 7:10:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PageImages](
	[ImageId] [int] IDENTITY(1,1) NOT NULL,
	[PageId] [int] NULL,
	[Sort] [int] NULL,
	[Caption] [nvarchar](250) NULL,
	[FileName] [varchar](150) NULL,
	[DateAdded] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[Width] int,
	[Height] int
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
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
ALTER TABLE dbo.PageImages ADD CONSTRAINT
	PK_PageImages PRIMARY KEY CLUSTERED 
	(
	ImageId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.PageImages SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

GO

/****** Object:  Index [PageImages_PageIdIndex]    Script Date: 5/3/2018 7:39:42 AM ******/
CREATE NONCLUSTERED INDEX [PageImages_PageIdIndex] ON [dbo].[PageImages]
(
	[PageId] ASC,
	[Sort] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO



GO
/****** Object:  Trigger [dbo].[Page_Delete]    Script Date: 5/3/2018 7:34:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER TRIGGER [dbo].[Page_Delete]
   ON  [dbo].[Pages]
   AFTER DELETE
AS 
BEGIN

Delete from PageDataPropertyValue where PageId in (select PageID from deleted);
Delete From HashTags_Relations where RelateTo in (select PageID from deleted) and RelationType in( 1)
Delete from PageImages where PageId in (select PageID from deleted);

END






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
ALTER TABLE dbo.Pages ADD
	ImageWidth int NULL,
	ImageHeight int NULL
GO
ALTER TABLE dbo.Pages SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


Go

Drop View Pages_View

GO

/****** Object:  View [dbo].[Pages_View]    Script Date: 5/31/2018 10:43:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*--------------------*/
CREATE VIEW [dbo].[Pages_View]
AS
SELECT        P.PageId, P.ParentId, P.URL, P.Title, P.Header, P.SmallDescription, P.PageContent, P.Status, P.Language, P.PageFile, P.Image, P.CreatedBy, P.ModifiedBy, P.DateCreated, P.DateModified, P.PublishDate, P.Ranking, P.Views, 
                         P.History, P.PageType, P.PageTemplate, dbo.Pages_GetPath(P.PageId) AS FullURL, P1.Title AS ParentTitle, PT.Title AS Template, PT.Filename AS TemplateFileName, P.IsSecure, P.AccessRoles, P.EditingRoles, M.UserName, 
                         M.FirstName, M.LastName, ISNULL(M.FirstName, '') + ' ' + ISNULL(M.LastName, '') AS Name, P.Tags, ISNULL(pl.UserRating, 0) AS UserRating, P1.URL AS ParentURL, P.Keywords, M.Geuid, M.Picture AS MemberPicture, 
                         P1.ImageHeight, P1.ImageWidth
FROM            dbo.Pages AS P LEFT OUTER JOIN
                         dbo.Pages AS P1 ON P.ParentId = P1.PageId LEFT OUTER JOIN
                         dbo.PageTemplates AS PT ON P.PageTemplate = PT.TemplateId LEFT OUTER JOIN
                         dbo.Members AS M ON M.MemberId = P.CreatedBy LEFT OUTER JOIN
                             (SELECT        PageId, COUNT(Feeling) AS UserRating
                               FROM            dbo.Page_Likes
                               GROUP BY PageId) AS pl ON P.PageId = pl.PageId

GO