using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using lw.WebTools;
using System.Threading;
using System.Xml;
using lw.Data;



namespace SABIS.Controls
{
	public class SABISNewsDataSource : lw.DataControls.CustomDataSource
	{
		public static ReaderWriterLock _lock = new ReaderWriterLock();

		public override object Data
		{
			get
			{
				DataTable dt = new DataTable();
				try
				{
					_lock.AcquireReaderLock(-1);

                    object obj = WebContext.Cache[CacheNews];

					if (obj != null)
					{
						dt = obj as DataTable;
					}
					else
					{
						_lock.ReleaseReaderLock();
						_lock.AcquireWriterLock(-1);

						XmlTextReader reader = new XmlTextReader(FeedUrl);
						DataSet ds = new DataSet();
						ds.ReadXml(reader);
						dt = ds.Tables[2];
						if (Max != null)
							dt = DBUtils.GetTop(dt.DefaultView, Max.Value, "").Table;

                        WebContext.Cache.Insert(CacheNews, dt, null, DateTime.Now.AddHours(6),
							TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);

					}
				}
				finally
				{
					_lock.ReleaseLock();
				}


				return dt;
			}
		}

        string _feedUrl = null;
        public virtual string FeedUrl
        {
            get
            {
                if (_feedUrl == null)
                {
                   _feedUrl= lw.WebTools.Config.GetFromWebConfig(cte.EnglishRssFeed);
                   if (String.IsNullOrWhiteSpace(_feedUrl))
                   {
                       _feedUrl = cte.DefaultEnglishRssFeed;
                   }
                }

                return _feedUrl;
            }
            set
            {
                _feedUrl = value;
            }
        }

        string _cacheNews = null;
        public virtual string CacheNews
        {
            get
            {
                if (_cacheNews == null)
                {
                    _cacheNews = "SABISNews";
                }

                return _cacheNews;
            }
            set
            {
                _cacheNews = value;
            }
        }


		int? _max = null;
		public int? Max
		{
			get
			{
				return _max;
			}
			set
			{
				_max = value;
			}
		}

		public override bool HasData
		{
			get
			{
				return ((DataTable)Data).Rows.Count > 0;
			}
		}
	}
}