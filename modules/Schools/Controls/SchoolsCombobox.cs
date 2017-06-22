using System.Data;
using System.Text;
using System.Web.UI;
using lw.Data;
using lw.Products;

namespace SABIS.Controls
{
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
				DataView schools = DBUtils.GetDataSet(sql, lw.Products.cte.lib).Tables[0].DefaultView;
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
}
