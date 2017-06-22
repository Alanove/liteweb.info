using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;	

using lw.CTE;
using lw.DataControls;
using lw.Events;

namespace lw.Events.Controls
{
    public class calendarDataProvider : DataProvider
    {
        bool _bound = false;

        public override void DataBind()
        {
            if (_bound)
                return;
            _bound = true;

            CalendarManager nMgr = new CalendarManager();

            if (CalendarId != null)
            {
                this.DataItem = nMgr.GetEventDetails(CalendarId.Value);
            }

            base.DataBind();
        }


        #region Properties
        int? calendarId;
        public int? CalendarId
        {
            get
            {
                if (calendarId == null)
                {
                    string obj = MyPage.GetQueryValue(RoutingParameters.CalendarId);
                    if (string.IsNullOrWhiteSpace(obj))
                    {
						obj = MyPage.GetQueryValue("CalendarId");
                    }
                    if (!string.IsNullOrWhiteSpace(obj))
                        calendarId = int.Parse(obj);
                }
                return calendarId;
            }
            set
            {
                calendarId = value;
            }
        }
        #endregion
    }
}
