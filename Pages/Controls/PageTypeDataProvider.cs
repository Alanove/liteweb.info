using lw.CTE;
using lw.DataControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lw.Pages.Controls
{
    public class PageTypeDataProvider : DataProvider
    {
        bool _bound = false;

        public override void DataBind()
        {
            if (_bound)
                return;
            _bound = true;

            PagesManager pMgr = new PagesManager();

            if (TypeId != null)
            {
                this.DataItem = pMgr.GetPageType(TypeId.Value);
            }

            base.DataBind();
        }


        #region Properties
        int? typeId;
        public int? TypeId
        {
            get
            {
                if (typeId == null)
                {
                    string obj = MyPage.GetQueryValue(RoutingParameters.PageTypeId);
                    if (string.IsNullOrWhiteSpace(obj))
                    {
                        obj = MyPage.GetQueryValue("Id");
                    }
                    if (!string.IsNullOrWhiteSpace(obj))
                        typeId = int.Parse(obj);
                }
                return typeId;
            }
            set
            {
                typeId = value;
            }
        }
        #endregion
    }
}
