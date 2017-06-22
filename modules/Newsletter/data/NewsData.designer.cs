namespace lw.Newsletter
{
	partial class NewsData
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewsData));
			this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsltetterGroupsAdp = new System.Data.SqlClient.SqlDataAdapter();
			this._EmptyCommand = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand8 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand6 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand6 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand6 = new System.Data.SqlClient.SqlCommand();
			this._sqlCalendarCategories = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand3 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsletterUsersAdp = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand4 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsletterView = new System.Data.SqlClient.SqlDataAdapter();
			// 
			// sqlSelectCommand2
			// 
			this.sqlSelectCommand2.CommandText = "select * from NewsletterGroups";
			// 
			// sqlInsertCommand2
			// 
			this.sqlInsertCommand2.CommandText = "INSERT INTO [NewsletterGroups] ([GroupName]) VALUES (@GroupName);\r\nSELECT GroupId" +
				", GroupName FROM NewsletterGroups WHERE (GroupId = SCOPE_IDENTITY())";
			this.sqlInsertCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@GroupName", System.Data.SqlDbType.VarChar, 0, "GroupName")});
			// 
			// sqlUpdateCommand2
			// 
			this.sqlUpdateCommand2.CommandText = "UPDATE    NewsletterGroups\r\nSET              GroupName = @GroupName\r\nWHERE     (G" +
				"roupId = @GroupId); \r\nSELECT GroupId, GroupName FROM NewsletterGroups WHERE (Gro" +
				"upId = @GroupId)";
			this.sqlUpdateCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@GroupName", System.Data.SqlDbType.VarChar, 50, "GroupName"),
            new System.Data.SqlClient.SqlParameter("@GroupId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "GroupId", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlDeleteCommand2
			// 
			this.sqlDeleteCommand2.CommandText = "DELETE FROM NewsletterGroups\r\nWHERE     (GroupId = @GroupId)";
			this.sqlDeleteCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@GroupId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "GroupId", System.Data.DataRowVersion.Original, null)});
			// 
			// _sqlNewsltetterGroupsAdp
			// 
			this._sqlNewsltetterGroupsAdp.DeleteCommand = this.sqlDeleteCommand2;
			this._sqlNewsltetterGroupsAdp.InsertCommand = this.sqlInsertCommand2;
			this._sqlNewsltetterGroupsAdp.SelectCommand = this.sqlSelectCommand2;
			this._sqlNewsltetterGroupsAdp.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "NewsletterGroups", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("GroupId", "GroupId"),
                        new System.Data.Common.DataColumnMapping("GroupName", "GroupName")})});
			this._sqlNewsltetterGroupsAdp.UpdateCommand = this.sqlUpdateCommand2;
			// 
			// sqlSelectCommand8
			// 
			this.sqlSelectCommand8.CommandText = "select * from CalendarCategories";
			// 
			// sqlInsertCommand6
			// 
			this.sqlInsertCommand6.CommandText = "INSERT INTO [CalendarCategories] ([CategoryName]) VALUES (@CategoryName);\r\nSELECT" +
				" CategoryId, CategoryName FROM CalendarCategories WHERE (CategoryId = SCOPE_IDEN" +
				"TITY())";
			this.sqlInsertCommand6.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.VarChar, 0, "CategoryName")});
			// 
			// sqlUpdateCommand6
			// 
			this.sqlUpdateCommand6.CommandText = resources.GetString("sqlUpdateCommand6.CommandText");
			this.sqlUpdateCommand6.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.VarChar, 0, "CategoryName"),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryId", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CategoryName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@CategoryId", System.Data.SqlDbType.Int, 4, "CategoryId")});
			// 
			// sqlDeleteCommand6
			// 
			this.sqlDeleteCommand6.CommandText = "DELETE FROM [CalendarCategories] WHERE (([CategoryId] = @Original_CategoryId) AND" +
				" ((@IsNull_CategoryName = 1 AND [CategoryName] IS NULL) OR ([CategoryName] = @Or" +
				"iginal_CategoryName)))";
			this.sqlDeleteCommand6.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_CategoryId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryId", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CategoryName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null)});
			// 
			// _sqlCalendarCategories
			// 
			this._sqlCalendarCategories.DeleteCommand = this.sqlDeleteCommand6;
			this._sqlCalendarCategories.InsertCommand = this.sqlInsertCommand6;
			this._sqlCalendarCategories.SelectCommand = this.sqlSelectCommand8;
			this._sqlCalendarCategories.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "CalendarCategories", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("CategoryId", "CategoryId"),
                        new System.Data.Common.DataColumnMapping("CategoryName", "CategoryName")})});
			this._sqlCalendarCategories.UpdateCommand = this.sqlUpdateCommand6;
			// 
			// sqlSelectCommand3
			// 
			this.sqlSelectCommand3.CommandText = "select * from NewsletterUsers";
			// 
			// sqlInsertCommand3
			// 
			this.sqlInsertCommand3.CommandText = resources.GetString("sqlInsertCommand3.CommandText");
			this.sqlInsertCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 0, "Name"),
            new System.Data.SqlClient.SqlParameter("@Email", System.Data.SqlDbType.VarChar, 0, "Email"),
            new System.Data.SqlClient.SqlParameter("@Gender", System.Data.SqlDbType.Bit, 0, "Gender"),
            new System.Data.SqlClient.SqlParameter("@GroupId", System.Data.SqlDbType.Int, 0, "GroupId"),
            new System.Data.SqlClient.SqlParameter("@AgeGroup", System.Data.SqlDbType.Int, 0, "AgeGroup"),
            new System.Data.SqlClient.SqlParameter("@DateAdded", System.Data.SqlDbType.DateTime, 0, "DateAdded"),
            new System.Data.SqlClient.SqlParameter("@Phone", System.Data.SqlDbType.VarChar, 0, "Phone")});
			// 
			// sqlUpdateCommand3
			// 
			this.sqlUpdateCommand3.CommandText = resources.GetString("sqlUpdateCommand3.CommandText");
			this.sqlUpdateCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 150, "Name"),
            new System.Data.SqlClient.SqlParameter("@Email", System.Data.SqlDbType.VarChar, 100, "Email"),
            new System.Data.SqlClient.SqlParameter("@Gender", System.Data.SqlDbType.Bit, 1, "Gender"),
            new System.Data.SqlClient.SqlParameter("@GroupId", System.Data.SqlDbType.Int, 4, "GroupId"),
            new System.Data.SqlClient.SqlParameter("@AgeGroup", System.Data.SqlDbType.Int, 4, "AgeGroup"),
            new System.Data.SqlClient.SqlParameter("@DateAdded", System.Data.SqlDbType.DateTime, 8, "DateAdded"),
            new System.Data.SqlClient.SqlParameter("@Phone", System.Data.SqlDbType.VarChar, 20, "Phone"),
            new System.Data.SqlClient.SqlParameter("@UserId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "UserId", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlDeleteCommand3
			// 
			this.sqlDeleteCommand3.CommandText = "DELETE FROM NewsletterUsers\r\nWHERE     (UserId = @UserId)";
			this.sqlDeleteCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@UserId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "UserId", System.Data.DataRowVersion.Original, null)});
			// 
			// _sqlNewsletterUsersAdp
			// 
			this._sqlNewsletterUsersAdp.DeleteCommand = this.sqlDeleteCommand3;
			this._sqlNewsletterUsersAdp.InsertCommand = this.sqlInsertCommand3;
			this._sqlNewsletterUsersAdp.SelectCommand = this.sqlSelectCommand3;
			this._sqlNewsletterUsersAdp.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "NewsletterUsers", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("UserId", "UserId"),
                        new System.Data.Common.DataColumnMapping("Name", "Name"),
                        new System.Data.Common.DataColumnMapping("Email", "Email"),
                        new System.Data.Common.DataColumnMapping("Gender", "Gender"),
                        new System.Data.Common.DataColumnMapping("GroupId", "GroupId"),
                        new System.Data.Common.DataColumnMapping("AgeGroup", "AgeGroup"),
                        new System.Data.Common.DataColumnMapping("DateAdded", "DateAdded"),
                        new System.Data.Common.DataColumnMapping("Phone", "Phone")})});
			this._sqlNewsletterUsersAdp.UpdateCommand = this.sqlUpdateCommand3;
			// 
			// sqlSelectCommand4
			// 
			this.sqlSelectCommand4.CommandText = "selecT * from NewsletterView";
			// 
			// _sqlNewsletterView
			// 
			this._sqlNewsletterView.SelectCommand = this.sqlSelectCommand4;
			this._sqlNewsletterView.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "newsletterview", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("UserId", "UserId"),
                        new System.Data.Common.DataColumnMapping("Name", "Name"),
                        new System.Data.Common.DataColumnMapping("Email", "Email"),
                        new System.Data.Common.DataColumnMapping("Gender", "Gender"),
                        new System.Data.Common.DataColumnMapping("AgeGroup", "AgeGroup"),
                        new System.Data.Common.DataColumnMapping("DateAdded", "DateAdded"),
                        new System.Data.Common.DataColumnMapping("Phone", "Phone"),
                        new System.Data.Common.DataColumnMapping("GroupId", "GroupId"),
                        new System.Data.Common.DataColumnMapping("GroupName", "GroupName")})});

		}

		#endregion

		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsltetterGroupsAdp;
		private System.Data.SqlClient.SqlCommand _EmptyCommand;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand8;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand6;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand6;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand6;
		private System.Data.SqlClient.SqlDataAdapter _sqlCalendarCategories;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand3;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand3;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand3;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand3;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsletterUsersAdp;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand4;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsletterView;
	}
}
