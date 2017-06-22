
namespace lw.Payments
{
	public class PaymentSettings
	{
		public string Name = "";
		public string Key = "";
		public PaymentSettings(string name)
		{
			this.Name = name;
			this.Key = name;
		}
		public PaymentSettings(string name, string key)
		{
			this.Name = name;
			this.Key = key;
		}
		public string DisplayName
		{
			get {return Key + "_DisplayName";}
		}
		public string Enabled
		{
			get { return Key + "_Enabled"; }
		}
		public string AdditionalCost
		{
			get{return Key + "_AdditionalCost";}
		}
		public string MerchantNumber
		{
			get { return Key + "_MerchantNumber"; }
		}
		public string TransKey
		{
			get { return Key + "_MD5Key"; }
		}
		public string URL
		{
			get { return Key + "_URL"; }
		}
		public string VerficationURL
		{
			get { return Key + "_VerificationURL"; }
		}
		public string TransType
		{
			get { return Key + "_TransType"; }
		}
		public string StoreID
		{
			get { return Key + "_StoreID"; }
		}
		public string Email {
			get { return Key + "_Email"; }
		}
		public string ConfigFile 
		{
			get
			{
				return Key + "_ConfigFile";
			}
		}
		public string IsDefault
		{
			get
			{
				return Key + "_IsDefault";
			}
		}
	}
}
