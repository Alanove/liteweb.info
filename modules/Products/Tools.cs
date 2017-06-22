using System;
using System.Data;
using System.Text;

namespace lw.Products
{
	public class Tools
	{
		public static bool CheckStatus(ItemStatus itemStatus, DataRow Item)
		{
			return CheckStatus(itemStatus, (int)Item["Status"]);
		}
		public static bool CheckStatus(ItemStatus itemStatus, int status)
		{
			return ((int)itemStatus & status) != 0;
		}
		public static string GetItemStatus(DataRow Item)
		{
			return GetItemStatus((int)Item["Status"]);
		}
		public static string GetItemStatus(int status)
		{
			StringBuilder sb = new StringBuilder();
			string sep = "";
			foreach (ItemStatus stat in Enum.GetValues(typeof(ItemStatus)))
			{
				if ((status & (int)stat) != 0)
				{
					sb.Append(string.Format("{0}{1}", sep, stat.ToString()));
					sep = ",";
				}
			}
			return sb.ToString();
		}
	}
}
