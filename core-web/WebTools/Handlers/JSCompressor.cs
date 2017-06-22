using System.Web;

namespace lw.WebTools.Compression
{
	public class JSCompressor : IHttpHandler
	{
		public JSCompressor()
		{
		}
		public void ProcessRequest(HttpContext context)
		{
			System.Web.HttpRequest Request = context.Request;
			System.Web.HttpServerUtility Server = context.Server;
			System.Web.HttpResponse Response = context.Response;
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
