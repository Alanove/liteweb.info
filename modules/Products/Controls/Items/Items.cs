using System.Data;
using System.Web.UI;
using lw.DataControls;


namespace lw.Products.Controls
{
	public class Items : CustomRepeater
	{
		bool _bound = false;
		ItemsMgr pMgr = new ItemsMgr();
		string cond = "";
			
		public Items()
		{
		}
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			string search = "";

			int _categoryId = -1;
			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.CategoryId");
			if (obj != null)
				_categoryId = (int)obj;

			DataTable itemsView;

			if (Cond != "")
			{
				search += " And " + Cond;
			}

			search += string.Format(" and Status&{0}<>0", (int)ItemStatus.Enabled);

			if (_categoryId != -1)
				search += string.Format(" And CategoryId={0}", _categoryId);

			itemsView = pMgr.GetItemsView(search.Substring(5));

			this.DataSource = itemsView;


			this.Visible = itemsView.Rows.Count > 0;


			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			object obj = this.DataSource;
			if (obj != null && ((DataTable)obj).Rows.Count > 0)
				base.Render(writer);
		}
		public string Cond
		{
			get
			{
				return cond;
			}
			set
			{
				cond = value;
			}
		}
	}
}