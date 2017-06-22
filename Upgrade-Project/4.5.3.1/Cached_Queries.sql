GO
/****** Object:  Table [dbo].[CachedQueries]    Script Date: 6/17/2017 9:01:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CachedQueries](
	[KeywordId] [int] IDENTITY(1,1) NOT NULL,
	[Keyword] [nvarchar](512) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_CachedQueries] PRIMARY KEY CLUSTERED 
(
	[KeywordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CachedQueries_Pages]    Script Date: 6/17/2017 9:01:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CachedQueries_Pages](
	[KeywordId] [int] NOT NULL,
	[PageId] [int] NOT NULL,
	[Rank] [int] NULL,
 CONSTRAINT [PK_CachedQueries_Pages] PRIMARY KEY CLUSTERED 
(
	[KeywordId] ASC,
	[PageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [CachedQueries_Keyword_Index]    Script Date: 6/17/2017 9:01:51 PM ******/
CREATE NONCLUSTERED INDEX [CachedQueries_Keyword_Index] ON [dbo].[CachedQueries]
(
	[Keyword] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER PROCEDURE [dbo].[RelatedPages]
@PageId int,
@Max int
AS
BEGIN
	Declare @Title nvarchar(100), @Header nvarchar(100), @SmallDescription nvarchar(256), @Tags  nvarchar(256);

	Select @Title=Title, @Header = Header, @SmallDescription = SmallDescription, @Tags=Tags from Pages where PageId=@PageId;

declare @keyword nvarchar(4000);
set @KeyWord = left(@Title + ' ' + @Tags  + ' ' + @Header + ' ' + @SmallDescription, 200);

declare @KeywordId int;
set @KeywordId=-1;

Begin Transaction

Select @KeywordId=KeywordId from CachedQueries where Keyword=@keyword and Date>DATEADD(m, -1, getdate())

Declare @SQL nvarchar(max);

If (@KeywordId = -1)
Begin
--select 'keyword not found' as ballout;
	Delete from CachedQueries where Keyword=@keyword;
	Insert into CachedQueries values (@Keyword, getdate());
	set @KeywordId= @@IDENTITY;

	set @SQL = N'insert into CachedQueries_Pages select Distinct Top ' + Cast(@Max as varchar) +' @KeywordId, [Key], Rank 
		from FREETEXTTABLE(Pages, (Tags, Title, Header, SmallDescription),  @Keyword) where  [Key] <> @PageId order by Rank Desc'

	set @keyword = replace(@keyword, '''', '''''');

	EXEC sp_executesql @SQL, 
		N'@KeywordId int, @Keyword nvarchar(4000), @PageId int', 
		@KeywordId=@KeywordId, @keyword = @keyword, @PageId = @PageId

End

Commit

	set @SQL = N'select Top ' + Cast(@Max as varchar) + ' Pages_View.*,CachedQueries_Pages.Rank from Pages_View
		Inner Join CachedQueries_Pages on Pages_View.PageId = CachedQueries_Pages.PageId
		where CachedQueries_Pages.KeywordId=@KeywordId
		Order By Rank DESC';

	EXEC sp_executesql @SQL, 
		N'@KeywordId int', 
		@KeywordId=@KeywordId
END



Go

CREATE TRIGGER Cached_Queries_Delete
   ON  CachedQueries
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	delete from CachedQueries_Pages where KeywordId in (select KeywordId from deleted);

END
GO


ALTER TRIGGER [dbo].[Page_Delete]
   ON  [dbo].[Pages]
   AFTER DELETE
AS 
BEGIN

Delete from PageDataPropertyValue where PageId in (select PageID from deleted);

Delete from CachedQueries_Pages where PageId  in (select PageId from deleted);

END

Go
