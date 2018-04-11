GO

/****** Object:  StoredProcedure [dbo].[HashTags_Update]    Script Date: 4/4/2018 12:05:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[HashTags_Update]
	@tags nvarchar(max),
	@RelationId int,
	@RelationType smallint
AS
BEGIN
	SET NOCOUNT ON;

	declare @tagsTable TABLE(id nvarchar(100));
	
	-- Converts the input tags string into am SQL table
	Insert into @tagsTable(id) select Distinct id from dbo.StringToTable(@tags);

	--select * from @tagsTable;

	-- Deleting tags that are no longer related to this post
	delete from HashTags_Relations  where 
		RelateTo = @RelationId 
		and RelationType = @RelationType 
		and TagId not in (
			select TagId from HashTags where Tag in (
				select id from @tagsTable
			)
		);

	-- Creating new tags that do not already exist in the DB
	Insert into HashTags (Tag)  select id from @tagsTable where id not in (select Tag from HashTags);

	--select * from HashTags;

	-- Deleting the tags that are already related if any
	delete from @tagsTable where id in 	(
		select Tag from HashTags where TagId in (
			select TagId from HashTags_Relations where RelateTo = @RelationId and RelationType = @RelationType
		)
	);

	-- Creating the relation for new tags
	Insert into HashTags_Relations (TagId, RelateTo, RelationType) 
		select Distinct JT.TagId, @RelationId, @RelationType from (
			select T.TagId from HashTags T Inner Join @tagsTable TT on T.Tag = TT.id 
		) JT;
END




GO

