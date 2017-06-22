using System.Data;
using System.Text;
using System.Web.UI;
using lw.Data;
using lw.Products;
using lw.WebTools;

namespace SABIS.Controls
{
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
				if (region["Title"].ToString().ToLower() == "corporate")
					continue;
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

						result.Append(string.Format(@"<option value=""{0}"" {2}>{1}</option>",
							school["ItemId"], text,
							_value == school["ItemId"].ToString() ? "selected=true" : ""
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
