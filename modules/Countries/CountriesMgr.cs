using System;
using System.Data;

using lw.Data;
using lw.Utils;
using lw.WebTools;

namespace lw.Countries
{
	public class CountriesMgr: DirectorBase
	{
		public CountriesMgr():base(cte.LibraryName)
		{
			
		}
		/* Just for demo.. need to be removed */
		public DataView GetRegion(int CountryId)
		{
			string sql = "select * from countries";
			sql += " where countryid=" + CountryId;
			
			return DBUtils.GetDataSet(sql, cte.LibraryName).Tables[0].DefaultView;
		}


		/*************************************/


		public DataView GetCountries(string condition)
		{
			CountriesDs cDataSet = new CountriesDs();
			IDataAdapter adp = base.GetAdapter(cte.CountriesAdp);

			if (!String.IsNullOrWhiteSpace(condition))
			{
				condition += " and TopParent is Null";
			}
			else
				condition += "TopParent is Null";

			return ((CountriesDs)base.FillData(adp, cDataSet, condition)).Countries.DefaultView;
		}
		
		public DataView GetRegions(int CountryId)
		{
			return GetRegions(CountryId, "");
		}

		public DataView GetRegions(int CountryId, string cond)
		{
			string condition = "ParentId = " + CountryId.ToString();

			if (!String.IsNullOrWhiteSpace(cond))
			{
				condition += " and " + cond;
			}

			string sql = "select * from Countries";

			if (!String.IsNullOrWhiteSpace(condition))
			{
				sql = sql + " where  " + condition;
			}

			return DBUtils.GetDataSet(sql, cte.LibraryName).Tables[0].DefaultView;
		}

		public DataView GetRegions (string cond)
		{
			string condition = "1 = 1";

			if (!String.IsNullOrWhiteSpace (cond))
			{
				condition += " and " + cond;
			}

			string sql = "select * from Countries";

			if (!String.IsNullOrWhiteSpace (condition))
			{
				sql = sql + " where  " + condition;
			}

			return DBUtils.GetDataSet (sql, cte.LibraryName).Tables[0].DefaultView;
		}

		public DataRow GetCountry(string Code)
		{
			DataView countries = GetCountries(String.Format("CountryCode='{0}' or Name='{0}'", StringUtils.SQLEncode(Code)));
			if (countries.Count > 0)
				return countries[0].Row;
			return null;
		}

		public DataRow GetCountry(int CountryId)
		{
			DataView countries = GetCountries(String.Format("CountryId={0}", CountryId));
			if (countries.Count > 0)
				return countries[0].Row;
			return null;
		}


		public DataView GetCountries()
		{
			return GetCountries("");
		}

		public int AddCountry(string code, string name, string localName)
		{
			CountriesDs _ds = new CountriesDs();

			IDataAdapter Adp = base.GetAdapter(cte.CountriesAdp);
			CountriesDs.CountriesRow row = _ds.Countries.NewCountriesRow();

			row.CountryCode = code;
			row.Name = name;
			row.LocalName = localName;

			_ds.Countries.AddCountriesRow(row);
			base.UpdateData(Adp, _ds);

			return row.CountryId;

		}

		public int? AddRegion(int CountryId, int ParentId, string Name, string LocalName)
		{
			string sql = string.Format("Insert Into Countries (TopParent, ParentId, Name, LastModified, DateCreated) values ({0}, {1}, N'{2}', getdate(), getdate());select @@identity as RetId",
						CountryId, ParentId, StringUtils.SQLEncode(Name), StringUtils.SQLEncode(LocalName));

			DataSet thisDs = DBUtils.GetDataSet(sql, cte.LibraryName);

			if(thisDs.Tables[0].Rows.Count > 0)
				return Int32.Parse(thisDs.Tables[0].Rows[0]["RetId"].ToString());

			return null;
		}

		public int? AddUniversity(int CountryId, int RegionId, string Name, string Website)
		{
			string sql = string.Format("Insert Into Universities (CountryId, RegionId, Name, UniqueName, Website, LastModified, DateCreated) values ({0}, {1}, N'{2}', N'{3}', N'{4}', getdate(), getdate());select @@identity as RetId",
						CountryId, RegionId, StringUtils.SQLEncode(Name), StringUtils.SQLEncode(StringUtils.ToURL(Name)),
							StringUtils.SQLEncode(Website));

			DataSet thisDs = DBUtils.GetDataSet(sql, cte.LibraryName);

			if (thisDs.Tables[0].Rows.Count > 0)
				return Int32.Parse(thisDs.Tables[0].Rows[0]["RetId"].ToString());

			return null;
		}

		public DataView GetUniversities(string cond)
		{
			string sql = "select * from Universities";
			if (!String.IsNullOrWhiteSpace(cond))
			{
				sql += " where " + cond;
			}
			return DBUtils.GetDataSet(sql, cte.LibraryName).Tables[0].DefaultView;
		}


		public static DataTable States
		{
			get
			{
				return XmlManager.GetTableFromXmlDataSet("States.config", "States");
			}
		}
	}

	public class cte
	{
		public const string LibraryName = "Countries";
		public const string CountriesAdp = "CountriesAdp";

		public const string RegionsAdp = "RegionsAdp";
	}
}
