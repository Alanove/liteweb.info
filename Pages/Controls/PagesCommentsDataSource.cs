using System;
using System.Data;
using System.Text;
using System.Web.UI;
using lw.Base;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;
using lw.Data;

namespace lw.Pages.Controls
{
	public class PagesCommentsDataSource : PagesDataSource
	{
		public PagesCommentsDataSource()
		{
			this.GetComments = true;
		}
	}
}