using System.Web.UI;

using lw.DataControls;


namespace lw.Products.Controls
{
	public class PartOfPackagesSource : CustomDataSource
	{
		bool bound = false;
		public PartOfPackagesSource()
		{
			this.DataLibrary = lw.Products.cte.lib;
		}
		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			int itemId = -1;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.ItemId");
			if (obj != null)
				itemId = (int)obj;

			if (itemId > 0)
				this.SelectCommand = @"select *  from PackageParentListView where ChildItem = " + itemId.ToString() + " order by Sort asc"; 

			base.DataBind();
		}

	}
	
}