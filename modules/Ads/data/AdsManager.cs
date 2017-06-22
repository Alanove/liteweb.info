using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.GraphicUtils;
using lw.Utils;
using lw.WebTools;


namespace lw.Ads
{
	/// <summary>
	/// A class to manage the ads in the system inherited from LINQManager
	/// </summary>
	public class AdsManager : LINQManager
	{
		/// <summary>
		/// AdsManager constructor inherited from LINQManager
		/// cte.lib: the name of the Database library associated with this DLL
		/// </summary>
		public AdsManager():base(cte.lib)
		{
		}

		#region Zones
		/// <summary>
		/// Creates an advertisement zone.
		/// </summary>
		/// <param name="Name">Name of the zone</param>
		/// <param name="Status"></param>
		/// <param name="Width">Width to auto resize related images</param>
		/// <param name="Height">Height to auto resize related images</param>
		/// <param name="Description">Description, used internaly to describe how and where this zone is used.</param>
		/// <returns>The ZoneId</returns>
		public int CreateZone(string Name, bool Status, short? Width, short? Height, string Description)
		{
			var q = from _zone in GetZones()
					where _zone.Name == Name
					select _zone;
			if (q.Count() > 0)
				return -1;

			Ads_Zone zone = new Ads_Zone
			{
				Name = Name,
				Alias = StringUtils.ToURL(Name),
				Status = Status,
				Width = Width,
				Height = Height,
				Description = Description,
			};

			AdsData.Ads_Zones.InsertOnSubmit(zone);
			AdsData.SubmitChanges();

			return zone.ZoneId;
		}

		public int UpdateZone(int ZoneId, string Name, bool Status, short? Width, short? Height, string Description)
		{
			var q = from _zone in GetZones()
					where _zone.Name == Name && _zone.ZoneId != ZoneId
					select _zone;
			if (q.Count() > 0)
				return -1;

			Ads_Zone zone = AdsData.Ads_Zones.Single(z => z.ZoneId == ZoneId);
			
			zone.Name = Name;
			zone.Alias = StringUtils.ToURL(Name);
			zone.Status = Status;
			zone.Width = Width;
			zone.Height = Height;
			zone.Description = Description;

			AdsData.SubmitChanges();

			return zone.ZoneId;
		}

		public IQueryable<Ads_Zone> GetZones()
		{
			var query = from zone in AdsData.Ads_Zones 
						select zone;
			return query;
		}
		public Ads_Zone GetZone(int? ZoneId)
		{
			if (ZoneId != null)
				return GetZone(ZoneId.Value);
			return null;
		}

		public Ads_Zone GetZone(int ZoneId)
		{
			return AdsData.Ads_Zones.Single(z => z.ZoneId == ZoneId);
		}

		public void DeleteZone(int ZoneId)
		{
			var ads = from ad in AdsData.Ads
					  where ad.ZoneId == ZoneId
					  select ad;
			foreach (Ad ad in ads)
				DeleteAd(new int[] {ad.AdId});

			var zone = AdsData.Ads_Zones.Single(z => z.ZoneId == ZoneId);
			AdsData.Ads_Zones.DeleteOnSubmit(zone);
			AdsData.SubmitChanges();
		}
		#endregion

		#region Keywords
		
		public int CreateKeyword(string Name)
		{
			var q = from key in GetKeywords()
					where key.Keyword == Name
					select key;
			if (q.Count() > 0)
				return -1;

			int? KeywordId = 0;

			AdsData.Ads_CreateKeyword(Name.Trim(), ref KeywordId);

			return KeywordId.Value;
		}

		public int UpdateKeyword(int KeywordId, string Name)
		{
			var query = from key in AdsData.Ads_Keywords
						where key.Keyword == Name && key.KeywordId != KeywordId
						select key;

			if (query.Count() > 0)
				return -1;

			Ads_Keyword keyword = AdsData.Ads_Keywords.Single(
					key => key.KeywordId == KeywordId);

			keyword.Keyword = Name;
			AdsData.SubmitChanges();

			return keyword.KeywordId;
		}

		public bool DeleteKeyword(int KeywordId)
		{
			Ads_Keyword keyword = AdsData.Ads_Keywords.Single(
					temp => temp.KeywordId == KeywordId);

			if (keyword != null)
			{
				AdsData.Ads_Keywords.DeleteOnSubmit(keyword);
				AdsData.SubmitChanges();
			}
			return true;
		}

