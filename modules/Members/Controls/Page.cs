using System;

namespace lw.Members.Controls
{
	public class Page : lw.Base.CustomPage
	{
		
		/// <summary>
		/// Updates the user last accessed and online status
		/// Refreshes the cach, or timeout
		/// </summary>
		/// <param name="e"></param>
		protected override void OnUnload(EventArgs e)
		{
			lw.Members.Security.Caching.UpdateUserCache();
			base.OnUnload(e);
		}
	}
}
