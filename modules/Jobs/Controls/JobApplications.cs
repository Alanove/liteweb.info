using System;
using System.Text;
using lw.DataControls;
using lw.Utils;

namespace lw.Jobs.Controls
{
	public class JobApplicationsSource : CustomDataSource
	{
		bool _bound = false;
		string category = "";

		public JobApplicationsSource()
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

			SelectCommand = string.Format("SELECT * FROM JobApplicationsView WHERE status=1 {0}", 
				cond);
			OrderBy = "ModifiedDate DESC";

			base.DataBind();
		}
	}
}
