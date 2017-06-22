using System;

using lw.Utils;

namespace lw.Polls.Controls
{
	public class PollsDataSrc : lw.DataControls.CustomDataSource
	{
		string _pollReference = "";
		bool _bound = false;

		public PollsDataSrc()
		{
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			this.SelectCommand = "select * from Polls";

			if (!String.IsNullOrWhiteSpace(PollReference))
			{
				this.SelectCommand += " where Reference='" + StringUtils.SQLEncode(_pollReference) + "'";
			}

			if (string.IsNullOrWhiteSpace(OrderBy))
			{
				OrderBy = "PollNbr ASC";
			}
			if (!EnablePaging)
			{
				this.SelectCommand += " Order By " + OrderBy;
			}
			
			

			base.DataBind();
		}

		#region Properties
		public string PollReference
		{
			get
			{
				if (StringUtils.IsNullOrWhiteSpace(_pollReference))
				{
					string uniqueName = (this.Page as lw.Base.CustomPage).GetQueryValue("PollReference");
				}
				return _pollReference;
			}
			set { _pollReference = value; }
		}
		#endregion
	}
}
