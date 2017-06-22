using System;
using System.Data;
using lw.CTE;
using lw.Utils;
using lw.WebTools;

namespace lw.Shipping
{
	public class ShippingManager
	{
		string localCountyCode, localCountry;
		decimal localFreeShippingLimit = -1, domesticFreeShippingLimit = -1;
		decimal handlingFeeMin = -1, handlingFee = -1;

		Config cfg;

		public ShippingManager()
		{
			cfg = new Config();
		}

		public decimal CalculateShipping()
		{
			return CalculateShipping(null, "", "", null);
		}
		public decimal CalculateShipping(decimal weight, decimal total)
		{
			return CalculateShipping(weight, "", "All", total);
		}

		public decimal CalculateShipping(decimal? weight, string country, string region, decimal? total)
		{
			Config cfg = new Config();
			string flatShipping = cfg.GetKey(lw.CTE.Settings.FlatShippingRate);

			if (!string.IsNullOrWhiteSpace(flatShipping))
			{
				return decimal.Parse(flatShipping);
			}

			decimal ret = 0;

			int groupId = -1;
			string type = "C";

			CustomShippingManager cs = new CustomShippingManager();

			if (country == LocalCountryCode || country == "")
			{
				if (LocalFreeShippingLimit != 0 && total > LocalFreeShippingLimit)
					return 0;

				type = "R";

				DataRow[] regions = cs.DS.Tables["eRegions"].Select("DisplayName='" + region + "'");
				if (regions.Length > 0)
				{
					groupId = Int32.Parse(regions[0]["GroupId"].ToString());
				}
			}
			else
			{
				if (DomesticFreeShippingLimit != 0 && total > DomesticFreeShippingLimit)
					return 0;

				DataRow[] countries = cs.DS.Tables["eCountries"].Select("Name='" + country + "'");
				if (countries.Length > 0)
				{
					groupId = Int32.Parse(countries[0]["GroupId"].ToString());
				}
			}

			string query = string.Format("GroupId='{0}' and GroupType='{1}' and UpTo>={2}", groupId, type, weight);

			DataView shippingView = new DataView(cs.DS.Tables["ShippingWeight"], query, "UpTo ASC", DataViewRowState.CurrentRows);

			if(shippingView.Count > 0)
			{
				ret = decimal.Parse(shippingView[0]["Value"].ToString());
			}

			return ret;
		}

		#region Handling

		public decimal CalculateHandling(decimal orderTotal)
		{
			return orderTotal < HandlingFeeMin ? HandlingFee : 0;
		}

		#endregion

		#region Properties

		public decimal HandlingFeeMin
		{
			get
			{
				if (handlingFeeMin == -1)
				{
					string obj = cfg.GetKey(Settings.HandlingFeeMin);
					if (obj != null && obj != "")
						handlingFeeMin = decimal.Parse(obj);
					else
						handlingFeeMin = 0;
				}
				return handlingFeeMin;
			}
		}
		public decimal HandlingFee
		{
			get
			{
				if (handlingFee == -1)
				{
					string obj = cfg.GetKey(Settings.HandlingFee);
					if (obj != null && obj != "")
						handlingFee = decimal.Parse(obj);
					else
						handlingFee = 0;
				}
				return handlingFee;
			}
		}
		
		public string LocalCountryCode
		{
			get
			{
				if(localCountyCode == null)
					localCountyCode = cfg.GetKey(Settings.LocalCountry);
				return localCountyCode;
			}
		}
		public string LocalCountry
		{
			get
			{
				if (localCountry == null)
				{
					lw.Countries.CountriesMgr cMgr = new lw.Countries.CountriesMgr();
					DataView countries = cMgr.GetCountries("CountryCode='" + StringUtils.SQLEncode(LocalCountryCode) + "'");
					if (countries.Count > 0)
						localCountry =  countries[0]["Name"].ToString();
					else
						localCountry = "";
				}
				return localCountry;
			}
		}
		public decimal LocalFreeShippingLimit
		{
			get
			{
				if (localFreeShippingLimit == -1)
				{
					string obj = cfg.GetKey(Settings.LocalFreeShippingLimit);
					if (obj != null && obj != "")
						localFreeShippingLimit = decimal.Parse(obj);
					else
						localFreeShippingLimit = 0;
				}
				return localFreeShippingLimit;
			}
		}
		public decimal DomesticFreeShippingLimit
		{
			get
			{
				if (domesticFreeShippingLimit == -1)
				{
					string obj = cfg.GetKey(Settings.DomesticFreeShippingLimit);
					if (obj != null && obj != "")
						domesticFreeShippingLimit = decimal.Parse(obj);
					else
						domesticFreeShippingLimit = 0;
				}
				return domesticFreeShippingLimit;
			}
		}
		public bool IntegrateShipping
		{
			get
			{
				if (cfg.GetKey(Settings.IntegrateShipping) == "on")
					return true;
				return false;
			}
		}
		#endregion


	}
}
