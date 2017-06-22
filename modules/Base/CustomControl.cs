

using System;
namespace lw.Base
{
	public class CustomControl : System.Web.UI.UserControl
	{
		string _id = "";
		public string _ID
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}
		public override string UniqueID
		{
			get
			{
				if (_id != "")
					return _id;
				return base.UniqueID;
			}
		}

		CustomPage _page;
		public CustomPage CustomPage
		{
			get
			{
				if (_page == null)
				{
					_page = this.Page as CustomPage;
					if (_page == null)
						throw new Exception("A custom user control, must be put inside a lw custom page.");
				}
				return _page;
			}
		}



		object _data;
		public object Data
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}
	}
}
