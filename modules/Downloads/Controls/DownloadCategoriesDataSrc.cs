using System;
using System.Text;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Networking;
using lw.WebTools;

namespace lw.Downloads.Controls
{
	public class DownloadCategoriesDataSrc : CustomDataSource
	{
		bool _bound = false;
		string _top = "";
		DownloadStatus status = DownloadStatus.All;
		bool _networkBound = false;
		string type = "";
        string _condition = "";

		public DownloadCategoriesDataSrc()
		{
			this.DataLibrary = cte.lib;
			this.OrderBy = "Type Desc";
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			StringBuilder query = new StringBuilder();
 
			query.Append("select ");

			// select top ..
			if(!String.IsNullOrWhiteSpace(Top))
				query.Append("Top " + Top);
			
			query.Append(" D.*, C.[Count] from DownloadTypes D ");
			query.Append("left outer Join");
			query.Append("(select count(*) as [Count],DownloadType as TypeId from Downloads ");

			bool whereAdded = false;

			if (status != DownloadStatus.All)
			{
				if (!whereAdded)
					query.Append(" where ");
				else
					query.Append(" and ");

				whereAdded = true;

				query.Append(string.Format("Status={0}", (int)status));
			}
			else
			{
				if (!whereAdded)
					query.Append(" where ");
				else
					query.Append(" and ");

				whereAdded = true;

				query.Append(string.Format("Status<>{0}", (int)DownloadStatus.Disabled));
			}

			if(NetworkBound)
			{
				NetworkRelations networkRelations = new NetworkRelations();
				if(!whereAdded)
					query.Append(" where ");
				else
					query.Append(" and ");

				whereAdded = true;

				query.Append(networkRelations.GetRelationQueryByMember(cte.NetworkRelationTable, cte.NetworkRelateToField, WebContext.Profile.UserId));
			}
         
			query.Append(" Group By DownloadType) C on D.TypeId = C.TypeId");


            if (!string.IsNullOrWhiteSpace(Condition))
            {
                query.Append(string.Format(" where " + Condition));
            }

			if (CMSMode == true)
			{
				query.Clear();
				query.Append("Select * from DownloadTypes");
			}

			this.SelectCommand = query.ToString();

			base.DataBind();
		}

		public string Top
		{
			get { return _top; }
			set { _top = value; }
		}


		public DownloadStatus Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}

        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

		/// <summary>
		/// Makes the list network bound returns only the items that are related to the current member's network
		/// </summary>
		public bool NetworkBound
		{
			get { return _networkBound && WebContext.Profile.dbUserName != lw.CTE.Admin.SuperAdmin; }
			set { _networkBound = value; }
		}
		
	}

	
}