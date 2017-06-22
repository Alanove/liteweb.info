using System;
using System.Web;
using System.Web.UI.WebControls;



namespace lw.Ads.Handlers
{
	/// <summary>
	/// Processed when an AD is clicked 
	/// </summary>
	public class AdsHandler : WebControl, IHttpHandler, System.Web.SessionState.IRequiresSessionState
	{
		public void ProcessRequest(HttpContext context)
		{
			int AdId = Int32.Parse(context.Request.QueryString["AdId"]);

			AdsManager aMgr = new AdsManager();

			aMgr.UpdateAdClicks(AdId);

			Ads.Ad a = aMgr.GetSingleAd(AdId);
			if (a != null)
				context.Response.Redirect(a.URL);

			context.Response.Redirect(lw.WebTools.WebContext.Root);
		}
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}
