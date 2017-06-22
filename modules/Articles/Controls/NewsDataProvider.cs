using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lw.CTE;
using lw.DataControls;
using lw.Articles;

namespace lw.Articles.Controls
{
    public class newsDataProvider : DataProvider
    {
        bool _bound = false;

        public override void DataBind()
        {
            if (_bound)
                return;
            _bound = true;

            NewsManager nMgr = new NewsManager();

            if (NewsId != null)
            {
                this.DataItem = nMgr.GetNewsDetails(NewsId.Value);
            }

            base.DataBind();
        }


        #region Properties
        int? newsId;
        public int? NewsId
        {
            get
            {
                if (newsId == null)
                {
                    string obj = MyPage.GetQueryValue(RoutingParameters.NewsId);
                    if (string.IsNullOrWhiteSpace(obj))
                    {
                        obj = MyPage.GetQueryValue("Id");
                    }
                    if (!string.IsNullOrWhiteSpace(obj))
                        newsId = int.Parse(obj);
                }
                return newsId;
            }
            set
            {
                newsId = value;
            }
        }
        #endregion
    }
}
