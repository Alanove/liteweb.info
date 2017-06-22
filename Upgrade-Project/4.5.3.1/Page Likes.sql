﻿GO

/****** Object:  Table [dbo].[Page_Likes]    Script Date: 1/16/2017 3:42:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Page_Likes](
	[PageId] [int] NOT NULL,
	[MemberId] [int] NOT NULL,
	[Feeling] [int] NULL,
 CONSTRAINT [PK_Page_Likes] PRIMARY KEY CLUSTERED 
(
	[PageId] ASC,
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-----
drop view pages_view

----------------------
create view Pages_View as
SELECT        
P.PageId, P.ParentId, P.URL, P.Title, P.Header, P.SmallDescription, P.PageContent, P.Status, P.Language, P.PageFile, P.Image, P.CreatedBy, P.ModifiedBy, P.DateCreated, P.DateModified, P.PublishDate, 
P.Ranking, P.Views, P.History, P.PageType, P.PageTemplate, dbo.Pages_GetPath(P.PageId) AS FullURL, P1.Title AS ParentTitle, 
PT.Title AS Template, PT.Filename AS TemplateFileName, P.IsSecure,
P.AccessRoles, P.EditingRoles, M.UserName, M.FirstName, M.LastName, M.FirstName + ' ' + M.LastName AS Name, P.Tags , 
ISNULL(pl.UserRating, 0) AS UserRating, P1.URL AS ParentURL, P.Keywords
FROM            dbo.Pages AS P LEFT OUTER JOIN
                         dbo.Pages AS P1 ON P.ParentId = P1.PageId LEFT OUTER JOIN
                         dbo.PageTemplates AS PT ON P.PageTemplate = PT.TemplateId LEFT OUTER JOIN
                         dbo.Members AS M ON M.MemberId = P.CreatedBy LEFT OUTER JOIN
                             (SELECT        PageId, COUNT(Feeling) AS UserRating
                               FROM            dbo.Page_Likes
                               GROUP BY PageId) AS pl ON P.PageId = pl.PageId