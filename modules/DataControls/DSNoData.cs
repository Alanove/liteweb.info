using System;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace lw.DataControls
{
	/// <summary>
	/// Can be bound to a datasource and will be displayed when the datasource has no data.
	/// It's automatically bound if placed inside a datasource
	/// </summary>
	public class DSNoData : lw.Base.BaseControl
	{
		string source = "";
		bool _bound = false;
		bool _hasData = false;
		string cssClass = "";

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			
			CustomDataSource dataSrc = null;

			if (!String.IsNullOrWhiteSpace(source))
			{
				Control ctrl = Page.FindControl(source);
				if (ctrl != null)
					dataSrc = ctrl as CustomDataSource;
			}
			else
			{
				dataSrc = this.Parent as CustomDataSource;
			}
			
			if (dataSrc != null)
			{
				_hasData = dataSrc.HasData;
			}
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (!_hasData)
			{
				if (!string.IsNullOrWhiteSpace(cssClass))
				{
					writer.Write("<div class='no-items " + cssClass + "'>");
				}
				else
				{
					writer.Write("<div class='no-items'>");
				}
				base.Render(writer);
				writer.Write("</div>");
			}
		}
		public string Source
		{
			get
			{
				return Source;
			}
			set
			{
				source = value;
			}
		}
		public string CssClass
		{
			get
			{
				return CssClass;
			}
			set
			{
				cssClass = value;
			}
		}


		public bool HasData
		{
			get
			{
				return _hasData;
			}
		}
	}
}
