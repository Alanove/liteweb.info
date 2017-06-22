using System.Data;


namespace lw.Products.Controls
{
	public class CategoryList : System.Web.UI.HtmlControls.HtmlSelect
	{
		string category = "", selId = "";
		bool display = false;

		bool bound = false;

		public CategoryList()
		{
		}

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			DataView source = new DataView();
			CategoriesMgr pMgr = new CategoriesMgr();
			int catId = -1;
			if (category != "")
			{
				DataView cats = pMgr.GetCategories(string.Format("name='{0}'", category)).DefaultView;
				if (cats.Count > 0)
				{
					catId = (int)cats[0]["CategoryId"];
					source = pMgr.GetChildrenCategories(catId).DefaultView;
					source.Sort = "SortingOrder";
				}
			}



			this.DataSource = source;
			this.DataTextField = "Title";
			this.DataValueField = "CategoryId";

			this.Visible = source.Count > 0;

			base.DataBind();
		}

		public override string UniqueID
		{
			get
			{
				if (selId != "")
					return selId;
				return base.UniqueID;
			}
		}

		public string Category
		{
			get
			{
				return category;
			}
			set
			{
				category = value;
			}
		}
		public string SelId
		{
			get
			{
				return selId;
			}
			set
			{
				selId = value;
			}
		}
	}
}