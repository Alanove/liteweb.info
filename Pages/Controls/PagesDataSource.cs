using System;
using System.Data;
using System.Text;
using System.Web.UI;
using lw.Base;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;
using lw.Data;

namespace lw.Pages.Controls
{
	public class PagesDataSource : CustomDataSource
	{
		bool _bound = false;
		string type = null;
		string month = "";
		string year = "";
		Languages _Language = Languages.English;
		string _CustomSort = null;
		PagesSort _Sort = PagesSort.Ranking;
		int? max = null;
		int? _parentId = null;
		bool _networkBound = false;
		string _networkName = null;
		bool _getFromParent = false;



		public PagesDataSource()
		{
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (!_autoDataBind)
				return;

			if (_bound)
				return;
			_bound = true;

			BuildQuery();


			base.DataBind();
		}
		public void DataBind(bool Force)
		{
			BuildQuery();


			base.DataBind();
		}


		/// <summary>
		/// Builds the select command for the data source
		/// </summary>
		public void BuildQuery()
		{
			if (String.IsNullOrWhiteSpace(SelectCommand))
			{


				StringBuilder cond = new StringBuilder();

				PagesManager pMgr = new PagesManager();

				if (ParentId != null && !_getFromParent)
				{
					if (!Recursive)
						cond.AppendFormat(" And ParentId={0}", _parentId);
					else
					{
						string tempSql = string.Format("select * from dbo.Pages_GetDescendants({0})", _parentId);
						DataSet tempDs = DBUtils.GetDataSet(tempSql, cte.lib);

						string sep = "";

						StringBuilder tempSB = new StringBuilder();
						foreach (DataRow newsType in tempDs.Tables[0].Rows)
						{
							if (_recursiveLevel != null && _recursiveLevel != (int)newsType["Level"])
								continue;
							tempSB.Append(sep);
							tempSB.Append(newsType["PageId"].ToString());
							sep = ",";
						}

						string typeIds = tempSB.ToString();
						if (!String.IsNullOrWhiteSpace(typeIds))
						{
							cond.Append(string.Format(" And ParentId in ({0})", typeIds));
						}
					}
				}
				else
				{
					if (!String.IsNullOrWhiteSpace(ParentURL))
					{
						if (!Recursive)
							cond.AppendFormat(" And ParentId in (select PageId from Pages where URL=N'{0}' or Title=N'{0}')",
								StringUtils.SQLEncode(ParentURL));
						else
						{
							//TODO: fix database connection
							_parentId = pMgr.GetPage(ParentURL).PageId;

							string tempSql = string.Format("select * from dbo.Pages_GetDescendants({0})", _parentId);
							DataSet tempDs = DBUtils.GetDataSet(tempSql, cte.lib);

							string sep = "";

							StringBuilder tempSB = new StringBuilder();
							foreach (DataRow newsType in tempDs.Tables[0].Rows)
							{
								if (_recursiveLevel != null && _recursiveLevel != (int)newsType["Level"])
									continue;

								tempSB.Append(sep);
								tempSB.Append(newsType["PageId"].ToString());
								sep = ",";
							}

							string typeIds = tempSB.ToString();
							if (!String.IsNullOrWhiteSpace(typeIds))
							{
								cond.Append(string.Format(" And ParentId in ({0})", typeIds));
							}
						}
					}
				}
				if (month != null && month != "")
				{
					DateTime d = DateTime.Parse(month);
					DateTime d1 = d.AddMonths(1);

					cond.Append(string.Format(" And PublishDate Between '{0:M}' and '{1:M}'", d, d1));
				}

				if (!String.IsNullOrEmpty(Year))
				{
					cond.Append(string.Format(" And datepart(yyyy,  PublishDate) = '{0}'", Year));
				}


				if (!MyPage.Editable && !CMSMode.Value)
				{
					cond.AppendFormat(" and Status in ({0}, {1}, {2}, {3})",
								(byte)PageStatus.Published,
								(byte)PageStatus.Dynamic,
								(byte)PageStatus.Important,
								(byte)PageStatus.Menu);

					cond.Append(" And  (PublishDate is Null Or PublishDate <= getDate())");
				}
				else
				{
					cond.AppendFormat(" and Status not in ({0})",
								(byte)PageStatus.Deleted);
				}


				if (!String.IsNullOrWhiteSpace(_customCondition))
				{
					cond.Append(_customCondition);
				}

				string _source = WebContext.Request["source"];
				if (_source != null && _source.ToLower() == "categories")
				{
					string _typeId = WebContext.Request["TypeId"];
					if (!String.IsNullOrWhiteSpace(_typeId))
					{
						cond.Append(string.Format(" AND PageType={0}", StringUtils.SQLEncode(_typeId)));
					}
				}
				else if (_source != null && _source.ToLower() == "templates")
				{
					string templateId = WebContext.Request["TemplateId"];
					if (!String.IsNullOrWhiteSpace(templateId))
					{
						cond.Append(string.Format(" AND PageTemplate={0}", StringUtils.SQLEncode(templateId)));
					}
				}

				//string pId = WebContext.Request["ParentId"];
				//if (!String.IsNullOrWhiteSpace(pId))
				//{
				//	cond.Append(string.Format(" And ParentId={0}", StringUtils.SQLEncode(pId)));
				//}

				string _q = WebContext.Request["q"];

				if (!String.IsNullOrWhiteSpace(_q))
				{
					cond.Append(string.Format(" And (Title like N'%{0}%' or URL like N'%{0}%' or Header like N'%{0}%')", StringUtils.SQLEncode(_q)));
				}

				string _status = WebContext.Request["Status"];

				if (!String.IsNullOrWhiteSpace(_status))
				{
					cond.Append(string.Format(" And Status={0}", StringUtils.SQLEncode(_status)));
				}


				string sql = "";
				if (cond.Length > 0)
					sql = cond.ToString().Substring(5);

				string _max = "";
				if (Max != null)
					_max = string.Format(" Top {0}", Max);
				else if (_getPageProperties)
					_max = " Top 100 PERCENT";

				this.SelectCommand = string.Format("select {1}* from " + SQLSource + " where {0}", sql, _max);
			}

			if (!EnablePaging)
			{
				this.SelectCommand += " Order By " + this.OrderBy;
			}
			

			if (_getPageProperties)
			{
				string tempSql = "select * from PageDataPropertiesView where PageId in (select PageId from (" + this.SelectCommand + ") P)";
				DataTable properties = DBUtils.GetDataSet(tempSql, cte.lib).Tables[0];

				MyPage.AddContext(cte.PageProperties + "-" + this.ID, properties, true);
			}

		
			//WebContext.Response.Write(this.SelectCommand);
			//WebContext.Response.End();
		}

