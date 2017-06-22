/****** Object:  View [dbo].[VideosView]    Script Date: 8/9/2016 1:46:20 PM ******/
DROP VIEW [dbo].[VideosView]
GO

/****** Object:  View [dbo].[VideosView]    Script Date: 8/9/2016 1:46:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VideosView]
AS
SELECT        V.VideoId, V.Title, V.Status, V.Object, V.Description, V.CategoryId, V.CreatorId, V.DateCreated, V.DateModified, V.UniqueName, V.ThumbImage, V.ModifierId, V.VideoFile, V.VideoLength, V.Language, 
                         VC.Title AS VideoCategoryTitle, VC.UniqueName AS VideoCategoryUniqueName, VC.ThumbWidth, VC.ThumbHeight, VC.ThumbSize, VC.Status AS VideoCategoryStatus, VC.CategoryId AS VideoCategoryId, 
                         M.UserName
FROM            dbo.Videos AS V INNER JOIN
                         dbo.VideoCategories AS VC ON V.CategoryId = VC.CategoryId INNER JOIN
                         dbo.Members AS M ON V.CreatorId = M.MemberId

GO