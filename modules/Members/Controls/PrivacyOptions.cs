using System;
using System.Web.UI.WebControls;

using lw.Utils;

namespace lw.Members.Controls
{
	public class PrivacyOptionsCheckList : System.Web.UI.WebControls.CheckBoxList
	{
		string _option = "";
		bool _bound = false;
		int _value = -1;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;


			foreach (Privacy stat in Enum.GetValues(typeof(Privacy)))
			{
				this.Items.Add(new ListItem(EnumHelper.GetDescription(stat), ((int)stat).ToString()));
			}

			base.DataBind();

		}
		protected override void OnLoad(EventArgs e)
		{
			////get from database
			if (this._value > 0)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					if ((_value & Int32.Parse(Items[i].Value)) != 0)
					{
						Items[i].Selected = true;
					}
				}
			}
			base.OnLoad(e);
		}

		#region Properties
		public int Value
		{
			get
			{
				int v = 0;
				for (int i = 0; i < Items.Count; i++)
				{
					if (this.Page.Request.Form[string.Format("{0}:{1}", this.UniqueID, i)] == "on")
					{
						Items[i].Selected = true;
						v |= Int32.Parse(Items[i].Value);
					}
				}
				return v;
			}
			set
			{
				_value = value;
			}
		}
		public string Option
		{
			get
			{
				return _option;
			}
			set
			{
				_option = value;
			}
		}
		#endregion
	}
}