using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading;
using lw.CTE;
using lw.Utils;

namespace lw.WebTools
{
	/// <summary>
	/// Summary description for Config.
	/// </summary>
	public class Config
	{

		public static ReaderWriterLock _lock = new ReaderWriterLock();

		public Config()
		{

		}
		public static string GetFromWebConfig(string paramName)
		{
			return ConfigurationManager.AppSettings[paramName];
		}
		public void SaveChanges(System.Web.HttpRequest req)
		{
			SaveChanges(req, "");
		}
		public void SaveChanges(System.Web.HttpRequest req, string checkboxes)
		{
			SaveChanges(req, checkboxes.Trim() != ""? checkboxes.Split(','): null);
		}
		public void SaveChanges(System.Web.HttpRequest _HttpRequest, string[] check)
		{
			foreach (string key in _HttpRequest.Form)
			{
				if (key.ToLower() == "__viewstate")
					continue;
				SetKey(key, _HttpRequest[key]);
			}
			if (check != null)
			{
				foreach (string key in check)
				{
					if (_HttpRequest[key] != "")
						SetKey(key, _HttpRequest[key]);
					else
						SetKey(key, "");
				}
			}
			AcceptChanges();
		}
		public string GetAll()
		{
			StringBuilder sb = new StringBuilder();
			string sep = "";
			foreach (DataRow row in this._Parameters.Rows)
			{
				sb.Append(string.Format("{0}\"{1}\":{2}",
					sep, row["key"],
					StringUtils.AjaxEncode(row["value"].ToString())));
				sep = ",";
			}
			return "{" + sb.ToString() + "}";
		}
		public string GetKey(string key)
		{
			string ret = "";
			try
			{
				_lock.AcquireReaderLock(-1);

				DataRow[] drs = this._Parameters.Select("key='" + key + "'");

				if (drs.Length > 0)
					ret = drs[0]["value"].ToString();
			}
			finally
			{
				_lock.ReleaseReaderLock();
			}
			return ret;
		}

		public void SetKey(string key, string v)
		{
			try
			{
				_lock.AcquireWriterLock(-1);

				DataRow[] drs = this._Parameters.Select("key='" + key + "'");
				if (drs.Length > 0)
				{
					drs[0]["value"] = v;
				}
				else
				{
					DataRow dr = this._Parameters.NewRow();
					dr["key"] = key;
					dr["value"] = v;
					this._Parameters.Rows.Add(dr);
				}

				this._Parameters.AcceptChanges();
			}
			finally
			{
				_lock.ReleaseWriterLock();
			}
		}
		DataTable _Parameters
		{
			get
			{
				return this._ConfigDataSet.Tables["Parameters"];
			}
		}
		public void AcceptChanges()
		{
			try
			{
				_lock.AcquireWriterLock(-1);

				string path = WebContext.Server.MapPath(WebContext.StartDir + ConfigCte.ParamFile);
				this._ConfigDataSet.AcceptChanges();
				this._ConfigDataSet.WriteXml(path, XmlWriteMode.WriteSchema);
			}
			finally
			{
				_lock.ReleaseWriterLock();
			}
		}
		DataSet _ConfigDataSet
		{
			get
			{
				object obj = WebContext.Cache[ConfigCte.ConfigCacheKey];
				if (obj != null)
					return (DataSet)obj;

				string path = WebContext.Server.MapPath(WebContext.Root + ConfigCte.ParamFile);
				DataSet ds = new DataSet();
				try
				{
					_lock.AcquireReaderLock(-1);

					ds.ReadXml(path);

					System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(path);
					WebContext.Cache.Insert(ConfigCte.ConfigCacheKey, ds, dep);
				}
				finally
				{
					_lock.ReleaseReaderLock();
				}
				return ds;
			}
		}
		public static string GetConnectionString(string connectionStrType)
		{
			string key = "cs-" + connectionStrType;
			if (WebContext.Cache[key] != null)
				return WebContext.Cache[key].ToString();

			bool encryptConnections = false;

			if (Config.GetFromWebConfig(AppConfig.EncryptConnections) != null)
			{
				try
				{
					encryptConnections = bool.Parse(Config.GetFromWebConfig(AppConfig.EncryptConnections));
				}
				catch
				{
				}
			}
			string connectionString = "";

			if (System.Web.Configuration.WebConfigurationManager.ConnectionStrings[connectionStrType] != null)
			{
				connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[connectionStrType].ConnectionString;

				if (String.IsNullOrEmpty(connectionString))
				{
					Exception ex = new Exception("Connection string not found: " + connectionStrType);
					throw (ex);
				}

				if (encryptConnections)
					connectionString = Cryptography.Decrypt(connectionString, AppConfig.__);

				WebContext.Cache.Insert(key, connectionString);

			}
			return connectionString;
		}


	}
}
