namespace lw.PhotoAlbums
{
	partial class PhotoAlbumsData
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoAlbumsData));
			this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this._sqlPhotoAlbumCategoriesAdp = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
			this._sqlPhotoAlbumsAdp = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlDeleteCommand3 = new System.Data.SqlClient.SqlCommand();
			this._sqlPhotoAlbumImagesAdp = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlSelectCommand4 = new System.Data.SqlClient.SqlCommand();
			this._sqlAlbumsView = new System.Data.SqlClient.SqlDataAdapter();
			// 
			// sqlConnection1
			// 
			this.sqlConnection1.ConnectionString = "Data Source=DSGN10\\SQL2005;Initial Catalog=digital-signage;Persist Security Info=" +
    "True;User ID=sa;Password=sabis";
			this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "select * from PhotoAlbumsCategories\r\n";
			this.sqlSelectCommand1.Connection = this.sqlConnection1;
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = "INSERT INTO [PhotoAlbumsCategories] ([CategoryName]) VALUES (@CategoryName);\r\nSEL" +
    "ECT CategoryId, CategoryName FROM PhotoAlbumsCategories WHERE (CategoryId = SCOP" +
    "E_IDENTITY())";
			this.sqlInsertCommand1.Connection = this.sqlConnection1;
			this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.VarChar, 0, "CategoryName")});
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
			this.sqlUpdateCommand1.Connection = this.sqlConnection1;
			this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.VarChar, 0, "CategoryName"),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryId", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CategoryName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@CategoryId", System.Data.SqlDbType.Int, 4, "CategoryId")});
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = "DELETE FROM [PhotoAlbumsCategories] WHERE (([CategoryId] = @Original_CategoryId) " +
    "AND ((@IsNull_CategoryName = 1 AND [CategoryName] IS NULL) OR ([CategoryName] = " +
    "@Original_CategoryName)))";
			this.sqlDeleteCommand1.Connection = this.sqlConnection1;
			this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_CategoryId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryId", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CategoryName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null)});
			// 
			// _sqlPhotoAlbumCategoriesAdp
			// 
			this._sqlPhotoAlbumCategoriesAdp.DeleteCommand = this.sqlDeleteCommand1;
			this._sqlPhotoAlbumCategoriesAdp.InsertCommand = this.sqlInsertCommand1;
			this._sqlPhotoAlbumCategoriesAdp.SelectCommand = this.sqlSelectCommand1;
			this._sqlPhotoAlbumCategoriesAdp.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "PhotoAlbumsCategories", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("CategoryId", "CategoryId"),
                        new System.Data.Common.DataColumnMapping("CategoryName", "CategoryName")})});
			this._sqlPhotoAlbumCategoriesAdp.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlSelectCommand2
			// 
			this.sqlSelectCommand2.CommandText = "SELECT        Id, Name, DisplayName, Description, Status, Sort, Image, HasIntroPa" +
    "ge, CategoryId, DateAdded, DateModified, Language, AlbumDate\r\nFROM            Ph" +
    "otoAlbums";
			this.sqlSelectCommand2.Connection = this.sqlConnection1;
			// 
			// sqlInsertCommand2
			// 
			this.sqlInsertCommand2.CommandText = resources.GetString("sqlInsertCommand2.CommandText");
			this.sqlInsertCommand2.Connection = this.sqlConnection1;
			this.sqlInsertCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 150, "Name"),
            new System.Data.SqlClient.SqlParameter("@DisplayName", System.Data.SqlDbType.NVarChar, 150, "DisplayName"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NText, 1073741823, "Description"),
            new System.Data.SqlClient.SqlParameter("@Status", System.Data.SqlDbType.Bit, 1, "Status"),
            new System.Data.SqlClient.SqlParameter("@Sort", System.Data.SqlDbType.Int, 4, "Sort"),
            new System.Data.SqlClient.SqlParameter("@Image", System.Data.SqlDbType.VarChar, 150, "Image"),
            new System.Data.SqlClient.SqlParameter("@HasIntroPage", System.Data.SqlDbType.Bit, 1, "HasIntroPage"),
            new System.Data.SqlClient.SqlParameter("@CategoryId", System.Data.SqlDbType.Int, 4, "CategoryId"),
            new System.Data.SqlClient.SqlParameter("@DateAdded", System.Data.SqlDbType.DateTime, 8, "DateAdded"),
            new System.Data.SqlClient.SqlParameter("@DateModified", System.Data.SqlDbType.DateTime, 8, "DateModified"),
            new System.Data.SqlClient.SqlParameter("@Language", System.Data.SqlDbType.SmallInt, 2, "Language"),
            new System.Data.SqlClient.SqlParameter("@AlbumDate", System.Data.SqlDbType.DateTime, 8, "AlbumDate")});
			// 
			// sqlUpdateCommand2
			// 
			this.sqlUpdateCommand2.CommandText = resources.GetString("sqlUpdateCommand2.CommandText");
			this.sqlUpdateCommand2.Connection = this.sqlConnection1;
			this.sqlUpdateCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 150, "Name"),
            new System.Data.SqlClient.SqlParameter("@DisplayName", System.Data.SqlDbType.NVarChar, 150, "DisplayName"),
            new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NText, 1073741823, "Description"),
            new System.Data.SqlClient.SqlParameter("@Status", System.Data.SqlDbType.Bit, 1, "Status"),
            new System.Data.SqlClient.SqlParameter("@Sort", System.Data.SqlDbType.Int, 4, "Sort"),
            new System.Data.SqlClient.SqlParameter("@Image", System.Data.SqlDbType.VarChar, 150, "Image"),
            new System.Data.SqlClient.SqlParameter("@HasIntroPage", System.Data.SqlDbType.Bit, 1, "HasIntroPage"),
            new System.Data.SqlClient.SqlParameter("@CategoryId", System.Data.SqlDbType.Int, 4, "CategoryId"),
            new System.Data.SqlClient.SqlParameter("@DateAdded", System.Data.SqlDbType.DateTime, 8, "DateAdded"),
            new System.Data.SqlClient.SqlParameter("@DateModified", System.Data.SqlDbType.DateTime, 8, "DateModified"),
            new System.Data.SqlClient.SqlParameter("@Language", System.Data.SqlDbType.SmallInt, 2, "Language"),
            new System.Data.SqlClient.SqlParameter("@AlbumDate", System.Data.SqlDbType.DateTime, 8, "AlbumDate"),
            new System.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Id", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlDeleteCommand2
			// 
			this.sqlDeleteCommand2.CommandText = resources.GetString("sqlDeleteCommand2.CommandText");
			this.sqlDeleteCommand2.Connection = this.sqlConnection1;
			this.sqlDeleteCommand2.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_Id", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Id", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Name", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_DisplayName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "DisplayName", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_DisplayName", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DisplayName", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Description", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Description", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Status", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Status", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Status", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Status", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Sort", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Sort", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Sort", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Sort", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Image", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Image", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Image", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Image", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_HasIntroPage", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "HasIntroPage", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_HasIntroPage", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "HasIntroPage", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CategoryId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CategoryId", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CategoryId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CategoryId", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_DateAdded", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "DateAdded", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_DateAdded", System.Data.SqlDbType.DateTime, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DateAdded", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_DateModified", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "DateModified", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_DateModified", System.Data.SqlDbType.DateTime, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "DateModified", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_Language", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Language", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_Language", System.Data.SqlDbType.SmallInt, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Language", System.Data.DataRowVersion.Original, null)});
			// 
			// _sqlPhotoAlbumsAdp
			// 
			this._sqlPhotoAlbumsAdp.DeleteCommand = this.sqlDeleteCommand2;
			this._sqlPhotoAlbumsAdp.InsertCommand = this.sqlInsertCommand2;
			this._sqlPhotoAlbumsAdp.SelectCommand = this.sqlSelectCommand2;
			this._sqlPhotoAlbumsAdp.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "PhotoAlbums", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("Id", "Id"),
                        new System.Data.Common.DataColumnMapping("Name", "Name"),
                        new System.Data.Common.DataColumnMapping("DisplayName", "DisplayName"),
                        new System.Data.Common.DataColumnMapping("Description", "Description"),
                        new System.Data.Common.DataColumnMapping("Status", "Status"),
                        new System.Data.Common.DataColumnMapping("Sort", "Sort"),
                        new System.Data.Common.DataColumnMapping("Image", "Image"),
                        new System.Data.Common.DataColumnMapping("HasIntroPage", "HasIntroPage"),
                        new System.Data.Common.DataColumnMapping("CategoryId", "CategoryId"),
                        new System.Data.Common.DataColumnMapping("DateAdded", "DateAdded"),
                        new System.Data.Common.DataColumnMapping("DateModified", "DateModified"),
                        new System.Data.Common.DataColumnMapping("Language", "Language")})});
			this._sqlPhotoAlbumsAdp.UpdateCommand = this.sqlUpdateCommand2;
			// 
			// sqlSelectCommand3
			// 
			this.sqlSelectCommand3.CommandText = "SELECT     Id, AlbumId, Sort, Caption, FileName, DateAdded, DateModified\r\nFROM   " +
    "      PhotoAlbumImages";
			this.sqlSelectCommand3.Connection = this.sqlConnection1;
			// 
			// sqlInsertCommand3
			// 
			this.sqlInsertCommand3.CommandText = resources.GetString("sqlInsertCommand3.CommandText");
			this.sqlInsertCommand3.Connection = this.sqlConnection1;
			this.sqlInsertCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@AlbumId", System.Data.SqlDbType.Int, 0, "AlbumId"),
            new System.Data.SqlClient.SqlParameter("@Sort", System.Data.SqlDbType.Int, 0, "Sort"),
            new System.Data.SqlClient.SqlParameter("@Caption", System.Data.SqlDbType.VarChar, 0, "Caption"),
            new System.Data.SqlClient.SqlParameter("@FileName", System.Data.SqlDbType.VarChar, 0, "FileName"),
            new System.Data.SqlClient.SqlParameter("@DateAdded", System.Data.SqlDbType.DateTime, 0, "DateAdded"),
            new System.Data.SqlClient.SqlParameter("@DateModified", System.Data.SqlDbType.DateTime, 0, "DateModified")});
			// 
			// sqlUpdateCommand3
			// 
			this.sqlUpdateCommand3.CommandText = resources.GetString("sqlUpdateCommand3.CommandText");
			this.sqlUpdateCommand3.Connection = this.sqlConnection1;
			this.sqlUpdateCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@AlbumId", System.Data.SqlDbType.Int, 4, "AlbumId"),
            new System.Data.SqlClient.SqlParameter("@Sort", System.Data.SqlDbType.Int, 4, "Sort"),
            new System.Data.SqlClient.SqlParameter("@Caption", System.Data.SqlDbType.VarChar, 250, "Caption"),
            new System.Data.SqlClient.SqlParameter("@FileName", System.Data.SqlDbType.VarChar, 150, "FileName"),
            new System.Data.SqlClient.SqlParameter("@DateAdded", System.Data.SqlDbType.DateTime, 8, "DateAdded"),
            new System.Data.SqlClient.SqlParameter("@DateModified", System.Data.SqlDbType.DateTime, 8, "DateModified"),
            new System.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Id", System.Data.DataRowVersion.Original, null)});
			// 
			// sqlDeleteCommand3
			// 
			this.sqlDeleteCommand3.CommandText = "DELETE FROM PhotoAlbumImages\r\nWHERE     (Id = @Id)";
			this.sqlDeleteCommand3.Connection = this.sqlConnection1;
			this.sqlDeleteCommand3.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Id", System.Data.DataRowVersion.Original, null)});
			// 
			// _sqlPhotoAlbumImagesAdp
			// 
			this._sqlPhotoAlbumImagesAdp.DeleteCommand = this.sqlDeleteCommand3;
			this._sqlPhotoAlbumImagesAdp.InsertCommand = this.sqlInsertCommand3;
			this._sqlPhotoAlbumImagesAdp.SelectCommand = this.sqlSelectCommand3;
			this._sqlPhotoAlbumImagesAdp.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "PhotoAlbumImages", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("Id", "Id"),
                        new System.Data.Common.DataColumnMapping("AlbumId", "AlbumId"),
                        new System.Data.Common.DataColumnMapping("Sort", "Sort"),
                        new System.Data.Common.DataColumnMapping("Caption", "Caption"),
                        new System.Data.Common.DataColumnMapping("FileName", "FileName"),
                        new System.Data.Common.DataColumnMapping("DateAdded", "DateAdded"),
                        new System.Data.Common.DataColumnMapping("DateModified", "DateModified")})});
			this._sqlPhotoAlbumImagesAdp.UpdateCommand = this.sqlUpdateCommand3;
			// 
			// sqlSelectCommand4
			// 
			this.sqlSelectCommand4.CommandText = resources.GetString("sqlSelectCommand4.CommandText");
			this.sqlSelectCommand4.Connection = this.sqlConnection1;
			// 
			// _sqlAlbumsView
			// 
			this._sqlAlbumsView.SelectCommand = this.sqlSelectCommand4;
			this._sqlAlbumsView.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "AlbumsView", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("Id", "Id"),
                        new System.Data.Common.DataColumnMapping("Name", "Name"),
                        new System.Data.Common.DataColumnMapping("DisplayName", "DisplayName"),
                        new System.Data.Common.DataColumnMapping("Description", "Description"),
                        new System.Data.Common.DataColumnMapping("Status", "Status"),
                        new System.Data.Common.DataColumnMapping("Sort", "Sort"),
                        new System.Data.Common.DataColumnMapping("Image", "Image"),
                        new System.Data.Common.DataColumnMapping("HasIntroPage", "HasIntroPage"),
                        new System.Data.Common.DataColumnMapping("CategoryId", "CategoryId"),
                        new System.Data.Common.DataColumnMapping("DateAdded", "DateAdded"),
                        new System.Data.Common.DataColumnMapping("DateModified", "DateModified"),
                        new System.Data.Common.DataColumnMapping("Language", "Language"),
                        new System.Data.Common.DataColumnMapping("UniqueName", "UniqueName"),
                        new System.Data.Common.DataColumnMapping("ImageCount", "ImageCount"),
                        new System.Data.Common.DataColumnMapping("CategoryName", "CategoryName")})});

		}

		#endregion

		private System.Data.SqlClient.SqlConnection sqlConnection1;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Data.SqlClient.SqlDataAdapter _sqlPhotoAlbumCategoriesAdp;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private System.Data.SqlClient.SqlDataAdapter _sqlPhotoAlbumsAdp;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand3;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand3;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand3;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand3;
		private System.Data.SqlClient.SqlDataAdapter _sqlPhotoAlbumImagesAdp;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand4;
		private System.Data.SqlClient.SqlDataAdapter _sqlAlbumsView;
	}
}
