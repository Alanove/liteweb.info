using System;
using System.Web.UI;
using System.Data;
using System.Reflection;

using lw.CTE;
using lw.Data;
using lw.WebTools;
using lw.Utils;

namespace lw.DataControls
{
	public class CustomDataSource : lw.Base.BaseControl, lw.Data.IDataSource
	{
		bool enablePaging = false;
		int pageSize = 10;
		string dataLibrary = "";
		string selectCommand = "";
		int page = 0;
		object data = null;
		string orderBy;
		lw.Data.IDataSource dataSrc = null;
		string dataString;
		string dataStringValues;



		/// <summary>
		/// Creates the data
		/// </summary>
		void getData()
		{
			if (DataSrc == null)
			{
				if (!Visible)
				{
					DataSrc = new EmptyDataSrc();
				}
				else
				{
					// Connects to the database
					if (data == null)
					{
						if (!String.IsNullOrWhiteSpace(SelectCommand))
						{
							if (enablePaging)
							{
								dataSrc = new PagingDataSource(selectCommand, _Page, OrderBy, DataLibrary);
								((PagingDataSource)dataSrc).PageSize = PageSize;
							}
							else
								dataSrc = new DataSource(selectCommand, DataLibrary);
							try
							{
								data = dataSrc.GetData();
							}
							catch (Exception ex)
							{
								ErrorContext.Add("CustomDataSource", ex);
								ErrorContext.Add("CustomDataSource-Select", SelectCommand);
							}
							checkEmptyValue((DataTable)data);
						}
					}

					// Use Config files
					if (data == null)
					{
						if (!String.IsNullOrWhiteSpace(configFile))
						{
							DataSet ds = XmlManager.GetDataSet(configFile);
							if (ds != null && ds.Tables.Count > 0)
							{
								if (!String.IsNullOrWhiteSpace(configTable))
									data = ds.Tables[configTable];
								else
									data = ds.Tables[0];

								

								if (!String.IsNullOrEmpty(OrderBy))
								{
									data = new DataView((DataTable)data, "", OrderBy, DataViewRowState.CurrentRows);
									checkEmptyValue((DataView)data);	
								}
								else
									checkEmptyValue((DataTable)data);
							}
						}
					}

					// Use an enum type
					if (data == null)
					{
						if (!String.IsNullOrWhiteSpace(enumType))
						{
							string[] temp = enumType.Split(',');

							Assembly assem = Assembly.Load(temp[1]);
							Type typ = assem.GetType(temp[0]);
							FieldInfo[] fields = typ.GetFields();

							DataTable table = new DataTable();
							table.Columns.Add("Value");
							table.Columns.Add("Name");
                            table.Columns.Add("NameAttribute");
                            table.Columns.Add("DescriptionAttribute");
                            table.Columns.Add("PermissionGroupAttribute");

							checkEmptyValue(table);

							foreach (var field in fields)
							{
								if (field.Name.Equals("value__")) continue;

								DataRow row = table.NewRow();
								row["Value"] = field.GetRawConstantValue();
								row["Name"] = field.Name;
                                row["NameAttribute"] = EnumHelper.GetName((Enum)Enum.Parse(typ, field.Name));
                                row["DescriptionAttribute"] = EnumHelper.GetDescription((Enum)Enum.Parse(typ, field.Name));
                                row["PermissionGroupAttribute"] = EnumHelper.GetPermissionGroup((Enum)Enum.Parse(typ, field.Name));
								table.Rows.Add(row);
							}
							data = table;
							
						}
					}

					// Use Direct string input 1,2,3
					if (data == null)
					{
						if (!String.IsNullOrWhiteSpace(dataString))
						{
							string[] temp = dataString.Split(new Char[] { ',', ';', '|', '`' });
							DataTable table = new DataTable();
							table.Columns.Add("Value");
							table.Columns.Add("Name");

							checkEmptyValue(table);

							if (!String.IsNullOrWhiteSpace(DataStringValues))
							{
								string[] valuesTemp = DataStringValues.Split(new char[] { ',', ';', '-', '`' });
								
								try
								{
									if (valuesTemp.Length != temp.Length)
										throw new Exception("DataString and DataStringValues must have equal amount of values");
									else
									{
										for (int i = 0; i < temp.Length; i++)
										{
											DataRow row = table.NewRow();
											row["Value"] = valuesTemp[i];
											row["Name"] = temp[i];
											table.Rows.Add(row);

											data = table;
										}
									}
								}
								catch (Exception ex)
								{
									ErrorContext.Add("CustomDataSource", ex.Message);
								}
							} else {
								foreach (string str in temp) {
									DataRow row = table.NewRow();
									row["Value"] = str;
									row["Name"] = str;
									table.Rows.Add(row);

									data = table;
								}
							}
						}
					}

					if (dataSrc == null && data!=null)
					{						
						EmptyDataSrc tempSrc = new EmptyDataSrc();
						tempSrc.Data = data; 
						if (data != null)
						{
							if (data as DataView != null)
								tempSrc.RowsCount = ((DataView)data).Table.Rows.Count;
							else
								tempSrc.RowsCount = ((DataTable)data).Rows.Count;
							tempSrc.HasData = tempSrc.RowsCount > 0;
						}
						dataSrc = tempSrc;
					}
				}
			}
			else
				data = DataSrc.Data;
		}
        void checkEmptyValue(DataView dv)
        {
            checkEmptyValue(dv.Table);
        }

