Drop View Pages_View

---

Create View Pages_View
as
SELECT        P.PageId, P.ParentId, P.URL, P.Title, P.Header, P.SmallDescription, P.PageContent, P.Status, P.Language, P.PageFile, P.Image, P.CreatedBy, P.ModifiedBy, P.DateCreated, P.DateModified, P.PublishDate, 
                         P.Ranking, P.Views, P.UserRating, P.History, P.PageType, P.PageTemplate, dbo.Pages_GetPath(P.PageId) AS FullURL, P1.Title AS ParentTitle, PT.Title AS Template, PT.Filename AS TemplateFileName, 
                         P.IsSecure, P.Tags, P.AccessRoles, P.EditingRoles, M.UserName, M.FirstName, M.LastName, M.FirstName + ' ' + M.LastName AS Name
FROM            dbo.Pages AS P LEFT OUTER JOIN
                         dbo.Pages AS P1 ON P.ParentId = P1.PageId LEFT OUTER JOIN
                         dbo.PageTemplates AS PT ON P.PageTemplate = PT.TemplateId LEFT OUTER JOIN
                         dbo.Members AS M ON M.MemberId = P.CreatedBy