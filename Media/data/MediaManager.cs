using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

using lw.Data;
using lw.WebTools;
using lw.CTE;
using System.Data;
using lw.Widgets.data;
using lw.Utils;
using lw.GraphicUtils;
using lw.HashTags;
using System.Text;

namespace lw.Widgets
{
	public class MediaManager : LINQManager
	{
		public MediaManager()
			: base(cte.lib)
		{
		}

		#region General functions

		/// <summary>
		/// Deletes the files from their folders
		/// </summary>
		/// <param name="FileName">name of the file to be deleted, to be taken from DB</param>
		/// <param name="Type">Type of media in order to know in what folder to look for the file</param>
		/// <param name="FolderDate">
		/// Date of the file creation, to be taken from the DB->Media->DateAdded.
		/// If null, DateTime.Now should be taked
		/// Note that the format should me MM-yyyy, otherwise it won't work
		/// </param>
		/// <returns>Enum of type FileResponses</returns>
		public static FileResponses DeleteFile(string FileName, DefaultMediaTypes? Type, string FolderDate)
		{
			if (String.IsNullOrWhiteSpace(FileName) || Type == null)
				return FileResponses.Error;

			string path = WebContext.Server.MapPath(GetFilePath(Type, FolderDate));
			string pathOfFile = Path.Combine(path, FileName);

			if (!File.Exists(pathOfFile))
				return FileResponses.FileNotFound;
			else
			{
				File.Delete(pathOfFile);
				if (Type == DefaultMediaTypes.Image)
				{
					string ext = Path.GetExtension(FileName);
					pathOfFile = null;
					string iSize = "-t";

					while (pathOfFile == null)
					{
						pathOfFile = Path.Combine(path, FileName.Replace(ext, iSize + ext));
						if (!File.Exists(pathOfFile))
							return FileResponses.FileNotFound;
						else
						{
							File.Delete(pathOfFile);
							if (iSize != "-l")
								pathOfFile = null;

							if (iSize == "-m" && iSize != "-l")
								iSize = "-l";
							else iSize = "-m";
						}
					}
				}
				return FileResponses.Success;
			}
		}

		/// <summary>
		/// Updates the actual file in it's folder
		/// </summary>
		/// <param name="File">The new file to be uploaded</param>
		/// <param name="MediaId">The Id of the Media to which the file is related</param>
		/// <param name="DeleteOld">Determins if we should keep or delete the old file from the server</param>
		/// <param name="OldFileName">The name of the present file in the DB</param>
		/// <param name="FileDateFolder">
		/// Date of the file creation, to be taken from the DB->Media->DateAdded
		/// Note that the format should me MM-yyyy, otherwise it won't work
		/// </param>
		/// <param name="MediaType">Type of media in order to know in what folder to look for the file</param>
		/// <returns>Enum of type FileResponses</returns>
		public static FileResponses UpdateFile(HttpPostedFile File, int MediaId, bool DeleteOld, string OldFileName, string FileDateFolder, DefaultMediaTypes? MediaType)
		{
			if (DeleteOld)
			{
				DeleteFile(OldFileName, MediaType, FileDateFolder);
			}

			if (MediaType == DefaultMediaTypes.Image)
				return AddImage(File, MediaId, MediaType);
			else if (MediaType == DefaultMediaTypes.Download)
				return AddDownloadFile(File, MediaId);
			else if (MediaType == DefaultMediaTypes.Video)
				return AddDownloadFile(File, MediaId);
			else return FileResponses.Error;
		}

		/// <summary>
		/// Get the path of the specific Media based on it's Related Widget type and it's WidgetId
		/// </summary>
		/// <param name="MediaType">Type of the requested Media Widget</param>
		/// <param name="FolderDate">
		/// Date of the file creation, to be taken from the DB->Media->DateAdded.
		/// If null, DateTime.Now should be taked
		/// Note that the format should me MM-yyyy, otherwise it won't work
		/// </param>
		/// <returns>string of the path to folder</returns>
		public static string GetFilePath(DefaultMediaTypes? WidgetType, string FolderDate)
		{
			string path = "";

			switch (WidgetType)
			{
				case DefaultMediaTypes.Image:
					path = WebContext.Root + "/" + CTE.Folders.MediaPhotoAlbums;
					break;
				case DefaultMediaTypes.Video:
					path = WebContext.Root + "/" + CTE.Folders.MediaVideoAlbums;
					break;
				case DefaultMediaTypes.Download:
					path = WebContext.Root + "/" + CTE.Folders.MediaDownloads;
					break;
				case DefaultMediaTypes.EmbedObject:
					path = WebContext.Root + "/" + CTE.Folders.MediaEmbedObjects;
					break;
				case DefaultMediaTypes.PopUp:
					path = WebContext.Root + "/" + CTE.Folders.MediaPopUps;
					break;
				case DefaultMediaTypes.Form:
					path = WebContext.Root + "/" + CTE.Folders.MediaForms;
					break;
				case DefaultMediaTypes.Poll:
					path = WebContext.Root + "/" + CTE.Folders.MediaPolls;
					break;
				default:
					path = WebContext.Root + "/" + CTE.Folders.Media;
					break;
			}
			if (FolderDate == null)
				FolderDate = string.Format("{0:MM-yyyy}", DateTime.Now);

			path = string.Format("{0}/{1}", path, FolderDate);
			return path;
		}

