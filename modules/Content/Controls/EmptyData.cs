using System.Web.UI;


using lw.DataControls;

namespace lw.Content.Controls
{
	public class EmptyData : DisplayContainer
	{
		string _source = "";
		bool _bound = false;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			Display = false;

			if (_source != "")
			{
				Control ctrl = Page.FindControl(_source);
				if (ctrl != null)
				{
					CustomDataSource dataSrc = ctrl as CustomDataSource;
					if(ctrl != null)
						Display = !dataSrc.HasData;
				}
			}

			base.DataBind();
		}

		public string Source
		{
			get { return _source; }
			set { _source = value; }
		}
	}
}
