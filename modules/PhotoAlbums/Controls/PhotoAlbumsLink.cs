using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.Base;
using lw.CTE.Enum;
using lw.Utils;
using lw.WebTools;

using lw.CTE;


namespace lw.PhotoAlbums.Controls
{
	public class PhotoAlbumsLink : HtmlAnchor
	{
		bool _bound = false;
		PhotoAlbumsLinkType _type = PhotoAlbumsLinkType.Photos;
		bool _IncludePhotoAlbumsPath = false;
		string _innerText = "";
		string _format = "{0}";
		bool _hideText = false;
		string _path = "";
		string _category = null;
		int? _categoryId = null;
		string _extension = "";
		CustomPage _page;

		PhotoAlbumsManager pMgr;

		public PhotoAlbumsLink()
		{
			pMgr = new PhotoAlbumsManager();
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			_page = this.Page as CustomPage;

			int _itemId = -1;
			object obj = DataBinder.Eval(this.NamingContainer, "DataItem");

			DataRowView _PhotoAlbumDetails = obj as DataRowView;
			DataRow _PhotoAlbum = null;

			if (_PhotoAlbumDetails != null)
			{
				_PhotoAlbum = _PhotoAlbumDetails.Row;
			}
			else
			{
				_PhotoAlbum = obj as DataRow;
			}

			string text = "";

			if (_PhotoAlbum == null)
			{
				obj = DataBinder.Eval(this.NamingContainer, "DataItem.Id");
				if (!String.IsNullOrEmpty(obj.ToString()))
					_itemId = (int)obj;
				else
					return;

				DataView dv;

				if (CategoryId > 0)
					dv = pMgr.GetAllPhotoAlbums(string.Format("id={0} and UniqueName='{1}'", _itemId, Category));
				else
					dv = pMgr.GetPhotoAlbums(string.Format("id={0}", _itemId));

				if (dv != null && dv.Count > 0)
				{
					_PhotoAlbum = dv[0].Row;
				}
			}

			if (_PhotoAlbum == null)
				return;

			text = _PhotoAlbum["DisplayName"].ToString();


			switch (Type)
			{
				case PhotoAlbumsLinkType.ArchiveByType:
					text = (string)_PhotoAlbum["CategoryName"];
					this.HRef = string.Format("{2}/{0}/{1}{3}",
						Path == "" ? "photo-gallery" : Path,
						_PhotoAlbum["UniqueName"],
						WebContext.Root,
						Extension
					);
					break;
				case PhotoAlbumsLinkType.AlbumId:
					if (IncludePhotoAlbumsPath)
					{
						this.HRef = string.Format("{3}/{0}{1}/{2}{4}",
							Path == null ? "" : Path + "/",
							_PhotoAlbum["Name"],
							_PhotoAlbum["Id"],
							WebContext.Root, Extension
						);
					}
					else
					{
						this.HRef = string.Format("{2}/{0}{1}{3}",
							Path == null ? "" : Path + "/",
							_PhotoAlbum["Id"],
							WebContext.Root, Extension
						);
					}
					break;
				case PhotoAlbumsLinkType.Photos:
					string _network = _page.GetQueryValue(RoutingParameters.Network);
					if (!String.IsNullOrWhiteSpace(_network))
					{
						if (IncludePhotoAlbumsPath)
						{
							this.HRef = string.Format("{3}/{0}/{1}/{2}{4}",
								Path == "" ? _network + "/photo-gallery" : Path,
								_PhotoAlbum["UniqueName"],
								_PhotoAlbum["Name"],
								WebContext.Root, Extension
							);
						}
						else
						{
							this.HRef = string.Format("{2}/{0}/{1}{3}",
								Path == "" ? _network + "/photo-gallery" : Path,
								_PhotoAlbum["Name"],
								WebContext.Root, Extension
							);
						}
					}
					else
					{
						if (IncludePhotoAlbumsPath)
						{
							this.HRef = string.Format("{3}/{0}/{1}/{2}{4}",
								Path == "" ? "photo-gallery" : Path,
								_PhotoAlbum["UniqueName"],
								_PhotoAlbum["Name"],
								WebContext.Root, Extension
							);
						}
						else
						{
							this.HRef = string.Format("{2}/{0}/{1}{3}",
								Path == "" ? "photo-gallery" : Path,
								_PhotoAlbum["Name"],
								WebContext.Root, Extension
							);
						}
					}
					break;
				default:
					if (IncludePhotoAlbumsPath)
					{
						this.HRef = string.Format("{3}/{0}/{1}/{2}{4}",
							Path == "" ? "photo-gallery" : Path,
							_PhotoAlbum["UniqueName"],
							_PhotoAlbum["Name"],
							WebContext.Root, Extension
						);
					}
					else
					{
						this.HRef = string.Format("{2}/{0}/{1}{3}",
							Path == "" ? "photo-gallery" : Path,
							_PhotoAlbum["Name"],
							WebContext.Root, Extension
						);
					}
					break;
			}
			if (this.Controls.Count == 0)
			{
				this.InnerHtml = lw.Utils.StringUtils.AddSup(string.Format(Format, text));
			}

			base.DataBind();
			this.Attributes.Add("title", string.Format(Format, text));

			if (MyPage.Editable && ImEditable)
			{
				this.Attributes.Add("data-editable", "true");
				this.Attributes.Add("data-id", _PhotoAlbum["Id"].ToString());
				this.Attributes.Add("data-type", "photoalbums");
			}
		}

		CustomPage myPage = null;
		CustomPage MyPage
		{
			get
			{
				if (myPage == null)
				{
					myPage = this.Page as CustomPage;
				}
				return myPage;
			}

		}


		public PhotoAlbumsLinkType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}
		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}
		public string Path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}
		public bool IncludePhotoAlbumsPath
		{
			get { return _IncludePhotoAlbumsPath; }
			set { _IncludePhotoAlbumsPath = value; }
		}
		public bool HideText
		{
			get { return _hideText; }
			set { _hideText = value; }
		}
		public string Category
		{
			get
			{
				if (StringUtils.IsNullOrWhiteSpace(_category))
				{
					string uniqueName = _page.GetQueryValue(RoutingParameters.AlbumCategory);

					if (!StringUtils.IsNullOrWhiteSpace(uniqueName))
					{
						_category = StringUtils.SQLEncode(uniqueName);
					}
				}
				return _category;
			}
			set { _category = value; }
		}
		public int? CategoryId
		{
			get
			{
				if (_categoryId == null)
				{
					string id = _page.GetQueryValue(RoutingParameters.AlbumCategoryID);

					if (!StringUtils.IsNullOrWhiteSpace(id))
					{
						_categoryId = Int32.Parse(id);
					}
					else
					{
						if (!String.IsNullOrEmpty(Category))
						{
							DataRow cat = pMgr.GetCategoriesByName(Category);
							if (cat != null)
							{
								_categoryId = (int)cat["CategoryId"];
							}
						}
					}
				}
				return _categoryId;
			}
			set { _categoryId = value; }
		}

		string Extension
		{
			get { return _extension; }
			set { _extension = value; }
		}


		bool _imEditable = false;
		/// <summary>
		/// Indicates if this link should contain the editing parameters
		/// </summary>
		public bool ImEditable
		{
			get
			{
				return _imEditable;
			}
			set
			{
				_imEditable = value;
			}
		}
	}
}
