using lw.CTE;
using lw.DataControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lw.Pages.Controls
{
    public class PageTemplateDataProvider : DataProvider
    {
        bool _bound = false;

        public override void DataBind()
        {
            if (_bound)
                return;
            _bound = true;

            PagesManager pMgr = new PagesManager();

            if (TemplateId != null)
            {
                this.DataItem = pMgr.GetPageTemplate(TemplateId.Value);
            }

            base.DataBind();
        }


        #region Properties
        int? templateId;
        public int? TemplateId
        {
            get
            {
                if (templateId == null)
                {
                    string obj = MyPage.GetQueryValue(RoutingParameters.PageTemplateId);
                    if (string.IsNullOrWhiteSpace(obj))
                    {
                        obj = MyPage.GetQueryValue("Id");
                    }
                    if (!string.IsNullOrWhiteSpace(obj))
                        templateId = int.Parse(obj);
                }
                return templateId;
            }
            set
            {
                templateId = value;
            }
        }
        #endregion
    }
}
