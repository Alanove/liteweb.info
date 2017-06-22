using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web.Caching;
using lw.CTE;
using lw.Utils;

namespace lw.WebTools
{
	public class XmlManager
	{
		public static ReaderWriterLock _lock = new ReaderWriterLock();


		public XmlManager()
		{
		}
		///////////////Get UtilsTo ManageXML//////////////
		public static string GetFromWebConfig(string paramName)
		{
			return Config.GetFromWebConfig(paramName);
		}
		public static DataTable GetTableFromXmlDataSet(string dataSet, string table)
		{
			DataTable T = null;
			return XmlManager.GetDataSet(dataSet).Tables[table];
		}
		public static string GetValueFromXmlDataTable(DataTable SpecifyTable, string KeyName, string KeyValue, string KeySpecifyValue)
		{
			string pValue = null;
			string ComposeSelectedKey = string.Format("{0}='{1}'", KeyName, KeySpecifyValue);
			try
			{
				pValue = SpecifyTable.Select(ComposeSelectedKey)[0][KeyValue].ToString();
			}
			catch (Exception exception)
			{
			}
			return pValue;
		}
		

		/// <summary>
		/// Returns the full path of a default config file 
		/// </summary>
		/// <param name="dataSet"></param>
		/// <returns></returns>
		public static string DataSetPath(string dataSet)
		{
			return WebContext.Server.MapPath(WebContext.StartDir + "/" + Folders.DataSetFolder + "/" + dataSet);
		}

		/// <summary>
		/// Retrives a dataset from a config file
		/// </summary>
		/// <param name="dataSet">The path of the data file</param>
		/// <returns>The fetched dataset</returns>
		public static DataSet GetDataSet(string dataSet)
		{
			return GetDataSet(dataSet, null);
		}

		/// <summary>
		/// Retrives a dataset from a config file
		/// </summary>
		/// <param name="dataSet">The path of the data file</param>
		/// <param name="callback">The callback function, called when the file is changed</param>
		/// <returns>The fetched dataset</returns>
		public static DataSet GetDataSet(string dataSet, CacheItemRemovedCallback callback)
		{
			try
			{
				_lock.AcquireReaderLock(-1);

				DataSet ds = WebContext.Cache[dataSet] as DataSet;
				if (ds == null)
				{
					ds = new DataSet();
					string file = DataSetPath(dataSet);
					if (!File.Exists(file))
						return new DataSet();
					
					ds.ReadXml(file, XmlReadMode.ReadSchema);
					ds.AcceptChanges();
					

					CacheDependency dep = new CacheDependency(file);

					int cacheTime = 30;

					string obj = Config.GetFromWebConfig("ContentCachTime");
					if (obj != null)
					{
						cacheTime = Int32.Parse(obj);
					}

					WebContext.Cache.Add(dataSet, ds, dep, System.Web.Caching.Cache.NoAbsoluteExpiration,
						TimeSpan.FromDays(cacheTime), System.Web.Caching.CacheItemPriority.Default, callback);
				}

				return ds;
			}
			catch (Exception ex)
			{
				ErrorHandler.Log("Error reading Data Set: " + dataSet + Environment.NewLine + ex.Message);
				throw (new Exception("Error reading Data Set."));
			}
			finally
			{
				_lock.ReleaseReaderLock();
			}
		}

		/// <summary>
		/// Rewrites the Dataset ds into the file provided in the dataSetPath
		/// </summary>
		/// <param name="dataSetPath">file path</param>
		/// <param name="dataSet">The dataset object</param>
		public static void SetDataSet(string dataSetPath, DataSet dataSet)
		{
			SetDataSet(dataSetPath, dataSet, true);
		}

		/// <summary>
		/// Rewrites the Dataset ds into the file provided in the dataSetPath
		/// </summary>
		/// <param name="dataSetPath">file path</param>
		/// <param name="dataSet">The dataset object</param>
		/// <param name="backup">Flag to either backup old dataset if exists. Defaults: true</param>
		public static void SetDataSet(string dataSetPath, DataSet dataSet, bool backup)
		{
			try
			{
				_lock.AcquireWriterLock(-1);

				string file = DataSetPath(dataSetPath);

				if (backup)
				{
					try
					{
						string path = DataSetPath("");
						path = Path.Combine(path, "_backups");
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						string dirName = StringUtils.GetFriendlyFileName(dataSetPath);

						path = Path.Combine(path, dirName);
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						DirectoryInfo _backups = new DirectoryInfo(path);

						FileInfo[] backups = _backups.GetFiles();

						if (backups.Length >= 15)
						{
							DateTime minDate = backups[0].CreationTime;
							FileInfo wastedFile = backups[0];
							foreach (FileInfo _file in backups)
							{
								if (minDate < _file.CreationTime)
									continue;
								wastedFile = _file;
							}
							File.Delete(wastedFile.FullName);
						}
						if (File.Exists(file))
							File.Copy(file, Path.Combine(path, DateTime.Now.ToLongDateString() + "-" + System.Guid.NewGuid() + ".config"), true);
					}
					catch
					{
					}
				}

				dataSet.WriteXml(file, XmlWriteMode.WriteSchema);
			}
			catch (Exception ex)
			{
				ErrorHandler.Log("Error writing Data Set: " + dataSet + Environment.NewLine +  ex.Message);
				throw (new Exception("Error writing Data Set."));
			}
			finally
			{
				_lock.ReleaseWriterLock();
			}
		}
	}
}
