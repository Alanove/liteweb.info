using System;
using System.Linq;

using lw.Data;

namespace lw.Members
{
	public class MemberResetPassword : LINQManager
	{
		public MemberResetPassword()
			: base(cte.lib)
		{

		}

		public Members_ResetPassword GetResetMemberInfo(int memberId)
		{
			var q = DataContext.Members_ResetPasswords.Where(temp => temp.MemberId == memberId);
			if (q.Count() > 0)
				return q.First();
			return null;
		}

		public Members_ResetPassword GetResetCodeInfo(string code)
		{

			var q = DataContext.Members_ResetPasswords.Where(temp => temp.Code == code);
			if (q.Count() > 0)
				return q.First();
			return null;
		}

		public bool AddResetPasswordCode(int memberId, string code)
		{
			var q = from rel in DataContext.Members_ResetPasswords
					where rel.Code == code
					select rel;

			if (q.Count() > 0)
			{
				return false;
			}

			Members_ResetPassword m = new Members_ResetPassword
			{
				MemberId = memberId,
				Code = code,
				Date = DateTime.Now
			};

			DataContext.Members_ResetPasswords.InsertOnSubmit(m);
			Save();
			return true;
		}

		public bool UpdateResetPasswordCodeTime(int memberId)
		{
			var q = GetResetMemberInfo(memberId);

			q.Date = DateTime.Now;

			DataContext.SubmitChanges();
			return true;
		}

		public void DeleteResetCodeInfo(string code)
		{
			var resetCode = GetResetCodeInfo(code);

			DataContext.Members_ResetPasswords.DeleteOnSubmit(resetCode);
			DataContext.SubmitChanges();
		}

		public void DeleteResetCodeInfo(int memberId)
		{
			var q = GetResetMemberInfo(memberId);

			DataContext.Members_ResetPasswords.DeleteOnSubmit(q);
			DataContext.SubmitChanges();
		}

		#region Variables


		public MembersDataContextDataContext DataContext
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new MembersDataContextDataContext(Connection);
				return (MembersDataContextDataContext)_dataContext;
			}
		}

		#endregion
	}
}
