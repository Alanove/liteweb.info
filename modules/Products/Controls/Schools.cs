using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using lw.WebTools;

namespace lw.Products.Controls
{
	public class SchoolCategories : Repeater
	{
		bool bound = false;
		string _Filter = "";
		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			BrandsMgr pMgr = new BrandsMgr();
			DataView regions = pMgr.GetBrands(Filter).DefaultView;
			regions.Sort = "Title";
			this.DataSource = regions;

			base.DataBind();
		}
		public string Filter
		{
			get
			{
				return _Filter;
			}
			set
			{
				_Filter = value;
			}
		}
	}
	public class Schools : Repeater
	{
		bool bound = false;
		string region = "";

		public Schools()
		{
		}
		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			BrandsMgr pMgr = new BrandsMgr();

			string cond = "";
			Object obj = null;
			try
			{
				obj = DataBinder.Eval(this.NamingContainer, "DataItem.BrandId");
			}
			catch
			{
			}
			if (obj == null)
			{
				if (region != "")
				{
					DataView regions = pMgr.GetBrands("Title='" + lw.Utils.StringUtils.SQLEncode(region) + "'").DefaultView;
					if (regions.Count > 0)
						cond = string.Format("BrandId={0}", regions[0]["BrandId"]);
				}
			}
			else
				cond = string.Format("BrandId={0}", obj);

			string sql = "select * from Items where " + cond;
			DataView schools = lw.Data.DBUtils.GetDataSet(sql, lw.Products.cte.lib).Tables[0].DefaultView;
			schools.Sort = "Title, ProductNumber";
			this.DataSource = schools;

			base.DataBind();
		}
		public string Region
		{
			get
			{
				return region;
			}
			set
			{
				region = value;
			}
		}
	}
	public class SchoolLink : HyperLink
	{
		bool bound;

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Website");

			if (obj != null && obj != System.DBNull.Value)
			{
				string link = obj.ToString();
				if(link.IndexOf("no website") < 0)
					this.NavigateUrl = link;
			}

			//obj = DataBinder.Eval(this.NamingContainer, "DataItem.Title");
			//this.Text = obj.ToString();

			this.ToolTip = this.NavigateUrl;
			this.Target = "_blank";

			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!bound)
				DataBind();
			base.Render(writer);
		}
	}

	public class SchoolsCombobox : System.Web.UI.HtmlControls.HtmlSelect
	{
		bool bound = false;
		string query = "";
		StringBuilder result;


		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			BrandsMgr pMgr = new BrandsMgr();

			DataView regions = pMgr.GetBrands("").DefaultView;

			result = new StringBuilder();

			result.Append(@"<script type=""text/javascript"">
function Sabis_OpenWebsite(sel){
	if(sel.value != """")
		window.open(sel.value);
}
</script>");
			result.Append("<select ");

			foreach (string attributeKey in this.Attributes.Keys)
			{
				result.Append(string.Format(@" {0}=""{1}""", attributeKey, Attributes[attributeKey]));
			}
			result.Append(" onchange=\"Sabis_OpenWebsite(this)\"");
			result.Append(">");

			result.Append("<option value=\"\">Click to Select School Website</option>");

			foreach (DataRowView region in regions)
			{
				string cond = "BrandId=" + region["BrandId"].ToString();
				string sql = "select * from Items where " + cond;
				DataView schools = lw.Data.DBUtils.GetDataSet(sql, lw.Products.cte.lib).Tables[0].DefaultView;
				schools.Sort = "Title, ProductNumber";


				if (schools.Count > 0)
				{
					result.Append(string.Format(@"<optgroup title=""{0}"" class=""{1}"" label=""{0}"">",
						region["Title"], region["Title"].ToString().Replace(" ", "-")));


					foreach (DataRowView school in schools)
					{
						string text = school["Title"].ToString();

						if (school["ProductNumber"] != System.DBNull.Value && school["ProductNumber"].ToString() != "")
							text += ", " + school["ProductNumber"].ToString();

						string website = school["Website"].ToString();

						if (website == "" || website.IndexOf("no website") >= 0)
						{
							text += " (no website)";
							website = "";
						}
						result.Append(string.Format(@"<option value=""{0}"">{1}</option>",
							website, text));
					}
					result.Append("</optgroup>");
				}
			}
			result.Append("</select>");
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(result.ToString());
		}
		protected string Query
		{
			get
			{
				return query;
			}
			set
			{
				query = value;
			}
		}
	}

	public class SchoolsComboboxSP : System.Web.UI.HtmlControls.HtmlSelect
	{
		bool bound = false;
		string query = "";
		StringBuilder result;


		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			BrandsMgr pMgr = new BrandsMgr();

			DataView regions = pMgr.GetBrands("").DefaultView;

			result = new StringBuilder();

			result.Append(@"<script type=""text/javascript"">
function Sabis_OpenWebsite(sel){
	if(sel.value != """")
		window.open(sel.value);
}
</script>");
			result.Append("<select ");

			foreach (string attributeKey in this.Attributes.Keys)
			{
				result.Append(string.Format(@" {0}=""{1}""", attributeKey, Attributes[attributeKey]));
			}
			result.Append(" onchange=\"Sabis_OpenWebsite(this)\"");
			result.Append(">");

			result.Append("<option value=\"\">Haga clic para seleccionar el sitio de red de un colegio</option>");

			foreach (DataRowView region in regions)
			{
				string cond = "BrandId=" + region["BrandId"].ToString();
				string sql = "select * from Items where " + cond;
				DataView schools = lw.Data.DBUtils.GetDataSet(sql, lw.Products.cte.lib).Tables[0].DefaultView;
				schools.Sort = "Title, ProductNumber";


				if (schools.Count > 0)
				{
					result.Append(string.Format(@"<optgroup title=""{0}"" class=""{1}"" label=""{0}"">",
						region["Title"], region["Title"].ToString().Replace(" ", "-")));


					foreach (DataRowView school in schools)
					{
						string text = school["Title"].ToString();

						if (school["ProductNumber"] != System.DBNull.Value && school["ProductNumber"].ToString() != "")
							text += ", " + school["ProductNumber"].ToString();

						string website = school["Website"].ToString();

						if (website == "" || website.IndexOf("no website") >= 0)
						{
							text += " (no website)";
							website = "";
						}
						result.Append(string.Format(@"<option value=""{0}"">{1}</option>",
							website, text));
					}
					result.Append("</optgroup>");
				}
			}
			result.Append("</select>");
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(result.ToString());
		}
		protected string Query
		{
			get
			{
				return query;
			}
			set
			{
				query = value;
			}
		}
	}
	public class SchoolsComboboxReg : System.Web.UI.HtmlControls.HtmlSelect
	{
		bool bound = false;
		string query = "";
		StringBuilder result;
		bool addEmptyOption = false;
		string emptyOptionText = "";


		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			BrandsMgr pMgr = new BrandsMgr();

			DataView regions = pMgr.GetBrands("").DefaultView;

			result = new StringBuilder();

			result.Append("<select ");

			foreach (string attributeKey in this.Attributes.Keys)
			{
				result.Append(string.Format(@" {0}=""{1}""", attributeKey, Attributes[attributeKey]));
			}
			result.Append(string.Format(" name='{0}' id='{0}'", this.ID));
			result.Append(">");

			if (true || AddEmptyOption)
				result.Append("<option value=\"\">" + EmptyOptionText + "</option>");

			string _value = WebContext.Request.Form[this.ID];

			foreach (DataRowView region in regions)
			{
				if(region["Title"].ToString().ToLower() == "corporate")
					continue;
				string cond = "BrandId=" + region["BrandId"].ToString();
				string sql = "select * from Items where " + cond;
				DataView schools = lw.Data.DBUtils.GetDataSet(sql, lw.Products.cte.lib).Tables[0].DefaultView;
				schools.Sort = "Title, ProductNumber";


				if (schools.Count > 0)
				{
					result.Append(string.Format(@"<optgroup title=""{0}"" class=""{1}"" label=""{0}"">",
						region["Title"], region["Title"].ToString().Replace(" ", "-")));


					foreach (DataRowView school in schools)
					{
						string text = school["Title"].ToString();

						if (school["ProductNumber"] != System.DBNull.Value && school["ProductNumber"].ToString() != "")
							text += ", " + school["ProductNumber"].ToString();

						result.Append(string.Format(@"<option value=""{0}"" {2}>{1}</option>",
							school["ItemId"], text,
							_value == school["ItemId"].ToString()? "selected=true": ""
							));
					}
					result.Append("</optgroup>");
				}
			}
			result.Append("</select>");
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(result.ToString());
		}
		protected string Query
		{
			get
			{
				return query;
			}
			set
			{
				query = value;
			}
		}
		protected bool AddEmptyOption
		{
			get { return addEmptyOption; }
			set { addEmptyOption = value; }
		}
		protected string EmptyOptionText
		{
			get { return emptyOptionText; }
			set { emptyOptionText = ""; }
		}
	}

}
