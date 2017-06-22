using System;
using System.Text;
using System.Xml;
using lw.Data;


namespace lw.Products
{
	public class PackagesMgr : DirectorBase
	{
		public PackagesMgr()
			: base(cte.lib)
		{

		}

		public void AddItemToItem(int itemId1, int itemId2)
		{
			string temp = @"if (Not Exists(select * from ItemPackages where ItemId={0} and ItemId1={1}))
								insert into ItemPackages(ItemId, ItemId1, QTY, Sort) values ({0}, {1}, 1, 1000);";

			temp = string.Format(temp, itemId1, itemId2);

			DBUtils.ExecuteQuery(temp, cte.lib);
		}

		public void RemoveItemFromItem(int itemId1, int itemId2)
		{
			string temp = @"delete from ItemPackages where ItemId={0} and ItemId1={1}";

			temp = string.Format(temp, itemId1, itemId2);

			DBUtils.ExecuteQuery(temp, cte.lib);
		}

		public void UpdateItemPackage(int ItemId, string Xml)
		{
			if (Xml.Trim() == "")
				return;
			string _deleteFormat = string.Format("delete from ItemPackages where ItemId={0} and ItemId1={{0}}", ItemId);
			string _insertFormat = string.Format("insert into ItemPackages (ItemId, ItemId1, QTY, Sort) values ({0}, {{0}}, {{1}}, {{2}})", ItemId);
			string _updateFormat = string.Format("update ItemPackages set QTY={{1}}, Sort={{2}} where ItemId={0} and ItemId1={{0}}", ItemId);

			StringBuilder query = new StringBuilder();

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(Xml);

			XmlNode packages = xmlDoc.SelectSingleNode("packages");

			int sort = 0;
			foreach (XmlNode package in packages.ChildNodes)
			{
				int relationId = Int32.Parse(package.SelectSingleNode("RelationId").Attributes["value"].Value);
				int itemId = Int32.Parse(package.SelectSingleNode("ItemId").Attributes["value"].Value);
				double qty = Int32.Parse(package.SelectSingleNode("qty").Attributes["value"].Value);
				bool del = bool.Parse(package.SelectSingleNode("Delete").Attributes["value"].Value);

				if (relationId > 0)
				{
					if (!del)
						query.Append(string.Format(_updateFormat, itemId, qty, sort++));
					else
						query.Append(string.Format(_deleteFormat, itemId));
				}
				else
				{
					if (!del)
						query.Append(string.Format(_insertFormat, itemId, qty, sort++));
				}
			}

			string sql = query.ToString();
			if(sql.Trim() != "")
				DBUtils.ExecuteQuery(sql, cte.lib);
		}

		public Packages.ItemPackagesViewDataTable GetItemPackages(int ItemId)
		{
			PackagesTableAdapters.ItemPackagesViewTableAdapter packages = new lw.Products.PackagesTableAdapters.ItemPackagesViewTableAdapter();
			return packages.GetItemPackages(ItemId);
		}
	}
}