using System.Collections.Generic;

namespace lw.Payments
{
	public enum PaymentTypes
	{
		PO, NC, Paypal
	}
	public class Payments
	{
		static List<Payment> _Options;

		//static HybridDictionary _Options = null;
		public static List<Payment> Options
		{
			get
			{
				if (_Options == null)
				{
					_Options = new List<Payment>();

					_Options.Add(new Payment("Pay On Delivery", PaymentTypes.PO.ToString()));
					_Options.Add(new Payment("Net Commerce", PaymentTypes.NC.ToString()));
					_Options.Add(new Payment("Paypal", PaymentTypes.NC.ToString()));
				}
				return _Options;
			}
		}
		public static Payment DefaultPayment
		{
			get
			{
				return Options.Find(
					delegate(Payment p)
					{
						return p.IsDefault;
					}
				);
			}
		}
		public static Payment GetPayment(string PaymentKey)
		{
			return Options.Find(
				delegate(Payment p)
				{
					return p.Key == PaymentKey;
				}
			);
		}
	}
}
