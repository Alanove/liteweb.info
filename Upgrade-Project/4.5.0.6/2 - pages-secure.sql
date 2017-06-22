---------- Alain

ALTER TABLE dbo.Pages ADD
	IsSecure bit NULL
GO
ALTER TABLE dbo.Pages SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



---------------------

drop view Pages_View


------------------------------

GO

/****** Object:  View [dbo].[Pages_View]    Script Date: 3/13/2015 2:34:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Pages_View]
AS
SELECT        P.PageId, P.ParentId, P.URL, P.Title, P.Header, P.SmallDescription, P.PageContent, P.Status, P.Language, P.PageFile, P.Image, P.CreatedBy, P.ModifiedBy, P.DateCreated, P.DateModified, P.PublishDate, 
                         P.Ranking, P.Views, P.UserRating, P.History, P.PageType, P.PageTemplate, dbo.Pages_GetPath(P.PageId) AS FullURL, P1.Title AS ParentTitle, PT.Title AS Template, PT.Filename AS TemplateFileName, 
                         P.IsSecure
FROM            dbo.Pages AS P LEFT OUTER JOIN
                         dbo.Pages AS P1 ON P.ParentId = P1.PageId LEFT OUTER JOIN
                         dbo.PageTemplates AS PT ON P.PageTemplate = PT.TemplateId

GO