		public IQueryable<Ads_Keyword> GetKeywords()
		{
			var query = from keyword in AdsData.Ads_Keywords
						select keyword;
			return query;
		}

		public Ads_Keyword GetKeyword(int KeywordId)
		{
			return GetKeywords().Single(temp => temp.KeywordId == KeywordId);
		}

		public int GetKeywordId(string Keyword)
		{
			return CreateKeyword(Keyword);
		}

		#endregion

		#region Ad Keywords Relation

		public bool AdInKeyword(int AdId, string Keyword)
		{
			return AdInKeyword(AdId, GetKeywordId(Keyword));
		}

		public bool AdInKeyword(int AdId, int KeywordId)
		{
			return (from rel in AdsData.Ads_AdKeywordRelations
					where rel.AdId == AdId && rel.KeywordId == KeywordId
					select rel).Count() > 0;
		}

		#endregion

		#region Ads

		/// <summary>
		/// Creates an advertisement
		/// </summary>
		/// <param name="Name">Display Name</param>
		/// <param name="AdvertiserId">Member Id</param>
		/// <param name="Status">lw.CTE.Enum.AdStatus</param>
		/// <param name="ZoneId">Zone</param>
		/// <param name="StartDate">Start Date</param>
		/// <param name="EndDate">End Date</param>
		/// <param name="PaymentType">lw.CTE.Enum.AdPaymentTypes</param>
		/// <param name="UnitsPurchased">Number of purchased units will be used against number of impressions depending on Payment Type and CostPerUnit</param>
		/// <param name="CostPerUnit">Cost per purchased unit</param>
		/// <param name="Image">Ad Image</param>
		/// <param name="AutoResize">AutoResize if true the image will be automatically resized to the Zone dimensions</param>
		/// <param name="Description">Descritpion (can appear on the title or alt tag)</param>
		/// <param name="HtmlCode">HtmlCode we can display it as html on the website</param>
		/// <param name="Url">The Ad URL</param>
		/// <param name="NewWindow">if the Ad should open in a new window</param>
		/// <param name="Weight">the more weight the more the add is important and can be displayed more frequently</param>
		/// <param name="MaxImpressionPerHour">MaxImpressionsPerHour</param>
		/// <param name="MaxImpressionPerDay">MaxImpressionsPerHour</param>
		/// <param name="Keywords">Related keywords, new keywords are automatically created and inserted into the Ad_Keywords Table</param>
		/// <returns>Created AD ID</returns>
		public int CreateAd(string Name, int? AdvertiserId, AdStatus? Status, 
			int? ZoneId, DateTime? StartDate, DateTime? EndDate, AdPaymentTypes? PaymentType,
			int? UnitsPurchased, int? CostPerUnit, 
			HttpPostedFile Image, bool? AutoResize,
			string Description,
			string HtmlCode, string Url, bool? NewWindow, short? Weight, 
			int? MaxImpressionPerHour, int? MaxImpressionPerDay, 
			string Keywords)
		{
			if (StringUtils.IsNullOrWhiteSpace(Name))
				return -1;

			var q = from _ad in GetAds()
					where
						_ad.Name == Name
					select _ad;
			if (q.Count() > 0)
				return -1;

			Ad ad = new Ad
			{
				Name = Name,
				AdvertiserId = AdvertiserId,
				Status = (short)Status,
				ZoneId = ZoneId,
				StartDate = StartDate,
				EndDate = EndDate,
				PaymentType = (short)PaymentType,
				UnitsPurchased = UnitsPurchased,
				CostPerUnit = CostPerUnit,
				Image = "",
				Description = Description,
				HtmlCode = HtmlCode,
				URL = Url,
				NewWindow = NewWindow,
				Weight = Weight,
				MaxImpressionPerHour = MaxImpressionPerHour,
				MaxImpressionPerDay = MaxImpressionPerDay,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};

			AdsData.Ads.InsertOnSubmit(ad);
			AdsData.SubmitChanges();

			if(Image !=null && Image.ContentLength > 0)
			{
				string ImageName = Path.GetFileNameWithoutExtension(Image.FileName);
				ImageName = string.Format("{0}_{1}{2}", ImageName, ad.AdId,
					Path.GetExtension(Image.FileName));

				string path = WebContext.Server.MapPath(Path.Combine(WebContext.StartDir, Folders.AdsFolder));

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				path = Path.Combine(path, ImageName);

				Image.SaveAs(path);

				if (AutoResize != null && AutoResize.Value == true)
				{
					Ads_Zone zone = GetZone(ZoneId);
					if (zone.Width > 0 && zone.Height > 0)
						ImageUtils.Resize(path, path, (int)zone.Width, (int)zone.Height);
				}

				ad.Image = ImageName;
				AdsData.SubmitChanges();
			}

			if (!String.IsNullOrEmpty(Keywords))
			{
				string[] keywords = Keywords.Split(new char[] {',', ' ', ';'});

				var keywordsQuery = from keyword in AdsData.Ads_Keywords
							where keywords.Contains(keyword.Keyword)
							select keyword;

				foreach (string key in keywords)
				{
					Ads_AdKeywordRelation rel;
					if (keywordsQuery.Where(temp => temp.Keyword == key).Count() > 0)
					{
						rel = new Ads_AdKeywordRelation
						{
							AdId = ad.AdId,
							KeywordId = keywordsQuery.Single(temp => temp.Keyword == key).KeywordId
						};
					}
					else
					{
						rel = new Ads_AdKeywordRelation
						{
							AdId = ad.AdId,
							KeywordId = CreateKeyword(key)
						};
					}
					AdsData.Ads_AdKeywordRelations.InsertOnSubmit(rel);
				}
				AdsData.SubmitChanges();
			}
			return ad.AdId;
		}

