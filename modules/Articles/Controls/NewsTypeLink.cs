using System.Web.UI;
using System.Web.UI.HtmlControls;

using lw.WebTools;

namespace lw.Articles.Controls
{
	public class NewsTypeLink : HtmlAnchor
	{
		bool bound = false;
		string _selectedClass = "selected";
		string path = "";

		public NewsTypeLink()
		{
			Config cfg = new Config();
			string folder = cfg.GetKey(lw.CTE.Settings.NewsTypesFolder);
		}

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			object link = DataBinder.Eval(NamingContainer, "DataItem.UniqueName");

			object obj = WebContext.Request.QueryString["project"];
			if (obj != null &&
				obj.ToString().ToLower() == link.ToString())
			{
				this.Attributes["class"] = SelectedClass;
			}

			
			this.HRef = string.Format("~/{0}/{1}.aspx",
				path,
				link);

			string text = DataBinder.Eval(NamingContainer, "DataItem.Name").ToString();
			this.Title = this.InnerHtml = text;
			base.DataBind();
		}

		public string SelectedClass
		{
			get { return _selectedClass; }
			set { _selectedClass = value; }
		}
		public string Path
		{
			get { return path; }
			set { path = value; }
		}
	}
}
