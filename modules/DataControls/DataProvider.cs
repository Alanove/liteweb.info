using System.Web.UI;
using System.Web.UI.WebControls;


namespace lw.DataControls
{
	[ParseChildren(ChildrenAsProperties = false)]
	public class DataProvider : lw.Base.BaseControl, INamingContainer
	{
		object dataItem;

		public object DataItem
		{
			get
			{
				return dataItem;
			}
			set
			{
				dataItem = value;
			}
		}
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			return;
		}
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			return;
		}
	}

}
