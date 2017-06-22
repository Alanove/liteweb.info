using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
//using Microsoft.WindowsAPICodePack.Shell;


using lw.CTE;
using lw.CTE.Enum;
using lw.Data;
using lw.GraphicUtils;
using lw.Utils;
using lw.WebTools;

namespace lw.VideoGallery
{
	public class MediaGalleryManager : LINQManager
	{
		public MediaGalleryManager()
			: base(cte.lib)
		{ }

		#region Videos


		public IQueryable<Video> GetVideos()
		{
			var query = from video in MediaData.Videos
						select video;
			return query;
		}

		public Video GetVideo(int VideoId)
		{
			return MediaData.Videos.Single(temp => temp.VideoId == VideoId);
		}

		public VideosView GetVideoDetails(int VideoId)
		{
			var q = from v in MediaData.VideosViews
					where v.VideoId == VideoId
					select v;
			if (q.Count() > 0)
				return q.First();
			return null;
		}
		public VideosView GetVideoDetails(string UniqueName)
		{
			var q = from v in MediaData.VideosViews
					where v.UniqueName == UniqueName
					select v;
			if (q.Count() > 0)
				return q.First();
			return null;
		}


		public int AddVideo(string Title, VideoStatus Status, string Object, string Description, int? CategoryId,
			int? CreatorId, HttpPostedFile Image, bool? AutoResize, Languages? lan)
		{
			return AddVideo(Title, Status, Object, Description, CategoryId,
				CreatorId, Image, AutoResize, lan, null);
		}

		public int AddVideo(string Title, VideoStatus Status, string Object, string Description, int? CategoryId,
			int? CreatorId, HttpPostedFile Image, bool? AutoResize, Languages? lan, HttpPostedFile VideoFile)
		{
			if (StringUtils.IsNullOrWhiteSpace(Title))
				return -1;

			if (lan == null)
				lan = Languages.Default;

			Video video = new Video
			{
				Title = Title,
				Status = (byte)Status,
				Object = Object,
				Description = Description,
				CategoryId = CategoryId,
				CreatorId = CreatorId.Value,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				UniqueName = StringUtils.ToURL(Title),
				ModifierId = CreatorId,
				Language = (short)lan
			};

			MediaData.Videos.InsertOnSubmit(video);
			MediaData.SubmitChanges();

			bool generateThumb = Image != null && Image.ContentLength > 0;


			if (VideoFile != null && VideoFile.ContentLength > 0)
			{
				string videoName = Path.GetFileNameWithoutExtension(VideoFile.FileName);
				videoName = string.Format("{0}_{1}{2}", videoName, video.VideoId,
					Path.GetExtension(VideoFile.FileName));


				string path = WebContext.Server.MapPath("~/" + Folders.VideosFolder);

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				path = Path.Combine(path, videoName);

				VideoFile.SaveAs(path);


				//ShellFile so = ShellFile.FromFilePath(path);


                //decimal nanoseconds;
                //decimal.TryParse(so.Properties.System.Media.Duration.Value.ToString(), out nanoseconds);

                //decimal seconds = nanoseconds * (decimal)0.0000001;

                //video.VideoLength = seconds;

                //if (!generateThumb)
                //{
                //    string imageName = Path.GetFileNameWithoutExtension(videoName) + ".Jpg";

                //    string thumbImagePath = WebContext.Server.MapPath("~/" + Folders.VideoThumbsFolder);

                //    if (!Directory.Exists(thumbImagePath))
                //    {
                //        Directory.CreateDirectory(thumbImagePath);
                //    }

                //    string imagePath = Path.Combine(thumbImagePath, imageName);

                //    //so.Thumbnail.Bitmap.Save (imagePath);
                //    System.Drawing.Bitmap image;

                //    //force the actual thumbnail, not the icon

                //    //so.Thumbnail.FormatOption = ShellThumbnailFormatOptions.ThumbnailOnly;

                //    //image = so.Thumbnail.ExtraLargeBitmap;
                //    image.Save(imagePath);

                //    VideoCategory cat = GetVideoCategory(CategoryId.Value);
                //    if (cat.ThumbWidth > 0 && cat.ThumbHeight > 0)
                //    {
                //        ImageUtils.CropImage(imagePath, imagePath, (int)cat.ThumbWidth, (int)cat.ThumbHeight, ImageUtils.AnchorPosition.Default);
                //    }
                //    else
                //    {
                //        Config cfg = new Config();
                //        Dimension dim = new Dimension(cfg.GetKey(lw.CTE.parameters.VideoThumbSize));

                //        if (dim.Width > 0 && dim.Height > 0)
                //        {
                //            ImageUtils.CropImage(imagePath, imagePath, (int)dim.Width, (int)dim.Height, ImageUtils.AnchorPosition.Default);
                //        }
                //    }
                //    video.ThumbImage = imageName;
                //}

				video.VideoFile = videoName;

				MediaData.SubmitChanges();
			}


			if (Image != null && Image.ContentLength > 0)
			{
				string ImageName = Path.GetFileNameWithoutExtension(Image.FileName);
				ImageName = string.Format("{0}_{1}{2}", ImageName, video.VideoId,
					Path.GetExtension(Image.FileName));


				string path = WebContext.Server.MapPath("~/" + Folders.VideoThumbsFolder);

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				path = Path.Combine(path, ImageName);

				Image.SaveAs(path);

				if (AutoResize != null && AutoResize.Value == true)
				{
					VideoCategory cat = GetVideoCategory(CategoryId.Value);
					if (cat.ThumbWidth > 0 && cat.ThumbHeight > 0)
					{
						//ImageUtils.Resize(path, path, (int)cat.ThumbWidth, (int)cat.ThumbHeight);
						ImageUtils.CropImage(path, path, (int)cat.ThumbWidth, (int)cat.ThumbHeight, ImageUtils.AnchorPosition.Default);
					}
					else
					{
						Config cfg = new Config();
						Dimension dim = new Dimension(cfg.GetKey(lw.CTE.parameters.VideoThumbSize));

						if (dim.Width > 0 && dim.Height > 0)
						{
							ImageUtils.CropImage(path, path, (int)dim.Width, (int)dim.Height, ImageUtils.AnchorPosition.Default);
						}
					}
				}

				video.ThumbImage = ImageName;

				MediaData.SubmitChanges();

			}
			return video.VideoId;
		}

