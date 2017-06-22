

CREATE TRIGGER [dbo].[Trg_DeletePhotoAlbum]
   ON  [dbo].[PhotoAlbums]
   AFTER Delete
AS 
BEGIN
	Delete From PhotoAlbumImages where AlbumId in (select Id from deleted)
END
