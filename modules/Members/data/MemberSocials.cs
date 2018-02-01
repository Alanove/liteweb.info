using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using lw.CTE.Enum;
using lw.Data;
using lw.Utils;

namespace lw.Members
{
	public class MemberSocials : LINQManager
	{
		public MemberSocials()
			: base(cte.lib)
		{

		}

		/// <summary>
		/// Add a social to a member
		/// </summary>
		/// 
		public void ManageMemberSocials(int memberId, System.Web.HttpRequest req)
		{

			Dictionary<int, string> _Dic = new Dictionary<int, string>();
			Dictionary<string, string> _Updated = new Dictionary<string, string>();

			foreach (UserSocial social in Enum.GetValues(typeof(UserSocial)))
			{
				_Dic.Add((int)social, social.ToString());
			}

			DataView mem = GetSocialsView(memberId);

			foreach (DataRowView drv in mem)
			{
				if (!StringUtils.IsNullOrWhiteSpace(req.Form[_Dic[Int32.Parse(drv["Type"].ToString())]]))
				{
					UpdateSocialOfMember((int)drv["SocialId"], memberId, (UserSocial)Enum.Parse(typeof(UserSocial), drv["Type"].ToString()),
											req.Form[_Dic[Int32.Parse(drv["Type"].ToString())]], Parsers.Int(req["Privacy"]));
					_Updated.Add(_Dic[Int32.Parse(drv["Type"].ToString())], _Dic[Int32.Parse(drv["Type"].ToString())]);
				}
				else
					DeleteSocial((int)drv["SocialId"]);

			}

			foreach (UserSocial social in Enum.GetValues(typeof(UserSocial)))
			{
				if (!StringUtils.IsNullOrWhiteSpace(req.Form[social.ToString()]))
				{
					if (!_Updated.ContainsKey(social.ToString()))
						AddSocialToMember(memberId, (UserSocial)Enum.Parse(typeof(UserSocial), social.ToString()),
							req.Form[social.ToString()], Parsers.Int(req["Privacy"]));
				}
			}
		}

		public void AddSocialToMember(int memberId, System.Web.HttpRequest req)
		{
			UserSocial type = UserSocial.facebook;
			if (!StringUtils.IsNullOrWhiteSpace(req.Form["Type"]))
				type = (UserSocial)Enum.Parse(typeof(UserSocial), req.Form["Type"]);

			AddSocialToMember(memberId, type, req.Form["Value"], Parsers.Int(req.Form["Privacy"]));
		}

		public void AddSocialToMember(int memberId, UserSocial Type, string Value, int? Privacy)
		{
			DataView mem = GetSocialsView(memberId, Type);

			if (mem.Count > 0)
			{
				UpdateSocialOfMember((int)mem[0]["SocialId"], memberId, (UserSocial)Enum.Parse(typeof(UserSocial), mem[0]["Type"].ToString()),
											mem[0]["Type"].ToString(), null);
			}
			else
			{
				MemberSocial s = new MemberSocial
				{
					MemberId = memberId,
					Type = (int)Type,
					Value = Value,
					Privacy = Privacy
				};
				DataContext.MemberSocials.InsertOnSubmit(s);
				Save();
			}
		}

		/// <summary>
		/// Update social of a member
		/// </summary>
		/// 
		public int UpdateSocialOfMember(int SocialId, int memberId, System.Web.HttpRequest req)
		{
			UserSocial type = UserSocial.facebook;
			if (!StringUtils.IsNullOrWhiteSpace(req.Form["Type"]))
				type = (UserSocial)Enum.Parse(typeof(UserSocial), req.Form["Type"]);

			return UpdateSocialOfMember(SocialId, memberId, type, req.Form["Value"], Parsers.Int(req.Form["Privacy"]));
		}
		public int UpdateSocialOfMember(int SocialId, int memberId, UserSocial Type, string Value, int? Privacy)
		{
			var social = GetSocial(SocialId);

			social.Type = (int)Type;
			social.Value = Value;
			social.Privacy = Privacy;

			DataContext.SubmitChanges();

			return SocialId;
		}

		/// <summary>
		/// Delete an social
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public bool DeleteSocial(int SocialId)
		{
			var social = GetSocial(SocialId);

			DataContext.MemberSocials.DeleteOnSubmit(social);
			DataContext.SubmitChanges();

			return true;
		}






		public MemberSocial GetSocial(int SocialId)
		{
			return DataContext.MemberSocials.Single(temp => temp.SocialId == SocialId);
		}


		public IQueryable<MemberSocial> GetSocials(int MemberId)
		{
			var q = from rel in DataContext.MemberSocials
					where rel.MemberId == MemberId
					select rel;
			if (q.Count() == 0)
				return null;
			return q;
		}



		public DataView GetSocialsView(int MemberId)
		{
			string sql;

			sql = string.Format("select * from MemberSocial Where MemberId={0}",
						MemberId);

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0].DefaultView;
		}

		public DataView GetSocialsView(int MemberId, UserSocial Type)
		{
			string sql;

			sql = string.Format("select * from MemberSocial Where MemberId={0} and Type={1}",
						MemberId,
						(int)Type
						);

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0].DefaultView;
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
