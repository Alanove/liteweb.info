using System;
using System.Collections;
using System.Data;
using System.Text;
using lw.Data;
using lw.Networking;
using lw.WebTools;

namespace lw.PhotoAlbums
{
	public class PhotoAlbumsNetwork : NetworkRelations
	{
		string _lib = "";
		public PhotoAlbumsNetwork()
		{
			this._lib = cte.lib;
		}
		public DataTable GetRelatedNetworks(int AlbumId, string Condition)
		{
			return GetRelatedNetworks(cte.NetworkRelationTable, cte.NetworkRelateToField, AlbumId, Condition);
		}
		public DataTable GetRelatedNetworks(int AlbumId)
		{
			return GetRelatedNetworks(cte.NetworkRelationTable, cte.NetworkRelateToField, AlbumId);
		}
		public DataTable GetRelatedAlbums(int NetworkId)
		{
			return GetRelatedItems(cte.NetworkRelationTable, NetworkId);
		}
		public void UpdateAlbumRelations(int AlbumId, ArrayList NetworkIds)
		{
			UpdateItemRelations(cte.NetworkRelationTable, cte.NetworkRelateToField, AlbumId, NetworkIds);
		}
		public string GetRelationQueryByMember(int MemberId)
		{
			return GetRelationQueryByMember(cte.NetworkRelationTable, cte.NetworkRelateToField, MemberId);
		}

		public DataTable GetAlbumsByNetwork(int MemberId, DateTime? Date, string condition)
		{
			if (String.Compare(WebContext.Profile.dbUserName, Config.GetFromWebConfig("Admin"), true) == 0)
			{
				PhotoAlbumsManager paMgr = new PhotoAlbumsManager();
				return paMgr.GetPhotoAlbums(condition).Table;
			}
			else
			{
				StringBuilder cond = new StringBuilder();

				cond.Append(" and " + GetRelationQueryByMember(MemberId));
				if (Date != null)
					cond.Append(string.Format(" and DateAdded>='{0}'", Date));


				string sql = string.Format("select * from AlbumsFullView where 1=1" + (condition != null ? condition : "") + " {0}",
					cond.ToString());

				return DBUtils.GetDataSet(sql, _lib).Tables[0];
			}
		}
		public void UpdateAlbumPreferedNetworks(int AlbumId, int networkId, bool prefered)
		{
			string sqlFalse = string.Format("update  " + cte.NetworkRelationTable + " set prefered='{0}' where Id={1}",
				false, AlbumId);
			DBUtils.ExecuteQuery(sqlFalse, cte.lib);

			string sql = string.Format("update {3} set prefered='{0}' where Id={1} and networkId={2}",
				   prefered, AlbumId, networkId, cte.NetworkRelationTable);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}
		public string GetAlbumPreferedNetwork(int AlbumId)
		{
			string sql = string.Format("select NetworkId from {0} where Id = {1} and Prefered = {2}", cte.NetworkRelationTable, AlbumId, 1);
			var dbMemberId = DBUtils.GetDataSet(sql, lw.PhotoAlbums.cte.lib);

			if (dbMemberId.Tables[0].Rows.Count != 0)
			{
				return dbMemberId.Tables[0].Rows[0]["NetworkId"].ToString();
			}
			else return null;
		}
	}
}
