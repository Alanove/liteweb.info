using System;
using System.Web.UI;

using lw.Base;

namespace lw.Content.Controls
{
	[ParseChildren(ChildrenAsProperties = false)]
	public class DisplayContainer : System.Web.UI.WebControls.PlaceHolder
	{
		string source = "";
		DisplayCondition? displayTest = null;
		bool? display = null;

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (Display != null)
			{
				if (!Display.Value)
					return;
			}
			base.Render(writer);
		}

		/// <summary>
		/// Any DataSource extensing HasData
		/// </summary>
		public string Source
		{
			get { return source; }
			set { source = value; }
		}

		/// <summary>
		/// If true the container is displayed when there is data
		/// If false the container is displayed when there is no data, ex: a coming soon message
		/// </summary>
		public DisplayCondition? DisplayTest
		{
			get { return displayTest; }
			set { displayTest = value; }
		}

		/// <summary>
		/// Determines the display status, overrides any other parameter
		/// </summary>
		public bool? Display
		{
			get
			{
				if (DisplayTest != null)
				{
					switch (DisplayTest)
					{
						case DisplayCondition.ParentHasData:
						case DisplayCondition.ParentNoData:
							Control ctrl = Page.FindControl(source);
							lw.Data.IDataSource dataSrc = ctrl as lw.Data.IDataSource;

							if (dataSrc == null)
							{
								dataSrc = this.Parent as lw.Data.IDataSource;
							}

							if (dataSrc != null)
							{
								display = displayTest == DisplayCondition.ParentNoData ? !dataSrc.HasData : dataSrc.HasData;
							}
							break;
						case DisplayCondition.CheckInnerProperties:
							foreach (Control _ctrl in this.Controls)
							{
								lw.DataControls.IDataProperty prop = _ctrl as lw.DataControls.IDataProperty;
								if (prop != null)
								{
									if (prop.IVisible)
									{
										display = true;
										break;
									}
								}
							}
							if(display == null)
								display = false;
							break;
						case DisplayCondition.MultipleNoData:
							string[] sources = Source.Split(new char[] {',', '-', ' ' , ';'}, StringSplitOptions.RemoveEmptyEntries);

							CustomPage page = this.Page as CustomPage;

							foreach (string src in sources)
							{
								Control _ctrl = page.FindControlRecursive(page, src);
								lw.Data.IDataSource _src = _ctrl as lw.Data.IDataSource;

								if (_src != null && _src.HasData)
								{
										display = false;
										break;
								}
							}

							break;
						default:
							break;
					}
				}

				return display;
			}
			set
			{
				display = value;
			}
		}

		protected override ControlCollection CreateControlCollection()
		{
			return base.CreateControlCollection();
		}
	}
}
