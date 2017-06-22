


namespace lw.Content.Controls
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class AutoContent : Content
	{
		public AutoContent()
		{
			OverridePageProperties = true;
		}
		public  override void DataBind()
		{
			string url = this.Page.Request.Url.ToString();
			url = url.Substring(url.LastIndexOf("/") + 1).Split('#')[0].Split('?')[0];
			url = url.Split('.')[0];
			this.PageName = url;

			base.DataBind();
		}
	}
}
