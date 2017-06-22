using System.Web.UI;
using System.Web.UI.WebControls;



namespace lw.Products.Controls
{
	public class ItemSpecs : Literal
	{
		bool _bound = false;
		string headerStyle = "", leftCol = "", rightCol = "", tableStyle = "", alternateBG = "";
		string ret = "";

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Specs");

			if (obj != null && obj.ToString() != "")
			{
				System.IO.StringReader sr = new System.IO.StringReader(obj.ToString());

				System.Text.StringBuilder sb = new System.Text.StringBuilder();

				string line = sr.ReadLine();
				bool start = true, alternate = false;
				while (line != null)
				{
					if (line.IndexOf("[") >= 0 && line.IndexOf("]") == line.Length - 1)
					{
						if (!start)
							sb.Append("</table>");
						sb.Append(string.Format("<table class=\"{0}\">", tableStyle));
						string title = line.Substring(1, line.Length - 2);
						if (title.Trim() != "")
							sb.Append(string.Format("<tr><td colspan=2 class=\"{0}\">{1}</td></tr>", headerStyle, title));
						start = false;
					}
					else
					{
						sb.Append(string.Format("<tr{0}>", (alternate ? " style=\"background-color:" + alternateBG + "\"" : "")));
						int i = line.IndexOf(":");
						if (i >= 0 && i < line.Length - 2)
						{
							sb.Append(string.Format("<td class=\"{0}\">{1}</td>", leftCol, line.Substring(0, i)));
							sb.Append(string.Format("<td class=\"{0}\">{1}</td>", rightCol, line.Substring(i + 1)));
						}
						else
							sb.Append(string.Format("<td class=\"{0}\" colspan=2>{1}</td>", leftCol, line));

						sb.Append("</tr>");
						alternate = !alternate;
					}
					line = sr.ReadLine();
				}
				if (!start)
					sb.Append("</table>");
				ret = sb.ToString();
			}
			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (!_bound)
				this.DataBind();
			if (this.ret != "")
			{
				writer.Write(ret);
				base.Render(writer);
			}
		}

		public string HeaderStyle
		{
			get
			{
				return headerStyle;
			}
			set
			{
				headerStyle = value;
			}
		}
		public string TableStyle
		{
			get
			{
				return tableStyle;
			}
			set
			{
				tableStyle = value;
			}
		}
		public string LeftCol
		{
			get
			{
				return leftCol;
			}
			set
			{
				leftCol = value;
			}
		}
		public string RightCol
		{
			get
			{
				return rightCol;
			}
			set
			{
				rightCol = value;
			}
		}
		public string AlternateBG
		{
			get
			{
				return alternateBG;
			}
			set
			{
				alternateBG = value;
			}
		}
	}
}