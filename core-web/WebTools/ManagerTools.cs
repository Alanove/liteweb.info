
namespace lw.WebTools
{
	/// <summary>
	/// Summary description for ManagerTools.
	/// </summary>
	public class ManagerTools
	{
		public ManagerTools()
		{
		}
		public static string RefreshOpener
		{
			get
			{
				return "<script>try{top.Main.RefreshOpener(window)}catch(e){}</script>";
			}
		}

	}
}
