using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using lw.CTE;
using lw.CTE.Enum;
using lw.Utils;

namespace lw.Data
{
	/// <summary>
	/// Summary description for DataUtils.
	/// </summary>
	public class DBUtils
	{
		const string lwRanDomColumn = "_liteweb_ran_guid";

		/// <summary>
		/// Randomize the set of rows inside a datable
		/// by adding a random field and return the table sorted by that field
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <param name="n">Number of wanted rows</param>
		/// <returns>DataView containing the randomized data</returns>
		public static DataView Randomize(DataTable dt, int n)
		{
			System.Guid geuid = System.Guid.NewGuid();
			if (!dt.Columns.Contains(lwRanDomColumn))
				dt.Columns.Add(lwRanDomColumn, geuid.GetType());
			
			foreach(DataRow dr in dt.Rows)
				dr[lwRanDomColumn] = System.Guid.NewGuid();

			dt.AcceptChanges();

			return GetTop(new DataView(dt, "", lwRanDomColumn, DataViewRowState.CurrentRows), n, lwRanDomColumn);
		}
		/// <summary>
		/// Randomize the set of rows inside a datable
		/// by adding a random field and return the table sorted by that field
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <param name="n">Number of wanted rows</param>
		/// <returns>DataView containing the randomized data</returns>
		public static DataView Randomize(DataTable dt)
		{
			return Randomize(dt, dt.Rows.Count);
		}

		/// <summary>
		/// returns the top (n) rows of a dataview sorted by the sort parameter
		/// </summary>
		/// <param name="dv">DataView </param>
		/// <param name="n">Number of desired rows</param>
		/// <param name="sort">The sort field</param>
		/// <returns>DataView</returns>
		public static DataView GetTop(DataView dv, int n, string sort)
		{
			DataTable dt = dv.Table.Clone();

			dv.Sort = sort;

			for(int i = 0; i < Math.Min(dv.Count, n); i++)
			{
				dt.ImportRow(dv[i].Row);
			}

			return new DataView(dt, "", sort, DataViewRowState.CurrentRows);
		}

		/// <summary>
		/// Executes the sql statement and saves the results in a DataSet
		/// If multiple querries are defined, DataSet will contain more than one table
		/// </summary>
		/// <param name="sql">SQL String ex: select * from Products</param>
		/// <param name="connection">The Connection String</param>
		/// <returns>DataSet Containing the querry execution results</returns>
		public static DataSet GetDataSet(string sql, SqlConnection connection)
		{
			SqlCommand cmd = new SqlCommand();

			cmd.CommandText = sql;


			return GetDataSet(cmd, connection);
		}


		/// <summary>
		/// Executes the sql statement and saves the results in a DataSet
		/// If multiple querries are defined, DataSet will contain more than one table
		/// </summary>
		/// <param name="command">Any SQL Command or Stored Procedure</param>
		/// <param name="connection">The connection used in this execution</param>
		/// <returns>DataSet Containing the querry execution results</returns>
		public static DataSet GetDataSet(SqlCommand command, SqlConnection connection)
		{
			SqlDataAdapter adp = new SqlDataAdapter(command);
			command.Connection = connection;

			DataSet ds = new DataSet();

			adp.Fill(ds);
			return ds;
		}

		/// <summary>
		/// Executes the sql statement and saves the results in a DataSet
		/// If multiple querries are defined, DataSet will contain more than one table
		/// </summary>
		/// <param name="command">Any SQL Command or Stored Procedure</param>
		/// <param name="library">The related data library</param>
		/// <returns>DataSet Containing the querry execution results</returns>
		public static DataSet GetDataSet(SqlCommand command, string library)
		{
			DirectorBase db = new DirectorBase(library);
			SqlConnection connection = (SqlConnection)db.GetConnection();

			return GetDataSet(command, connection);
		}


		/// <summary>
		/// Executes the sql statement and saves the results in a DataSet
		/// If multiple querries are defined, DataSet will contain more than one table
		/// </summary>
		/// <param name="sql">SQL String ex: select * from Products</param>
		/// <param name="library">The related data library</param>
		/// <returns>DataSet Containing the querry execution results</returns>
		public static DataSet GetDataSet(string sql, string library)
		{
			DirectorBase db = new DirectorBase(library);
			SqlConnection connection = (SqlConnection)db.GetConnection();

			return GetDataSet(sql, connection);
		}

