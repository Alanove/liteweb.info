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
	Keywords nvarchar(512) NULL
GO
ALTER TABLE dbo.Pages SET (LOCK_ESCALATION = TABLE)
GO
COMMIT





/****** Object:  View [dbo].[Pages_View]    Script Date: 7/7/2016 11:44:39 AM ******/
DROP VIEW [dbo].[Pages_View]
GO

/****** Object:  View [dbo].[Pages_View]    Script Date: 7/7/2016 11:44:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Pages_View]
AS
SELECT        P.PageId, P.ParentId, P.URL, P.Title, P.Header, P.SmallDescription, P.PageContent, P.Status, P.Language, P.PageFile, P.Image, P.CreatedBy, P.ModifiedBy, P.DateCreated, P.DateModified, P.PublishDate, 
                         P.Ranking, P.Views, P.UserRating, P.History, P.PageType, P.PageTemplate, dbo.Pages_GetPath(P.PageId) AS FullURL, P1.Title AS ParentTitle, PT.Title AS Template, PT.Filename AS TemplateFileName, 
                         P.IsSecure, P.Tags, P.AccessRoles, P.EditingRoles, M.UserName, M.FirstName, M.LastName, M.FirstName + ' ' + M.LastName AS Name, P1.URL AS ParentURL, P.Keywords
FROM            dbo.Pages AS P LEFT OUTER JOIN
                         dbo.Pages AS P1 ON P.ParentId = P1.PageId LEFT OUTER JOIN
                         dbo.PageTemplates AS PT ON P.PageTemplate = PT.TemplateId LEFT OUTER JOIN
                         dbo.Members AS M ON M.MemberId = P.CreatedBy

GO


/****** Object:  View [dbo].[Pages_View_Comments]    Script Date: 7/7/2016 11:48:02 AM ******/
DROP VIEW [dbo].[Pages_View_Comments]
GO

/****** Object:  View [dbo].[Pages_View_Comments]    Script Date: 7/7/2016 11:48:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Pages_View_Comments]
AS
SELECT        pv.PageId, pv.ParentId, pv.URL, pv.Title, pv.Header, pv.SmallDescription, pv.PageContent, pv.Status, pv.Language, pv.PageFile, pv.Image, pv.CreatedBy, pv.CreatedBy AS MemberId, pv.ModifiedBy, 
                         pv.DateCreated, pv.DateModified, pv.PublishDate, pv.Ranking, pv.Views, pv.UserRating, pv.History, pv.PageType, pv.PageTemplate, pv.FullURL, pv.ParentTitle, pv.Template, pv.TemplateFileName, 
                         C.CommentsCount, M.UserName, M.FirstName, M.LastName, M.FirstName + ' ' + M.LastName AS Name, M.Picture, M.Privacy, pv.IsSecure, pv.Keywords, pv.ParentURL, pv.Tags
FROM            dbo.Pages_View AS pv LEFT OUTER JOIN
                             (SELECT        COUNT(*) AS CommentsCount, RelationId AS PageId
                               FROM            dbo.Comments_Pages
                               GROUP BY RelationId) AS C ON pv.PageId = C.PageId LEFT OUTER JOIN
                         dbo.Members AS M ON M.MemberId = pv.CreatedBy

GO