		void checkEmptyValue(DataTable table)
		{
			if (table != null && addEmptyValue != null && addEmptyValue.Value)
			{
				DataRow row = table.NewRow();
				if (!String.IsNullOrWhiteSpace(emptyValue))
				{
					if (table.Columns["Name"] != null)
					{
						row["Name"] = emptyValue;
					}
					if (table.Columns["Title"] != null)
					{
						row["Title"] = emptyValue;
					}
					if (table.Columns["DisplayName"] != null)
					{
						row["DisplayName"] = emptyValue;
					}
					if (table.Columns["Type"] != null && table.Columns["Type"].DataType == typeof(string))
					{
						row["Type"] = emptyValue;
					}
					if (table.Columns["CategoryName"] != null)
					{
						row["CategoryName"] = emptyValue;
					}
					if (table.Columns["GroupName"] != null)
					{
						row["GroupName"] = emptyValue;
					}
				}
				table.Rows.InsertAt(row, 0);
			}
		}

		/// <summary>
		/// Returns the data object
		/// </summary>
		public object GetData()
		{
			return Data;
		}


		/// <summary>
		/// Returns the data object
		/// </summary>
		public virtual object Data
		{
			get
			{
				if (data == null)
					getData();
				return data;
			}
			set
			{
				data = value;
			}
		}

		/// <summary>
		/// Flag that determines the total number of rows in the datasource
		/// </summary>
		public virtual int RowsCount
		{
			get
			{
				if (data == null)
					getData();

				return dataSrc.RowsCount;
			}
		}

		/// <summary>
		/// Flag to determine if the datasource has data
		/// </summary>
		public virtual bool HasData
		{
			get
			{
				if (data == null)
					getData();

				return DataSrc.HasData;
			}
		}

		/// <summary>
		/// Gets the current page (used with EnablePaging=true)
		/// </summary>
		int _Page
		{
			get
			{
				object obj = MyPage.GetQueryValue(DataCte.PagingParam);
				if (obj != null && !String.IsNullOrWhiteSpace(obj.ToString()))
				{
					try
					{
						page = Int32.Parse(obj.ToString());
						page--;
					}
					catch
					{
					}
				}
				return page;
			}
		}
		#region Properties

		public virtual lw.Data.IDataSource DataSrc
		{
			get
			{
				return dataSrc;
			}
			set { dataSrc = value; }
		}

		/// <summary>
		/// Flag to enable paging in the datasource
		/// default: false
		/// PS: EnablePaging works only with database sources and selectcommand
		/// </summary>
		public bool EnablePaging
		{
			get
			{
				return enablePaging;
			}
			set
			{
				enablePaging = value;
			}
		}

		/// <summary>
		/// Used with enable paging to define page size
		/// default: 10
		/// </summary>
		public int PageSize
		{
			get
			{
				object obj = null;
				obj = WebContext.Request[DataCte.PagingSizeParam];
				if (obj != null && obj.ToString() != "")
					pageSize = Int32.Parse(obj.ToString());

				return pageSize;
			}
			set
			{
				pageSize = value;
			}
		}


		/// <summary>
		/// Order by this field.
		/// </summary>
		public virtual string OrderBy
		{
			get
			{
				return orderBy;
			}
			set
			{
				orderBy = value;
			}
		}

		/// <summary>
		/// The datalibrary on which the source will use to connect to the DB
		/// set in: database.config
		/// </summary>
		public string DataLibrary
		{
			get
			{
				return dataLibrary;
			}
			set
			{
				dataLibrary = value;
			}
		}

		/// <summary>
		/// The select command for the datasource
		/// ex: select * from items
		/// </summary>
		public string SelectCommand
		{
			get
			{
				return selectCommand;
			}
			set
			{
				selectCommand = value;
			}
		}


		/// <summary>
		/// Provides a direct datasource string seperated by any of the following seperators: ", : - ;"
		/// Returns datatable with one column "Value"
		/// </summary>
		public string DataString
		{
			get
			{
				return dataString;
			}
			set
			{
				dataString = value;
			}
		}


		/// <summary>
		/// Provides a direct datasource values string seperated by any of the following seperators: ", : - ;"
		/// the number of values should be equal to the number of string names... otherwise it will throw an exception
		/// </summary>
		public string DataStringValues
		{
			get
			{
				return dataStringValues;
			}
			set
			{
				dataStringValues = value;
			}
		}


		string enumType;
		/// <summary>
		/// Points to the type of datasource, can be an enum class
		/// Must be set in the following format: Enum,Assembly
		/// Returns datatable with two columns "Value","Name"
		/// </summary>
		public string EnumType
		{
			get
			{
				return enumType;
			}
			set
			{
				enumType = value;
			}
		}

		string configFile;
		/// <summary>
		/// Points to any config file in the config folder.
		/// If config table is not defined the first table in the dataset will be used.
		/// </summary>
		public string ConfigFile
		{
			get
			{
				return configFile;
			}
			set
			{
				configFile = value;
			}
		}

		string configTable;
		/// <summary>
		/// Points to the table that will be used from the configfile.
		/// </summary>
		public string ConfigTable
		{
			get
			{
				return configTable;
			}
			set
			{
				configTable = value;
			}
		}


		bool? addEmptyValue;
		/// <summary>
		/// Adds an empty value at the begenning on the source
		/// </summary>
		public bool? AddEmptyValue
		{
			get
			{
				return addEmptyValue;
			}
			set
			{
				addEmptyValue = value;
			}
		}

		string emptyValue;
		/// <summary>
		/// The string that will be added in the empty valye option
		/// Used with AddEmptyValue.
		/// </summary>
		public string EmptyValue
		{
			get
			{
				return emptyValue;
			}
			set
			{
				emptyValue = value;
			}
		}


		

		#endregion
	}
}
