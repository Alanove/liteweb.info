using System;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.Utils;
using lw.WebTools;

namespace lw.Downloads
{
	public class Downloads
	{
		string _lib = "";
		public Downloads()
		{
			this._lib = cte.lib;
		}

		#region get

		public DataTable GetDownloadsByNetwork(int MemberId, DateTime? Date)
		{
			return GetDownloadsByNetwork(MemberId, Date, null);
		}

		public DataTable GetDownloadsByNetwork(int MemberId, DateTime? Date, string condition)
		{
			//if (String.Compare(WebContext.Profile.dbUserName, Config.GetFromWebConfig("Admin"), true) == 0)
			//	return GetDownloads(condition);
			//else
			//{
			//	DownloadNetworks downloadNetworks = new DownloadNetworks();
			//	return downloadNetworks.GetDownloadsByNetwork(MemberId, Date, condition);
			//}
			DownloadNetworks downloadNetworks = new DownloadNetworks();
			return downloadNetworks.GetDownloadsByNetwork(MemberId, Date, condition);

		}


		public DataTable GetDownloads(string cond)
		{
			string sql = string.Format("select d.*,  '{0}/' + d.UniqueName + '/' + d.FileName as DownloadLink, FileSize/1024 as KB from DownloadsView d",
				DownloadsVR);
			if (!StringUtils.IsNullOrWhiteSpace(cond))
				sql += " where " + cond;
			return DBUtils.GetDataSet(sql, _lib).Tables[0];
		}
		public DataTable GetDownloadsByType(int DownloadType)
		{
			return GetDownloads(string.Format("DownloadType={0}", DownloadType));
		}

		public DataTable GetDownloads(DateTime month)
		{
			string cond = "DateAdded Between '{0}' and '{1}'";

			DateTime DateFrom = new DateTime(month.Year, month.Month, 1, 0, 0, 0);
			DateTime DateTo = new DateTime(month.Year, month.Month, month.AddMonths(1).AddDays(-1).Day, 23, 59, 59);

			cond = string.Format(cond, DateFrom, DateTo);

			return GetDownloads(cond);
		}

		public DataTable GetDownloadsByType(string DownloadType)
		{
			return GetDownloads(string.Format("Type='{0}' or UniqueName='{0}'", StringUtils.SQLEncode(DownloadType)));
		}
		public DataRow GetDownload(int DownloadId)
		{
			DataTable dt = GetDownloads("DownloadId=" + DownloadId.ToString());
			return dt.Rows.Count > 0 ? dt.Rows[0] : null;
		}
		public DataTable GetDownloadsByType(string DownloadType, lw.CTE.Enum.DownloadStatus Status)
		{
			return GetDownloads(string.Format("(Type='{0}' or UniqueName='{0}') and Status={1}",
				StringUtils.SQLEncode(DownloadType),
				(int)Status
				));
		}

		public DataView GetDownloadMax(int max, string sql)
		{
			string _max = "100 PERCENT";
			if (max > 0)
				_max = max.ToString();

			if (sql.Trim() != "")
				sql = " Where " + sql;

			sql = string.Format("select Top {0} d.*,  '{1}/' + d.UniqueName + '/' + d.FileName as DownloadLink, FileSize/1024 as KB from DownloadsView d {2}  Order By d.DateModified Desc",
				_max,
				DownloadsVR,
				sql);

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0].DefaultView;
		}

		#endregion

		#region add

		public int AddDownload(string title, HttpPostedFile file)
		{
			SqlDateTime date = new SqlDateTime(DateTime.Now);
			return AddDownload(-1, title, "", file, date);
		}

		public int AddDownload(int DownloadTypeId, string Title, string Description, string FileName,
			SqlDateTime Date)
		{
			return AddDownload(DownloadTypeId, Title, Description, FileName, Date, lw.CTE.Enum.DownloadStatus.MainPage);
		}

