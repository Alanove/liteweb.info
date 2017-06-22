using System.Web.UI;


namespace lw.Polls.Controls
{
	public class PollAnswersResultsDataSrc : lw.DataControls.CustomDataSource
	{
		bool _bound = false;
		int?  _pollId = null;

		public PollAnswersResultsDataSrc()
		{
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			this.SelectCommand = "select * from PollResults";

			if (PollId != null)
			{
				this.SelectCommand += string.Format(" where PollId={0}", _pollId.Value);
			}

			if (string.IsNullOrWhiteSpace(OrderBy))
			{
				OrderBy = "SortOrder ASC";
			}
			if (!EnablePaging)
			{
				this.SelectCommand += " Order By " + OrderBy;
			}
			
			base.DataBind();
		}

		#region Properties
		public int? PollId
		{
			get
			{
				_pollId = DataBinder.Eval(this.NamingContainer, "DataItem.PollId") as int?;
				if (_pollId == null)
				{
					string uniqueName = (this.Page as lw.Base.CustomPage).GetQueryValue("PollId");
				}
				return _pollId;
			}
			set { _pollId = value; }
		}
		#endregion
	}
}
