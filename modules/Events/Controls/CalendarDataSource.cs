using lw.CTE.Enum;
using lw.DataControls;
using lw.Networking;
using lw.Utils;
using lw.WebTools;
using System;


namespace lw.Events.Controls
{
	public class CalendarDataSource : CustomDataSource
	{
		bool _bound = false;
		string _top = "100 PERCENT";
		string _category = null;
		int? _categoryId = null;
		string _dateFrom = null;
		string _dateTo = null;
		Languages _language = Languages.Default;
		bool? _isPrevious = null;
		bool _networkBound = false;
		string _networkName = null;
		string _status = "";
		bool? _upComing = null;



		CalendarManager cMgr;


		public CalendarDataSource()
		{
			this.DataLibrary = cte.lib;
			this.OrderBy = "DateFrom ASC";
			cMgr = new CalendarManager();
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			this.SelectCommand = string.Format("Select top {0} * from CalendarView where 1=1", Top);


			if (CMSMode == null || CMSMode == false)
			{
				/* Reserved for use UTC
				if (isPrevious == true)
					this.SelectCommand += string.Format(" Where DATEADD(minute, DATEPART(minute, Time), DATEADD(hour, -DATEPART(hour, DateFrom), DATEADD(hour, DATEPART(hour, Time), DateFrom))) < GETUTCDATE()");
				else
					this.SelectCommand += string.Format(" Where DATEADD(minute, DATEPART(minute, Time), DATEADD(hour, -DATEPART(hour, DateFrom), DATEADD(hour, DATEPART(hour, Time), DateFrom))) > GETUTCDATE()");

				 */

				if (NetworkBound)
				{
					NetworkRelations networkRelations = new NetworkRelations();
					this.SelectCommand += string.Format(" and " + networkRelations.GetRelationQueryByNetwork(cte.NetworkRelationTable, cte.NetworkRelateToField, Int32.Parse(networkName)));
				}

				if (UpComing != null)
				{
					if (UpComing == false)
					{
						this.SelectCommand += string.Format(@" And 
DateFrom  <=
DATEADD(minute, -DATEPART(minute, GetDate()),
	DATEADD(hour, -DATEPART(hour, GetDate()), 
		DATEADD(second, -DATEPART(second, GetDate()), 
			DATEADD(millisecond, -DATEPART(millisecond, GetDate()), GetDate())
		)
	)
)");
					}
					else
					{
						this.SelectCommand += string.Format(" And DateTo >= '{0}'", DateTime.Today);
					}
				}


				if (IsPrevious != null)
				{
					if (_isPrevious == true)
					{
						this.SelectCommand += string.Format(@" And 
DateFrom  <
DATEADD(minute, -DATEPART(minute, GetDate()),
	DATEADD(hour, -DATEPART(hour, GetDate()), 
		DATEADD(second, -DATEPART(second, GetDate()), 
			DATEADD(millisecond, -DATEPART(millisecond, GetDate()), GetDate())
		)
	)
)");
					}
					else
					{
						this.SelectCommand += string.Format(@" And 
DateFrom  >=
DATEADD(minute, -DATEPART(minute, GetDate()),
	DATEADD(hour, -DATEPART(hour, GetDate()), 
		DATEADD(second, -DATEPART(second, GetDate()), 
			DATEADD(millisecond, -DATEPART(millisecond, GetDate()), GetDate())
		)
	)
)");
					}
				}
				string st = "in(0, 1)";
				if (Status == "true")
					st = "= 1";
				if (Status == "false")
					st = "= 0";

				this.SelectCommand += string.Format(" And Status {0}", st);

				if (_language != Languages.Default)
					this.SelectCommand += string.Format(" and Language={0}", (int)_language);

				if (CategoryId > 0)
					this.SelectCommand += string.Format(" And CategoryId&power(2, {0}) = power(2, {0})", CategoryId);

				if (DateFrom != null && DateFrom != "")
				{
					if (DateFrom == "Now")
						this.SelectCommand += string.Format(" And DateTo >= '{0}'", DateTime.Today);
					else
						this.SelectCommand += string.Format(" And DateTo >= '{0}'", DateTime.Parse(DateFrom));
				}

				if (DateTo != null)
				{
					this.SelectCommand += string.Format(" And DateFrom <= '{0}'", DateTime.Parse(DateTo));
				}


				if (!String.IsNullOrWhiteSpace(Filter))
				{
					this.SelectCommand += " and  " + Filter;
				}

				if (!EnablePaging)
				{
					this.SelectCommand += " Order By " + this.OrderBy;
				}
			}
			else
			{
				string str = "";

				System.Web.HttpRequest Request = WebContext.Request;

				if (Request["q"] != null && Request["q"] != "")
					str += string.Format(" and (Description like '%{0}%' or Title like '%{0}%')", lw.Utils.StringUtils.SQLEncode(Request["q"]));

				if (Request["Status"] != null && Request["Status"] != "")
					str += String.Format(" And Status={0}", Int32.Parse(Request["Status"]));

				if (Request["CalendarCategory"] != null && Request["CalendarCategory"] != "")
					str += String.Format(" And CategoryId&{0}={0}", Math.Pow(2, Int32.Parse(Request["CalendarCategory"])));

				if (Request["DateFrom"] != null && Request["DateFrom"] != "")
					str += string.Format(" And DateFrom>= '{0}'", Request["DateFrom"]);

				if (Request["DateTo"] != null && Request["DateTo"] != "")
					str += string.Format(" And DateTo < DateAdd(day, 1, '{0}')", lw.Utils.StringUtils.SQLEncode(Request["DateTo"]));

				string lan = Request["Language"];
				if (!String.IsNullOrEmpty(lan) && lan != "0")
					str += string.Format(" and Language={0}", Int32.Parse(Request["Language"]));

				str = str == "" ? str : " where " + str.Substring(5);


				this.SelectCommand += str;
			}

			base.DataBind();
		}

		public string Top
		{
			get { return _top; }
			set { _top = value; }
		}
		public string Category
		{
			get { return _category; }
			set { _category = value; }
		}
		public bool? IsPrevious
		{
			get { return _isPrevious; }
			set { _isPrevious = value; }
		}

		public bool? UpComing
		{
			get { return _upComing; }
			set { _upComing = value; }
		}
		public int? CategoryId
		{
			get { return _categoryId; }
			set { _categoryId = value; }
		}
		public Languages Language
		{
			get { return _language; }
			set { _language = value; }
		}
		public string DateFrom
		{
			get { return _dateFrom; }
			set { _dateFrom = value; }
		}
		public string DateTo
		{
			get { return _dateTo; }
			set { _dateTo = value; }
		}

		/// <summary>
		/// If left empty it will default to 'All', if set to true it will get status = 1 if set to false it will get status = 0
		/// </summary>
		public string Status
		{
			get { return _status; }
			set { _status = value; }
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

		/// <summary>
		/// Makes the list network bound returns only the items that are related to the current member's network
		/// </summary>
		public bool NetworkBound
		{
			get { return _networkBound; }
			set { _networkBound = value; }
		}


		#region Properties

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

	}
		#endregion
}