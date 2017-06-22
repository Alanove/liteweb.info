using System.Data;

using lw.WebTools;

namespace lw.Shipping
{
	public class CustomShippingManager
	{
		CustomShippingDS ds;

		string config = lw.CTE.Content.Shipping;

		public CustomShippingManager()
		{ }

		#region Methods

		public DataView CountryGroups()
		{
			return CountryGroups("");
		}
		public DataView CountryGroups(string cond)
		{
			return new DataView(DS.CountryGroups, cond, "GroupName", DataViewRowState.CurrentRows);
		}

		public DataView RegionGroups()
		{
			return RegionGroups("");
		}
		public DataView RegionGroups(string cond)
		{
			return new DataView(DS.RegionGroups, cond, "GroupName", DataViewRowState.CurrentRows);
		}
		
		public DataView ECountries()
		{
			return ECountries("");
		}
		public DataView ECountries(string cond)
		{
			return new DataView(DS.eCountries, cond, "Name", DataViewRowState.CurrentRows).ToTable(true, new string[] {"Name"}).DefaultView;
		}

		public DataView GetShippingCountries()
		{
			return ECountries();
		}
		public DataView GetLocalRegions()
		{
			return ERegions();
		}

		public DataView ERegions()
		{
			return ERegions("");
		}
		public DataView ERegions(string cond)
		{
			return new DataView(DS.eRegions, cond, "DisplayName", DataViewRowState.CurrentRows);
		}

		public DataView GetWRanges(string GroupId, string Type)
		{
			return new DataView(DS.ShippingWeight, "GroupId=" + GroupId + " and GroupType='" + Type + "'", "UpTo asc", DataViewRowState.CurrentRows);
		}

		public DataView GetCRanges(string GroupId, string Type)
		{
			return new DataView(DS.ShippingOrderCost, "GroupId=" + GroupId + " and GroupType='" + Type + "'", "UpTo asc", DataViewRowState.CurrentRows);
		}

		public void SetWeightValues(string GroupId, DataTable ShippingWeight)
		{
			this._SetGroupTable(GroupId, ShippingWeight, "ShippingWeight", "C");
		}

		public void SetCostValues(string GroupId, DataTable ShippingOrderCost)
		{
			this._SetGroupTable(GroupId, ShippingOrderCost, "ShippingOrderCost", "C");
		}

		public void SetECountries(string GroupId, DataTable eCountries)
		{
			this._SetGroupTable(GroupId, eCountries, "eCountries");
		}

		public void SetERegions(string GroupId, DataTable eRegions)
		{
			this._SetGroupTable(GroupId, eRegions, "eRegions");
		}
		

		public string AddRegionGroup(string GroupName)
		{
			CustomShippingDS.RegionGroupsRow row = DS.RegionGroups.NewRegionGroupsRow();
			row.GroupName = GroupName;
			DS.RegionGroups.Rows.Add(row);
			AcceptChanges();

			return row["GroupId"].ToString();
		}
		public void UpdateRegionGroup(string GroupId, string GroupName)
		{
			DataView dv = RegionGroups(string.Format("GroupId={0}", GroupId));
			if (dv.Count == 0)
				return;
			CustomShippingDS.RegionGroupsRow row = (CustomShippingDS.RegionGroupsRow)dv[0].Row;
			row.GroupName = GroupName;

			row.Table.AcceptChanges();
			AcceptChanges();
		}

		public void DeleteRegionGroup(string GroupId)
		{
			DataView dv = RegionGroups(string.Format("GroupId={0}", GroupId));
			if (dv.Count == 0)
				return;
			CustomShippingDS.RegionGroupsRow row = (CustomShippingDS.RegionGroupsRow)dv[0].Row;


			row.Delete();
			AcceptChanges();

			System.Data.DataRow[] drCountries = DS.eCountries.Select("GroupId=" + GroupId);
			foreach (DataRow _dr in drCountries)
				_dr.Delete();
			AcceptChanges();

			System.Data.DataRow[] drOrderCost = DS.ShippingOrderCost.Select("GroupType='R' and GroupId=" + GroupId);
			foreach (DataRow _dr in drOrderCost)
				_dr.Delete();
			AcceptChanges();

			System.Data.DataRow[] drWeight = DS.ShippingWeight.Select("GroupType='R' and GroupId=" + GroupId);
			foreach (DataRow _dr in drWeight)
				_dr.Delete();
			AcceptChanges();
		}



		public string AddCountryGroup(string GroupName)
		{
			CustomShippingDS.CountryGroupsRow row = DS.CountryGroups.NewCountryGroupsRow();
			row.GroupName = GroupName;
			DS.CountryGroups.Rows.Add(row);
			AcceptChanges();

			return row["GroupId"].ToString();
		}


		public void UpdateCountryGroup(string GroupId, string GroupName)
		{
			DataView dv = CountryGroups(string.Format("GroupId={0}", GroupId));
			if (dv.Count == 0)
				return;
			
			CustomShippingDS.CountryGroupsRow row = (CustomShippingDS.CountryGroupsRow)dv[0].Row;
			row.GroupName = GroupName;

			row.Table.AcceptChanges();
			AcceptChanges();
		}
		public void DeleteCountryGroup(string GroupId)
		{
			DataView dv = CountryGroups(string.Format("GroupId={0}", GroupId));

			if (dv.Count == 0)
				return;
			CustomShippingDS.CountryGroupsRow row = (CustomShippingDS.CountryGroupsRow)dv[0].Row;

			row.Delete();
			AcceptChanges();

			System.Data.DataRow[] drCountries = DS.eCountries.Select("GroupId=" + GroupId);
			foreach (DataRow _dr in drCountries)
				_dr.Delete();
			AcceptChanges();

			System.Data.DataRow[] drOrderCost = DS.ShippingOrderCost.Select("GroupType='C' and GroupId=" + GroupId);
			foreach (DataRow _dr in drOrderCost)
				_dr.Delete();
			AcceptChanges();

			System.Data.DataRow[] drWeight = DS.ShippingWeight.Select("GroupType='C' and GroupId=" + GroupId);
			foreach (DataRow _dr in drWeight)
				_dr.Delete();
			AcceptChanges();
		}

		void _SetGroupTable(string GroupId, DataTable ImTable, string TableName, string Type)
		{
			System.Data.DataRow[] drs = DS.Tables[TableName].Select("GroupId=" + GroupId + " and GroupType='" + Type + "'");
			foreach (DataRow dr in drs)
				dr.Delete();
			DS.Tables[TableName].AcceptChanges();
			foreach (DataRow _mdr in ImTable.Rows)
				DS.Tables[TableName].ImportRow(_mdr);
			AcceptChanges();
		}

		void _SetGroupTable(string GroupId, DataTable ImTable, string TableName)
		{
			System.Data.DataRow[] drs = DS.Tables[TableName].Select("GroupId=" + GroupId);
			foreach (DataRow dr in drs)
				dr.Delete();
			DS.Tables[TableName].AcceptChanges();
			foreach (DataRow _mdr in ImTable.Rows)
				DS.Tables[TableName].ImportRow(_mdr);

			AcceptChanges();
		}

		void AcceptChanges()
		{
			DS.AcceptChanges();
			XmlManager.SetDataSet(config, DS);
		}

		#endregion

		#region variables

		public CustomShippingDS DS
		{
			get
			{
				if (ds == null)
				{
					ds = new CustomShippingDS();
					DataSet _ds = XmlManager.GetDataSet(config);
					if (_ds != null)
						ds.Merge(_ds);
				}
				return ds;
			}
		}

		#endregion
	}
}
