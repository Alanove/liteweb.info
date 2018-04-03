/****** Object:  View [dbo].[Pages_View]    Script Date: 3/9/2018 8:08:50 AM ******/
DROP VIEW [dbo].[Pages_View]
GO

/****** Object:  View [dbo].[Pages_View]    Script Date: 3/9/2018 8:08:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*--------------------*/
CREATE VIEW [dbo].[Pages_View]
AS
SELECT        P.PageId, P.ParentId, P.URL, P.Title, P.Header, P.SmallDescription, P.PageContent, P.Status, P.Language, P.PageFile, P.Image, P.CreatedBy, P.ModifiedBy, P.DateCreated, P.DateModified, P.PublishDate, P.Ranking, P.Views, 
                         P.History, P.PageType, P.PageTemplate, dbo.Pages_GetPath(P.PageId) AS FullURL, P1.Title AS ParentTitle, PT.Title AS Template, PT.Filename AS TemplateFileName, P.IsSecure, P.AccessRoles, P.EditingRoles, M.UserName, 
                         M.FirstName, M.LastName, ISNULL(M.FirstName, '') + ' ' + ISNULL(M.LastName, '') AS Name, P.Tags, ISNULL(pl.UserRating, 0) AS UserRating, P1.URL AS ParentURL, P.Keywords, M.Geuid, M.Picture
FROM            dbo.Pages AS P LEFT OUTER JOIN
                         dbo.Pages AS P1 ON P.ParentId = P1.PageId LEFT OUTER JOIN
                         dbo.PageTemplates AS PT ON P.PageTemplate = PT.TemplateId LEFT OUTER JOIN
                         dbo.Members AS M ON M.MemberId = P.CreatedBy LEFT OUTER JOIN
                             (SELECT        PageId, COUNT(Feeling) AS UserRating
                               FROM            dbo.Page_Likes
                               GROUP BY PageId) AS pl ON P.PageId = pl.PageId

GO