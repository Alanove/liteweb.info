using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using lw.CTE.Enum;
using lw.Data;
using lw.Utils;
using lw.WebTools;

namespace lw.Polls
{
	/// <summary>
	/// Summary description for PollsManager
	/// </summary>

	public class PollsManager : LINQManager
	{
		public const string ImagesFolder = "Prv/Images/polls";

		public PollsManager()
			: base(cte.lib)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void UpdatePollStatus(int PollId, lw.CTE.Enum.Status stat)
		{
			string sql = "Update Polls set Status={0}, DateModified=getDate() where PollID={1}";
			sql = string.Format(sql, (int)stat, PollId);

			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		public Poll AddPoll(string PollName)
		{
			var q = from _poll in PollsData.Polls
					where _poll.DisplayText == PollName
					select _poll;
			if (q.Count() > 0)
				return null;

			Poll poll = new Poll
			{
				DisplayText = PollName,
				UniqueName = StringUtils.ToURL(PollName),
				Status = (int)lw.CTE.Enum.Status.Pending,
				IsDefault = false,
				DateCreated = System.DateTime.Now,
				DateModified = System.DateTime.Now
			};

			PollsData.Polls.InsertOnSubmit(poll);
			PollsData.SubmitChanges();

			return poll;
		}

		public Poll GetPoll(int PollId)
		{
			var query = from poll in PollsData.Polls select poll;

			if (query.Count() > 0)
				return PollsData.Polls.Single(poll => poll.PollID == PollId);

			return null;
		}

		public Poll GetPoll(string  UID)
		{
			var query = from poll in PollsData.Polls select poll;

			if (query.Count() > 0)
				return PollsData.Polls.Single(poll => poll.UID == new System.Guid(UID));

			return null;
		}

		public string GetCorrectAnswer(int pollId)
		{
			var query = from answer in PollsData.PollAnswers
						where answer.Correct == true && answer.PollID == pollId
						select answer;
			if (query.Count() > 0)
				return query.First(answer => answer.Correct == true).DisplayText;
			return "";
		}
		public bool CheckIsCorrectAnswer(int pollId, int answerId)
		{
			var query = from answer in PollsData.PollAnswers
						where answer.Correct == true && answer.PollID == pollId
						select answer;

			if (query.Count() > 0)
				return query.First(answer => answer.Correct == true).PollAnswerID == answerId;

			return true;
		}

		public Poll GetCurrentPoll()
		{
			int? week = -1;
			int? day = -1;
			int? pollId = PollsData.Polls_GetCurrentPoll(week, day);
			if (pollId.Value > 0)
			{
				return GetPoll(pollId.Value);
			}
			return null;
		}

		public void DeletePoll(int PollId)
		{
			UpdatePollStatus(PollId, Status.Disabled);

			string sql = "Delete From Polls where POllId={0}";

			sql = string.Format(sql, PollId);
			DBUtils.ExecuteQuery(sql, cte.lib);
			//delete poll image later
		}

		public void UpdatePoll(int PollId, string DisplayText,
			Status stat,
			string Category, short? Difficulty, string Reference,
			short? Day, short? Week, short? Year, 
			HttpPostedFile Picture, bool DeleteOldPicture,
			string AdditionalText, int? PollNbr, bool IsDefault,
			bool? Can_Have_Other, string OtherText)
		{
			Poll poll = GetPoll(PollId);
			if (poll == null)
				return;

			poll.DisplayText = DisplayText;
			poll.Category = Category;
			poll.Difficulty = Difficulty;
			poll.Reference = Reference;
			poll.Day = Day;
			poll.Week = Week;
			poll.Year = Year;
			poll.AdditionalText = AdditionalText;
			poll.PollNbr = PollNbr;
			poll.IsDefault = IsDefault;
			poll.Status = (int)stat;
			poll.DateModified = System.DateTime.Now;
			poll.Can_Have_Other = Can_Have_Other;
			poll.Other_Text = OtherText;

			if (IsDefault)
			{
				string sql = string.Format("Update Polls set IsDefault=0 where PollID<>{0}", PollId);
				DBUtils.ExecuteQuery(sql, "NewsManager");
			}

			if (DeleteOldPicture)
				poll.Picture = "";

			string path = WebContext.Server.MapPath(Path.Combine(WebContext.StartDir, ImagesFolder));
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			var oldImage = poll.Picture;

			if (Picture != null && Picture.ContentLength > 0)
			{
				DeleteOldPicture = true;

				string ImageName = Path.GetFileNameWithoutExtension(Picture.FileName);
				ImageName = string.Format("{0}_{1}{2}", ImageName, poll.PollID,
					Path.GetExtension(Picture.FileName));

				string _path = Path.Combine(path, ImageName);

				Picture.SaveAs(_path);

				if (ImageName == poll.Picture)
					DeleteOldPicture = false;

				poll.Picture = ImageName;
			}

			if (DeleteOldPicture && !string.IsNullOrEmpty(oldImage))
			{
				path = Path.Combine(path, oldImage);
				if (File.Exists(path))
					File.Delete(path);
			}

			PollsData.SubmitChanges();
		}


		public DataTable GetPolls(string cond)
		{
			string sql = "select * from Polls";

			if (!string.IsNullOrWhiteSpace(cond))
			{
				sql += " where " + cond;
			}

			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}

		#region Variables


		public PollsContextDataContext PollsData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new PollsContextDataContext(Connection);
				return (PollsContextDataContext)_dataContext;
			}
		}

		#endregion
	}
}