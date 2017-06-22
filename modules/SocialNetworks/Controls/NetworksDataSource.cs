using System;
using System.Data;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Networking;
using lw.Utils;
using lw.WebTools;

namespace lw.Networking.Controls
{
	public class NetworksDataSource : CustomDataSource
	{
		lw.Base.CustomPage _page;
		bool _bound = false;
		string _condition = "";
		string _orderBy = "";
		string _parentUniqueName = "";
		bool _networkBound = true;
		Status? status = null;
		string _top = "100 PERCENT";

		public NetworksDataSource()
		{
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			_page = this.Page as lw.Base.CustomPage;


			this.SelectCommand = "select Top " + Top + " * from " + TableName;
			string cond = "";
			if (Status != null)
			{
				cond = " where Status = " + (int)Status;
			}
			else
			{
				cond = " where 1=1 ";
			}

			if (!string.IsNullOrEmpty(Condition))
			{
				cond += string.Format(" and {0}", Condition);
			}

			if (MemberId != null && String.Compare(WebContext.Profile.dbUserName, Config.GetFromWebConfig("Admin"), true) != 0 && NetworkBound)
			{
				cond += " and NetworkId in (select NetworkId from MemberNetworks where MemberId={0}";
				if (onlyPreferred != null && onlyPreferred.Value)
				{
					cond += " and Prefered=1";
				}
				cond += ")";
				cond = string.Format(cond, memberId);
			}

			if (ParentId != null)
			{
				cond += " and ParentId=" + _parentId.Value.ToString();
			}

			if(!string.IsNullOrWhiteSpace(ParentUniqueName) && (ParentId == null)){
				NetworksManager nMgr = new NetworksManager();
				var parentName = nMgr.GetNetwork(ParentUniqueName);
				int? pId = null;
				if(parentName!=null){
					pId = parentName.NetworkId;
				}
				if (pId != null)
					cond += " and ParentId = " + pId;
			}

			if (CMSMode != null && CMSMode.Value)
			{
				string q = WebContext.Request["q"];
				string netDateFrom = WebContext.Request["DateFrom"];
				string netDateTo = WebContext.Request["DateTo"];

				if(!string.IsNullOrWhiteSpace(q))
					cond += string.Format(" and (Name like '%{0}%' or UniqueName like '%{0}%')", StringUtils.SQLEncode(q));

					if (!string.IsNullOrWhiteSpace(netDateFrom) && !string.IsNullOrWhiteSpace(netDateTo))
						cond += string.Format(" and DateCreated between '{0}' and '{1}'", DateTime.Parse(netDateFrom), DateTime.Parse(netDateTo));
					else if (!string.IsNullOrWhiteSpace(netDateFrom))
						cond += string.Format(" and DateCreated >= '{0}'", DateTime.Parse(netDateFrom));
					else if (!string.IsNullOrWhiteSpace(netDateTo))
						cond += string.Format(" and DateCreated <= '{0}'", DateTime.Parse(netDateTo));
			}

			if (!string.IsNullOrWhiteSpace(OrderBy))
			{
				cond += " Order By " + OrderBy;
			}

			this.SelectCommand += cond;

			base.DataBind();
		}


#region Properties

		int? memberId;
		public int? MemberId
		{
			get
			{
				if (memberId == null)
				{
					if (WebContext.Profile.UserLogged)
					{
						memberId = WebContext.Profile.UserId;
					}
				}
				return memberId;
			}
			set
			{
				memberId = value;
			}

		}

		public string Top
		{
			get { return _top; }
			set { _top = value; }
		}
		
		bool? onlyPreferred;
		/// <summary>
		/// Determines if the source should display only the prefered networks for the logged in member
		/// </summary>
		public bool? OnlyPreferred
		{
			get
			{
				return onlyPreferred;
			}
			set
			{
				onlyPreferred = value;
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

		public string Condition
		{
			get
			{
				return _condition;
			}
			set
			{
				_condition = value;
			}
		}
		public bool NetworkBound
		{
			get { return _networkBound; }
			set { _networkBound = value; }
		}

		string _tableName = "Networks";

		/// <summary>
		/// the table name where this datasource should read from
		/// can be any table or view 
		/// default: Networks
		/// </summary>
		public string TableName
		{
			get
			{
				return _tableName;
			}
			set
			{
				_tableName = value;
			}
		}


		/// <summary>
		/// the table name where this datasource should read from
		/// can be any table or view 
		/// default: Networks
		/// </summary>
		public string OrderBy
		{
			get
			{
				return _orderBy;
			}
			set
			{
				_orderBy = value;
			}
		}

		public Status? Status
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

		public string ParentUniqueName
		{
			get
			{
				return _parentUniqueName;
			}
			set
			{
				_parentUniqueName = value;
			}
		}

		int? _parentId = null;
		public int? ParentId
		{
			get
			{
				if (_parentId == null)
				{
					try
					{
						object parent = DataBinder.Eval(this.NamingContainer, "DataItem.NetworkId");
						if (parent != null)
							_parentId = (int)parent;
					}
					catch
					{
					}
				}
				return _parentId;
			}
			set {
				_parentId = value;
			}
		}
	}
#endregion
}