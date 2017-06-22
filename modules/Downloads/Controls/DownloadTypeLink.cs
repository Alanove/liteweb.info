using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.Base;
using lw.WebTools;

namespace lw.Downloads.Controls
{
	public class DownloadTypeLink : HtmlAnchor
	{
		bool bound = false;
		string _selectedClass = "selected";
		string path = "";
		string extension = "";

		public DownloadTypeLink()
		{
			Config cfg = new Config();
			path = cfg.GetKey(lw.CTE.Settings.DownloadTypesFolder);
		}

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			object link = DataBinder.Eval(NamingContainer, "DataItem.UniqueName");

			CustomPage page = this.Page as CustomPage;

			if(page != null)
			{
				if (page.GetQueryValue(lw.Downloads.cte.TypeQueryStringName) == link.ToString())
				{
					this.Attributes["class"] = SelectedClass;
				}
			}

			
			this.HRef = string.Format("{3}/{0}{1}{2}",
				!String.IsNullOrWhiteSpace(path)? path + "/": "",
				link, 
				Extension,
				WebContext.Root
			);

			string text = DataBinder.Eval(NamingContainer, "DataItem.Type").ToString();
			this.Title = text;

			if (this.Controls.Count == 0)
				this.InnerHtml = text;

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
		public string Extension
		{
			get { return extension; }
			set { extension = value; }
		}
	}
}
