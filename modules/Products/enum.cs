
using lw.Utils;

namespace lw.Products
{
	public enum PriceCategory
	{
		[Description("Price")]
		Price,

		[Description("Sale Price")]
		SalePrice,

		[Description("Reseller Price")]
		ResellerPrice
	}
	public enum ListItemsBy
	{
		Category,
		Brand,
		Relation,
		Package,
		Search,
		All
	}
	public enum CategoryType
	{
		Regular, Hierarchy, RootByBrand, Root, Parent, Search, Category
	}
}
