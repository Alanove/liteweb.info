using System;
using System.Data;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Networking;
using lw.Utils;
using lw.WebTools;
using lw.CTE;

namespace lw.PhotoAlbums.Controls
{
	public class PhotoAlbumsDataSource : CustomDataSource
	{
		int _relationId = -1;
		bool _bound = false;
		string _relateTo = "";
		string _top = "100 PERCENT";
		string _category = null;
		int? _categoryId = null;
		bool _randomise = false;
		bool _networkBound = false;
		string _networkName = null;
		Languages _language = Languages.Default;
		string _condition = "";
		bool _status = true;
		int? _pagesCategoryId = null;



		PhotoAlbumsManager pMgr;


		public PhotoAlbumsDataSource()
		{
			this.DataLibrary = cte.lib;
			OrderBy = "Sort Desc";
			pMgr = new PhotoAlbumsManager();
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			if (String.IsNullOrWhiteSpace(Category))
			{
				if (Randomise)
				{
					this.SelectCommand = string.Format("Select top {0} *, newid() as ___Ran from AlbumsView", Top);
					this.OrderBy = "___Ran";
				}
				else
					this.SelectCommand = string.Format("Select top {0} * from AlbumsView", Top);
			}
			else
			{
				if (Randomise)
				{
					this.SelectCommand = string.Format("Select top {0} *, newid() as ___Ran from AlbumsFullView", Top);
					this.OrderBy = "___Ran";
				}
				else
					this.SelectCommand = string.Format("Select top {0} * from AlbumsFullView", Top);
			}


			if (!CMSMode.Value && !MyPage.Editable)
				this.SelectCommand += string.Format(" Where Status={0}", Status == true ? "1" : "0");
			else
				this.SelectCommand += " Where 1=1";
			//TODO: Display Disabled Albums in case an administrator is logged in

			if (!String.IsNullOrWhiteSpace(Category))
			{
				this.SelectCommand += string.Format(" And (UniqueName='{0}' or CategoryName='{0}')", StringUtils.SQLEncode(Category));
			}
			else if (CategoryId > 0)
				this.SelectCommand += string.Format(" And CategoryId&power(2, {0}) = power(2, {0})", CategoryId);

			if (_language != Languages.Default)
				this.SelectCommand += string.Format(" And (Language={0} or Language={1})", (int)_language, (int)Languages.Default);

			if (!string.IsNullOrWhiteSpace(Condition))
			{
				this.SelectCommand += string.Format(" And " + Condition);
			}

			if (PagesCategoryId.HasValue && PagesCategoryId.Value > 0)
			{

			} this.SelectCommand += string.Format(" And CategoryId <> {0}", PagesCategoryId);

			if (NetworkBound)
			{
				NetworkRelations networkRelations = new NetworkRelations();
				SelectCommand += " and " + networkRelations.GetRelationQueryByNetwork(cte.NetworkRelationTable, cte.NetworkRelateToField, Int32.Parse(networkName));
			}

			if (RelateToSection != null)
			{

				int relationValue = -1;

				switch (RelateToSection)
				{
					case SiteSections.Page:
						relationValue = (int)ControlUtils.GetBoundedDataField(this.NamingContainer, "PageId");
						break;
					default: 
						break;
				}

				this.SelectCommand += String.Format(" and Id in (select AlbumId from PhotoAlbumRelations where RelateToId={0} and Section={1})", relationValue, (int)RelateToSection);
			}

			if (CMSMode != null && CMSMode.Value)
			{
				string q = WebContext.Request["q"];
				string cat = WebContext.Request["CategoryId"];
				string net = WebContext.Request["NetworkId"];

				if (!string.IsNullOrWhiteSpace(q))
					this.SelectCommand += string.Format(" and (Name like '%{0}%' or DisplayName like '%{0}%')", StringUtils.SQLEncode(q));

				if (!string.IsNullOrWhiteSpace(cat))
					this.SelectCommand += string.Format(" and (categoryid & POWER(2, {0}) = POWER(2, {0}))", StringUtils.SQLEncode(cat));


				if (!string.IsNullOrWhiteSpace(net))
				{
					if (net != "-1")
						this.SelectCommand += string.Format(" and Id in (select Id from PhotoAlbumsNetwork where NetworkId={0})", Int32.Parse(net));
					else
						this.SelectCommand += string.Format(" and Id in (select Id from PhotoAlbumsNetwork)");
				}
			}

			if (!String.IsNullOrWhiteSpace(Filter))
				this.SelectCommand += " and " + Filter;

			if (!EnablePaging)
			{
				this.SelectCommand += " Order By " + this.OrderBy;
			}

			base.DataBind();
		}