		public int AddDownload(int DownloadTypeId, string Title, string Description, string FileName,
			SqlDateTime Date, lw.CTE.Enum.DownloadStatus Status)
		{
			DataRow downloadType = GetDownloadType(DownloadTypeId);
			if (downloadType == null)
			{
				DataTable types = GetDownloadTypes();
				if (types.Rows.Count > 0)
				{
					downloadType = types.Rows[0];
					DownloadTypeId = (int)downloadType["TypeId"];
				}
				else
				{
					throw new Exception("Download Type cannot be empty.");
				}
			}
			string file = Path.Combine(DownloadsTemp, FileName);
			long fileSize = 0;
			if (!File.Exists(file))
			{
				//throw (new Exception("File Not Found!"));
			}
			else
			{
				if (GetDownloads(string.Format("Title=N'{0}' and DownloadType={1}",
					StringUtils.SQLEncode(Title), DownloadTypeId)).Rows.Count > 0)
				{
					File.Delete(file);
					throw new Exception("File Already Exist!");
				}
				FileName = StringUtils.ToURL(Title) + Path.GetExtension(FileName);
				string _path = Path.Combine(DownloadsFolder, downloadType["UniqueName"].ToString());
				if (!Directory.Exists(_path))
					Directory.CreateDirectory(_path);

				string pathTo = Path.Combine(_path, FileName);
				if (File.Exists(pathTo))
					File.Delete(pathTo);

				File.Move(file, pathTo);

				file = pathTo;

				if (File.Exists(file))
				{
					FileInfo fi = new FileInfo(pathTo);
					if (Date.IsNull)
					{
						Date = fi.CreationTime;
					}
					fileSize = fi.Length;
				}
			}
			string sql = string.Format(@"Insert into Downloads (Title, Description, DateAdded, 
										DateModified, DownloadType, FileName, FileSize, Status, Sort) values 
										(N'{0}', N'{1}', '{2}', '{3}', {4}, N'{5}', {6},{7},{8});
										select @@Identity as DownloadId",
									StringUtils.SQLEncode(Title),
									StringUtils.SQLEncode(Description), Date.Value,
									DateTime.Now, DownloadTypeId, FileName,
									fileSize,
									(byte)Status, 1000);

