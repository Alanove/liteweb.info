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
    public class PageTypeDataSource : CustomDataSource
    {
        bool _bound = false;
        string category = null;
        string thumbSize = null;
        string mediumSize = null;
        string largeSize = null;
        int? _top = null;
        CustomPage _page;
        string _CustomSort = null;
        PageTypeSort _Sort = PageTypeSort.TypeAsc;

        public PageTypeDataSource()
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
                cond.Append(string.Format(" And Type='{0}'", Category));

            if (ThumbSize != null)
                cond.Append(string.Format(" And ThumbSize='{0}'", ThumbSize));

            if (MediumSize != null)
                cond.Append(string.Format(" And MediumSize='{0}'", MediumSize));

            if (LargeSize != null)
                cond.Append(string.Format(" And LargeSize='{0}'", LargeSize));

            string sql = "";
            if (cond.Length > 0)
                sql = cond.ToString().Substring(5);

            string _top = "";
            if (Top != null)
                _top = string.Format(" Top {0}", Top);

            // Checks if we're logged in CMS or not to change query accordingly
            this.SelectCommand = string.Format("select {0} * from PageTypes", _top);


            if (string.IsNullOrWhiteSpace(sql))
                sql = "1=1";

            if (CMSMode == null || !CMSMode.Value)
            {
                this.SelectCommand += string.Format(" where {0}", sql);
            }
            else
            {
                string _q = WebContext.Request["q"];
                string _type = WebContext.Request["TypeId"];

                if (!String.IsNullOrWhiteSpace(_q))
                {
                    cond.Append(string.Format(" And (Type like '%{0}%'", StringUtils.SQLEncode(_q)));
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

        public PageTypeSort Sort
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
                if (Sort != PageTypeSort.Custom)
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
					category = _page.GetQueryValue("PageType");
				}
				return category;
			}
			set { category = value; }
		}

        public string ThumbSize
        {
            get
            {
                if (thumbSize == null)
                {
                    thumbSize = _page.GetQueryValue("ThumbSize");
                }
                return thumbSize;
            }
            set { thumbSize = value; }
        }

        public string MediumSize
        {
            get
            {
                if (mediumSize == null)
                {
                    mediumSize = _page.GetQueryValue("MediumSize");
                }
                return mediumSize;
            }
            set { mediumSize = value; }
        }

        public string LargeSize
        {
            get
            {
                if (largeSize == null)
                {
                    largeSize = _page.GetQueryValue("LargeSize");
                }
                return largeSize;
            }
            set { largeSize = value; }
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
