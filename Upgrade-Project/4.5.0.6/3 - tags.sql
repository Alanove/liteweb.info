GO

/****** Object:  Table [dbo].[Tags]    Script Date: 3/20/2015 12:49:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HashTags](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](50) NULL,
 CONSTRAINT [PK_HashTags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GO

/****** Object:  Table [dbo].[HashTags_Relations]    Script Date: 3/20/2015 12:50:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HashTags_Relations](
	[TagId] [int] NOT NULL,
	[RelateTo] [int] NOT NULL,
	[RelationType] [smallint] NOT NULL,
 CONSTRAINT [PK_HashTags_Relations] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[RelateTo] ASC,
	[RelationType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



-----
ALTER TABLE dbo.Pages ADD
	Tags nvarchar(256) NULL


GO

/****** Object:  StoredProcedure [dbo].[HashTags_Update]    Script Date: 3/23/2015 9:15:45 AM ******/
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



GO

/****** Object:  UserDefinedFunction [dbo].[Pages_GetDescendantsByPageName]    Script Date: 3/23/2015 3:30:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Pages_GetDescendantsByPageName]
(
	@Page nvarchar(256)
)
RETURNS @ret Table 
(
	PageId int,
	URL nvarchar(256),
	Title nvarchar(256),
	ParentId int,
	[Level] int
)
AS
BEGIN
	Declare @PageId int;
	select top 1 @PageId=PageId from Pages where URL=@Page or Title=@Page;
	
	insert into  @ret (PageId, URL, Title, ParentId, [Level]) select PageId, URL, Title, ParentId, [Level] from dbo.Pages_GetDescendants(@PageId)

	RETURN 
END

GO




Create View HashTagRelationView as
select ht.*,htr.RelateTo, htr.RelationType from HashTags_Relations htr
inner Join HashTags ht on 
htr.TagId = ht.TagId