		public int UpdateVideo(int VideoId, string Title, VideoStatus Status, string Object, string Description, int? CategoryId,
			int ModifierId, HttpPostedFile Image, bool? AutoResize, bool DeleteOldImage, Languages? lan, HttpPostedFile VideoFile)
		{
			if (lan == null)
				lan = Languages.Default;

			var video = GetVideo(VideoId);

			video.Title = Title;
			video.Status = (byte)Status;
			video.Object = Object;
			video.Description = Description;
			video.CategoryId = CategoryId;
			video.DateModified = DateTime.Now;
			video.UniqueName = StringUtils.ToURL(Title);
			video.ModifierId = ModifierId;
			video.Language = (short)lan;

			MediaData.SubmitChanges();

			string path = WebContext.Server.MapPath(Path.Combine(WebContext.StartDir, Folders.VideosFolder));
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			var oldImage = video.ThumbImage;

			bool generateThumb = Image != null && Image.ContentLength > 0;


			if (VideoFile != null && VideoFile.ContentLength > 0)
			{
				string videoName = Path.GetFileNameWithoutExtension(VideoFile.FileName);
				videoName = string.Format("{0}_{1}{2}", videoName, video.VideoId,
					Path.GetExtension(VideoFile.FileName));


				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				path = Path.Combine(path, videoName);

				VideoFile.SaveAs(path);


                //ShellFile so = ShellFile.FromFilePath(path);


                //decimal nanoseconds;
                //decimal.TryParse(so.Properties.System.Media.Duration.Value.ToString(), out nanoseconds);

                //decimal seconds = nanoseconds * (decimal)0.0000001;

                //video.VideoLength = seconds;

				if (!generateThumb)
				{


					string imageName = Path.GetFileNameWithoutExtension(videoName) + ".Jpg";

					string imagePath = Path.Combine(WebContext.Server.MapPath("~/" + Folders.VideoThumbsFolder),
						imageName);

					//so.Thumbnail.Bitmap.Save(imagePath);

					VideoCategory cat = GetVideoCategory(CategoryId.Value);
					if (cat.ThumbWidth > 0 && cat.ThumbHeight > 0)
					{
						ImageUtils.CropImage(imagePath, imagePath, (int)cat.ThumbWidth, (int)cat.ThumbHeight, ImageUtils.AnchorPosition.Default);
					}
					else
					{
						Config cfg = new Config();
						Dimension dim = new Dimension(cfg.GetKey(lw.CTE.parameters.VideoThumbSize));

						if (dim.Width > 0 && dim.Height > 0)
						{
							ImageUtils.CropImage(imagePath, imagePath, (int)dim.Width, (int)dim.Height, ImageUtils.AnchorPosition.Default);
						}
					}
					video.ThumbImage = imageName;
				}

				video.VideoFile = videoName;

				MediaData.SubmitChanges();
			}

			DeleteOldImage = DeleteOldImage || (Image != null && Image.ContentLength > 0);

			if (DeleteOldImage && !StringUtils.IsNullOrWhiteSpace(video.ThumbImage))
			{
				if (File.Exists(Path.Combine(path, oldImage)))
					File.Delete(Path.Combine(path, oldImage));
				video.ThumbImage = "";
			}

			if (AutoResize != null && AutoResize.Value == true)
			{
				VideoCategory cat = GetVideoCategory(CategoryId.Value);
				if (cat.ThumbWidth > 0 && cat.ThumbHeight > 0)
				{
					//ImageUtils.Resize(path, path, (int)cat.ThumbWidth, (int)cat.ThumbHeight);
					ImageUtils.CropImage(path, path, (int)cat.ThumbWidth, (int)cat.ThumbHeight, ImageUtils.AnchorPosition.Default);
				}
				else
				{
					Config cfg = new Config();
					Dimension dim = new Dimension(cfg.GetKey(lw.CTE.parameters.VideoThumbSize));

					if (dim.Width > 0 && dim.Height > 0)
					{
						ImageUtils.CropImage(path, path, (int)dim.Width, (int)dim.Height, ImageUtils.AnchorPosition.Default);
					}
				}
			}

			MediaData.SubmitChanges();
			return VideoId;
		}

