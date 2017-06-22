using System;
using System.Data;
using System.Linq;
using lw.CTE.Enum;
using lw.Data;
using lw.Utils;

namespace lw.Members
{
	public class MemberEducations : LINQManager
	{
		public MemberEducations()
			: base(cte.lib)
		{

		}

		/// <summary>
		/// Add a education to a member
		/// </summary>
		/// 
		public void AddEducationToMember(int memberId, System.Web.HttpRequest req)
		{
			UserEducation type = UserEducation.University;
			if (!StringUtils.IsNullOrWhiteSpace(req.Form["Type"]))
				type = (UserEducation)Enum.Parse(typeof(UserEducation), req.Form["Type"]);

			//AddEducationToMember(memberId, req.Form["Name"], req.Form["Degree"], req.Form["Major"], req.Form["City"], req.Form["Country"], 
				//Parsers.Date(req.Form["DateFrom"]), Parsers.Date(req.Form["DateTo"]), req.Form["Note"], type, Parsers.Int(req.Form["Privacy"]));
		}

		public void AddEducationToMember(int memberId, 
			int? CountryId, int? RegionId, string City, 
			int? UniversityId, string Name, string Degree, string Major, string Minor,
			DateTime? DateFrom, DateTime? DateTo,
			string Note, UserEducation Type, int? Privacy)
		{

			MemberEducation e = new MemberEducation
			{
				MemberId = memberId,
				CountryId = CountryId,
				RegionId = RegionId,
				City = City,
				UniversityId = UniversityId,
				Name = Name,
				Degree = Degree,
				Major = Major,
				Minor = Minor,
				DateFrom = DateFrom,
				DateTo = DateTo,
				Note = Note,
				Type = (int)Type,
				Privacy = Privacy
			};
			DataContext.MemberEducations.InsertOnSubmit(e);
			Save();
		}

		/// <summary>
		/// Update education of a member
		/// </summary>
		/// 
		public int UpdateEducationOfMember(int locationId, int memberId, System.Web.HttpRequest req)
		{
			UserEducation type = UserEducation.University;
			if (!StringUtils.IsNullOrWhiteSpace(req.Form["Type"]))
				type = (UserEducation)Enum.Parse(typeof(UserEducation), req.Form["Type"]);

			return UpdateEducationOfMember(locationId, memberId, req.Form["Degree"], req.Form["Major"], req.Form["Minor"], req.Form["City"], req.Form["Country"], 
				Parsers.Date(req.Form["DateFrom"]), Parsers.Date(req.Form["DateTo"]), req.Form["Note"], type);
		}
		public int UpdateEducationOfMember(int educationId, int memberId, string Degree, string Major, string Minor, string City, string Country, DateTime? DateFrom, DateTime? DateTo,
			string Note, UserEducation Type)
		{
			var education = GetEducation(educationId);

			if (education.MemberId != memberId)
			{
				return -1;
			}

			education.Degree = Degree;
			education.Major = Major;
			education.Minor = Minor;
			education.City = City;
			education.DateFrom = DateFrom;
			education.DateTo = DateTo;
			education.Note = Note;
			education.Type = (int)Type;

			DataContext.SubmitChanges();

			return educationId;			
		}

		/// <summary>
		/// Delete an education
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public bool DeleteEducation(int memberid, int educationId)
		{
			var education = GetEducation(educationId);

			DataContext.MemberEducations.DeleteOnSubmit(education);
			DataContext.SubmitChanges();

			return true;
		}






		public MemberEducation GetEducation(int EducationId)
		{
			return DataContext.MemberEducations.Single(temp => temp.EducationId == EducationId);
		}


		public IQueryable<MemberEducation> GetEducations(int MemberId)
		{
			var q = from rel in DataContext.MemberEducations
					where rel.MemberId == MemberId
					select rel;
			if (q.Count() == 0)
				return null;
			return q;
		}


		public DataView GetEducationsView(int MemberId)
		{
			string sql;

			sql = string.Format("select * from MemberEducationView Where MemberId={0}",
						MemberId);

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
