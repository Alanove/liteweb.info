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
using System.Collections;
using lw.PhotoAlbums;
using lw.Utils;
using lw.Pages;
using lw.CTE;


namespace lw.Searcher.Controls
{
    public class SearchResultsDataSource : lw.DataControls.CustomDataSource
    {
        public static ReaderWriterLock _lock = new ReaderWriterLock();

		/// <summary>
		/// Returns the searched data containing an additional data field Path
		/// </summary>
        public DataView Results
        {
            get
            {
                DataTable dt = new DataTable();

                try
                {
                    _lock.AcquireReaderLock(-1);

                    if (!String.IsNullOrWhiteSpace(Query))
                    {
                        string cacheKey = "SABISSearchResults_" + Query;

                        object obj = WebContext.Cache[cacheKey];

                        if (obj != null)
                        {
                            dt = obj as DataTable;
                        }
                        else
                        {
                            _lock.ReleaseReaderLock();
                            _lock.AcquireWriterLock(-1);

                            DataSet ds = GetSearchResults(q);

                            dt = ds.Tables[0];
                            dt.Columns.Add("Path");

                            PhotoAlbumsManager phMgr = new PhotoAlbumsManager();
							PagesManager pMgr = new PagesManager();

                            ArrayList toDelete = new ArrayList();

                            bool allLevels = bool.Parse(WebTools.WebUtils.GetFromWebConfig(cte.SearchAllLevels));
							
							string photoGalUrl = WebTools.WebUtils.GetFromWebConfig(AppConfig.PhotoGalleryUrl);
							
							if (string.IsNullOrWhiteSpace(photoGalUrl))
								photoGalUrl = AppConfig.PhotoGalleryURLDefault;

							var parentPage = pMgr.GetPageView(photoGalUrl);

							DataView albumsDV = new DataView(dt, "TableType = 'PhotoAlbums'", "", DataViewRowState.CurrentRows);
							string filter = "";
							string sep = "";

							foreach (DataRowView drv in albumsDV)
							{
								filter += sep + drv["Id"].ToString();
								sep = ",";
							}

							DataView photoAlbums = null;
							if (filter != "")
								photoAlbums = phMgr.GetAllPhotoAlbums("Id in (" + filter + ")");

							if (string.IsNullOrWhiteSpace(WebContext.Profile.OperatorGroupName))
							{
								for (int i = dt.Rows.Count - 1; i >= 0; i--)
								{
									DataRow dr = dt.Rows[i];
									if (Int32.Parse(dr["Status"].ToString()) != 1)
										dr.Delete();
									if (i == 0)
										dt.AcceptChanges();
								}
							}

							foreach (DataRow dr in dt.Rows)
                            {
                                if (String.IsNullOrEmpty(dr["Name"].ToString()))
                                {
                                    toDelete.Add(dr);
                                    continue;
                                }

                                switch (dr["TableType"].ToString())
                                {
                                    case "PhotoAlbums":
										photoAlbums.RowFilter = "Id = " + dr["Id"].ToString();

										if (photoAlbums.Count > 0)
										{
											dr["Path"] = string.Format("<a href='{0}/{3}/{1}'>{2}</a>",
												WebContext.Root,
												photoAlbums[0]["Name"].ToString(),
												StringUtils.AddSup(StringUtils.StripOutHtmlTags(photoAlbums[0]["DisplayName"].ToString())),
												"photo-gallery");
											if (photoAlbums[0]["CategoryId"].ToString() == lw.PhotoAlbums.cte.PagesPhotoAlbumsCategoryId.ToString())
												toDelete.Add(dr);
										}
                                        break;
                                    case "News":
                                    case "Pages":
                                        if (!allLevels)
                                        {
                                            if (dr["UniqueName"].ToString().IndexOf("/") <= 0)
                                            {
                                                toDelete.Add(dr);
                                                break;
                                            }
                                        }
                                        string p = dr["UniqueName"].ToString();

                                        if (p.IndexOf("testimonials/") != -1)
                                            p = p.Replace("testimonials/", "testimonials#");

                                        dr["Path"] = string.Format("<a href='{0}/{1}'>{2}</a>",
                                            WebContext.Root,
                                            p,
                                            StringUtils.AddSup(StringUtils.StripOutHtmlTags(dr["Name"].ToString())));
                                        break;
                                    case "Downloads":
                                        lw.Downloads.Downloads d = new lw.Downloads.Downloads();
                                        string sql = "Select UniqueName From DownloadTypes Where TypeId = (Select DownloadType From Downloads Where Title = '" + dr["Name"] + "')";
                                        DataRow downloadsDs = DBUtils.GetDataSet(sql, lw.Downloads.cte.lib).Tables[0].Rows[0];

                                        string a = "";

                                        if (downloadsDs != null)
                                        {
                                            a = downloadsDs["UniqueName"].ToString();
                                        }

                                        dr["Path"] = string.Format("<a href='{0}/downloads/" + a + "/{1}'>{2}</a>",
                                            WebContext.Root,
                                            dr["UniqueName"],
                                            StringUtils.AddSup(StringUtils.StripOutHtmlTags(dr["Name"].ToString())));
                                        break;
                                    default:
                                        toDelete.Add(dr);
                                        break;
                                }
                            }
                            if (toDelete.Count > 0)
                            {
                                for (int i = 0; i < toDelete.Count; i++)
                                {
                                    DataRow temp = toDelete[i] as DataRow;
                                    temp.Delete();
                                }
                                dt.AcceptChanges();
                            }

                            WebContext.Cache.Insert(cacheKey, dt, null, DateTime.Now.AddHours(6),
                                TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
                        }
                    }
                }
                finally
                {
                    _lock.ReleaseLock();
                }

                return dt.DefaultView;
            }
        }

		/// <summary>
		/// Returns the data row count
		/// </summary>
        public override int RowsCount
        {
            get
            {
                return Results.Count;
            }
        }

		/// <summary>
		/// Gets or sets the data related to the data source
		/// </summary>
        public override object Data
        {
            get
            {
                if (Max != null && Results.Count > Max)
                {
                    return DBUtils.GetTop(Results, Max.Value, "");
                }
                return Results;
            }
            set
            {
                base.Data = value;
            }
        }

		/// <summary>
		/// Calls the Stored Procedure and gets the results from the database.
		/// Note that these data are raw data that contains only the ids and type of returned items
		/// To get the full data will all the results call <seealso cref="Data"/> or <seealso cref="Results"/>
		/// </summary>
		/// <param name="SearchQuery">The search query</param>
		/// <returns>The dataset containing the search results</returns>
        public DataSet GetSearchResults(string SearchQuery)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DECLARE @RC int DECLARE @KeyWord nvarchar(250) ");
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                sb.Append("select @KeyWord = N'" + StringUtils.SQLEncode(SearchQuery) + "'");
            }
            sb.Append(" EXECUTE @RC = [dbo].[SearchSite] @KeyWord");

            var a = DBUtils.GetDataSet(sb.ToString(), lw.Searcher.cte.lib);

            return a;
        }

		/// <summary>
		/// Returns true if the search returned any results.
		/// </summary>
        public override bool HasData
        {
            get
            {
                return RowsCount > 0;
            }
        }

        int? max = null;
        /// <summary>
        /// Defines the max number of returned items
        /// Returns all if null
        /// </summary>
        public int? Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
            }
        }

        string q = null;
        /// <summary>
        /// Gets or sets the searchable querry
        /// </summary>
		public string Query
        {
            get
            {
                if (String.IsNullOrEmpty(q))
                    q = MyPage.GetQueryValue("q");
                return q;
            }
            set
            {
                q = value;
            }
        }
    }
}