using System;
using System.Linq;
using System.Data;
using System.Text;

using lw.Data;

namespace lw.Members
{
	public class MemberNetworksManager : LINQManager
	{
		public MemberNetworksManager()
			: base(lw.Networking.cte.lib)
		{

		}

		/// <summary>
		/// Ads the member to a social network
		/// </summary>
		/// <param name="memberId">The ID of the Member</param>
		/// <param name="networkId">The ID of the network</param>
		public void AddMemberToNetwork(int memberId, int networkId)
		{
			var q = from rel in DataContext.MemberNetworks 
						where rel.MemberId == memberId && rel.NetworkId == networkId
					select rel;
			if(q.Count() > 0)
			{
				Errors.Add(new Exception("Member is already part of the network"));
				return;
			}

			MemberNetwork n = new MemberNetwork
			{
				MemberId = memberId,
				NetworkId = networkId
			};
			DataContext.MemberNetworks.InsertOnSubmit(n);
			Save();
		}

		public void UpdateMemberPreferedNetworks(int memberId, bool prefered)
		{
			string sql = string.Format("update membernetworks set prefered='{0}' where memberId={1}",
				   prefered, memberId);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		public void UpdateMemberPreferedNetwork(int memberId, int networkId, bool prefered)
		{

			string sql = string.Format("update membernetworks set prefered='{0}' where memberId={1} and networkid={2}",
				   prefered, memberId, networkId);
			DBUtils.ExecuteQuery(sql, cte.lib);

			//UpdateMemberPreferedNetworks(memberId, false);

			//var network = GetMemberNetwork(memberId, networkId);

			//if (network != null)
			//{

			//	network.Prefered = prefered;

			//	DataContext.SubmitChanges();
			//}
		}

		/// <summary>
		/// Removes the member from the network
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="networkId"></param>
		public void RemoveMemberFromNetwork(int memberId, int networkId)
		{
			var q = from rel in DataContext.MemberNetworks
					where rel.MemberId == memberId && rel.NetworkId == networkId
					select rel;
			if (q.Count() == 0)
			{
				Errors.Add(new Exception("Member is not part of the network"));
				return;
			}
			DataContext.MemberNetworks.DeleteOnSubmit(q.First());
			Save();
		}

		/// <summary>
		/// Returns the networks that are related to a member
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="networkId"></param>
		public IQueryable<MemberNetwork> GetMemberNetworks(int memberId)
		{
			var q = from rel in DataContext.MemberNetworks
					where rel.MemberId == memberId
					select rel;
			if (q.Count() == 0)
			{
				Errors.Add(new Exception("Member is not part of any network"));
				return null;
			}
			return q;
		}

		public MemberNetwork GetMemberNetwork(int memberId, int networkId)
		{
			var q = from rel in DataContext.MemberNetworks
					where rel.MemberId == memberId && rel.NetworkId == networkId
					select rel;
			if (q.Count() == 0)
			{
				Errors.Add(new Exception("Member is not part of any network"));
				return null;
			}
			return q.First();
		}

		public MemberNetwork GetMemberPreferedNetwork(int memberId)
		{
            var q = GetMemberPreferedNetworks(memberId);
			if (q.Count() == 0)
			{
				Errors.Add(new Exception("Member is not part of any network"));
				return null;
			}
			return q.First();
		}


        public IQueryable<MemberNetwork> GetMemberPreferedNetworks(int memberId)
        {
            var q = from rel in DataContext.MemberNetworks
                    where rel.MemberId == memberId && rel.Prefered == true
                    select rel;
            if (q.Count() == 0)
            {
                Errors.Add(new Exception("Member is not part of any network"));
                return null;
            }
            return q;
        }

        public DataSet GetMemberPreferedNetworks_DataSet(int memberId)
        {
            var sql = String.Format("select Networks.NetworkId, Networks.Name, Networks.UniqueName, Networks.Description, "
            + " Networks.DateCreated, Networks.LastModified, MemberNetworks.Prefered from Networks inner join MemberNetworks "
            + " on Networks.NetworkId = MemberNetworks.NetworkId where MemberNetworks.MemberId = {0} and MemberNetworks.Prefered = 1 order by DateCreated", memberId);
            return DBUtils.GetDataSet(sql, cte.lib);
        }

		/// <summary>
		/// Returns the networks with details that are related to a member
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="networkId"></param>
		public IQueryable<MemberNetworksView> GetMemberNetworksView(int memberId)
		{
			var q = from rel in DataContext.MemberNetworksViews
					where rel.MemberId == memberId
					select rel;
			if (q.Count() == 0)
			{
				Errors.Add(new Exception("Member is not part of any network"));
				return null;
			}
			return q;
		}

		public MemberNetworksView GetPreferedNetworkDetails(int memberId)
		{
			var q = from rel in DataContext.MemberNetworksViews
					where rel.MemberId == memberId && rel.Prefered == true
					select rel;
			if (q.Count() == 0)
			{
				Errors.Add(new Exception("Member is not part of any prefered network"));
				return null;
			}
			return q.First();
		}

		public DataTable GetMembersViewDT(string cond)
		{
			StringBuilder sql = new StringBuilder("select * from MemberNetworksView where 1=1");

			if (!String.IsNullOrEmpty(cond))
				sql.Append(string.Format(" and {0}", cond));

			return DBUtils.GetDataSet(sql.ToString(), cte.lib).Tables[0];
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