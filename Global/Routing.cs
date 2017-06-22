using System;
using System.Data;
using System.Web.Caching;
using System.Web.Routing;
using lw.WebTools;

namespace lw.Global
{
	public class Routing
	{
		/// <summary>
		/// Inits the routing paths
		/// Directly connected to the Routing Config file and will reset once the file is changed.
		/// For routing to work global.asax must be included on the root of the website 
		/// and must inherit lw.Base.Global,
		/// You can create your own Global File and call Initrouting yourself in Application_BeginRequest
		/// </summary>
		public static void InitRouting()
		{
		
			if (RouteTable.Routes.Count > 0)
				return;

			RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
			RouteTable.Routes.Ignore("{resource}.lwjs");
			RouteTable.Routes.Ignore("{resource}.Less");

			///Inline CMS Paths
			RouteTable.Routes.MapPageRoute(
								"Inline CMS",
								lw.CTE.cms.InlineCMSVirutalPath,
								lw.CTE.cms.InlineCMSRealPath
							);
			RouteTable.Routes.MapPageRoute(
								"Inline CMS Logout",
								lw.CTE.cms.InlineCMSVirualLogoutPath,
								lw.CTE.cms.InlineCMSrealLogoutPath
							);

			DataSet urlRoutingDS = XmlManager.GetDataSet(CTE.Content.UrlRouting, ResetRouting);
			if (urlRoutingDS != null)
			{
				DataTable urlRouting = urlRoutingDS.Tables["UrlRouting"];
				if (urlRouting != null)
				{
					foreach (DataRow routing in urlRouting.Rows)
					{
						RouteTable.Routes.MapPageRoute(
								routing["Description"].ToString().Trim(),
								routing["Route"].ToString().Trim(),
								routing["Redirection"].ToString().Trim()
							);
					}
				}
			}
		}

		/// <summary>
		/// Called once the routing config file is changed
		/// </summary>
		public static void ResetRouting(string key, object v, CacheItemRemovedReason r)
		{
			try
			{
				RouteTable.Routes.Clear();
				InitRouting();
			}
			catch(Exception ex)
			{
				Error.Handler.HandleError(ex, "");
			}
		}
	}
}