		/// <summary>
		/// Creates an advertisement
		/// </summary>
		/// <param name="AdId">AD ID</param>
		/// <param name="Name">Display Name</param>
		/// <param name="AdvertiserId">Member Id</param>
		/// <param name="Status">lw.CTE.Enum.AdStatus</param>
		/// <param name="ZoneId">Zone</param>
		/// <param name="StartDate">Start Date</param>
		/// <param name="EndDate">End Date</param>
		/// <param name="PaymentType">lw.CTE.Enum.AdPaymentTypes</param>
		/// <param name="UnitsPurchased">Number of purchased units will be used against number of impressions depending on Payment Type and CostPerUnit</param>
		/// <param name="CostPerUnit">Cost per purchased unit</param>
		/// <param name="Image">Ad Image</param>
		/// <param name="DeleteOldImage">Delete Old Image flag, if true the image will deleted, if we have new image the parameter is ignored.</param>
		/// <param name="AutoResize">AutoResize if true the image will be automatically resized to the Zone dimensions</param>
		/// <param name="Description">Descritpion (can appear on the title or alt tag)</param>
		/// <param name="HtmlCode">HtmlCode we can display it as html on the website</param>
		/// <param name="Url">The Ad URL</param>
		/// <param name="NewWindow">if the Ad should open in a new window</param>
		/// <param name="Weight">the more weight the more the add is important and can be displayed more frequently</param>
		/// <param name="MaxImpressionPerHour">MaxImpressionsPerHour</param>
		/// <param name="MaxImpressionPerDay">MaxImpressionsPerHour</param>
		/// <param name="Keywords">Related keywords, new keywords are automatically created and inserted into the Ad_Keywords Table</param>
		/// <returns>AD ID or -1 in case of error</returns>
		public int UpdateAd(int AdId, string Name, int? AdvertiserId, AdStatus? Status,
			int? ZoneId, DateTime? StartDate, DateTime? EndDate, AdPaymentTypes? PaymentType,
			int? UnitsPurchased, int? CostPerUnit,
			HttpPostedFile Image, bool? AutoResize, bool DeleteOldImage,
			string Description,
			string HtmlCode, string Url, bool? NewWindow, short? Weight,
			int? MaxImpressionPerHour, int? MaxImpressionPerDay,
			string Keywords)
		{
			var q = from _ad in GetAds()
					where 
						_ad.Name == Name && _ad.AdId != AdId
					select _ad;
			if (q.Count() > 0)
				return -1;

			Ad ad = AdsData.Ads.Single(temp => temp.AdId == AdId);

			ad.Name = Name;
			ad.AdvertiserId = AdvertiserId;
			ad.Status = (short)Status;
			ad.ZoneId = ZoneId;
			ad.StartDate = StartDate;
			ad.EndDate = EndDate;
			ad.PaymentType = (short)PaymentType;
			ad.UnitsPurchased = UnitsPurchased;
			ad.CostPerUnit = CostPerUnit;
			ad.Description = Description;
			ad.HtmlCode = HtmlCode;
			ad.URL = Url;
			ad.NewWindow = NewWindow;
			ad.Weight = Weight;
			ad.MaxImpressionPerHour = MaxImpressionPerHour;
			ad.MaxImpressionPerDay = MaxImpressionPerDay;
			ad.DateModified = DateTime.Now;

			string path = WebContext.Server.MapPath(Path.Combine(WebContext.StartDir, Folders.AdsFolder));
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			var oldImage = ad.Image;

			if (Image != null && Image.ContentLength > 0)
			{
				DeleteOldImage = true;

				string ImageName = Path.GetFileNameWithoutExtension(Image.FileName);
				ImageName = string.Format("{0}_{1}{2}", ImageName, ad.AdId,
					Path.GetExtension(Image.FileName));

				string _path = Path.Combine(path, ImageName);

				Image.SaveAs(_path);

				if (AutoResize != null && AutoResize.Value)
				{
					Ads_Zone zone = GetZone(ZoneId);
					if (zone.Width > 0 && zone.Height > 0)
						ImageUtils.Resize(_path, _path, (int)zone.Width, (int)zone.Height);
				}

				if (ImageName == ad.Image)
					DeleteOldImage = false;

				ad.Image = ImageName;
				AdsData.SubmitChanges();
			}

			if (DeleteOldImage)
			{
				path = Path.Combine(path, oldImage);
				if(File.Exists(path))
					File.Delete(path);
			}

			List<int> thisAdKeywords = new List<int>();

			if (!String.IsNullOrEmpty(Keywords))
			{
				string[] keywords = Keywords.Split(',');

				var keywordsQuery = from keyword in AdsData.Ads_Keywords
									where keywords.Contains(keyword.Keyword)
									select keyword;

				foreach (string key in keywords)
				{
					Ads_AdKeywordRelation rel;
					if (keywordsQuery.Where(temp => temp.Keyword == key).Count() > 0)
					{
						int _keyId = keywordsQuery.Single(temp => temp.Keyword == key).KeywordId;
						if(!AdInKeyword(AdId, _keyId))
						{
							rel = new Ads_AdKeywordRelation
							{
								AdId = ad.AdId,
								KeywordId = _keyId
							};
							AdsData.Ads_AdKeywordRelations.InsertOnSubmit(rel);
						}
						thisAdKeywords.Add(_keyId);
					}
					else
					{
						rel = new Ads_AdKeywordRelation
						{
							AdId = ad.AdId,
							KeywordId = CreateKeyword(key)
						};
						thisAdKeywords.Add(rel.KeywordId);
						AdsData.Ads_AdKeywordRelations.InsertOnSubmit(rel);
					}
				}
			}

			var deleteQuery = from relation in AdsData.Ads_AdKeywordRelations
							  where !thisAdKeywords.Contains(relation.KeywordId) && relation.AdId == AdId
							  select relation;

			AdsData.Ads_AdKeywordRelations.DeleteAllOnSubmit(deleteQuery);

			AdsData.SubmitChanges();

			return AdId;
		}

