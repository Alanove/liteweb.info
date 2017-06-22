
namespace lw.WebTools
{
	/// <summary>
	/// Summary description for Javascript.
	/// </summary>
	public class Javascript
	{
		public Javascript()
		{
		}
		public static void Redirect(string path, string target)
		{
			WebContext.Response.Write(string.Format("<script>{0}.location.href=\"{1}\"</script>", target, path));
			WebContext.Response.End();
		}
		public static void Redirect(string path)
		{
			Redirect(path, "_self");
		}
	}
}
