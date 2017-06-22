using System;
using System.Text;
using lw.DataControls;
using lw.Utils;

namespace lw.Jobs.Controls
{
	public class JobOffersSource : CustomDataSource
	{
		bool _bound = false;
		string category = "";

		public JobOffersSource()
		{
			this.DataLibrary = "Jobs";
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			StringBuilder cond = new StringBuilder();

			JobsManager jMgr = new JobsManager();

			category = this.Page.Request.QueryString["category"];

			if (!String.IsNullOrEmpty(category))
			{
				cond.Append(string.Format(" And CategoryId='{0}'", StringUtils.SQLEncode(category)));
			}

			SelectCommand = string.Format("SELECT * FROM JobOffersView WHERE status=1 AND ('{0}' <= ExpiryDate OR ExpiryDate is NULL) {1} ", 
				DateTime.Now.ToShortDateString(), 
				cond);
			OrderBy = "ModifiedDate DESC";

			base.DataBind();
		}
	}
}
