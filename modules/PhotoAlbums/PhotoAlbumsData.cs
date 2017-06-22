using System.ComponentModel;

using lw.Data; 

namespace lw.PhotoAlbums
{
	public partial class PhotoAlbumsData : ToCastToComponenet
	{
		public PhotoAlbumsData()
		{
			InitializeComponent();
			initData();
		}

		public PhotoAlbumsData(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
			initData();
		}
		protected override void initData()
		{
			this.AddDataComponent(cte.PhotoAlbumCategoriesAdp, this._sqlPhotoAlbumCategoriesAdp);
			this.AddDataComponent(cte.PhotoAlbumImagesAdp, this._sqlPhotoAlbumImagesAdp);
			this.AddDataComponent(cte.PhotoAlbumsAdp, this._sqlPhotoAlbumsAdp);

			this.AddDataComponent(cte.AlbumsViewAdp, this._sqlAlbumsView);
		}
	}
}
