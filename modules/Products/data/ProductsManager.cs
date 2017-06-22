using System.Data;
using System.Web.UI;

using lw.Data;

namespace lw.Products
{
	/// <summary>
	/// Summary description for ProductsManager.
	/// </summary>

	public class ProductsManager:DirectorBase
	{
		ItemsMgr _itemsMgr;
		public ProductsManager():base(cte.lib)
		{
			_itemsMgr = new ItemsMgr();
		}
		

		

		#region Data Sources

		public object ItemsSrc(System.Web.UI.Control ctrl)
		{
			return this.ItemsSrc(ctrl, "");
		}
		public object ItemsSrc(Control ctrl, string cond)
		{
			string search = "";

			int _categoryId = -1;
			try
			{
				object obj = DataBinder.Eval(ctrl.NamingContainer, "DataItem.CategoryId");
				if (obj != null)
					_categoryId = (int)obj;
			}
			catch
			{
			}

			DataTable itemsView;

			if (cond != "")
			{
				search += " And " + cond;
			}
			if (_categoryId != -1)
			{
				search += string.Format(" And CategoryId={0}", _categoryId);
				itemsView = _itemsMgr.GetItemsView(search.Substring(5));
			}
			else
				itemsView = _itemsMgr.GetItems(search.Substring(5));

			return itemsView;
		}
		#endregion
	}
}
