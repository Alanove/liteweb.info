using System;
using System.Data;
using System.Text;
using System.Web.UI;
using lw.Base;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Networking;
using lw.Utils;
using lw.WebTools;
using lw.Data;

namespace lw.Articles.Controls
{
	public class NewsDataSrc : CustomDataSource
	{
		bool _bound = false;
		NewsTagStatus? status = null;
		string type = null;
		string month = "";
		string year = "";
		Languages _Language = Languages.English;
		string _CustomSort = null;
		ArticlesSort _Sort = ArticlesSort.DateDesc;
		int? max = null;
		int? _typeId = null;
		bool _networkBound = false;
		string _networkName = null;


		public NewsDataSrc()
		{
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			BuildQuery();


			base.DataBind();
		}


		/// <summary>
		/// Builds the select command for the data source
		/// </summary>
		public void BuildQuery()
		{
			StringBuilder cond = new StringBuilder();

			NewsManager nMgr = new NewsManager();

			if (TypeId != null)
			{
				if (!Recursive)
					cond.Append(string.Format(" And NewsType={0}", TypeId.Value));
				else
				{
					string tempSql = string.Format("select * from dbo.NewsTypes_GetDescendants({0})", TypeId);
					DataSet tempDs = DBUtils.GetDataSet(tempSql, cte.lib);

					string sep = "";

					StringBuilder tempSB = new StringBuilder();
					foreach (DataRow newsType in tempDs.Tables[0].Rows)
					{
						tempSB.Append(sep);
						tempSB.Append(newsType["TypeId"].ToString());
						sep = ",";
					}

					string typeIds = tempSB.ToString();
					if (!String.IsNullOrWhiteSpace(typeIds))
					{
						cond.Append(string.Format(" And NewsType in ({0})", typeIds));
					}
				}
			}
			if (month != null && month != "")
			{
				DateTime d = DateTime.Parse(month);
				DateTime d1 = d.AddMonths(1);

				cond.Append(string.Format(" And NewsDate Between '{0:M}' and '{1:M}'", d, d1));
			}
			if (!String.IsNullOrEmpty(Year))
			{
				cond.Append(string.Format(" And datepart(yyyy,  NewsDate) = '{0}'", Year));
			}

			if (NetworkBound)
			{
				NetworkRelations networkRelations = new NetworkRelations();
				cond.Append(" and " + networkRelations.GetRelationQueryByNetwork(cte.NetworkRelationTable, cte.NetworkRelateToField, Int32.Parse(networkName)));
			}
			//will show all enabled news if no status is specified
			if (Status == null)
			{
                if (CMSMode == null || !CMSMode.Value)
                {
                    cond.Append(string.Format(" And (Status={0} or Status={1})", (int)NewsTagStatus.Archive, (int)NewsTagStatus.MainPage));
                }
            }
			else
			{
				if (status == NewsTagStatus.Inherit)
				{
					try
					{
						int _status = -1;
						object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Status");
						if (obj != null)
							_status = Int32.Parse(obj.ToString());
						else
							_status = (int)NewsStatus.Archive;

						cond.Append(string.Format(" And Status={0}", _status));
					}
					catch
					{
					}
				}
				else
				{
					cond.Append(string.Format(" And Status={0}", (int)Status));
				}
			}
			cond.Append(string.Format(" And (NewsLanguage={0} or NewsLanguage={1})", (int)Language, (int)Languages.Default));

			if (!String.IsNullOrEmpty(WebContext.Request.QueryString["NewsCategory"]))
			{
				cond.Append(" And UniqueName='" + StringUtils.SQLEncode(WebContext.Request.QueryString["NewsCategory"]) + "'");
			}


			cond.Append(" And  (PublishDate is Null Or PublishDate <= getDate())");

			if (!String.IsNullOrWhiteSpace(_customCondition))
			{
				cond.Append(_customCondition);
			}

			string _q = WebContext.Request["q"];
			string _network = WebContext.Request["NetworkId"];

			if (!String.IsNullOrWhiteSpace(_q))
			{
				cond.Append(string.Format(" And (Title like '%{0}%' or UniqueName  like '%{0}%' or Header  like '%{0}%;')", StringUtils.SQLEncode(_q)));
			}

			if (!String.IsNullOrEmpty(_network))
			{
				cond.Append(string.Format(" and NewsId in (select NewsId from NewsNetwork where NetworkId={0})", Int32.Parse(_network)));
			}


			if (!String.IsNullOrWhiteSpace(Filter))
			{
				cond.Append(" and " + Filter);
			}

			string sql = "";
			if (cond.Length > 0)
				sql = cond.ToString().Substring(5);

			string _max = "";
			if (Max != null)
				_max = string.Format(" Top {0}", Max);

			this.SelectCommand = string.Format("select {1}* from NewsView where {0}", sql, _max);

			//WebContext.Response.Write(this.SelectCommand);
			//WebContext.Response.End();
			
			if (!EnablePaging)
			{
				this.SelectCommand += " Order By " + this.OrderBy;
			}
		}

