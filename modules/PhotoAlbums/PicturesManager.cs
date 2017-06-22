using System;
using System.IO;
using System.Linq;
using System.Web;
using lw.CTE;
using lw.CTE.Enum;
using lw.Data;
using lw.Utils;
using lw.WebTools;

namespace lw.PhotoAlbums
{
	/// <summary>
	/// Used to handle all dynamic pictures related tasks
	/// ex: Handles relationnal pictures (Products, News, ...)
	/// </summary>
	class PicturesManager: LINQManager
	{
		#region Private Variables
		SiteSections _section = SiteSections.Free;
		int? _id = null;
		#endregion

		#region Constructors
		public PicturesManager()
			: base(cte.lib)
		{
		}
		public PicturesManager(SiteSections section)
			: base(cte.lib)
		{
			this._section = section;
		}
		public PicturesManager(SiteSections section, int id)
			: base(cte.lib)
		{
			this._section = section;
			this._id = id;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Get All Pictures
		/// </summary>
		/// <returns>Pictures Query</returns>
		public IQueryable<Picture> GetAllPictures()
		{
			return from pic in PicturesData.Pictures 
				   select pic;
		}

		/// <summary>
		/// Get Pictures of a specified relation and section
		/// </summary>
		/// <param name="id">Id of the relation</param>
		/// <param name="section">Section</param>
		/// <returns></returns>
		public IQueryable<Picture> GetPictures(int id, SiteSections section)
		{
			var q = from pic in GetAllPictures()
					where pic.Id == id && pic.Section == (short)section
					select pic;
			return q;
		}
		public IQueryable<Picture> GetPictures()
		{
			return GetPictures(Id, Section);
		}
		public IQueryable<Picture> GetPictures(int id)
		{
			return GetPictures(id, Section);
		}
		public IQueryable<Picture> GetPictures(SiteSections section)
		{
			if(_id != null)
				return GetPictures(Id, section);
			var q = from pic in GetAllPictures()
					where pic.Section == (short)section
					select pic;
			return q;
		}


		public Picture GetPicture(int id, string fileName, SiteSections section)
		{
			var q = from pic in GetPictures(id, section)
					where pic.FileName == fileName
					select pic;
			
			if(q.Count() > 0)
				return q.Single(pic => pic.FileName == fileName);
			
			return null;
		}
		public Picture GetPicture(int id, string fileName)
		{
			return GetPicture(id, fileName, Section);
		}


		public int AddPicture(int id, HttpPostedFile picture,
			string caption,
			string url,
			int? sort,
			Status status,
			bool? resize)
		{
			return AddPicture(id, picture, Section, caption, url, sort, status, resize); 
		}
		public int AddPicture(int id, HttpPostedFile picture, 
			SiteSections section, string caption, 
			string url,
			int? sort,
			Status status,
			bool? resize)
		{
			string fileName = Path.GetFileName(picture.FileName);

			var q = from pic in GetPictures(id, section)
					where pic.FileName == fileName
					select pic;
			if (q.Count() > 0)
				return -1;

			Picture newPic = new Picture
			{
				Id = id,
				Caption = caption,
				Section = (short)section,
				Url = url,
				Sort = sort,
				Status = (short)status,
				DateAdded = DateTime.Now,
				DateModified = DateTime.Now
			};

			string path = WebContext.Server.MapPath(GetSectionPath(section));

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			path = WebContext.Server.MapPath(GetPicturesDirectory(id, section));
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			string largePicture = Path.Combine(path, fileName);

			path = WebContext.Server.MapPath(GetThumbPicturesDirectory(id, section));
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			string thumbPicture = Path.Combine(path, fileName);


			picture.SaveAs(largePicture);

			Config cfg = new Config();
			string size = cfg.GetKey(Settings.PhotoAlbumLargeImageSize);
			Dimension dim = new Dimension(size);

			if (dim.Valid)
			{
				GraphicUtils.ImageUtils.Resize(path, path, dim.IntHeight, dim.IntHeight);
			}

			size = cfg.GetKey(Settings.PhotoAlbumThumbImageSize);
			dim = new Dimension(size);
			if (size != "")
			{
				GraphicUtils.ImageUtils.Resize(path, thumbPicture, dim.IntHeight, dim.IntHeight);
			}
			newPic.FileName = fileName;

			PicturesData.Pictures.InsertOnSubmit(newPic);
			Save();

			return newPic.Id;
		}

		/// <summary>
		/// Delete a single picture
		/// </summary>
		/// <param name="id">id of the relation</param>
		/// <param name="fileName">File Name of the pictre</param>
		/// <param name="section">related Section</param>
		/// <returns>true of deleted</returns>
		public bool DeletePicture(int id, string fileName, SiteSections section)
		{
			Picture picture = GetPicture(id, fileName, section);
			if (picture != null)
			{
				picture.Status = (short)Status.Deleted;

				try
				{
					string path = WebContext.Server.MapPath(GetPicturePath(picture));
					if (File.Exists(path))
						File.Delete(path);

					path = WebContext.Server.MapPath(GetThumbPicturePath(picture));
					if (File.Exists(path))
						File.Delete(path);

					PicturesData.Pictures.DeleteOnSubmit(picture);
				}
				catch (Exception ex)
				{
					ErrorContext.Add("Fail to delete pictures", ex.Message);
				}
				finally
				{
					Save();
				}
			}

			return true;
		}
		public void DeletePicture(int id, string fileName)
		{
			DeletePicture(id, fileName, Section);
		}
		public void DeletePictures(int id, SiteSections section)
		{
			var q = GetPictures(id, section);
			foreach (Picture p in q)
			{
				p.Status = (short)Status.Deleted;
			}
			try
			{
				string path = WebContext.Server.MapPath(GetPicturesDirectory(id, section));
				if (Directory.Exists(path))
					Directory.Delete(path);
			}
			catch (Exception Ex)
			{
				ErrorContext.Add("fail to delete pictures for id: " + id.ToString(), Ex.Message);
			}
			finally
			{
				Save();
			}
		}
		public void DeletePictures(int id)
		{
			DeletePictures(id, Section);
		}
		public void DeletePictures()
		{
			DeletePictures(Id, Section);
		}

		public void UpdatePicture(int id, string fileName, SiteSections section, string caption, int sort)
		{
			Picture pic = GetPicture(id, fileName, section);
			if (pic != null)
			{
				pic.Caption = caption;
				pic.Sort = sort;
				pic.DateModified = DateTime.Now;
				pic.Status = (short)Status.Modified;
				Save();
			}
		}

		#endregion


		#region File and directory path
		public string GetPicturePath(Picture pic)
		{
			return Path.Combine(GetPicturesDirectory(pic.Id, pic.Section), pic.FileName);
		}
		public string GetThumbPicturePath(Picture pic)
		{
			return Path.Combine(GetThumbPicturesDirectory(pic.Id, pic.Section), pic.FileName);
		}
		public string GetSectionPath(SiteSections section)
		{
			return Path.Combine(WebContext.Root, 
					Path.Combine(Folders.CommonPictures, 
					section.ToString()));
		}
		public string GetSectionPath(short section)
		{
			return GetSectionPath((SiteSections)Enum.Parse(typeof(SiteSections), section.ToString()));
		}
		public string GetSectionPath()
		{
			return GetSectionPath(Section);
		}
		public string GetPicturesDirectory(int id, SiteSections section)
		{
			return Path.Combine(
						GetSectionPath(section), 
					id.ToString());
		}
		public string GetPicturesDirectory(int id, short section)
		{
			return GetPicturesDirectory(id, (SiteSections)Enum.Parse(typeof(SiteSections), section.ToString()));
		}
		public string GetPicturesDirectory(int id)
		{
			return GetPicturesDirectory(id, Section);
		}
		public string GetThumbPicturesDirectory(int id, SiteSections section)
		{
			return Path.Combine(
						Path.Combine(
							GetPicturesDirectory(id, section), 
							id.ToString()),
						"t");
		}
		public string GetThumbPicturesDirectory(int id, short section)
		{
			return GetThumbPicturesDirectory(id, (SiteSections)Enum.Parse(typeof(SiteSections), section.ToString()));
		}
		public string GetThumbPicturesDirectory(int id)
		{
			return GetThumbPicturesDirectory(id, Section);
		}
		#endregion
		
		#region Properties
		public PicturesDataDataContext PicturesData
		{
			get
			{
				return (PicturesDataDataContext)this.dataContext;
			}
		}
		public SiteSections Section
		{
			get { return _section; }
			set { _section = value; }
		}
		public int Id
		{
			get { return _id.Value; }
			set { _id = value; }
		}
		#endregion
	}
}
