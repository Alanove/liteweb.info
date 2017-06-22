using System;

using lw.WebTools;

namespace lw.Payments
{
	[Serializable()]
	public class Payment
	{
		public string _Name = "";
		public string _Key = "";
		PaymentSettings paymentSettings;
		Config cfg;

		public Payment()
		{
			this.paymentSettings = new PaymentSettings(_Name, _Key);
			cfg = new Config();
		}

		public Payment(string name, string key)
		{
			this.Name = name;
			this.Key = key;
			this.paymentSettings = new PaymentSettings(name, key);
			cfg = new Config();
		}
		public Payment(string name)
		{
			this.Name = name;
			this.Key = name;
			this.paymentSettings = new PaymentSettings(name);
			cfg = new Config();
		}

		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}
		public string Key
		{
			get { return _Key; }
			set { _Key = value; }
		}
		public string DisplayName
		{
			get
			{
				return cfg.GetKey(paymentSettings.DisplayName);
			}
			set
			{
				cfg.SetKey(paymentSettings.DisplayName, value);
			}
		}
		public bool Enabled
		{
			get
			{
				string temp = cfg.GetKey(paymentSettings.Enabled);
				if (temp != "")
					return bool.Parse(temp);
				return false;
			}
			set
			{
				cfg.SetKey(paymentSettings.Enabled, value.ToString());
			}
		}
		public decimal AdditionalCost
		{
			get
			{
				string temp = cfg.GetKey(paymentSettings.AdditionalCost);
				if (temp != "")
					return decimal.Parse(temp);
				return 0;
			}
			set
			{
				cfg.SetKey(paymentSettings.AdditionalCost, value.ToString());
			}
		}
		public string MerchantNumber
		{
			get
			{
				return cfg.GetKey(paymentSettings.MerchantNumber);
			}
			set
			{
				cfg.SetKey(paymentSettings.MerchantNumber, value);
			}
		}
		public string TransKey
		{
			get
			{
				return cfg.GetKey(paymentSettings.TransKey);
			}
			set
			{
				cfg.SetKey(paymentSettings.TransKey, value);
			}
		}
		public string URL
		{
			get
			{
				return cfg.GetKey(paymentSettings.URL);
			}
			set
			{
				cfg.SetKey(paymentSettings.URL, value);
			}
		}
		public string VerficationURL
		{
			get
			{
				return cfg.GetKey(paymentSettings.VerficationURL);
			}
			set
			{
				cfg.SetKey(paymentSettings.VerficationURL, value);
			}
		}
		public string TransType
		{
			get
			{
				return cfg.GetKey(paymentSettings.TransType);
			}
			set
			{
				cfg.SetKey(paymentSettings.TransType, value);
			}
		}
		public string StoreID
		{
			get
			{
				return cfg.GetKey(paymentSettings.StoreID);
			}
			set
			{
				cfg.SetKey(paymentSettings.StoreID, value);
			}
		}
		public string Email
		{
			get
			{
				return cfg.GetKey(paymentSettings.Email);
			}
			set
			{
				cfg.SetKey(paymentSettings.Email, value);
			}
		}
		public string ConfigFile
		{
			get
			{
				return cfg.GetKey(paymentSettings.ConfigFile);
			}
			set
			{
				cfg.SetKey(paymentSettings.ConfigFile, value);
			}
		}
		public bool IsDefault
		{
			get
			{
				string temp = cfg.GetKey(paymentSettings.IsDefault);
				if (temp != "")
					return bool.Parse(temp);
				return false;
			}
			set
			{
				cfg.SetKey(paymentSettings.IsDefault, value.ToString());
			}
		}
		public void AcceptChanges (){
			cfg.AcceptChanges();
		}
	}
}
