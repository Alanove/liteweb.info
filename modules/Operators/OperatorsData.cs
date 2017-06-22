
using lw.Data; 

namespace lw.Operators
{
	/// <summary>
	/// Summary description for OperatorsData.
	/// </summary>
	public class OperatorsData: ToCastToComponenet
	{
		private System.Data.SqlClient.SqlCommand sqlSelectCommand3;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand3;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private System.Data.SqlClient.SqlDataAdapter AdpT_OperatorGroups;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		protected System.ComponentModel.Container components = null;

		public OperatorsData(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
			initData();
		}

		public OperatorsData()
		{
			InitializeComponent();
			initData();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		protected override void initData()
		{
			this.AddDataComponent(cte.AdbT_OperatorGroups, this.AdpT_OperatorGroups);
		}
		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.sqlSelectCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
			this.AdpT_OperatorGroups = new System.Data.SqlClient.SqlDataAdapter();
			// 
			// sqlSelectCommand3
			// 
			this.sqlSelectCommand3.CommandText = "SELECT GroupId, GroupName, XmlFile FROM OperatorGroups";
			// 
			// sqlInsertCommand3
			// 
			this.sqlInsertCommand3.CommandText = "INSERT INTO OperatorGroups(GroupName, XmlFile) VALUES (@GroupName, @XmlFile); SEL" +
				"ECT GroupId, GroupName, XmlFile FROM OperatorGroups WHERE (GroupId = @@IDENTITY)" +
				"";
			this.sqlInsertCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@GroupName", System.Data.SqlDbType.VarChar, 50, "GroupName"),
            new System.Data.SqlClient.SqlParameter("@XmlFile", System.Data.SqlDbType.VarChar, 50, "XmlFile")});
			// 
			// sqlUpdateCommand2
			// 
			this.sqlUpdateCommand2.CommandText = "UPDATE OperatorGroups SET GroupName = @GroupName, XmlFile = @XmlFile WHERE (Group" +
				"Id = @GroupId)";
			this.sqlUpdateCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@GroupName", System.Data.SqlDbType.VarChar, 50, "GroupName"),
            new System.Data.SqlClient.SqlParameter("@XmlFile", System.Data.SqlDbType.VarChar, 50, "XmlFile"),
            new System.Data.SqlClient.SqlParameter("@GroupId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "GroupId", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlDeleteCommand2
			// 
			this.sqlDeleteCommand2.CommandText = "DELETE FROM OperatorGroups WHERE (GroupId = @GroupId)";
			this.sqlDeleteCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@GroupId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "GroupId", System.Data.DataRowVersion.Original, null)});
			// 
			// AdpT_OperatorGroups
			// 
			this.AdpT_OperatorGroups.DeleteCommand = this.sqlDeleteCommand2;
			this.AdpT_OperatorGroups.InsertCommand = this.sqlInsertCommand3;
			this.AdpT_OperatorGroups.SelectCommand = this.sqlSelectCommand3;
			this.AdpT_OperatorGroups.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "OperatorGroups", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("GroupId", "GroupId"),
                        new System.Data.Common.DataColumnMapping("GroupName", "GroupName"),
                        new System.Data.Common.DataColumnMapping("XmlFile", "XmlFile")})});
			this.AdpT_OperatorGroups.UpdateCommand = this.sqlUpdateCommand2;

		}
		#endregion

	}
}
