using System;
using System.Data;
using System.Linq;
using lw.Data;
using System.Text;

namespace lw.Members
{
	public class MembersLocations : LINQManager
	{
		public MembersLocations()
			: base(cte.lib)
		{

		}

		public void AddLocationToMember(int MemberId, AddressType addressType,
			int? CountryId, int? RegionId,
			string City, string Address, 
			string PostalCode, int? Privacy)
		{

			MemberLocation l = new MemberLocation
			{
				MemberId = MemberId,
				AddressType = (byte?)addressType,
				CountryId = CountryId,
				RegionId = RegionId,
				City = City,
				Address = Address,
				Privacy = Privacy,
				PostalCode = PostalCode
			};
			DataContext.MemberLocations.InsertOnSubmit(l);
			Save();
		}

		public int UpdateLocationOfMember(int LocationId, int MemberId, System.Web.HttpRequest dic)
		{
			int? regionId = null;
			string city = "";

			if(!String.IsNullOrWhiteSpace(dic["State"]))
			{
				if (lw.Utils.Validation.IsInteger(dic["State"]))
				{
					regionId = Int32.Parse(dic["State"]);
					city = dic["City"];
				}
				else
				{
					city = dic["State"];
				}
			}

			return UpdateLocationOfMember(LocationId, MemberId,
				(AddressType)Enum.Parse(typeof(AddressType), dic["AddressTypeSelect"]),
				Int32.Parse(dic["Country"]), regionId, city,
				dic["Address"], dic["PostalCode"], 0);
				
		}

		public int UpdateLocationOfMember(int locationId, int MemberId, AddressType addressType,
			int? CountryId, int? RegionId,
			string City, string Address,
			string PostalCode, int? Privacy)
		{
			var location = GetLocation(locationId);

			if (location.MemberId != MemberId)
				return -1;

			location.Address = Address;
			location.City = City;
			location.CountryId = CountryId;
			location.PostalCode = PostalCode;
			location.RegionId = RegionId;
			location.Privacy = Privacy;
			location.AddressType = (byte?)addressType;

			DataContext.SubmitChanges();

			return locationId;			
		}

		/// <summary>
		/// Delete a location
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public bool DeleteLocation(int MemberId, int locationId)
		{
			var location = GetLocation(locationId);

			if (location.MemberId != MemberId)
				return false;

			DataContext.MemberLocations.DeleteOnSubmit(location);
			DataContext.SubmitChanges();

			return true;
		}


		
		public MemberLocation GetLocation(int LocationId)
		{
			return DataContext.MemberLocations.Single(temp => temp.LocationId == LocationId);
		}

		public IQueryable<MemberLocation> GetLocations(int MemberId)
		{
			var q = from rel in DataContext.MemberLocations
					where rel.MemberId == MemberId
					select rel;
			if (q.Count() == 0)
				return null;
			return q;
		}

		public string GetGetLocationsViewSelectCommand(int MemberId)
		{
			StringBuilder sql = new StringBuilder("select MemberLocationView.* from MemberLocationView where 1=1");

			sql.Append(string.Format(" and MemberID={0}", MemberId));
			
			return sql.ToString();
		}

		public DataView GetLocationsView(int MemberId)
		{
			string sql;

			sql = string.Format("select * from MemberLocationView Where MemberId={0}",
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
