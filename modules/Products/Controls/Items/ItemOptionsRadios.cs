using System.Web.UI;
using System.Web.UI.WebControls;



namespace lw.Products.Controls
{
	public class ItemOptionsRadios : RadioButtonList
	{
		bool _bound = false;
		string _headerText = "";

		public ItemOptionsRadios()
		{
		}
		public override string UniqueID
		{
			get
			{
				return "Options";
			}
		}
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.ItemId");

			if (obj != null)
			{
				int itemId = (int)obj;

				ChoicesMgr cMgr = new ChoicesMgr();

				Choices.ItemOptionsInventoryViewDataTable options = cMgr.GetItemOptionInventory(itemId);

				if (options.Count == 0)
					this.Visible = false;

				foreach (Choices.ItemOptionsInventoryViewRow row in options)
				{
					Items.Add(new ListItem(row.OptionNames, row.Key));
				}
			}

			base.DataBind();
		}

	}
	
}