		public int UpdateVideo(int VideoId, string Title, VideoStatus Status, string Object, string Description, int? CategoryId,
			int ModifierId, HttpPostedFile Image, bool? AutoResize, bool DeleteOldImage, Languages? lan)
		{
			return UpdateVideoWithImage(VideoId, Title, Status, Object, Description, CategoryId, ModifierId, Image, AutoResize, DeleteOldImage, lan, null);

		}

		public int UpdateVideoWithImage(int VideoId, string Title, VideoStatus Status, string Object, string Description, int? CategoryId,
			int ModifierId, HttpPostedFile Image, bool? AutoResize, bool DeleteOldImage, Languages? lan, bool? Crop)
		{
			if (lan == null)
				lan = Languages.Default;

			var video = GetVideo(VideoId);

			video.Title = Title;
			video.Status = (byte)Status;
			video.Object = Object;
			video.Description = Description;
			video.CategoryId = CategoryId;
			video.DateModified = DateTime.Now;
			video.UniqueName = StringUtils.ToURL(Title);
			video.ModifierId = ModifierId;
			video.Language = (short)lan;

			MediaData.SubmitChanges();

			string path = WebContext.Server.MapPath(Path.Combine(WebContext.StartDir, Folders.VideoThumbsFolder));
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			var oldImage = video.ThumbImage;

			DeleteOldImage = DeleteOldImage || (Image != null && Image.ContentLength > 0);

			if (DeleteOldImage && !StringUtils.IsNullOrWhiteSpace(video.ThumbImage))
			{
				if (File.Exists(Path.Combine(path, oldImage)))
					File.Delete(Path.Combine(path, oldImage));
				video.ThumbImage = "";
			}

			if (Image != null && Image.ContentLength > 0)
			{
				string ImageName = Path.GetFileNameWithoutExtension(Image.FileName);
				ImageName = string.Format("{0}_{1}{2}", ImageName, video.VideoId,
					Path.GetExtension(Image.FileName));

				string _path = Path.Combine(path, ImageName);

				Image.SaveAs(_path);

				if (AutoResize != null && AutoResize.Value)
				{
					VideoCategory cat = GetVideoCategory(CategoryId.Value);
					if (cat.ThumbWidth > 0 && cat.ThumbHeight > 0)
					{
						if (Crop != null && Crop == true)
							ImageUtils.CropImage(_path, _path, (int)cat.ThumbWidth, (int)cat.ThumbHeight, ImageUtils.AnchorPosition.Default);
						else
							ImageUtils.Resize(_path, _path, (int)cat.ThumbWidth, (int)cat.ThumbHeight);
					}
					else
					{
						Config cfg = new Config();
						Dimension dim = new Dimension(cfg.GetKey(lw.CTE.parameters.VideoThumbSize));

						if (dim.Width > 0 && dim.Height > 0)
						{
							ImageUtils.CropImage(_path, _path, (int)dim.Width, (int)dim.Height, ImageUtils.AnchorPosition.Default);
						}
					}
				}

				video.ThumbImage = ImageName;
			}

			MediaData.SubmitChanges();
			return VideoId;
		}

		public void DeleteVideo(int VideoId)
		{
			var video = GetVideo(VideoId);

			MediaData.Videos.DeleteOnSubmit(video);
			DeleteVideoImage(VideoId);
			DeleteVideoFile(VideoId);
			MediaData.SubmitChanges();
		}

