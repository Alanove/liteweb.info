using System;
using System.Text;
using lw.Base;
using lw.DataControls;
using lw.WebTools;

using lw.Utils;

namespace lw.Articles.Controls
{
	public class NewsTypesDS : CustomDataSource
	{
		bool _bound = false;
		string category = null;
		int? _top = null;
		CustomPage _page;
		string uniqueName = null;
		string _CustomSort = null;
		TypesSort _Sort = TypesSort.Ranking;

		public NewsTypesDS()
		{
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			_page = this.Page as CustomPage;

			BuildQuery();


			base.DataBind();
		}

		public void BuildQuery()
		{
			StringBuilder cond = new StringBuilder();

			if (Category != null)
				cond.Append(string.Format(" And ParentUniqueName='{0}'", Category));

			if(UniqueName!=null)
				cond.Append(string.Format(" And UniqueName='{0}'", UniqueName));

			string sql = "";
			if (cond.Length > 0)
				sql = cond.ToString().Substring(5);

			string _top = "";
			if (Top != null)
				_top = string.Format(" Top {0}", Top);

            // Checks if we're logged in CMS or not to change query accordingly
			this.SelectCommand = string.Format("select {0} * from NewsTypesView", _top);


			if (string.IsNullOrWhiteSpace(sql))
				sql = "1=1";

			if (CMSMode == null || !CMSMode.Value)
			{
				this.SelectCommand += string.Format(" where {0} and status={1}", sql, (int)lw.CTE.Enum.Status.Enabled);
			}
			else
			{
				string _q = WebContext.Request["q"];
				string _type = WebContext.Request["TypeId"];

				if (!String.IsNullOrWhiteSpace(_q))
				{
					cond.Append(string.Format(" And (Name like '%{0}%' or UniqueName  like '%{0}%')", StringUtils.SQLEncode(_q)));
				}

				if (!String.IsNullOrEmpty(_type))
				{
					cond.Append(string.Format(" And TypeId={0}", Int32.Parse(_type)));
				}
				if (cond.Length > 0)
				{
					sql = cond.ToString().Substring(5);
					this.SelectCommand += string.Format(" where {0}", sql);
				}
			}
			this.SelectCommand += " Order By " + this.OrderBy;

			//WebContext.Response.Write(this.SelectCommand);
			//WebContext.Response.End();
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

		public TypesSort Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}

		public string CustomSort
		{
			get { return _CustomSort; }
			set { _CustomSort = value; }
		}

		public override string OrderBy
		{
			get
			{
				if (Sort != TypesSort.Custom)
				{
					return EnumHelper.GetDescription(Sort);
				}
				else
				{
					return CustomSort;
				}
			}
		}

		public string Category
		{
			get
			{
				if (category == null)
				{
					category = _page.GetQueryValue("NewsType");
				}
				return category;
			}
			set { category = value; }
		}

		public string UniqueName
		{
			get
			{
				return uniqueName;
			}
			set { uniqueName = value; }
		}
		public Int32? Top
		{
			get
			{
				return _top;
			}
			set
			{
				_top = value;
			}
		}
		#endregion
	}
}