		#endregion

		///Media related functionalities
		#region Media

		/// <summary>
		/// Adds the media depending on it's type
		/// </summary>
		/// <param name="Status">bool true = 1, false = 0</param>
		/// <param name="Caption">Title if coming from request or file name if request title was null</param>
		/// <param name="MediaFile">the media file to be added i.e.(image, video, download...)</param>
		/// <param name="MediaObject">the media object to be added i.e.(youtube link, vimeo link...)</param>
		/// <param name="MediaType">the type of the media, found in the Enum DefaultMediaTypes</param>
		/// <returns>data.Media records</returns>
		public data.Media AddMedia(bool? Status, string Caption, Stream MediaFile, string FileName, string MediaObject, DefaultMediaTypes MediaType)
		{
			string caption = !String.IsNullOrWhiteSpace(Caption) ? Caption : null;
			Stream mediaFile = MediaFile != null ? MediaFile : null;
			string mediaObject = !String.IsNullOrWhiteSpace(MediaObject) ? MediaObject : null;
			int creator = WebContext.Profile.UserId;
			string fileName = !String.IsNullOrWhiteSpace(FileName) ? FileName : null;

			return AddMedia(Status, caption, mediaFile, fileName, mediaObject, null, creator, creator, null, MediaType);
		}

		public data.Media AddMedia(bool? Status, string Caption, Stream MediaFile, string FileName, string MediaObject, DefaultMediaTypes MediaType, string Tags)
		{
			string caption = !String.IsNullOrWhiteSpace(Caption) ? Caption : null;
			Stream mediaFile = MediaFile != null ? MediaFile : null;
			string mediaObject = !String.IsNullOrWhiteSpace(MediaObject) ? MediaObject : null;
			int creator = WebContext.Profile.UserId;
			string fileName = !String.IsNullOrWhiteSpace(FileName) ? FileName : null;
			string tags = !String.IsNullOrWhiteSpace(Tags) ? Tags : null;

			return AddMedia(Status, caption, mediaFile, fileName, mediaObject, null, creator, creator, tags, MediaType);
		}

