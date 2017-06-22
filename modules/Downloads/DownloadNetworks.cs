using System;
using System.Collections;
using System.Data;
using System.Text;
using lw.CTE.Enum;
using lw.Data;
using lw.Networking;

namespace lw.Downloads
{
	public class DownloadNetworks : NetworkRelations
	{
		string _lib = "";
		public DownloadNetworks()
		{
			this._lib = cte.lib;
		}
		public DataTable GetRelatedNetworks(int DownloadId, string Condition)
		{
			return GetRelatedNetworks(cte.NetworkRelationTable, cte.NetworkRelateToField, DownloadId, Condition);
		}
		public DataTable GetRelatedNetworks(int DownloadId)
		{
			return GetRelatedNetworks(cte.NetworkRelationTable, cte.NetworkRelateToField, DownloadId);
		}
		public DataTable GetRelatedDownloads(int NetworkId)
		{
			return GetRelatedItems(cte.NetworkRelationTable, NetworkId);
		}
		public void UpdateDownloadRelations(int DownloadId, ArrayList NetworkIds)
		{
			UpdateItemRelations(cte.NetworkRelationTable, cte.NetworkRelateToField, DownloadId, NetworkIds);
		}
		public string GetRelationQueryByMember(int MemberId)
		{
			return GetRelationQueryByMember(cte.NetworkRelationTable, cte.NetworkRelateToField, MemberId);
		}

		public DataTable GetDownloadsByNetwork(int MemberId, DateTime? Date, string condition)
		{
			StringBuilder cond = new StringBuilder();

			cond.Append(" and " + GetRelationQueryByMember(MemberId));
			if (Date != null)
				cond.Append(string.Format(" and DateAdded>='{0}'", Date));


			string sql = string.Format("select d.*,  '{0}/' + d.UniqueName + '/' + d.FileName as DownloadLink, FileSize/1024 as KB from DownloadsView d where Status<>{1}" + (condition != null ? condition : "") + " {2} ORDER BY DateModified Desc",
				Downloads.DownloadsVR,
				(int)DownloadStatus.Disabled,
				cond.ToString());
			
			return DBUtils.GetDataSet(sql, _lib).Tables[0];
		}
		public void UpdateDownloadPreferedNetworks(int downloadId, int networkId, bool prefered)
		{
			string sqlFalse = string.Format("update  " + cte.NetworkRelationTable + " set prefered='{0}' where downloadId={1}",
				false, downloadId);
			DBUtils.ExecuteQuery(sqlFalse, cte.lib);

			string sql = string.Format("update {3} set prefered='{0}' where downloadId={1} and networkId={2}",
				   prefered, downloadId, networkId, cte.NetworkRelationTable);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}
		public string GetDownloadPreferedNetwork(int downloadId)
		{
			string sql = string.Format("select NetworkId from {0} where DownloadId = {1} and Prefered = {2}", cte.NetworkRelationTable, downloadId, 1);
			var dbMemberId = DBUtils.GetDataSet(sql, lw.Downloads.cte.lib);

			if (dbMemberId.Tables[0].Rows.Count != 0)
			{
				return dbMemberId.Tables[0].Rows[0]["NetworkId"].ToString();
			}
			else return null;
		}
	}
}
