using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;



namespace lw.Products.Controls
{
	public class ItemOptions : HtmlSelect
	{
		bool _bound = false;
		string _headerText = "";

		public ItemOptions()
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

				this.Items.Add(new ListItem(HeaderText, ""));

				foreach (Choices.ItemOptionsInventoryViewRow row in options)
				{
					Items.Add(new ListItem(row.OptionNames, row.Key));
				}
			}

			base.DataBind();
		}

		public string HeaderText
		{
			get { return _headerText; }
			set { _headerText = value; }
		}
	}
}