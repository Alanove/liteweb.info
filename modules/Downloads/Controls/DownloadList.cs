using System.Data;
using System.Text;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;

namespace lw.Downloads.Controls
{
	public class DownloadList : CustomRepeater
	{
		bool _bound = false;
		DownloadStatus status = DownloadStatus.All;
		bool _listAll = false;
		int max = -1;
		string type = "";

		public DownloadList()
		{
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			StringBuilder cond = new StringBuilder();

			if (Type != "")
			{
				cond.Append(string.Format(" And (Type='{0}' or UniqueName='{0}')", StringUtils.SQLEncode(Type)));
			}

			if(status != DownloadStatus.All)
				cond.Append(string.Format(" And Status={0}", (int)Status));
			else
				cond.Append(string.Format(" And Status<>{0}", (int)DownloadStatus.Disabled));


			string sql = "";
			sql = string.Format("Status<>0 {0}", cond);


			Downloads dMgr = new Downloads();

			DataView downloads = dMgr.GetDownloadMax(max, sql);

			this.DataSource = downloads;

			base.DataBind();
		}

		public bool ListAll
		{
			get
			{
				return _listAll;
			}
			set
			{
				_listAll = value;
			}
		}

		public DownloadStatus Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}

		public int Max
		{
			get
			{
				return max;
			}
			set
			{
				max = value;
			}
		}

		public string Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}
	}
}