		/// <summary>
		/// Specifies the ordering of the data source
		/// </summary>
		public override string OrderBy
		{
			get
			{
				if (Sort != PagesSort.Custom)
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
		/// <summary>
		/// Adds a custom condition to the list of other conditions
		/// It's a direct SQL query to 
		/// </summary>
		public string CustomCondition
		{
			get { return _customCondition; }
			set { _customCondition = value; }
		}

		bool _getPageProperties = false;
		/// <summary>
		/// A flag to define if the page properties should be fetched for the pages in this query.
		/// </summary>
		public bool GetPageProperties
		{
			get
			{
				return _getPageProperties;
			}
			set
			{
				_getPageProperties = value;
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
		public string ParentURL
		{
			get
			{
				if (type == null)
				{
					type = MyPage.GetQueryValue("ParentURL");
				}
				return type;
			}
			set
			{
				_getFromParent = true;
				type = value;
			}
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


		public PagesSort Sort
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

		public int? ParentId
		{
			get
			{
				if (_parentId == null && MyPage != null)
				{
					try
					{
						object obj = DataBinder.Eval(this.NamingContainer, "DataItem.PageId");
						if (obj != null)
							_parentId = (int)obj;
						return _parentId;
					}
					catch (Exception Ex)
					{ }

					string tempId = MyPage.GetQueryValue("ParentId");

					if (String.IsNullOrWhiteSpace(tempId))
					{
						tempId = MyPage.GetQueryValue("Id");
					}
					
					if (!String.IsNullOrWhiteSpace(tempId))
						_parentId = Int32.Parse(tempId);
				}

				return _parentId;
			}
			set
			{
				_parentId = value;
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

		int? _recursiveLevel = null;
		/// <summary>
		/// Specifies the number of children level that must be fetched
		/// Note if set for more than 3 it will greatly impacts the performance
		/// </summary>
		public int? RecursiveLEvel
		{
			get
			{
				return _recursiveLevel;
			}
			set
			{
				_recursiveLevel = value;
			}
		}

		bool _getComments = false;
		/// <summary>
		/// Defines if the sql source should be fetched with comments.
		/// </summary>
		public bool GetComments
		{
			get
			{
				return _getComments;
			}
			set
			{
				_getComments = value;
			}
		}

		string _sqlSource = "Pages_View";
		/// <summary>
		/// Returns the SQL Source Table or View
		/// </summary>
		public string SQLSource
		{
			get
			{
				if (_getComments)
					_sqlSource = "Pages_View_Comments";
				else
					_sqlSource = "Pages_View";

				return _sqlSource;
			}
		}


		bool _autoDataBind = true;
		/// <summary>
		/// If set to false, you will have to manually call DataBind(true) otherwise the datasource won't fetch its data or render.
		/// </summary>
		public bool AutoDataBind
		{
			get
			{
				return _autoDataBind;
			}
			set
			{
				_autoDataBind = value;
			}
		}

		//string _sortByDataProperty = null;

		///// <summary>
		///// If defined it will return the pages sorted by the value of the specified dataproperty
		///// </summary>
		//public string SortByDataProperty
		//{
		//	get
		//	{
		//		return _sortByDataProperty;
		//	}
		//	set
		//	{
		//		_sortByDataProperty = value;
		//		if (!String.IsNullOrWhiteSpace(value))
		//			this.GetPageProperties = true;
		//	}
		//}

		#endregion
	}
}