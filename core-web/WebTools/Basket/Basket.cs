using System;


namespace lw.WebTools
{
	[Serializable()]
	public class SessionBasket
	{
		public SessionBasket()
		{
		}

		public byte[] Items;

		public decimal Total = 0;
		public decimal SubTotal = 0;
		public decimal Discount = 0;
		public decimal Tax = 0;
		public decimal Shipping = 0;
		public decimal Handling = 0;
		public decimal GiftVoucherValue = 0;
		public decimal Weight = 0;
		public string GiftVouvher = "", Region = "", Country = "";

		public decimal PaymentCost = 0;
		public string PaymentType = "";
	}
}
