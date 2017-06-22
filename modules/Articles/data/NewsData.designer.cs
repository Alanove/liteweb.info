namespace lw.Articles
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
			this.sqlConnection2 = new System.Data.SqlClient.SqlConnection();
			this.sqlSelectCommand5 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsViewAdp = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand7 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand5 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand5 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand5 = new System.Data.SqlClient.SqlCommand();
			this._sqlCalendarAdp = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand8 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand6 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand6 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand6 = new System.Data.SqlClient.SqlCommand();
			this._sqlCalendarCategories = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand9 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsDateView = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand6 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand4 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand4 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand4 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsTypes = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand3 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsletterUsersAdp = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand4 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsletterView = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this._sqlNewsAdp = new System.Data.SqlClient.SqlDataAdapter();
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
			// sqlConnection2
			// 
			this.sqlConnection2.ConnectionString = "Data Source=DSGN10\\Sql2005;Initial Catalog=ism_2013;Persist Security Info=True;Us" +
    "er ID=sa;Password=sabis";
			this.sqlConnection2.FireInfoMessageEventOnUserErrors = false;
			// 
			// sqlSelectCommand5
			// 
			this.sqlSelectCommand5.CommandText = "Select * from newsView";
			// 
			// _sqlNewsViewAdp
			// 
			this._sqlNewsViewAdp.SelectCommand = this.sqlSelectCommand5;
			this._sqlNewsViewAdp.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "NewsView", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("NewsId", "NewsId"),
                        new System.Data.Common.DataColumnMapping("Title", "Title"),
                        new System.Data.Common.DataColumnMapping("Status", "Status"),
                        new System.Data.Common.DataColumnMapping("Header", "Header"),
                        new System.Data.Common.DataColumnMapping("NewsText", "NewsText"),
                        new System.Data.Common.DataColumnMapping("NewsDate", "NewsDate"),
                        new System.Data.Common.DataColumnMapping("NewsType", "NewsType"),
                        new System.Data.Common.DataColumnMapping("NewsLanguage", "NewsLanguage"),
                        new System.Data.Common.DataColumnMapping("TypeName", "TypeName"),
                        new System.Data.Common.DataColumnMapping("NewsFile", "NewsFile")})});
			// 
			// sqlSelectCommand7
			// 
			this.sqlSelectCommand7.CommandText = "select * from Calendar";
			// 
			// sqlInsertCommand5
			// 
			this.sqlInsertCommand5.CommandText = resources.GetString("sqlInsertCommand5.CommandText");
			this.sqlInsertCommand5.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Date", System.Data.SqlDbType.DateTime, 0, "Date"),
            new System.Data.SqlClient.SqlParameter("@CategoryId", System.Data.SqlDbType.Int, 0, "CategoryId"),
            new System.Data.SqlClient.SqlParameter("@Status", System.Data.SqlDbType.TinyInt, 0, "Status"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.VarChar, 0, "Description"),
            new System.Data.SqlClient.SqlParameter("@Description_FR", System.Data.SqlDbType.NVarChar, 0, "Description_FR"),
            new System.Data.SqlClient.SqlParameter("@Description_AR", System.Data.SqlDbType.NVarChar, 0, "Description_AR")});
			// 
			// sqlUpdateCommand5
			// 
			this.sqlUpdateCommand5.CommandText = resources.GetString("sqlUpdateCommand5.CommandText");
			this.sqlUpdateCommand5.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Date", System.Data.SqlDbType.DateTime, 8, "Date"),
            new System.Data.SqlClient.SqlParameter("@CategoryId", System.Data.SqlDbType.Int, 4, "CategoryId"),
            new System.Data.SqlClient.SqlParameter("@Status", System.Data.SqlDbType.TinyInt, 1, "Status"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.VarChar, 250, "Description"),
            new System.Data.SqlClient.SqlParameter("@Description_FR", System.Data.SqlDbType.NVarChar, 250, "Description_FR"),
            new System.Data.SqlClient.SqlParameter("@Description_AR", System.Data.SqlDbType.NVarChar, 250, "Description_AR"),
            new System.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Id", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlDeleteCommand5
			// 
			this.sqlDeleteCommand5.CommandText = "DELETE FROM Calendar\r\nWHERE     (Id = @Id)";
			this.sqlDeleteCommand5.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Id", System.Data.DataRowVersion.Original, null)});
			// 
			// _sqlCalendarAdp
			// 
			this._sqlCalendarAdp.DeleteCommand = this.sqlDeleteCommand5;
			this._sqlCalendarAdp.InsertCommand = this.sqlInsertCommand5;
			this._sqlCalendarAdp.SelectCommand = this.sqlSelectCommand7;
			this._sqlCalendarAdp.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "Calendar", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("Id", "Id"),
                        new System.Data.Common.DataColumnMapping("Date", "Date"),
                        new System.Data.Common.DataColumnMapping("CategoryId", "CategoryId"),
                        new System.Data.Common.DataColumnMapping("Status", "Status"),
                        new System.Data.Common.DataColumnMapping("Description", "Description"),
                        new System.Data.Common.DataColumnMapping("Description_FR", "Description_FR"),
                        new System.Data.Common.DataColumnMapping("Description_AR", "Description_AR")})});
			this._sqlCalendarAdp.UpdateCommand = this.sqlUpdateCommand5;
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
			// sqlSelectCommand9
			// 
			this.sqlSelectCommand9.CommandText = "select * from NewsDateView";
			// 
			// _sqlNewsDateView
			// 
			this._sqlNewsDateView.SelectCommand = this.sqlSelectCommand9;
			this._sqlNewsDateView.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "NewsDateView", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("NewsMonthName", "NewsMonthName"),
                        new System.Data.Common.DataColumnMapping("NewsMonth", "NewsMonth")})});
			// 
			// sqlSelectCommand6
			// 
			this.sqlSelectCommand6.CommandText = resources.GetString("sqlSelectCommand6.CommandText");
			this.sqlSelectCommand6.Connection = this.sqlConnection2;
			// 
			// sqlInsertCommand4
			// 
			this.sqlInsertCommand4.CommandText = resources.GetString("sqlInsertCommand4.CommandText");
			this.sqlInsertCommand4.Connection = this.sqlConnection2;
			this.sqlInsertCommand4.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 256, "Name"),
            new System.Data.SqlClient.SqlParameter("@UniqueName", System.Data.SqlDbType.NVarChar, 256, "UniqueName"),
            new System.Data.SqlClient.SqlParameter("@DateCreated", System.Data.SqlDbType.DateTime, 8, "DateCreated"),
            new System.Data.SqlClient.SqlParameter("@LastModified", System.Data.SqlDbType.DateTime, 8, "LastModified"),
            new System.Data.SqlClient.SqlParameter("@ParentId", System.Data.SqlDbType.Int, 4, "ParentId")});
			// 
			// sqlUpdateCommand4
			// 
			this.sqlUpdateCommand4.CommandText = resources.GetString("sqlUpdateCommand4.CommandText");
			this.sqlUpdateCommand4.Connection = this.sqlConnection2;
			this.sqlUpdateCommand4.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 256, "Name"),
            new System.Data.SqlClient.SqlParameter("@UniqueName", System.Data.SqlDbType.NVarChar, 256, "UniqueName"),
            new System.Data.SqlClient.SqlParameter("@TypeId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "TypeId", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlDeleteCommand4
			// 
			this.sqlDeleteCommand4.CommandText = "DELETE FROM NewsTypes\r\nWHERE     (TypeId = @TypeId)";
			this.sqlDeleteCommand4.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@TypeId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "TypeId", System.Data.DataRowVersion.Original, null)});
			// 
			// _sqlNewsTypes
			// 
			this._sqlNewsTypes.DeleteCommand = this.sqlDeleteCommand4;
			this._sqlNewsTypes.InsertCommand = this.sqlInsertCommand4;
			this._sqlNewsTypes.SelectCommand = this.sqlSelectCommand6;
			this._sqlNewsTypes.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "NewsTypes", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("TypeId", "TypeId"),
                        new System.Data.Common.DataColumnMapping("Name", "Name"),
                        new System.Data.Common.DataColumnMapping("UniqueName", "UniqueName")})});
			this._sqlNewsTypes.UpdateCommand = this.sqlUpdateCommand4;
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
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = "DELETE FROM News\r\nWHERE     (NewsId = @NewsId)";
			this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@NewsId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "NewsId", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
			this.sqlUpdateCommand1.Connection = this.sqlConnection2;
			this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 150, "Title"),
            new System.Data.SqlClient.SqlParameter("@Status", System.Data.SqlDbType.TinyInt, 1, "Status"),
            new System.Data.SqlClient.SqlParameter("@Header", System.Data.SqlDbType.NVarChar, 512, "Header"),
            new System.Data.SqlClient.SqlParameter("@NewsText", System.Data.SqlDbType.NText, 1073741823, "NewsText"),
            new System.Data.SqlClient.SqlParameter("@NewsDate", System.Data.SqlDbType.DateTime, 8, "NewsDate"),
            new System.Data.SqlClient.SqlParameter("@NewsType", System.Data.SqlDbType.Int, 4, "NewsType"),
            new System.Data.SqlClient.SqlParameter("@NewsLanguage", System.Data.SqlDbType.SmallInt, 2, "NewsLanguage"),
            new System.Data.SqlClient.SqlParameter("@NewsFile", System.Data.SqlDbType.NVarChar, 150, "NewsFile"),
            new System.Data.SqlClient.SqlParameter("@ModifierId", System.Data.SqlDbType.Int, 4, "ModifierId"),
            new System.Data.SqlClient.SqlParameter("@DateModified", System.Data.SqlDbType.DateTime, 8, "DateModified"),
            new System.Data.SqlClient.SqlParameter("@Ranking", System.Data.SqlDbType.Int, 4, "Ranking"),
            new System.Data.SqlClient.SqlParameter("@Views", System.Data.SqlDbType.Int, 4, "Views"),
            new System.Data.SqlClient.SqlParameter("@UserRating", System.Data.SqlDbType.Int, 4, "UserRating"),
            new System.Data.SqlClient.SqlParameter("@PublishDate", System.Data.SqlDbType.DateTime, 8, "PublishDate"),
            new System.Data.SqlClient.SqlParameter("@NewsId", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "NewsId", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = resources.GetString("sqlInsertCommand1.CommandText");
			this.sqlInsertCommand1.Connection = this.sqlConnection2;
			this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 256, "Title"),
            new System.Data.SqlClient.SqlParameter("@Status", System.Data.SqlDbType.TinyInt, 1, "Status"),
            new System.Data.SqlClient.SqlParameter("@Header", System.Data.SqlDbType.NVarChar, 512, "Header"),
            new System.Data.SqlClient.SqlParameter("@NewsText", System.Data.SqlDbType.NText, 1073741823, "NewsText"),
            new System.Data.SqlClient.SqlParameter("@NewsDate", System.Data.SqlDbType.DateTime, 8, "NewsDate"),
            new System.Data.SqlClient.SqlParameter("@NewsType", System.Data.SqlDbType.Int, 4, "NewsType"),
            new System.Data.SqlClient.SqlParameter("@NewsLanguage", System.Data.SqlDbType.SmallInt, 2, "NewsLanguage"),
            new System.Data.SqlClient.SqlParameter("@NewsFile", System.Data.SqlDbType.NVarChar, 150, "NewsFile"),
            new System.Data.SqlClient.SqlParameter("@CreatorId", System.Data.SqlDbType.Int, 4, "CreatorId"),
            new System.Data.SqlClient.SqlParameter("@ModifierId", System.Data.SqlDbType.Int, 4, "ModifierId"),
            new System.Data.SqlClient.SqlParameter("@DateAdded", System.Data.SqlDbType.DateTime, 8, "DateAdded"),
            new System.Data.SqlClient.SqlParameter("@DateModified", System.Data.SqlDbType.DateTime, 8, "DateModified"),
            new System.Data.SqlClient.SqlParameter("@Ranking", System.Data.SqlDbType.Int, 4, "Ranking"),
            new System.Data.SqlClient.SqlParameter("@Views", System.Data.SqlDbType.Int, 4, "Views"),
            new System.Data.SqlClient.SqlParameter("@UserRating", System.Data.SqlDbType.Int, 4, "UserRating"),
            new System.Data.SqlClient.SqlParameter("@publishDate", System.Data.SqlDbType.DateTime, 8, "PublishDate"),
            new System.Data.SqlClient.SqlParameter("@ThumbImage", System.Data.SqlDbType.NVarChar, 150, "ThumbImage"),
            new System.Data.SqlClient.SqlParameter("@LargeImage", System.Data.SqlDbType.NVarChar, 150, "LargeImage"),
            new System.Data.SqlClient.SqlParameter("@MediumImage", System.Data.SqlDbType.NVarChar, 150, "MediumImage")});
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "select * from news";
			// 
			// _sqlNewsAdp
			// 
			this._sqlNewsAdp.DeleteCommand = this.sqlDeleteCommand1;
			this._sqlNewsAdp.InsertCommand = this.sqlInsertCommand1;
			this._sqlNewsAdp.SelectCommand = this.sqlSelectCommand1;
			this._sqlNewsAdp.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "News", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("NewsId", "NewsId"),
                        new System.Data.Common.DataColumnMapping("Title", "Title"),
                        new System.Data.Common.DataColumnMapping("Status", "Status"),
                        new System.Data.Common.DataColumnMapping("Header", "Header"),
                        new System.Data.Common.DataColumnMapping("NewsText", "NewsText"),
                        new System.Data.Common.DataColumnMapping("NewsDate", "NewsDate"),
                        new System.Data.Common.DataColumnMapping("NewsType", "NewsType"),
                        new System.Data.Common.DataColumnMapping("NewsLanguage", "NewsLanguage"),
                        new System.Data.Common.DataColumnMapping("NewsFile", "NewsFile")})});
			this._sqlNewsAdp.UpdateCommand = this.sqlUpdateCommand1;

		}

		#endregion

		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsltetterGroupsAdp;
		private System.Data.SqlClient.SqlCommand _EmptyCommand;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand5;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsViewAdp;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand7;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand5;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand5;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand5;
		private System.Data.SqlClient.SqlDataAdapter _sqlCalendarAdp;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand8;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand6;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand6;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand6;
		private System.Data.SqlClient.SqlDataAdapter _sqlCalendarCategories;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand9;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsDateView;
		private System.Data.SqlClient.SqlConnection sqlConnection2;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand6;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand4;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand4;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand4;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsTypes;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand3;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand3;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand3;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand3;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsletterUsersAdp;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand4;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsletterView;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlDataAdapter _sqlNewsAdp;
		protected System.Data.SqlClient.SqlCommand sqlInsertCommand1;
	}
}
