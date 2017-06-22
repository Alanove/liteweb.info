using System;
using System.Collections;
using System.Data;
using lw.Data;

namespace lw.Networking
{
	public class NetworkRelations
	{
		/// <summary>
		/// Returns the networks related to the specified ID
		/// </summary>
		/// <param name="TableName">The table name used in <seealso cref="CreateRelation">CreateRelation</seealso>.</param>
		/// <param name="RelateToId">the name of the related field (ItemId, DownloadId, NewsId...)</param>
		/// <param name="ItemId">The value of the RelateToId</param>
		/// <param name="Condition">Optional: Condition to add at the end of the query</param>
		/// <returns>List of networks</returns>
		public DataTable GetRelatedNetworks(string TableName, string RelateToId, int ItemId, string Condition)
		{
			string sql = "select * from Networks where NetworkId in (Select NetworkId from {0} where {1}={2})";

			sql = string.Format(sql, TableName, RelateToId, ItemId);

			if(!String.IsNullOrEmpty(Condition))
			{
				sql = sql + " and " + Condition;
			}

			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}

		/// <summary>
		/// Returns the networks related to the specified ID
		/// </summary>
		/// <param name="TableName">The table name used in <seealso cref="CreateRelation">CreateRelation</seealso>.</param>
		/// <param name="ID">The Id of the item (ItemId, DownloadId, NewsId...)</param>
		/// <returns>List of networks</returns>
		public DataTable GetRelatedNetworks(string TableName, string RelateToId, int ItemId)
		{
			return GetRelatedNetworks(TableName, RelateToId, ItemId, "");
		}

		/// <summary>
		/// Returns datatable containing all the items related to the specified network
		/// Ex: all the downloads related to network id = 1
		/// </summary>
		/// <param name="TableName">Table Name ex: DownloadNetworks</param>
		/// <param name="RelateToField">Field Name ex: DownloadId</param>
		/// <param name="NetworkId">The NetworkId</param>
		/// <returns>Returns datatable containing all the items related to the specified network</returns>
		public DataTable GetRelatedItems(string TableName, int NetworkId)
		{
			string sql = "select * from {0} where NetworkId = {1}";
			sql = string.Format(sql, TableName, NetworkId);
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}

		/// <summary>
		/// Creates the database relation between a table and the network
		/// </summary>
		/// <param name="TableName">TableName that's related to network, ex: Members, Downloads...</param>
		/// <param name="NetworkRelationTableName">The Relation table name ex: MemberNetworks, DownloadNetworks...</param>
		/// <param name="RelateToField">The Name of ID column, ex: MemberId, DownloadId...</param>
		public void CreateRelation(string TableName, string NetworkRelationTableName, string RelateToField)
		{
			/*
			 * //no use since the exception will the thrown anyway with more details below.
			if (DBUtils.TableExists(TableName, cte.lib))
			{
				throw (new Exception("Table already exists"));
			}
			 */

			string sql = @"
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
CREATE TABLE {0}
(
	NetworkId int NOT NULL,
	{1} int NOT NULL,
	Role int Null,
	Prefered bit null
)  ON [PRIMARY]

ALTER TABLE {0} ADD CONSTRAINT
	PK_{0} PRIMARY KEY CLUSTERED 
	(
		NetworkId,
		{1}
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
COMMIT
";
			/*


";*/
			
			//Create the table
			sql = String.Format(sql, NetworkRelationTableName, RelateToField);
			DBUtils.ExecuteQuery(sql, cte.lib);


			sql = @"
CREATE TRIGGER Trg_Delete_NetworkId_RelateTo{1}
   ON  Networks
   AFTER Delete
AS 
BEGIN
	Delete From {0} where NetworkId in (select NetworkId from deleted)
END";
			//trigger to delete network ids from relation table if a network is deleted
			sql = String.Format(sql, NetworkRelationTableName, TableName);
			//WebContext.Response.Write(sql);
			//WebContext.Response.End();
			DBUtils.ExecuteQuery(sql, cte.lib);


			sql = @"
CREATE TRIGGER Trg_Delete_{0}_Networks
   ON  {0}
   AFTER Delete
AS 
BEGIN
	Delete From {1} where {2} in (select {2} from deleted)
END";
			//trigger to delete relate to ids from relation table if an item is deleted
			sql = String.Format(sql, TableName, NetworkRelationTableName, RelateToField);
			DBUtils.ExecuteQuery(sql, cte.lib);
		}

		/// <summary>
		/// Updates the relation between Network and Items
		/// </summary>
		/// <param name="TableName"></param>
		/// <param name="TableName">Table Name ex: Downloads</param>
		/// <param name="RelateToField">Field Name ex: DownloadId</param>
		/// <param name="ItemId">The value of the RelateToField</param>
		/// <param name="NetworkIds">the new networkids array</param>
		public void UpdateItemRelations(string TableName, string RelateToField, int ItemId, ArrayList NetworkIds)
		{
			string networks = String.Join(",", NetworkIds.ToArray());
			string cond = "";

			if(NetworkIds.Count > 0)
				cond = " and NetworkId not in (" + networks + ")";

			//first we delete the unrelated networks
			string deleteQuery = string.Format("Delete from {0} where {1}={2}{3}", 
				TableName, 
				RelateToField, 
				ItemId, 
				cond);
			try
			{
				DBUtils.ExecuteQuery(deleteQuery, cte.lib);
			}
			catch(Exception ex)
			{
				lw.Error.Handler.HandleError("Fail to delete old Unrelated Networks, class: lw.Networking.NetworkRelations.UpdateItemRelations", ex);
				throw(ex);
			}

			if (NetworkIds.Count > 0)
			{
				//Second we insert the newly related networks
				//dbo.IdsToTable is an sql function that will transform 
				//a list of comma seperated strings into a temporary table.
				string insertQuery = string.Format("Insert into {0} (NetworkId, {1}) select id, {2} from dbo.IdsToTable('{3}') where id not in (select NetworkId from {0} where {1}={2});",
					TableName,
					RelateToField,
					ItemId,
					networks);

				try
				{
					DBUtils.ExecuteQuery(insertQuery, cte.lib);
				}
				catch (Exception ex)
				{
					lw.Error.Handler.HandleError("Fail to insert new Related Networks, class: lw.Networking.NetworkRelations.UpdateItemRelations", ex);
					throw (ex);
				}
			}
		}


		/// <summary>
		/// Returns the query that will filter the items depending on the member networks
		/// Can be used inside any other Select on a table that is network bound.
		/// </summary>
		/// <param name="TableName">Table Name ex: DownloadNetworks</param>
		/// <param name="RelateToField">Field Name ex: DownloadId</param>
		/// <param name="MemberId">MemberId Profile.UserId</param>
		/// <returns></returns>
		public string GetRelationQueryByMember(string TableName, string RelateToField, int MemberId)
		{
			string query = string.Format(@"{0} in (select {0} from {1} where NetworkId in 
	(Select NetworkId from MemberNetworks where MemberId={2}))",
					RelateToField, TableName, MemberId);
			return query;
		}
		public string GetRelationQueryByNetwork(string TableName, string RelateToField, int NetworkId)
		{
			string query = string.Format(@"{0} in (select {0} from {1} where networkId = {2})", RelateToField, TableName, NetworkId);
			return query;
		}
	}
}