		/// <summary>
		/// Adds the media depending on it's type
		/// </summary>
		/// <param name="Status">bool true = 1, false = 0</param>
		/// <param name="Caption">
		/// caption if coming from request
		/// or file name if caption was null
		/// or null if request file and caption were null
		/// </param>
		/// <param name="MediaFile">the media file to be added i.e.(image, video, download...)</param>
		/// <param name="MediaObject">the media object to be added i.e.(youtube link, vimeo link...)</param>
		/// <param name="Sort">int number to set the sorting</param>
		/// <param name="CreatedBy">id of the user that added the media</param>
		/// <param name="ModifiedBy">id of the user that modified/added the media</param>
		/// <param name="Tags">string of tags</param>
		/// <param name="MediaType">the type of the media, found in the Enum DefaultMediaTypes</param>
		/// <returns>data.Media records</returns>
		public data.Media AddMedia(bool? Status, string Caption, Stream MediaFile, string FileName,
			string MediaObject, int? Sort, int? CreatedBy, int? ModifiedBy, string Tags, DefaultMediaTypes? MediaType)
		{
			var modifier = ModifiedBy != null ? ModifiedBy.GetValueOrDefault() : WebContext.Profile.UserId;
			var creator = CreatedBy != null ? CreatedBy.GetValueOrDefault() : WebContext.Profile.UserId;
			byte status = Status.GetValueOrDefault() ? (byte)1 : (byte)0;
			string caption = !String.IsNullOrWhiteSpace(Caption) ? Caption : null;
			Stream mediaFile = MediaFile != null ? MediaFile : null;
			string mediaObject = !String.IsNullOrWhiteSpace(MediaObject) ? MediaObject : null;
			string tags = !String.IsNullOrWhiteSpace(Tags) ? Tags : null;
			string ext = !String.IsNullOrWhiteSpace(FileName) ? Path.GetExtension(FileName) : ".jpg";
			HashTagTypes mediaType = new HashTagTypes();

			if (caption == null && MediaFile != null)
			{
				FileStream fs = MediaFile as FileStream;
				caption = fs.Name;
			}

			data.Media media = new data.Media
				{
					Status = status,
					Caption = caption,
					MediaObject = mediaObject,
					Sort = Sort,
					ModifiedBy = modifier,
					Tags = tags,
					DateAdded = DateTime.Now,
					DateModified = DateTime.Now,
					CreatedBy = WebContext.Profile.UserId
				};

			WidgetsData.Media.InsertOnSubmit(media);
			WidgetsData.SubmitChanges();

			var mediaImage = from M in WidgetsData.Media
							 where M.Id == media.Id
							 select M;

			if (MediaType != null)
			{
				var m = mediaImage.Single();
				int mId = media.Id;

				if (mediaFile != null)
				{
					switch (MediaType)
					{
						///adds the image itself
						case DefaultMediaTypes.Image:
							FileResponses image = AddImage(caption, mediaFile, ext, mId, MediaType);
							if (image == FileResponses.Success || image == FileResponses.FileExist)
								m.MediaFile = mediaFile != null ? caption.IndexOf(ext) != -1 ? mId + "-" + caption : StringUtils.ToURL(mId + "-" + caption) + ext : null;
							mediaType = HashTagTypes.MediaImages;
							break;
						///adds the image of the video
						case DefaultMediaTypes.Video:
							FileResponses video = AddImage(caption, mediaFile, ext, mId, MediaType);
							if (video == FileResponses.Success || video == FileResponses.FileExist)
								m.MediaFile = mediaFile != null ? caption.IndexOf(ext) != -1 ? mId + "-" + caption : StringUtils.ToURL(mId + "-" + caption) + ext : null;
							mediaType = HashTagTypes.MediaVideos;
							break;
						///adds the image of the embed object
						case DefaultMediaTypes.EmbedObject:
							FileResponses embeded = AddImage(caption, mediaFile, ext, mId, MediaType);
							if (embeded == FileResponses.Success || embeded == FileResponses.FileExist)
								m.MediaFile = mediaFile != null ? caption.IndexOf(ext) != -1 ? mId + "-" + caption : StringUtils.ToURL(mId + "-" + caption) + ext : null;
							mediaType = HashTagTypes.MediaEmbedObject;
							break;
						///adds the download
						case DefaultMediaTypes.Download:
							FileResponses download = AddDownloadFile(caption, mediaFile, ext, mId);
							if (download == FileResponses.Success || download == FileResponses.FileExist)
								m.MediaFile = mediaFile != null ? caption.IndexOf(ext) != -1 ? mId + "-" + caption : StringUtils.ToURL(mId + "-" + caption) + ext : null;
							mediaType = HashTagTypes.MediaDownloads;
							break;
						case DefaultMediaTypes.PopUp:
							FileResponses popup = AddImage(caption, mediaFile, ext, mId, MediaType);
							if (popup == FileResponses.Success || popup == FileResponses.FileExist)
								m.MediaFile = mediaFile != null ? caption.IndexOf(ext) != -1 ? mId + "-" + caption : StringUtils.ToURL(mId + "-" + caption) + ext : null;
							mediaType = HashTagTypes.MediaPopups;
							break;
						default:
							break;
					}
				}

				m.Type = (int)MediaType;
				WidgetsData.SubmitChanges();
			}

			HashTagsManager htMgr = new HashTagsManager();
			htMgr.UpdateTags(media.Id, mediaType, media.Caption + " " + media.Tags);
			return media;
		}