		/// <summary>
		/// Creates a stored procedure command
		/// this methos only created the Command
		/// in order to execute you must call the Command.Execute or any relevant function
		/// </summary>
		/// <param name="name">Stored Procedure name</param>
		/// <param name="library">DataLibrary</param>
		/// <returns>The procedure as command, parameters can be added using the AddProcedureParameter method</returns>
		public static SqlCommand StoredProcedure(string name, string library)
		{
			DirectorBase db = new DirectorBase(library);
			SqlConnection connection = (SqlConnection)db.GetConnection();

			SqlCommand command = new SqlCommand();
			command.CommandText = name;
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			return command;
		}

		/// <summary>
		/// Adds a parameter to an SqlCommand
		/// </summary>
		/// <param name="command">SqlCommand</param>
		/// <param name="parameter">Name of the parameter</param>
		/// <param name="type">ParameterType (SqlDbType)</param>
		/// <param name="value">Value</param>
		/// <param name="dir">Direction</param>
		public static void AddCommandParameter(SqlCommand command, string parameter, SqlDbType type, object value, ParameterDirection dir)
		{
			command.Parameters.Add(parameter, type);
			command.Parameters[parameter].Direction = dir;
			command.Parameters[parameter].Value = value;
		}
		/// <summary>
		/// This method is old use AddCommandParameter instead
		/// </summary>
		/// <param name="command"></param>
		/// <param name="parameter"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <param name="dir"></param>
		public static void AddProcedureParameter(SqlCommand command, string parameter, SqlDbType type, object value, ParameterDirection dir)
		{
			AddCommandParameter(command, parameter, type, value, dir);
		}
		
		/// <summary>
		/// Executes an sql query 
		/// </summary>
		/// <param name="sql">SQL String</param>
		/// <param name="library">Data Library</param>
		public static void ExecuteQuery(string sql, string library)
		{
			DirectorBase db = new DirectorBase(library);
			SqlConnection connection = (SqlConnection)db.GetConnection();

			SqlCommand cmd = new SqlCommand();

			cmd.CommandText = sql;
			cmd.Connection = connection;
			cmd.Connection.Open();

			cmd.ExecuteNonQuery();

			cmd.Connection.Close();
		}

