using System.Web.UI;

using lw.Base;

namespace lw.ShoppingCart.Controls
{
	[System.ComponentModel.DefaultProperty("method"),
		ToolboxData("<{0}:Cart runat=server></{0}:Cart>")]
	[ParseChildren(ChildrenAsProperties = false)]
	public class ShoppingCart : System.Web.UI.WebControls.WebControl
	{
		bool _bound = false;
		int _itemId = -1;
		bool _showCart = false;

		public ShoppingCart()
			: base("form")
		{
			this.Attributes["method"] = "post";
			this.Attributes["Action"] = "ShoppingCartAdd.axd";
			this.Attributes["onsubmit"] = "return Shopping_Cart_Validate(this)";
		}
		public override void DataBind()
		{
			if (this._bound)
				return;
			this._bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.ItemId");
			if (obj != null)
			{
				_itemId = (int)obj;
			}

			CustomPage page = this.Page as CustomPage;
			page.RegisterHeaderScript("Shopping_Cart_Validate", @"
function Shopping_Cart_Validate(form){
	if(form.Options){
		if(form.Options.type == ""select-one""){
			if(form.Options.selectedIndex == 0){
				alert(""Please select Color and Size from the Options List!"");
				form.Options.focus();
				return false;
			}
		}
		else {
	if(form.Options.length > 0 && form.Options[0].type == ""radio""){
		var radios = form.Options;
		for(var i = 0; i < radios.length; i ++){
			if(radios[i].checked)
				return true;
		}
		alert(""Please select your desired Color and Size"");
		radios[0].focus();	
		return true;
	}
}
		return true;
	}
}", false);


			CustomHiddenField hf = new CustomHiddenField("ItemId", _itemId.ToString());
			this.Controls.AddAt(0, hf);
			base.DataBind();
		}
		protected override ControlCollection CreateControlCollection()
		{
			return base.CreateControlCollection();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		}
		protected int ItemId
		{
			get
			{
				return _itemId;
			}
			set
			{
				_itemId = value;
			}
		}
	}
}