		/// <summary>
		/// Deletes the media based on it's Id
		/// </summary>
		/// <param name="MediaId">Id of the media to be deleted</param>
		/// <returns>true if sucess, false if not</returns>
		public bool DeleteMedia(int MediaId)
		{
			var query = from M in WidgetsData.Media
						where M.Id == MediaId
						select M;

			if (query.Count() > 0)
			{
				var q = query.Single();

				DefaultMediaTypes T = (DefaultMediaTypes)q.Type;
				string FileName = q.MediaFile;
				string FileDateFolder = string.Format("{0:MM-yyyy}", q.DateAdded);

				FileResponses d = DeleteFile(FileName, T, FileDateFolder);
				if (d == FileResponses.Error || d == FileResponses.FileNotFound)
				{
					return false;
				}
				
				WidgetsData.Media.DeleteOnSubmit(q);
				WidgetsData.SubmitChanges();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns a single data record based on the media id
		/// </summary>
		/// <param name="WidgetId">Id of the required media</param>
		/// <returns>Single record if found, null if no data found</returns>
		public data.Media GetMedia(int MediaId)
		{
			var query = from m in WidgetsData.Media
						where m.Id == MediaId
						select m;

			if (query.Count() > 0)
				return query.Single();
			return null;
		}

		/// <summary>
		/// Returns all Media having the same type
		/// </summary>
		/// <param name="WidgetType">Int of the type of media required, we get it from the enum "DefaultMediaTypes" under Widgets project => enum.cs</param>
		/// <returns>List of found records, null if no data found</returns>
		public IQueryable<data.Media> GetMediaByType(int MediaType)
		{
			var query = from m in WidgetsData.Media
						where m.Type == MediaType
						select m;
			if (query.Count() > 0)
				return query;
			return null;
		}

		/// <summary>
		/// Updates the media
		/// </summary>
		/// <param name="MediaId">Id of the media to be updated</param>
		/// <param name="MediaType">the type of the media, found in the Enum DefaultMediaTypes</param>
		/// <param name="Status">bool true = 1, false = 0</param>
		/// <param name="Caption">
		/// caption if coming from request
		/// or file name if caption was null
		/// or null if request file and caption were null
		/// </param>
		/// <param name="MediaFile">the media file to be added i.e.(image, video, download...)</param>
		/// <param name="DeleteOldFile">bool to determin if we should delete the old file or not</param>
		/// <param name="MediaObject">the media object to be added i.e.(youtube link, vimeo link...)</param>
		/// <param name="Sort">int number to set the sorting</param>
		/// <param name="ModifiedBy">id of the user that modified/added the media</param>
		/// <param name="Tags">string of tags</param>
		/// <returns>true if success, false if not</returns>
		public bool UpdateMedia(int MediaId, DefaultMediaTypes? MediaType, bool? Status, string Caption, HttpPostedFile MediaFile, bool DeleteOldFile, string MediaObject, int? Sort, int? ModifiedBy, string Tags)
		{
			data.Media thisMedia = GetMedia(MediaId);

			if (thisMedia == null)
				return false;

			var modifier = ModifiedBy != null ? ModifiedBy.GetValueOrDefault() : WebContext.Profile.UserId;
			byte status = Status.GetValueOrDefault() ? (byte)1 : (byte)0;
			string caption = !String.IsNullOrWhiteSpace(Caption) ? Caption : MediaFile != null ? Path.GetFileNameWithoutExtension(MediaFile.FileName) : null;
			HttpPostedFile mediaFile = MediaFile != null ? MediaFile : null;
			string mediaObject = !String.IsNullOrWhiteSpace(MediaObject) ? MediaObject : null;
			string tags = !String.IsNullOrWhiteSpace(Tags) ? Tags : null;

			if (MediaType != null)
			{
				int mId = thisMedia.Id;

				if (mediaFile != null)
				{
					string OldFileName = thisMedia.MediaFile;
					string FileDateFolder = string.Format("{0:MM-yyyy}", thisMedia.DateAdded);

					switch (MediaType)
					{
						///adds the image itself
						case DefaultMediaTypes.Image:
							FileResponses image = UpdateFile(mediaFile, mId, DeleteOldFile, OldFileName, FileDateFolder,  MediaType);
							if (image == FileResponses.Success)
								thisMedia.MediaFile = mediaFile != null ? mId + "-" + mediaFile.FileName : null;
							break;
						///adds the image of the video
						case DefaultMediaTypes.Video:
							FileResponses video = UpdateFile(mediaFile, mId, DeleteOldFile, OldFileName, FileDateFolder, MediaType);
							if (video == FileResponses.Success)
								thisMedia.MediaFile = mediaFile != null ? mId + "-" + mediaFile.FileName : null;
							break;
						///adds the download
						case DefaultMediaTypes.Download:
							FileResponses download = UpdateFile(mediaFile, mId, DeleteOldFile, OldFileName, FileDateFolder, MediaType);
							if (download == FileResponses.Success)
								thisMedia.MediaFile = mediaFile != null ? mId + "-" + mediaFile.FileName : null;
							break;
						default:
							break;
					}
				}
				thisMedia.Type = (int)MediaType;
			}

			thisMedia.Status = status;
			thisMedia.Caption = caption;
			thisMedia.MediaObject = mediaObject;
			thisMedia.Sort = Sort;
			thisMedia.DateModified = DateTime.Now;
			thisMedia.ModifiedBy = modifier;
			thisMedia.Tags = tags;

			WidgetsData.SubmitChanges();

			HashTagsManager htMgr = new HashTagsManager();
			htMgr.UpdateTags(thisMedia.Id, HashTagTypes.Media, thisMedia.Caption + " " + thisMedia.Tags);
			return true;
		}

		public bool LinkMediaToWidget(int WidgetId, int[] MediaIds)
		{
			if (MediaIds != null)
			{
				StringBuilder sb = new StringBuilder();
				string insertStatement = "Insert into MediaWidgets (MediaId, WidgetId) values ({0}, {1});";
				foreach (int M in MediaIds)
				{
					sb.Append(string.Format(insertStatement, M, WidgetId));
				}
				DBUtils.ExecuteQuery(sb.ToString(), cte.lib);

				return true;
			}
			else
			{
				return false;
			}
		}

		public void DeleteMediaFromWidget(int MediaId, int WidgetId)
		{
			string sql = string.Format("Delete From MediaWidgets Where MediaId = {0} and WidgetId = {1}", MediaId, WidgetId);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		public void DeleteMediaFromWidget(int[] MediaIds, int WidgetId)
		{
			var ids = "";
			foreach (int id in MediaIds)
			{
				var temp = ids;
				ids = temp + "," + id;
			}
			string sql = string.Format("Delete From MediaWidgets Where MediaId in ({0}) and WidgetId = {1}", ids.Substring(1, ids.Length - 1), WidgetId);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		///Images related functionalities
		#region Images

		/// <summary>
		/// adds an image to the folder related to the type
		/// </summary>
		/// <param name="Image">the file to be added to the folder</param>
		/// <param name="MediaId">
		/// Id of the media to relate the file to
		/// the id will be added to the file name at the beginning followed by a "-"
		/// </param>
		/// <param name="MediaType">Type of the media to which the file is related</param>
		/// <returns>Enum of type FileResponses</returns>
		public static FileResponses AddImage(HttpPostedFile Image, int MediaId, DefaultMediaTypes? MediaType)
		{
			if (MediaType == null)
				return FileResponses.Error;

			string path = WebContext.Server.MapPath(GetFilePath(MediaType, null));
			string imagePath;

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!string.IsNullOrEmpty(Image.FileName))
			{
				imagePath = Path.Combine(path, MediaId + "-" + Image.FileName);
				if (!File.Exists(imagePath))
					Image.SaveAs(imagePath);
				else
					return FileResponses.FileExist;
			}
			else
				return FileResponses.Error;

			if (MediaType == DefaultMediaTypes.Image)
				return AddImage(path, imagePath);
			else return FileResponses.Success;
		}

		/// <summary>
		/// adds an image to the folder related to the type
		/// </summary>
		/// <param name="Image">the file to be added to the folder</param>
		/// <param name="MediaId">
		/// Id of the media to relate the file to
		/// the id will be added to the file name at the beginning followed by a "-"
		/// </param>
		/// <param name="MediaType">Type of the media to which the file is related</param>
		/// <returns>Enum of type FileResponses</returns>
		public static FileResponses AddImage(string caption, Stream Image, string Ext, int MediaId, DefaultMediaTypes? MediaType)
		{
			if (MediaType == null)
				return FileResponses.Error;

			string path = WebContext.Server.MapPath(GetFilePath(MediaType, null));
			string imagePath;

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!string.IsNullOrEmpty(caption))
			{
				imagePath = Path.Combine(path, caption.IndexOf(Ext) != -1 ? MediaId + "-" + caption : StringUtils.ToURL(MediaId + "-" + caption) + Ext);
				if (!File.Exists(imagePath))
					IO.SaveStream(Image, imagePath);
				else
					return FileResponses.FileExist;
			}
			else
				return FileResponses.Error;

			if (MediaType == DefaultMediaTypes.Image || MediaType == DefaultMediaTypes.Video)
				return AddImage(path, imagePath);
			else return FileResponses.Success;
		}

		/// <summary>
		/// adds the resized images (thumb, medium, large) to the folder if the media was of type "Image"
		/// </summary>
		/// <param name="FolderPath">the path of the folder to save the image in</param>
		/// <param name="ImagePath">the path of the original image to be cropped and saved</param>
		/// <returns>Enum of type FileResponses</returns>
		public static FileResponses AddImage(string FolderPath, string ImagePath)
		{
			string imageName = Path.GetFileNameWithoutExtension(ImagePath);
			string imageExtension = Path.GetExtension(ImagePath);

			string thumbName = Path.Combine(FolderPath, string.Format("{0}-t{1}", imageName, imageExtension));
			string largeName = Path.Combine(FolderPath, string.Format("{0}-l{1}", imageName, imageExtension));
			string mediumName = Path.Combine(FolderPath, string.Format("{0}-m{1}", imageName, imageExtension));

			if (!String.IsNullOrWhiteSpace(ImagePath) && File.Exists(ImagePath))
			{

				imageName = Path.GetFileNameWithoutExtension(ImagePath);
				imageExtension = Path.GetExtension(ImagePath);

				thumbName = Path.Combine(FolderPath, string.Format("{0}-t{1}", imageName, imageExtension));
				largeName = Path.Combine(FolderPath, string.Format("{0}-l{1}", imageName, imageExtension));
				mediumName = Path.Combine(FolderPath, string.Format("{0}-m{1}", imageName, imageExtension));

				Dimension
					thumbSize = new Dimension(cte.ImageDefaultThumbSize),
					mediumSize = new Dimension(cte.ImageDetaultMediumSize),
					largeSize = new Dimension(cte.ImageDetaultLargeSize);

				GraphicUtils.ImageUtils.CropImage(ImagePath, thumbName, thumbSize.IntWidth, thumbSize.IntHeight, ImageUtils.AnchorPosition.Default);
				GraphicUtils.ImageUtils.Resize(ImagePath, mediumName, mediumSize.IntWidth, mediumSize.IntHeight);
				GraphicUtils.ImageUtils.Resize(ImagePath, largeName, largeSize.IntWidth, largeSize.IntHeight);

				return FileResponses.Success;
			}
			return FileResponses.Error;
		}

		#endregion

		///Downloads related functionalities
		#region Downloads

		/// <summary>
		/// adds the file to the downloads folder
		/// </summary>
		/// <param name="DownloadFile">file to be added to the folder</param>
		/// <param name="MediaId">
		/// Id of the media to relate the file to
		/// the id will be added to the file name at the beginning followed by a "-"
		/// </param>
		/// <returns>Enum of type FileResponses</returns>
		public static FileResponses AddDownloadFile(HttpPostedFile DownloadFile, int MediaId)
		{
			string path = WebContext.Server.MapPath(GetFilePath(DefaultMediaTypes.Download, null));

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!string.IsNullOrEmpty(DownloadFile.FileName))
			{
				string p = Path.Combine(path, MediaId + "-" + DownloadFile.FileName);
				if (!File.Exists(p))
				{
					DownloadFile.SaveAs(p);
					return FileResponses.Success;
				}
				return FileResponses.FileExist;
			}
			return FileResponses.Error;
		}

		/// <summary>
		/// adds the file to the downloads folder
		/// </summary>
		/// <param name="DownloadFile">file to be added to the folder</param>
		/// <param name="MediaId">
		/// Id of the media to relate the file to
		/// the id will be added to the file name at the beginning followed by a "-"
		/// </param>
		/// <returns>Enum of type FileResponses</returns>
		public static FileResponses AddDownloadFile(string caption, Stream DownloadFile, string Ext, int MediaId)
		{
			string path = WebContext.Server.MapPath(GetFilePath(DefaultMediaTypes.Download, null));

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!string.IsNullOrEmpty(caption))
			{
				string p = Path.Combine(path, caption.IndexOf(Ext) != -1 ? MediaId + "-" + caption : StringUtils.ToURL(MediaId + "-" + caption) + Ext);
				if (!File.Exists(p))
				{
					IO.SaveStream(DownloadFile, p);
					return FileResponses.Success;
				}
				return FileResponses.FileExist;
			}
			return FileResponses.Error;
		}

		#endregion

		#endregion

		///General Variables
		#region Variables

		/// <summary>
		/// media data context
		/// </summary>
		public data.WidgetsDataContext WidgetsData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new data.WidgetsDataContext(this.Connection);
				return (data.WidgetsDataContext)_dataContext;
			}
		}

		#endregion
	}
}