		/// <summary>
		/// SqlDataReader (remember to close the reader when finished)
		/// </summary>
		/// <param name="sql">SQL String</param>
		/// <param name="library">DataLibrary</param>
		/// <returns>SqlDataReader instance</returns>
		public static SqlDataReader ExecuteReader(string sql, string library)
		{
			DirectorBase db = new DirectorBase(library);
			SqlConnection connection = (SqlConnection)db.GetConnection();

			SqlCommand cmd = new SqlCommand();

			cmd.CommandText = sql;
			cmd.Connection = connection;
			cmd.Connection.Open();

			return cmd.ExecuteReader(CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Checks if a table already exists in the database of library (lib)
		/// </summary>
		/// <param name="tableName">Table Name</param>
		/// <param name="lib">Library name linked to a web.config connection string (check DataBase.Config  in Prv/Conf)</param>
		/// <returns>True if the table exists, false if not</returns>
		public static bool TableExists(string tableName, string lib)
		{
			string sql = "SELECT * FROM SysObjects WHERE [Name] = '{0}' And Type='U'";
			sql = string.Format(sql, StringUtils.SQLEncode(tableName));

			return GetDataSet(sql, lib).Tables[0].Rows.Count > 0;
		}

		/// <summary>
		/// Checks if a view already exists in the database of library (lib)
		/// </summary>
		/// <param name="tableName">View Name</param>
		/// <param name="lib">Library name linked to a web.config connection string (check DataBase.Config  in Prv/Conf)</param>
		/// <returns>True if the view exists, false if not</returns>
		public static bool ViewExists(string viewName, string lib)
		{
			string sql = "SELECT * FROM SysObjects WHERE [Name] = '{0}' And Type='V'";
			sql = string.Format(sql, StringUtils.SQLEncode(viewName));

			return GetDataSet(sql, lib).Tables[0].Rows.Count > 0;
		}

		/// <summary>
		/// Adds the language naming to table 
		/// ex: the arabic value is 5 this function adds a column <seealso cref="DataCte.LanguageDisplay"/>LanguageDisplay with the value Arabic
		/// </summary>
		/// <param name="table">DataTable any table</param>
		public static void AddLanguageToTable(DataTable table)
		{
			AddLanguageToTable(table, DataCte.LanguageField);
		}

		/// <summary>
		/// Adds the language naming to table 
		/// ex: the arabic value is 5 this function adds a column <seealso cref="DataCte.LanguageDisplay"/>LanguageDisplay with the value Arabic
		/// </summary>
		/// <param name="table">DataTable</param>
		/// <param name="LanguageField">the LanguageField default: Language</param>
		public static void AddLanguageToTable(DataTable table, string LanguageField)
		{
			if (table.Columns[DataCte.LanguageDisplay] == null)
				table.Columns.Add(DataCte.LanguageDisplay);

			if (table.Columns[LanguageField] == null)
				return;

			foreach (DataRow row in table.Rows)
			{
				if(row[LanguageField] != DBNull.Value && row[LanguageField] != null)
				{
					Languages lan = (Languages)Enum.Parse(typeof(Languages), row[LanguageField].ToString());
					row[DataCte.LanguageDisplay] = lan.ToString();
				}
			}
		}

		/// <summary>
		/// Exports an DataTable to and Excel Stream
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="excelDoc"></param>
		public static void ExportToExcel(DataTable dataTable, StreamWriter excelDoc)
		{
			DataSet ds = new DataSet();
			ds.Tables.Add(dataTable);
			ExportToExcel(ds, excelDoc);
		}
	
		/// <summary>
		/// Exports a datatable to an excel file
		/// </summary>
		/// <param name="dataTable">DataTable</param>
		/// <param name="fileName">File location (absolute)</param>
		public static void ExportToExcel(DataTable dataTable, string fileName)
		{
			DataSet ds = new DataSet();
			ds.Tables.Add(dataTable);
			ExportToExcel(ds, fileName);
		}
		
		/// <summary>
		/// Exports a dataset to an excel file 
		/// If the dataset contains more than one table
		/// Each table will be deployed in an excel sheet
		/// </summary>
		/// <param name="dataSet"><seealso cref="System.Data.DataSet"/>Dataset</param>
		/// <param name="fileName"></param>
		public static void ExportToExcel(DataSet dataSet, string fileName)
		{
			StreamWriter excelDoc = new System.IO.StreamWriter(fileName);
			ExportToExcel(dataSet, excelDoc);
		}
		
		/// <summary>
		/// Exports a dataset to a stream
		/// </summary>
		/// <param name="dataSet"><seealso cref="System.Data.DataSet"/>Dataset</param>
		/// <param name="excelDoc"><seealso cref="System.IO.StreaWriter"/>StreamWriter</param>
		public static void ExportToExcel(DataSet dataSet, StreamWriter excelDoc)
		{

			const string startExcelXML = "<xml version>\r\n<Workbook " +
				  "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
				  " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
				  "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
				  "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
				  "office:spreadsheet\">\r\n <Styles>\r\n " +
				  "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
				  "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
				  "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
				  "\r\n <Protection/>\r\n </Style>\r\n " +
				  "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
				  "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
				  "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
				  " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
				  "ss:ID=\"Decimal\">\r\n <NumberFormat " +
				  "ss:Format=\"0.0000\"/>\r\n </Style>\r\n " +
				  "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
				  "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
				  "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
				  "ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n " +
				  "</Styles>\r\n ";
			const string endExcelXML = "</Workbook>";

			int rowCount = 0;
			int sheetCount = 0;
			/*
		   <xml version>
		   <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
		   xmlns:o="urn:schemas-microsoft-com:office:office"
		   xmlns:x="urn:schemas-microsoft-com:office:excel"
		   xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
		   <Styles>
		   <Style ss:ID="Default" ss:Name="Normal">
			 <Alignment ss:Vertical="Bottom"/>
			 <Borders/>
			 <Font/>
			 <Interior/>
			 <NumberFormat/>
			 <Protection/>
		   </Style>
		   <Style ss:ID="BoldColumn">
			 <Font x:Family="Swiss" ss:Bold="1"/>
		   </Style>
		   <Style ss:ID="StringLiteral">
			 <NumberFormat ss:Format="@"/>
		   </Style>
		   <Style ss:ID="Decimal">
			 <NumberFormat ss:Format="0.0000"/>
		   </Style>
		   <Style ss:ID="Integer">
			 <NumberFormat ss:Format="0"/>
		   </Style>
		   <Style ss:ID="DateLiteral">
			 <NumberFormat ss:Format="mm/dd/yyyy;@"/>
		   </Style>
		   </Styles>
		   <Worksheet ss:Name="Sheet1">
		   </Worksheet>
		   </Workbook>
		   */
			excelDoc.Write(startExcelXML);
			foreach (DataTable currentTable in dataSet.Tables)
			{
				sheetCount++;
				rowCount = 0;

				excelDoc.Write("<Worksheet ss:Name=\"" + currentTable.TableName + "\">");
				excelDoc.Write("<Table>");
				excelDoc.Write("<Row>");
				for (int x = 0; x < currentTable.Columns.Count; x++)
				{
					excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
					excelDoc.Write(currentTable.Columns[x].ColumnName);
					excelDoc.Write("</Data></Cell>");
				}
				excelDoc.Write("</Row>");
				foreach (DataRow x in currentTable.Rows)
				{
					rowCount++;
					//if the number of rows is > 64000 create a new page to continue output

					if (rowCount == 64000)
					{
						rowCount = 0;
						sheetCount++;
						excelDoc.Write("</Table>");
						excelDoc.Write(" </Worksheet>");
						excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
						excelDoc.Write("<Table>");
					}
					excelDoc.Write("<Row>"); //ID=" + rowCount + "

					for (int y = 0; y < currentTable.Columns.Count; y++)
					{
						System.Type rowType;
						rowType = x[y].GetType();
						switch (rowType.ToString())
						{
							case "System.String":
								string XMLstring = x[y].ToString();
								XMLstring = XMLstring.Trim();
								XMLstring = XMLstring.Replace("&", "&");
								XMLstring = XMLstring.Replace(">", ">");
								XMLstring = XMLstring.Replace("<", "<");
								excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
											   "<Data ss:Type=\"String\">");
								excelDoc.Write(XMLstring);
								excelDoc.Write("</Data></Cell>");
								break;
							case "System.DateTime":
								//Excel has a specific Date Format of YYYY-MM-DD followed by  

								//the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000

								//The Following Code puts the date stored in XMLDate 

								//to the format above

								DateTime XMLDate = (DateTime)x[y];
								string XMLDatetoString = ""; //Excel Converted Date

								XMLDatetoString = XMLDate.Year.ToString() +
									 "-" +
									 (XMLDate.Month < 10 ? "0" +
									 XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
									 "-" +
									 (XMLDate.Day < 10 ? "0" +
									 XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
									 "T" +
									 (XMLDate.Hour < 10 ? "0" +
									 XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
									 ":" +
									 (XMLDate.Minute < 10 ? "0" +
									 XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
									 ":" +
									 (XMLDate.Second < 10 ? "0" +
									 XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
									 ".000";
								excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" +
											 "<Data ss:Type=\"DateTime\">");
								excelDoc.Write(XMLDatetoString);
								excelDoc.Write("</Data></Cell>");
								break;
							case "System.Boolean":
								excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
											"<Data ss:Type=\"String\">");
								excelDoc.Write(x[y].ToString());
								excelDoc.Write("</Data></Cell>");
								break;
							case "System.Int16":
							case "System.Int32":
							case "System.Int64":
							case "System.Byte":
								excelDoc.Write("<Cell ss:StyleID=\"Integer\">" +
										"<Data ss:Type=\"Number\">");
								excelDoc.Write(x[y].ToString());
								excelDoc.Write("</Data></Cell>");
								break;
							case "System.Decimal":
							case "System.Double":
								excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" +
									  "<Data ss:Type=\"Number\">");
								excelDoc.Write(x[y].ToString());
								excelDoc.Write("</Data></Cell>");
								break;
							case "System.DBNull":
								excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
									  "<Data ss:Type=\"String\">");
								excelDoc.Write("");
								excelDoc.Write("</Data></Cell>");
								break;
							default:
								throw (new Exception(rowType.ToString() + " not handled."));
						}
					}
					excelDoc.Write("</Row>");
				}
				excelDoc.Write("</Table>");
				excelDoc.Write(" </Worksheet>");
			}
			excelDoc.Write(endExcelXML);
			excelDoc.Close();
		}


		
	}
}