		public int RelationId
		{
			get { return _relationId; }
			set { _relationId = value; }
		}
		public string RelateTo
		{
			get { return _relateTo; }
			set { _relateTo = value; }
		}
		public string Condition
		{
			get { return _condition; }
			set { _condition = value; }
		}
		public string Top
		{
			get { return _top; }
			set { _top = value; }
		}
		public string Category
		{
			get
			{
				if (StringUtils.IsNullOrWhiteSpace(_category))
				{
					string uniqueName = MyPage.GetQueryValue(RoutingParameters.AlbumCategory);

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
					string id = MyPage.GetQueryValue(RoutingParameters.AlbumCategoryID);

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

		public int? PagesCategoryId
		{
			get
			{
				if (_pagesCategoryId == null)
				{
					Config cfg = new Config();
					if (!string.IsNullOrWhiteSpace(cfg.GetKey(cte.PagesPhotoAlbumsCategoryId)))
						_pagesCategoryId = Int32.Parse(cfg.GetKey(cte.PagesPhotoAlbumsCategoryId));
					else
						_pagesCategoryId = cte.PagesDefaultPhotoAlbumsCategoryId;
				}
				return _pagesCategoryId;
			}
			set { _pagesCategoryId = value; }
		}

		NetworksManager nMgr = new NetworksManager();

		public string networkName
		{
			get
			{
				if (StringUtils.IsNullOrWhiteSpace(_networkName))
				{
					string uniqueName = MyPage.GetQueryValue(RoutingParameters.Network);

					if (!StringUtils.IsNullOrWhiteSpace(uniqueName))
					{
						_networkName = StringUtils.SQLEncode(uniqueName);
					}
					if (StringUtils.IsNullOrWhiteSpace(uniqueName))
					{
						_networkName = MyPage.GetQueryValue(RoutingParameters.NetworkId);
					}
				}
				Network net = nMgr.GetNetwork(_networkName);
				return net.NetworkId.ToString();
			}
			set { _networkName = value; }
		}
		public bool Randomise
		{
			get { return _randomise; }
			set { _randomise = true; }
		}


		Languages Language
		{
			get { return _language; }
			set { _language = value; }
		}
		/// <summary>
		/// Makes the list network bound returns only the items that are related to the current member's network
		/// </summary>
		public bool NetworkBound
		{
			get { return _networkBound; }
			set { _networkBound = value; }
		}


		/// <summary>
		/// Sets or retunrs the status of returned photo albums
		/// </summary>
		public bool Status
		{
			get { return _status; }
			set { _status = value; }
		}

		string _filter = "";
		/// <summary>
		/// Optional Filter than can be added to the SQL querry
		/// </summary>
		public string Filter
		{
			get
			{
				return _filter;
			}
			set
			{
				_filter = value;
			}
		}

		SiteSections? _RelateToSection = null;
		/// <summary>
		/// Set or Get the RelateToSite Section if the album is related to any other object in the database.
		/// </summary>
		public SiteSections? RelateToSection
		{
			get
			{
				return _RelateToSection;
			}
			set
			{
				_RelateToSection = value;
			}
		}
	}
}