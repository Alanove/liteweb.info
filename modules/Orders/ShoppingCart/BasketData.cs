using System.Data;
using System.IO;
using lw.Payments;
using lw.WebTools;

namespace lw.ShoppingCart
{
	public class Basket
	{
		SessionBasket sBasket;

		public ShoppingDs.ShoppingItemsDataTable BasketItems;

		decimal _Total = 0;
		decimal _SubTotal = 0;
		decimal _Discount = 0;
		decimal _Tax = 0;
		decimal _Shipping = 0;
		decimal _GiftVoucherValue = 0;
		decimal _Weight = 0;
		decimal _Handling = 0;
		string _GiftVouvher = "", _Region = "", _Country = "";
		Payment _Payment = null;

		#region properties

		public decimal Total{
			get { return _Total; }
			set { _Total = value; }
		}
		public decimal SubTotal
		{
			get { return _SubTotal; }
			set { _SubTotal = value; }
		}
		public decimal Discount
		{
			get { return _Discount; }
			set { _Discount = value; }
		}
		public decimal Tax
		{
			get { return _Tax; }
			set { _Tax = value; }
		}
		public decimal Shipping
		{
			get { return _Shipping; }
			set { _Shipping = value; }
		}
		public decimal GiftVoucherValue
		{
			get { return _GiftVoucherValue; }
			set { _GiftVoucherValue = value; }
		}
		public decimal Weight
		{
			get { return _Weight; }
			set { _Weight = value; }
		}
		public decimal Handling
		{
			get { return _Handling; }
			set { _Handling = value; }
		}
		public string GiftVouvher
		{
			get {return _GiftVouvher;}
			set {_GiftVouvher = value;}
		}
		public string Region
		{
			get { return _Region; }
			set { _Region = value; }
		}
		public string Country
		{
			get { return _Country; }
			set { _Country = value; }
		}
		public Payment Payment
		{
			get { return _Payment; }
			set { _Payment = value; }
		}
		public decimal PaymentCost
		{
			get
			{
				return Payment != null ? Payment.AdditionalCost : 0;
			}
		}
		public string PaymentType
		{
			get
			{
				return Payment != null? Payment.Key : null;
			}
		}
		#endregion

		public Basket()
		{
			Init();
		}

		void Init()
		{
			sBasket = WebContext.Profile.Basket;

			this.Total = sBasket.Total;
			this.SubTotal = sBasket.SubTotal;
			this.Discount = sBasket.Discount;
			this.Tax = sBasket.Tax;
			this.Shipping = sBasket.Shipping;
			this.Handling = sBasket.Handling;
			this.GiftVoucherValue = sBasket.GiftVoucherValue;
			this.Weight = sBasket.Weight;
			this.GiftVouvher = sBasket.GiftVouvher;
			this.Region = sBasket.Region;
			this.Country = sBasket.Country;
			this.Payment = lw.Payments.Payments.GetPayment(sBasket.PaymentType);

			if (sBasket.Items != null && sBasket.Items.Length > 0)
				BasketItems = this.DeserializeTable(sBasket.Items);
			else
				BasketItems = new ShoppingDs.ShoppingItemsDataTable();
		}

		public void AcceptChanges()
		{
			sBasket.Items = this.SerializeTable(BasketItems);
			sBasket.Total = this.Total;
			sBasket.SubTotal = this.SubTotal;
			sBasket.Discount = this.Discount;
			sBasket.Tax = this.Tax;
			sBasket.Shipping = this.Shipping;

			sBasket.Handling = this.Handling;
			sBasket.GiftVoucherValue = this.GiftVoucherValue;
			sBasket.Weight = this.Weight;
			sBasket.GiftVouvher = this.GiftVouvher;
			sBasket.Region = this.Region;
			sBasket.Country = this.Country;
			sBasket.PaymentType = this.PaymentType;
			sBasket.PaymentCost = this.PaymentCost;

			WebContext.Profile.Basket = sBasket;
		}

		public byte[] SerializeTable(DataTable dt)
		{
			MemoryStream stream = new MemoryStream();
			dt.WriteXml(stream, XmlWriteMode.WriteSchema);
			stream.Close();
			return stream.ToArray();
		}
		public ShoppingDs.ShoppingItemsDataTable DeserializeTable(byte[] bytes)
		{
			System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes);
			ShoppingDs.ShoppingItemsDataTable dt = new ShoppingDs.ShoppingItemsDataTable();
			dt.ReadXml(stream);
			stream.Close();
			return dt;
		}
	}
}
