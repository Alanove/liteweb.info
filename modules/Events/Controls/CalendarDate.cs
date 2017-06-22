using System;
using System.Data;
using System.Web.UI;



namespace lw.Events.Controls
{
	public class CalendarDate : System.Web.UI.WebControls.Label
	{
		bool _bound = false;
		string _format = "{0}";
		string _format2 = "{1}";

		public CalendarDate()
		{
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;			

			int _itemId = -1;
			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Id");

			if (!String.IsNullOrEmpty(obj.ToString()))
				_itemId = (int)obj;
			else
				return;

			CalendarManager cMgr = new CalendarManager();
			DataRow _event;

			_event = cMgr.GetEventDetails(_itemId);

			DateTime datefrom = (DateTime)_event["DateFrom"];
			DateTime? dateto = null;
			if (_event["DateTo"] != System.DBNull.Value)
			{
				try
				{
					dateto = (DateTime)_event["DateTo"];
				}
				catch
				{

				}
			}
			if(datefrom.CompareTo(dateto) == 0)
				this.Text = string.Format(Format, datefrom);
			else
				this.Text = string.Format(Format + Format2, datefrom, dateto);

			base.DataBind();
		}

		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}
		public string Format2
		{
			get
			{
				return _format2;
			}
			set
			{
				_format2 = value;
			}
		}
	}
}
