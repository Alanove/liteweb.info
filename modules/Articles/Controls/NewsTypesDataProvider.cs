using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lw.CTE;
using lw.DataControls;

namespace lw.Articles.Controls
{
    public class NewsTypesDataProvider : DataProvider
    {
        bool _bound = false;

        public override void DataBind()
        {
            if (_bound)
                return;
            _bound = true;

            NewsManager nMgr = new NewsManager();

            if (TypeId != null)
            {
                this.DataItem = nMgr.GetNewsType(TypeId.Value);
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
                    string obj = MyPage.GetQueryValue(RoutingParameters.NewsTypeId);
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