			DataTable dt = DBUtils.GetDataSet(sql, cte.lib).Tables[0];
			return Int32.Parse(dt.Rows[0]["DownloadId"].ToString());
		}

		public int AddDownload(int DownloadTypeId, string Title, string Description, HttpPostedFile file,
			SqlDateTime Date)
		{
			return AddDownload(DownloadTypeId, Title, Description, file, Date, lw.CTE.Enum.DownloadStatus.Disabled);
		}

		public int AddDownload(int DownloadTypeId, string Title, string Description, HttpPostedFile file,
			SqlDateTime Date, lw.CTE.Enum.DownloadStatus Status)
		{
			if (file.ContentLength == 0)
			{
				throw new Exception("File is empty");
			}
			if (!Directory.Exists(DownloadsTemp))
				Directory.CreateDirectory(DownloadsTemp);

			string saveTo = Path.Combine(DownloadsTemp, Path.GetFileName(file.FileName));
			file.SaveAs(saveTo);
			if (String.IsNullOrEmpty(Title))
				Title = Path.GetFileNameWithoutExtension(file.FileName);
			return AddDownload(DownloadTypeId, Title, Description, Path.GetFileName(file.FileName), Date, Status);
		}

		public void BatchUpload(int DownloadType)
		{
			HttpRequest Request = WebContext.Request;

			StringBuilder exceptions = new StringBuilder();

			for (int i = 0; i < Request.Files.Count; i++)
			{
				HttpPostedFile file = Request.Files[i];
				try
				{
					AddDownload(DownloadType, "", "", file, new SqlDateTime());
				}
				catch (Exception Ex)
				{
					exceptions.AppendLine(file.FileName + ": " + Ex.Message);
				}
			}
			if (exceptions.Length > 0)
			{
				throw new Exception(exceptions.ToString());
			}
		}

		/// <summary>
		/// Add which file is downloaded by which member and the downloaded date of it
		/// </summary>
		/// <param name="MemberId"></param>
		/// <param name="DownloadId"></param>
		public void AddDownloadActivity(int MemberId, int DownloadId)
		{
			string sql = string.Format(@"Insert into Downloads_MembersActivity (MemberId, DownloadId, DateDownloaded) 
										values ({0}, {1}, '{2}')",
										MemberId,
										DownloadId,
										DateTime.Now);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		#endregion

		#region Update

		public void UpdateDownload(int DownloadId, string Description, int DownloadTypeId, lw.CTE.Enum.DownloadStatus Status)
		{
			UpdateDownload(DownloadId, null, Description, DownloadTypeId, Status, 1000);
		}

		public void UpdateDownload(int DownloadId, string Title, string Description, int DownloadTypeId, lw.CTE.Enum.DownloadStatus Status)
		{
			UpdateDownload(DownloadId, Title, Description, DownloadTypeId, Status, null);
		}

		public void UpdateDownload(int DownloadId, string Title, string Description, int DownloadTypeId, lw.CTE.Enum.DownloadStatus Status, int? Sort)
		{
			DataRow downloadType = GetDownloadType(DownloadTypeId);
			DataTable dt = GetDownloads(string.Format("DownloadId={0}",
				DownloadId));

			if (downloadType == null)
			{
				throw new Exception("Invalid Download Type");
			}

			if (downloadType["TypeId"] != dt.Rows[0]["DownloadType"])
			{
				string _pathFrom = Path.Combine(DownloadsFolder, dt.Rows[0]["UniqueName"].ToString());

				string FileName = dt.Rows[0]["FileName"].ToString();
				string file = Path.Combine(_pathFrom, FileName);

				string _pathTo = Path.Combine(DownloadsFolder, downloadType["UniqueName"].ToString());

				if (!Directory.Exists(_pathTo))
					Directory.CreateDirectory(_pathTo);

				string pathTo = Path.Combine(_pathTo, FileName);

				if (File.Exists(file))
					File.Move(file, pathTo);
			}

			string sql = string.Format(@"Update Downloads set DateModified='{0}', 
										DownloadType={1}, Status={2}, Description=N'{3}'{4}, Sort={6} where DownloadId={5}",
									DateTime.Now, DownloadTypeId,
									(byte)Status, StringUtils.SQLEncode(Description),
									Title == null ? "" : string.Format(", Title=N'{0}'", StringUtils.SQLEncode(Title)),
									DownloadId, Sort == null ? 1000 : Sort);
			DBUtils.ExecuteQuery(sql, _lib);
		}

		public void UpdateDownload(int DownloadId, string Title, string FileName, string Description,
				SqlDateTime Date, int DownloadTypeId)
		{
			UpdateDownload(DownloadId, Title, FileName, Description, Date, DownloadTypeId, 1000);
		}

		public void UpdateDownload(int DownloadId, string Title, string FileName, string Description,
				SqlDateTime Date, int DownloadTypeId, int? Sort)
		{
			DataRow downloadType = GetDownloadType(DownloadTypeId);
			if (downloadType == null)
			{
				throw new Exception("Invalid Download Type");
			}
			if (GetDownloads(string.Format("Title='{0}' and DownloadId<>{1} and DownloadType={2}",
				StringUtils.SQLEncode(Title), DownloadId, DownloadTypeId)).Rows.Count > 0)
			{
				throw new Exception("File Already Exist!");
			}
			string file = Path.Combine(DownloadsTemp, FileName);
			if (!File.Exists(file))
			{
				//File.Delete(file);
				throw (new Exception("File Not Found!"));
			}
			FileName = StringUtils.ToURL(Title) + Path.GetExtension(FileName);

			string _path = Path.Combine(DownloadsFolder, downloadType["UniqueName"].ToString());
			if (!Directory.Exists(_path))
				Directory.CreateDirectory(_path);

			string pathTo = Path.Combine(_path, FileName);
			File.Move(file, pathTo);

			FileInfo fi = new FileInfo(pathTo);
			if (Date.IsNull)
			{
				Date = fi.CreationTime;
			}

			string sql = string.Format(@"Update Downloads set Title='{0}', Description='{1}', 
										DateAdded='{2}', DateModified='{3}', 
										DownloadType={4}, FileName='{4}',FileSize={6}, Sort={7} where DownloadId={5}",
									StringUtils.SQLEncode(Title),
									StringUtils.SQLEncode(Description),
									Date.Value, DateTime.Now, DownloadTypeId, FileName,
									DownloadId, fi.Length, Sort == null ? 1000 : Sort);
			DBUtils.ExecuteQuery(sql, _lib);
		}

		public void UpdateDowloadFile(int DownloadId, HttpPostedFile file)
		{
			UpdateDownloadFile(DownloadId, file);
		}
		public void UpdateDownloadFile(int DownloadId, HttpPostedFile file)
		{
			if (file.ContentLength == 0)
				return;

			Downloads dS = new Downloads();
			DataRow download = dS.GetDownload(DownloadId);
			string downloadFileName;
			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.Downloads);
			string sql = "Select UniqueName From DownloadTypes Where TypeId = {0}";
			DataTable dr = DBUtils.GetDataSet(string.Format(sql, download["DownloadType"]), lw.Downloads.cte.lib).Tables[0];

			if (dr.Rows.Count > 0)
			{
				string dowloadType = dr.Rows[0]["UniqueName"].ToString();
				path = Path.Combine(path, dowloadType);

				if (download != null)
				{
					downloadFileName = download["FileName"].ToString();
					downloadFileName = Path.Combine(path, downloadFileName);

					long fileSize = 0;

					if (File.Exists(downloadFileName))
						File.Delete(downloadFileName);

					string newFile = Path.Combine(path, Path.GetFileName(file.FileName));
					file.SaveAs(newFile);
					FileInfo fi = new FileInfo(newFile);
					fileSize = fi.Length;
					string updateSQL = "Update Downloads set FileName = '{0}', FileSize='{2}' Where DownloadId = {1}";
					DBUtils.ExecuteQuery(string.Format(updateSQL, Path.GetFileName(file.FileName), DownloadId, fileSize), lw.Downloads.cte.lib);
				}
			}
		}
		#endregion

		#region Delete
		public void DeleteDownload(int DownloadId)
		{
			DataRow download = GetDownload(DownloadId);
			if (download == null)
				return;

			string file = Path.Combine(DownloadsFolder, download["UniqueName"].ToString());
			file = Path.Combine(file, download["FileName"].ToString());

			string sql = string.Format("delete from Downloads where DownloadId={0}",
				DownloadId);
			DBUtils.ExecuteQuery(sql, _lib);

			if (File.Exists(file))
				File.Delete(file);
		}
		#endregion

		#region Download Types
		public DataTable GetDownloadTypes()
		{
			return GetDownloadTypes("");
		}
		public DataTable GetDownloadTypes(string cond)
		{
			string sql = "select * from DownloadTypes";
			if (!StringUtils.IsNullOrWhiteSpace(cond))
				sql += " where " + cond;
			return DBUtils.GetDataSet(sql, _lib).Tables[0];
		}
		public DataRow GetDownloadType(int TypeId)
		{
			DataTable types = GetDownloadTypes(string.Format("TypeId={0}", TypeId));
			return types.Rows.Count > 0 ? types.Rows[0] : null;
		}
		public DataRow GetDownloadType(string UniqueName)
		{
			DataTable types = GetDownloadTypes(string.Format("UniqueName='{0}'", StringUtils.SQLEncode(UniqueName)));
			return types.Rows.Count > 0 ? types.Rows[0] : null;
		}


		/// <summary>
		/// Adds a new download category or type
		/// </summary>
		/// <param name="title">the name of the category</param>
		public void AddDownloadType(string title)
		{
			string sql = "Insert into DownloadTypes (Type, UniqueName) values (N'{0}', N'{1}');";

			sql = string.Format(sql,
				StringUtils.SQLEncode(title),
				StringUtils.ToURL(title));

			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		/// <summary>
		/// Deletes a type (not implemented yet)
		/// </summary>
		/// <param name="TypeId">The ID of the download category</param>
		public void DeleteDownloadType(int TypeId)
		{

		}

		/// <summary>
		/// Updates the downlaod type, renaming the folder in /downloads
		/// </summary>
		/// <param name="typeId">The id of the type</param>
		/// <param name="title">the new Name</param>
		public void UpdateDownloadType(int typeId, string title)
		{
			string sql = "Update DownloadTypes set Type= N'{0}', UniqueName=N'{1}' where TypeId = {2}";

			string dir = StringUtils.ToURL(title);

			string oldDownloadPath = GetDownloadType(typeId)["UniqueName"].ToString();

			oldDownloadPath = Path.Combine(DownloadsFolder, oldDownloadPath);

			string newPath = Path.Combine(DownloadsFolder, dir);

			if (Directory.Exists(oldDownloadPath))
			{
				if (!String.Equals(oldDownloadPath, newPath, StringComparison.OrdinalIgnoreCase))
					Directory.Move(oldDownloadPath, newPath);
			}

			sql = string.Format(sql,
				StringUtils.SQLEncode(title),
				dir, typeId);
			try
			{
				DBUtils.ExecuteQuery(sql, cte.lib);
			}
			catch (Exception Ex)
			{
				lw.Error.Handler.HandleError(Ex);


				Directory.Move(newPath, oldDownloadPath);
			}
		}

		#endregion

		#region static Downloads Folder, temp, virtual directory
		public static string DownloadsFolder
		{
			get
			{
				return string.Format("{0}/{1}", WebContext.Server.MapPath("~"),
					Folders.Downloads);
			}
		}
		public static string DownloadsTemp
		{
			get
			{
				return string.Format("{0}/{1}", WebContext.Server.MapPath("~"),
					Folders.Downloads);
			}
		}
		public static string DownloadsVR
		{
			get
			{
				return string.Format("{0}/{1}", WebContext.Root,
					Folders.Downloads);
			}
		}
		#endregion
	}
}
