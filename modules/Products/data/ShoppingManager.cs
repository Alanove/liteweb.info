using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.GraphicUtils;
using lw.Utils;
using lw.WebTools;
using System.Data.Linq;

namespace lw.Products
{
	public class ShoppingManager: LINQManager
	{
		public ShoppingManager()
			: base(cte.lib)
		{
		}


		public ISingleResult<GetShoppingDataResult>  GetShoppingItems()
		{
			return ShoppingDB.GetShoppingData();
		}



		#region Variables

		
		public ShoppingDBDataContext ShoppingDB
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new ShoppingDBDataContext(Connection);
				return (ShoppingDBDataContext)_dataContext;
			}
		}

		#endregion
	}
}
