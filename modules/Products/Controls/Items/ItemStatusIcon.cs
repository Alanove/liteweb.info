using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.Utils;
using lw.WebTools;


namespace lw.Products.Controls
{
	public class ItemStatusIcon : HtmlImage
	{
		bool _bound = false;
		string _class = "status-icon";
		ItemStatus icon = ItemStatus.None;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Status");
			if(obj == null)
			{
				this.Visible = false;
				return;
			}

			string title = DataBinder.Eval(this.NamingContainer, "DataItem.Title").ToString();
			this.Attributes["class"] = Class;

			int stat = (int)obj;

			string _stat = WebContext.Request.QueryString["status"];
			if (!String.IsNullOrEmpty(_stat))
				icon = (ItemStatus)Enum.Parse(typeof(ItemStatus), _stat);

			if (icon == ItemStatus.None)
			{
				if (Tools.CheckStatus(ItemStatus.BestPrice, stat))
				{
					this.Src = WebContext.Root + "/images/icons/" + EnumHelper.GetDescription(ItemStatus.BestPrice);
					this.Alt = "Best Price!";
					return;
				}
				if (Tools.CheckStatus(ItemStatus.Package, stat))
				{
					this.Src = WebContext.Root + "/images/icons/" + EnumHelper.GetDescription(ItemStatus.Package);
					this.Alt = "Best Price!";
					return;
				}
				if (Tools.CheckStatus(ItemStatus.OnSale, stat))
				{
					this.Src = WebContext.Root + "/images/icons/" + EnumHelper.GetDescription(ItemStatus.OnSale);
					this.Alt = title + " is On Sale!";
					return;
				}
				if (Tools.CheckStatus(ItemStatus.New, stat))
				{
					this.Src = WebContext.Root + "/images/icons/" + EnumHelper.GetDescription(ItemStatus.New);
					this.Alt = "New Item!";
					return;
				}
				if (Tools.CheckStatus(ItemStatus.Exclusive, stat))
				{
					this.Src = WebContext.Root + "/images/icons/" + EnumHelper.GetDescription(ItemStatus.Exclusive);
					this.Alt = title + " is found exclusively at " + (new Config()).GetKey("SiteName") + "!";
					return;
				}
				
			}
			else
			{
				if (Tools.CheckStatus(icon, stat))
				{
					this.Src = WebContext.Root + "/images/icons/" + EnumHelper.GetDescription(icon);
					if (icon == ItemStatus.FreeShipping)
					{
						this.Alt = "This Item Ships for Free!";
					}
					return;
				}
			}
			this.Visible = false;
			base.DataBind();
		}
		public ItemStatus Icon
		{
			get { return icon; }
			set { icon = value; }
		}
		public string Class
		{
			get{return _class;}
			set { _class = value; }
		}
	}
}