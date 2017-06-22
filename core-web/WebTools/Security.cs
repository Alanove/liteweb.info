using System.Web;

namespace lw.WebTools
{
	public class Security
	{
		/// <summary>
		/// Checks if the post comes from other server
		/// </summary>
		/// <returns></returns>
		public static bool IsValidRequest()
		{
			return IsValidRequest(WebContext.Request);
		}
		/// <summary>
		/// Checks if the post comes from other server
		/// </summary>
		/// <returns></returns>
		public static bool IsValidRequest(HttpRequest request)
		{
			return !(request.HttpMethod == "POST" && request.UrlReferrer != null && request.Url.Host != null && request.UrlReferrer.Host != request.Url.Host);
		}
	}
}
