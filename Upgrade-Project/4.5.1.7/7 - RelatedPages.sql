-- 

--Before running the below script, add "Tags" to the Pages full text catalog.

--


GO

/****** Object:  StoredProcedure [dbo].[RelatedPages]    Script Date: 6/6/2016 3:01:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RelatedPages]
@PageId int,
@Max int
AS
BEGIN



	SET NOCOUNT ON;

	Declare @Title nvarchar(100), @Header nvarchar(100), @SmallDescription nvarchar(256), @Tags  nvarchar(256);

	Select @Title=Title, @Header = Header, @SmallDescription = SmallDescription, @Tags=Tags from Pages where PageId=@PageId;

	Declare @KeyWord nvarchar(4000);

	set @KeyWord = Isnull(@Tags, '') + ' ' + @Title + ' ' + @Header + ' ' + @SmallDescription;

	Declare @SQL nvarchar(max);
	set @SQL = N'select Top ' + Cast(@Max as varchar) + ' Pages_View.* from Pages_View
		Inner Join FREETEXTTABLE(Pages, (Tags, Title, Header, SmallDescription), N''' + Replace(@Keyword, '''', '''''') +''') as K
		on PageId = k.[Key]
		where PageId <> @PageId
		Order By Rank DESC';

	

	EXEC sp_executesql @SQL, 
		N'@PageId int', 
		@PageId=@PageId
END

GO

