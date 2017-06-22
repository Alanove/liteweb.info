using System;
using System.Data;
using System.Text;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Articles.Controls
{
	public class NewsList : CustomRepeater
	{
		bool _bound = false;
		NewsTagStatus status = NewsTagStatus.Inherit;
		bool _listAll = false;
		int max = -1;
		string type = "";
		string month = "";
		Languages _Language = Languages.Default;

		public NewsList()
		{
		}
		public override void DataBind()
		{
			if (_bound)
				return;

			StringBuilder cond = new StringBuilder();

			NewsManager nMgr = new NewsManager();

			if (Type != "")
			{
				DataView types = nMgr.GetNewsTypes(string.Format("Name='{0}' or UniqueName='{0}'", StringUtils.SQLEncode(Type)));
				if (types.Count > 0)
					cond.Append(string.Format(" And NewsType={0}", types[0]["TypeId"]));
			}
			else
			{
				try
				{
					object obj = DataBinder.Eval(this.NamingContainer, "DataItem.NewsType");
					if (obj != null)
					{
						_bound = true;
						cond.Append(string.Format(" And NewsType={0}", obj));
					}
				}
				catch
				{
				}
			}

			if (month != null && month != "")
			{
				DateTime d = DateTime.Parse(month);
				DateTime d1 = d.AddMonths(1);

				cond.Append(string.Format(" And NewsDate Between '{0:d}' and '{1:d}'", d, d1));
			}

			if (!ListAll)
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
			cond.Append(string.Format(" And NewsLanguage={0}", (int)Language));

			//cond.Append(" order by Ranking Desc, DateAdded Desc");

			string sql = "";
			if (cond.Length > 0)
				sql = cond.ToString().Substring(5);

			

			DataView news = nMgr.GetNewsViewFromQuery(max, sql);

			this.DataSource = news;

			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			object obj = this.DataSource;
			//if (obj != null && ((DataView)obj).Count > 0)
				base.Render(writer);
		}
		
		public bool ListAll
		{
			get
			{
				return _listAll;
			}
			set
			{
				_listAll = value;
			}
		}
		public NewsTagStatus Status
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
		public int Max
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
		public string Type
		{
			get
			{
				if (String.IsNullOrEmpty(type))
				{
					object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "UniqueName");
					if (obj != null)
						type = obj.ToString();
				}
				return type;
			}
			set
			{
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

	}
}