		public override string OrderBy
		{
			get
			{
				if (Sort != ArticlesSort.Custom)
				{
					return EnumHelper.GetDescription(Sort);
				}
				else
				{
					return CustomSort;
				}
			}
		}


		#region Properties


		string _customCondition = "";
		public string CustomCondition
		{
			get { return _customCondition; }
			set { _customCondition = value; }
		}


		public NewsTagStatus? Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}
		public Languages Language
		{
			get
			{
				return _Language;
			}
			set
			{
				_Language = value;
			}
		}

		/// <summary>
		/// Returns or sets NewsType (Name) 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		/// 
		public string Type
		{
			get
			{
				if (type == null)
				{
					type = MyPage.GetQueryValue("NewsType");
				}
				return type;
			}
			set { type = value; }
		}

		public string Month
		{
			get
			{
				if (month == null || month == "")
					month = this.Page.Request.QueryString["month"];

				return month;
			}
			set
			{
				month = Month;
			}
		}
		public string Year
		{
			get
			{
				if (String.IsNullOrEmpty(year))
					year = WebContext.Request.QueryString["year"];

				return year;
			}
			set { year = value; }
		}


		public ArticlesSort Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}

		public string CustomSort
		{
			get { return _CustomSort; }
			set { _CustomSort = value; }
		}
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
        bool? _CMSMode = false;
        /// <summary>
        /// Indicates if the tag is used inside the CMS
        /// Can also be set in web.config.
        /// </summary>
        public bool? CMSMode
        {
            get
            {
                if (_CMSMode == null)
                {
                    string val = Config.GetFromWebConfig(lw.CTE.parameters.CMSMode);
                    _CMSMode = !String.IsNullOrEmpty(val) && bool.Parse(val.Trim());
                }
                return _CMSMode;
            }
            set
            {
                _CMSMode = value;
            }
        }

		NetworksManager nMgr = new NetworksManager();

		public string networkName
		{
			get
			{
				if (StringUtils.IsNullOrWhiteSpace(_networkName))
				{
					string uniqueName = null;

					if (!string.IsNullOrEmpty((this.Page as lw.Base.CustomPage).GetQueryValue("GroupName")))
						uniqueName = (this.Page as lw.Base.CustomPage).GetQueryValue("GroupName");
					else
						uniqueName = (this.Page as lw.Base.CustomPage).GetQueryValue("Network");

					if (!StringUtils.IsNullOrWhiteSpace(uniqueName))
					{
						_networkName = StringUtils.SQLEncode(uniqueName);
					}
				}
				Network net = nMgr.GetNetwork(_networkName);
				return net.NetworkId.ToString();
			}
			set { _networkName = value; }
		}

		public int? TypeId
		{
			get
			{
				if (_typeId == null && MyPage != null)
				{
					string tempId = MyPage.GetQueryValue("TypeId");

                    if (String.IsNullOrWhiteSpace(tempId))
                    {
						tempId = MyPage.GetQueryValue("Id");
                    }
					if (String.IsNullOrWhiteSpace(tempId))
					{
						NewsManager nMgr = new NewsManager();
						if (!String.IsNullOrEmpty(Type))
						{
							DataView types = nMgr.GetNewsTypes(string.Format("Name=N'{0}' or UniqueName=N'{0}'", StringUtils.SQLEncode(Type)));
							if (types.Count > 0)
								_typeId = (int)types[0]["TypeId"];
						}
						else if (!String.IsNullOrWhiteSpace(tempId))
						{
							DataView types = nMgr.GetNewsTypes(string.Format("TypeId=" + tempId, StringUtils.SQLEncode(Type)));
							if (types.Count > 0)
								_typeId = (int)types[0]["TypeId"];
						}
						else
						{
							try
							{
								object obj = DataBinder.Eval(this.NamingContainer, "DataItem.TypeId");
								if (obj != null)
									_typeId = (int)obj;
							}
							catch (Exception Ex)
							{ }
						}
					}
					else
					{
						string[] temp = tempId.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
						tempId = temp[temp.Length - 1];
						_typeId = Int32.Parse(tempId);
					}
				}
				return _typeId;

			}
			set
			{
				_typeId = value;
			}
		}
		/// <summary>
		/// Makes the list network bound returns only the items that are related to the current member's network
		/// </summary>
		public bool NetworkBound
		{
			get { return _networkBound; }
			set { _networkBound = value; }
		}

		bool _recursive = false;
		/// <summary>
		/// If true the data source will fetch the data from the category and all child categories
		/// Default: false
		/// </summary>
		public bool Recursive
		{
			get
			{
				return _recursive;
			}
			set
			{
				_recursive = value;
			}
		}

		string _filter;
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

		#endregion
	}
}