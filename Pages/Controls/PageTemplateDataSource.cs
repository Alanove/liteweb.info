using lw.Base;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lw.Pages.Controls
{
    public class PageTemplateDataSource : CustomDataSource
    {
        bool _bound = false;
        string template = null;
        string filename = null;        
        int? _top = null;
        CustomPage _page;
        string _CustomSort = null;
        PageTemplateSort _Sort = PageTemplateSort.TitleAsc;

        public PageTemplateDataSource()
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

            if (Template != null)
                cond.Append(string.Format(" And Title='{0}'", Template));

            if (Filename != null)
                cond.Append(string.Format(" And Filename='{0}'", Filename));

            string sql = "";
            if (cond.Length > 0)
                sql = cond.ToString().Substring(5);

            string _top = "";
            if (Top != null)
                _top = string.Format(" Top {0}", Top);

            // Checks if we're logged in CMS or not to change query accordingly
            this.SelectCommand = string.Format("select {0} * from PageTemplates", _top);


            if (string.IsNullOrWhiteSpace(sql))
                sql = "1=1";

            if (CMSMode == null || !CMSMode.Value)
            {
                //this.SelectCommand += string.Format(" where {0} and status={1}", sql, (int)lw.CTE.Enum.Status.Enabled);
            }
            else
            {
                string _q = WebContext.Request["q"];
                string _template = WebContext.Request["TemplateId"];

                if (!String.IsNullOrWhiteSpace(_q))
                {
                    cond.Append(string.Format(" And (Title like '%{0}%'", StringUtils.SQLEncode(_q)));
                }

                if (!String.IsNullOrEmpty(_template))
                {
                    cond.Append(string.Format(" And TemplateId={0}", Int32.Parse(_template)));
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

        public PageTemplateSort Sort
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
                if (Sort != PageTemplateSort.Custom)
                {
                    return EnumHelper.GetDescription(Sort);
                }
                else
                {
                    return CustomSort;
                }
            }
        }

		public string Template
		{
			get
			{
				if (template == null)
				{
                    template = _page.GetQueryValue("PageTemplate");
				}
                return template;
			}
            set { template = value; }
		}

        public string Filename
        {
            get
            {
                if (filename == null)
                {
                    filename = _page.GetQueryValue("Filename");
                }
                return filename;
            }
            set { filename = value; }
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
