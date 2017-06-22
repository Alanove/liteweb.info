using System;
using System.Data;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Widgets;
using lw.Utils;
using lw.WebTools;
using lw.CTE;
using System.Text;

namespace lw.Widgets.Controls
{
	public class MediaDataSource : CustomDataSource
	{
		bool _bound = false;
		int? _parentId = null;
		bool _getFromParent = false;
		string _condition = "";
		string _top = "";
		bool _status = true;
		string _orderBy = "DateAdded DESC";
		DefaultMediaTypes _type = DefaultMediaTypes.Image;

		MediaManager mMgr;

		public MediaDataSource()
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

		public void BuildQuery()
		{
			if (ParentId != null)
				this.SelectCommand = string.Format("Select * From Media Where Id in (Select MediaId From MediaWidgets Where WidgetId = {0})", ParentId);
			else
				this.SelectCommand = string.Format("Select * From Media Where Type = {0} Order By {1}", (int)Type, OrderBy);
		}

		public int? ParentId
		{
			get
			{
				if (_parentId == null && MyPage != null)
				{
					try
					{
						var p = this.Parent as CustomDataSource;
						if (p != null)
						{
							DataTable dt = p.Data as DataTable;

							_parentId = Int32.Parse(dt.Rows[0]["Id"].ToString());
						}
						else
						{
							object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Id");
							if (obj != null)
								_parentId = (int)obj;
						}
						return _parentId;
					}
					catch (Exception Ex)
					{ }

					string tempId = MyPage.GetQueryValue("ParentId");

					if (String.IsNullOrWhiteSpace(tempId))
					{
						tempId = MyPage.GetQueryValue("Id");
					}
				}
				return _parentId;

			}
			set
			{
				_parentId = value;
			}
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
		
		public bool Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public string OrderBy
		{
			get { return _orderBy; }
			set { _orderBy = value; }
		}

		public DefaultMediaTypes Type
		{
			get
			{ return _type; }
			set
			{ _type = value; }
		}
	}
}