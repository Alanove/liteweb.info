ALTER PROCEDURE [dbo].[SearchSite]
	@KeyWord varchar(250)
AS
BEGIN

select PageId as Id, Title as Name, SmallDescription as Description, FullURL as UniqueName, 'Pages' as TableType, K.Rank, Status
from Pages_View
Inner Join FREETEXTTABLE(Pages, (Title, Header, SmallDescription, PageContent, URL), @KeyWord) as K
on PageId = k.[Key]

Union All

select DownloadId as Id, Title as Name, Description, FileName as UniqueName, 'Downloads' as TableType, K.Rank, Status
from Downloads
Inner Join FREETEXTTABLE(Downloads, (Title, Description, FileName), @KeyWord) as K
on DownloadId = k.[Key]

Union All

select Id as Id, Caption as Name, Caption as Description, FileName as UniqueName, 'PhotoAlbumImages' as TableType, K.Rank, Status = 1 
from PhotoAlbumImages
Inner Join FREETEXTTABLE(PhotoAlbumImages, (Caption, FileName), @KeyWord) as K
on Id = k.[Key]

Union All

select Id as Id, Name, Description, DisplayName as UniqueName, 'PhotoAlbums' as TableType, K.Rank, Status
from PhotoAlbums
Inner Join FREETEXTTABLE(PhotoAlbums, (Description, Name, DisplayName), @KeyWord) as K
on Id = k.[Key]

Union All

select Id as Id, Title as Name, Description, Title as UniqueName, 'Calendar' as TableType, K.Rank, Status
from Calendar
Inner Join FREETEXTTABLE(Calendar, (Description, Title), @KeyWord) as K
on Id = k.[Key]

order by RANK desc
END