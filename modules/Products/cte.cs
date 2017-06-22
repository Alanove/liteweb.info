
using lw.Utils;

namespace lw.Products
{
	/// <summary>
	/// Class containing all the constants that are using in the <see cref="Products"/> library
	/// </summary>
	public class cte
	{
		public const double version = 2.001;

		public const string lib = "ProductsManager";


		public const string ItemIdsContext = "";
		public const string ItemPricesContext = "";
	}
	public enum ItemStatus
	{
		None = 0,
		Disabled = 1,		//2 ^ 0
		Enabled = 2,		//2 ^ 1

		ForSale = 4,		//2 ^ 2
		Taxable = 8,		//2 ^ 3

		[Description("bestprice.png")]
		Package = 16,		//2 ^ 4
		
		[Description("free-delivery.png")]
		FreeShipping = 32,	//2 ^ 5
		
		[Description("onsale.png")]
		OnSale = 64,		//2 ^ 6
		[Description("new.png")]
		New = 128,			//2 ^ 7

		[Description("bestprice.png")]
		BestPrice = 256,	//2 ^ 8
		BestSeller = 512,	//2 ^ 9

		[Description("comingsoon.png")]
		ComingSoon = 1024,	//2 ^ 10

		[Description("exclusive.png")]
		Exclusive = 2048 //2 ^ 11
	}
}