		public void DeleteVideoImage(int VideoId)
		{
			var video = GetVideo(VideoId);

			string path = WebContext.Server.MapPath("~/" + Folders.VideoThumbsFolder);

			if (!StringUtils.IsNullOrWhiteSpace(video.ThumbImage))
			{
				if (File.Exists(Path.Combine(path, video.ThumbImage)))
					File.Delete(Path.Combine(path, video.ThumbImage));
			}

		}

		public void DeleteVideoFile(int VideoId)
		{
			var video = GetVideo(VideoId);

			string path = WebContext.Server.MapPath("~/" + Folders.VideosFolder);

			if (!StringUtils.IsNullOrWhiteSpace(video.VideoFile))
			{
				if (File.Exists(Path.Combine(path, video.VideoFile)))
					File.Delete(Path.Combine(path, video.VideoFile));
			}

		}

		public DataTable SearchVideos(string title, VideoStatus? status, int? categoryId)
		{
			StringBuilder sql = new StringBuilder("select Videos.* from Videos where 1=1");

			if (!String.IsNullOrEmpty(title))
				sql.Append(string.Format(" and (Title like N'%{0}%' or Description like N'%{0}%')", StringUtils.SQLEncode(title)));

			if (status != null)
				sql.Append(string.Format(" and Status = {0}", (int)status));

			if (categoryId != null)
				sql.Append(string.Format(" and CategoryId='{0}'", categoryId.Value));

			sql.Append("order by Videos.Title");

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
		}

		#endregion

		#region VideoCategories


		public IQueryable<VideoCategory> GetVideoCategories()
		{
			var query = from videoCategory in MediaData.VideoCategories
						select videoCategory;
			return query;
		}

		public VideoCategory GetVideoCategory(int CategoryId)
		{
			return MediaData.VideoCategories.Single(temp => temp.CategoryId == CategoryId);
		}

		public int AddVideoCategory(string Title, short? ThumbWidth, short? ThumbHeight, bool Status)
		{
			if (StringUtils.IsNullOrWhiteSpace(Title))
				return -1;

			VideoCategory videocat = new VideoCategory
			{
				Title = Title,
				ThumbWidth = ThumbWidth,
				ThumbHeight = ThumbHeight,
				Status = Status,
				UniqueName = StringUtils.ToURL(Title)
			};

			MediaData.VideoCategories.InsertOnSubmit(videocat);
			MediaData.SubmitChanges();

			return videocat.CategoryId;
		}

		public int UpdateVideoCategory(int CategoryId, string Title, short? ThumbWidth, short? ThumbHeight, bool Status)
		{
			var videocat = GetVideoCategory(CategoryId);

			videocat.Title = Title;
			videocat.ThumbWidth = ThumbWidth;
			videocat.ThumbHeight = ThumbHeight;
			videocat.Status = Status;
			videocat.UniqueName = StringUtils.ToURL(Title);

			MediaData.SubmitChanges();

			return CategoryId;
		}

		public void DeleteVideoCategory(int CategoryId)
		{
			var videoCategory = GetVideoCategory(CategoryId);

			MediaData.VideoCategories.DeleteOnSubmit(videoCategory);
			MediaData.SubmitChanges();
		}

		#endregion

		#region

		public IQueryable<VideosView> GetVideosView()
		{
			var query = from video in MediaData.VideosViews
						select video;
			return query;
		}

		public VideosView GetVideoView(int VideoId)
		{
			return MediaData.VideosViews.Single(temp => temp.VideoId == VideoId);
		}

		public DataView GetVideoMax(int max, string sql)
		{
			string _max = "100 PERCENT";
			if (max > 0)
				_max = max.ToString();

			if (sql.Trim() != "")
				sql = " Where " + sql;


			sql = string.Format("select Top {0} VideosView.* from VideosView {1} Order By VideosView.DateModified Desc",
						_max, sql);

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0].DefaultView;
		}

		#endregion

		#region Static Methods


		public static string GetThumbImage(VideosView video)
		{
			if (StringUtils.IsNullOrWhiteSpace(video.ThumbImage))
				return "";

			return Path.Combine(Folders.VideoThumbsFolder, video.ThumbImage);
		}


		public static string GetThumbImage(string videoThumb)
		{
			return Path.Combine(Folders.VideoThumbsFolder, videoThumb);
		}


		public static string GetVideoVile(VideosView video)
		{
			if (StringUtils.IsNullOrWhiteSpace(video.VideoFile))
				return "";

			return Path.Combine(Folders.VideosFolder, video.VideoFile);
		}



		#endregion Static Methods


		#region Variables

		public MediaGalleryDataContext MediaData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new MediaGalleryDataContext(this.Connection);
				return (MediaGalleryDataContext)_dataContext;
			}
		}

		#endregion
	}
}
