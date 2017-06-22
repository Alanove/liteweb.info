using System;
using System.Data;
using System.Linq;
using lw.Data;
using lw.Utils;

namespace lw.Members
{
	public class MemberCareers : LINQManager
	{
		public MemberCareers()
			: base(cte.lib)
		{

		}

		public void AddCareerToMember(int memberId, int? CountryId, int? RegionId, 
			string Company, string Title, string Responsabilities, string City, string Country, 
			DateTime? DateFrom, DateTime? DateTo, int? Privacy, bool CurrentJob)
		{

			MemberCareer c = new MemberCareer
			{
				CountryId = CountryId,
				RegionId = RegionId,
				MemberId = memberId,
				Company = Company,
				Title = Title,
				Responsabilities = Responsabilities,
				City = City,
				DateFrom = DateFrom,
				DateTo = DateTo,
				Privacy = Privacy,
				CurrentJob = CurrentJob
			};
			DataContext.MemberCareers.InsertOnSubmit(c);
			Save();
		}

		/// <summary>
		/// Update career of a member
		/// </summary>
		/// 
		public int UpdateCareerOfMember(int CareerId, int memberId, System.Web.HttpRequest req)
		{
			int? regionId = null;
			string city = "";

			if (!String.IsNullOrWhiteSpace(req["State"]))
			{
				if (lw.Utils.Validation.IsInteger(req["State"]))
				{
					regionId = Int32.Parse(req["State"]);
					city = req["City"];
				}
				else
				{
					city = req["State"];
				}
			}

			DateTime? dateTo = null;
			DateTime? dateFrom = null;

			if (!(req.Form["CurrentJob"] == "on"))
			{
				dateTo = Parsers.Date(req.Form["DateTo"]);
			}
			if (!string.IsNullOrWhiteSpace(req.Form["DateFrom"]))
			{
				dateFrom = Parsers.Date(req.Form["DateFrom"]);
			}

			return UpdateCareerOfMember(CareerId, memberId,
				Int32.Parse(req.Form["Country"]), req.Form["Company"], req.Form["Title"], req.Form["Responsabilities"], req.Form["City"],
				dateFrom, dateTo,
				req.Form["CurrentJob"] == "on" ? true : false);
		}
		public int UpdateCareerOfMember(int CareerId, int memberId, 
			int? CountryId, string Company, string Title, string Responsabilities, string City,
			DateTime? DateFrom, DateTime? DateTo, bool CurrentJob)
		{
			var career = GetCareer(CareerId);

			career.CountryId = CountryId;
			career.Company = Company;
			career.Title = Title;
			career.Responsabilities = Responsabilities;
			career.City = City;
			career.DateFrom = DateFrom;
			career.DateTo = DateTo;
			career.CurrentJob = CurrentJob;

			DataContext.SubmitChanges();

			return CareerId;			
		}

		/// <summary>
		/// Delete a career
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public bool DeleteCareer(int memberId, int Career)
		{
			var career = GetCareer(Career);

			DataContext.MemberCareers.DeleteOnSubmit(career);
			DataContext.SubmitChanges();

			return true;
		}






		public MemberCareer GetCareer(int CareerId)
		{
			return DataContext.MemberCareers.Single(temp => temp.CareerId == CareerId);
		}


		public IQueryable<MemberCareer> GetCareers(int MemberId)
		{
			var q = from rel in DataContext.MemberCareers
					where rel.MemberId == MemberId
					select rel;
			if (q.Count() == 0)
				return null;
			return q;
		}


		public DataView GetCareersView(int MemberId)
		{
			string sql;

			sql = string.Format("select * from MemberCareerView Where MemberId={0}",
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