		public void UpdateAdImpressions(int AdId)
		{
			AdsData.Ads_UpdateImpressions(AdId, false);
		}
		public void UpdateAdClicks(int AdId)
		{
			AdsData.Ads_UpdateImpressions(AdId, true);
		}


		public IQueryable<AdsView> GetAds()
		{
			return from ad in AdsData.AdsViews select ad;
		}

		/// <summary>
		/// Get just one ad depending on zone and keywords
		/// </summary>
		/// <param name="Zone">Name of the Zone ex: left, top banner ...</param>
		/// <param name="Keywords">list of keywords</param>
		/// <param name="AdIds">optionnal: list of already displayed Ads to avoid repeated ads, this automaticcally acumulated with in the PageContext[cte.AdsContext]</param>
		/// <returns></returns>
		public AdsView GetAd(string Zone, string Keywords, string AdIds)
		{
			//We Need just one record
			StringBuilder sql = new StringBuilder("select top 1 AdsView.*, NewId() as Ran from AdsView where");

			//Status have to be default or active
			sql.Append(string.Format(" Status in ({0}, {1})", (int)AdStatus.Active, (int)AdStatus.Default));

			//Ad Zone
			if(!String.IsNullOrEmpty(Zone))
				sql.Append(string.Format(" and ZoneName=N'{0}'", StringUtils.SQLEncode(Zone)));

			/* 
			 * Check for keywords
			 * AdKeywordsRelations(AdId, KeywordId) link between ads and keywords
			 */
			if(!String.IsNullOrEmpty(Keywords))
			{
				sql.Append(String.Format(@" and AdId in (select AdId from Ads_AdKeywordRelation where KeywordId in 
											(select KeywordId from Ads_Keywords where Keyword in 
												(select id from [dbo].[StringToTable]('{0}'))
											)
										)", StringUtils.SQLEncode(Keywords)));
			}

			//Date have to be between start and end date
			sql.Append(" and getDate() between IsNull(StartDate, getDate()) and IsNull(EndDate, getDate())");


			// Check for payment types the units purchased must be more than impressions or clicks
			sql.Append(string.Format(" and (PaymentType={0} or", (int)AdPaymentTypes.None));
			sql.Append(string.Format("(PaymentType={0} and UnitsPurchased*CostPerUnit < Impressions) or", (int)AdPaymentTypes.PerImpression));
			sql.Append(string.Format("(PaymentType={0} and UnitsPurchased*CostPerUnit < Clicks))", (int)AdPaymentTypes.PerClick));

			//GetThisHousImpression calculate data from Ads_Report
			sql.Append(" and (MaxImpressionPerHour is null or MaxImpressionPerHour = 0 or MaxImpressionPerHour > [dbo].GetThisHourImpressions(AdId))");

			//GetThisDayImpressions calculate data from Ads_Report
			sql.Append(" and (MaxImpressionPerDay is null or MaxImpressionPerDay = 0 or MaxImpressionPerDay > [dbo].GetThisDayImpressions(AdId))");

			/*
			 * Check for ids that are already displayed
			 * Are passed in the AdIds parameter
			 * and are stored in page.PageContext["AdIds"]
			 */
			if (!String.IsNullOrEmpty(AdIds))
				sql.Append(string.Format(" and AdId not in (-1{0})", AdIds));

			/* 
			 * order by Status active = 2, default = 1 so active comes before default
			 * Ran to randomize
			 */
			sql.Append("order by AdsView.Status Desc,Ran");

			DataSet ds = DBUtils.GetDataSet(sql.ToString(), cte.lib);

			AdsView ad = null;

			if (ds.Tables[0].Rows.Count > 0)
			{
				DataRow row = ds.Tables[0].Rows[0];
				ad = new AdsView
				{
					AdId = (int)row["AdId"],
					Name = row["Name"].ToString(),
					AdvertiserId = (int?)row["AdvertiserId"],
					Status = (short?)row["Status"],
					ZoneId = (int?)row["ZoneId"],
					StartDate = (DateTime?)(row["StartDate"] != DBNull.Value? row["StartDate"] : null),
					EndDate = (DateTime?)(row["EndDate"] != DBNull.Value? row["EndDate"]:  null),
					PaymentType = (short?)(row["PaymentType"] != DBNull.Value? row["PaymentType"] :null),
					UnitsPurchased = (int?)(row["UnitsPurchased"] != DBNull.Value? row["UnitsPurchased"]: null),
					CostPerUnit = (decimal?)(row["CostPerUnit"] != DBNull.Value? row["CostPerUnit"]: null),
					Image = row["Image"].ToString(),
					Description = row["Description"].ToString(),
					HtmlCode = row["HtmlCode"].ToString(),
					URL = row["URL"].ToString(),
					NewWindow = (bool?)row["NewWindow"],
					Weight = (short?)(row["Weight"] != DBNull.Value? row["Weight"]: null),
					MaxImpressionPerDay = (int?)(row["MaxImpressionPerDay"] != DBNull.Value?row["MaxImpressionPerDay"] : null),
					MaxImpressionPerHour = (int?)(row["MaxImpressionPerHour"] != DBNull.Value? row["MaxImpressionPerHour"]: null),
					DateCreated = (DateTime?)row["DateCreated"],
					DateModified = (DateTime?)row["DateModified"]
				};
				
				ad.Image = String.IsNullOrEmpty(ad.Image) ? ad.Image :
					String.Format("{0}/{1}/{2}", WebContext.Root, Folders.AdsFolder, ad.Image);

				if(!String.IsNullOrEmpty(ad.URL))
				{
					ad.URL = String.IsNullOrEmpty(ad.URL) ? ad.URL :
						String.Format("{0}://{1}{2}/{3}?AdId={4}", 
							WebContext.Protocol, 
							WebContext.ServerName, 
							WebContext.Root,
							Files.AdsHandler, 
							ad.AdId);
				}
				/*
				 * {ad:image} and {ad:url} can be used in html 
				 * and are automaticcally replaced by their dynamic alternatives.
				 */
				ad.HtmlCode = ad.HtmlCode.Replace("{ad:image}", string.Format("<img src=\"{0}\" alt=\"{1}\" />", ad.Image, ad.Description));
				ad.HtmlCode = ad.HtmlCode.Replace("{ad:url}", ad.URL);
			}

			/*
			 * update impression counter is called when the add is actually displayed and not here.
			 * can be found uner lw.Controls.Ad (OnUnload)
			 */

			return ad;
		}

		/// <summary>
		/// This function  is used to search inside the CMS
		/// </summary>
		/// <param name="name">wild search between name and description of the ad</param>
		/// <param name="zoneId">From Zones drop down menu</param>
		/// <param name="keywords">list of keywords that we want to search for seperated by comma ","</param>
		/// <param name="status">Ad Status</param>
		/// <param name="url">Ad Url wild search</param>
		/// <param name="paymentType">Ad Payment</param>
		/// <param name="dateFrom">StartDate of the Ad</param>
		/// <param name="dateTo">End Date of the Ad</param>
		/// <returns>DataTable AdsView</returns>
		public DataTable GetSearchAds(string name, int? zoneId, string keywords, AdStatus? status, 
			string url,
			AdPaymentTypes? paymentType,
			DateTime? dateFrom, DateTime? dateTo)
		{
			StringBuilder sql = new StringBuilder("select AdsView.* from AdsView where 1=1");

			if(!String.IsNullOrEmpty(name))
				sql.Append(string.Format(" and (Description like N'%{0}%' or Name like N'%{0}%')", StringUtils.SQLEncode(name)));

			if(status != null)
				sql.Append(string.Format(" and Status = {0}", (int)status));

			if (zoneId != null)
				sql.Append(string.Format(" and ZoneId='{0}'", zoneId.Value));

			if (!string.IsNullOrEmpty(url))
				sql.Append(string.Format(" and URL like N'%{0}%'", url));

			/*
			 * StringToTable is a custom SQL function that transforms 
			 * a string[] to SQL Table with one columns called id
			 */
			if (!String.IsNullOrEmpty(keywords))
			{
				sql.Append(String.Format(@" and AdId in (select AdId from Ads_AdKeywordRelation where KeywordId in 
											(select KeywordId from Ads_Keywords where Keyword in 
												(select id from [dbo].[StringToTable]('{0}'))
											)
										)", StringUtils.SQLEncode(keywords)));
			}

			if(dateFrom != null)
				sql.Append(string.Format(" and StartDate >= '{0}'", dateFrom));

			if (dateTo != null)
				sql.Append(string.Format(" and EndDate <= '{0}'", dateTo));


			if(paymentType != null)
				sql.Append(string.Format(" and PaymentType={0}", (int)paymentType));

			sql.Append("order by AdsView.Name");

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
		}


		public Ad GetSingleAd(int AdId)
		{
			var query =  from ad in AdsData.Ads
						 where ad.AdId == AdId
				   select ad;
			if (query.Count() > 0)
				return query.Single();
			return null;
		}


		public void DeleteAd(int[] AdIds)
		{
			var q = from ad in AdsData.Ads
					where AdIds.Contains(ad.AdId)
					select ad;
			foreach (Ad ad in q.AsEnumerable())
			{
				if (!String.IsNullOrEmpty(ad.Image))
				{
					string path = string.Format("{0}/{1}/{2}",
						WebContext.Root, Folders.AdsFolder, ad.Image);
					path = WebContext.Server.MapPath(path);

					if (File.Exists(path))
						File.Delete(path);
				}
			}

			AdsData.Ads.DeleteAllOnSubmit(q);
			AdsData.SubmitChanges();
		}

		#endregion

		#region Variables

		
		public AdsDataContext AdsData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new AdsDataContext(Connection);
				return (AdsDataContext)_dataContext;
			}
		}

		#endregion
	}
}
