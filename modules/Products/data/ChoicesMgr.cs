using System;
using System.Data;
using System.Text;
using System.Web;
using System.IO;

using lw.Data;
using lw.Utils;
using lw.WebTools;
using lw.CTE;


namespace lw.Products
{
	public class ChoicesMgr : DirectorBase
	{
		public ChoicesMgr()
			: base(cte.lib)
		{

		}

		#region Choices
		public void AddChoiceGroup(string GroupName)
		{
			ChoicesTableAdapters.ItemChoicesTableAdapter choices = new lw.Products.ChoicesTableAdapters.ItemChoicesTableAdapter();
			choices.AddChoice(GroupName);
		}
		public void UpdateChoice(int ChoiceId, string Choice)
		{
			ChoicesTableAdapters.ItemChoicesTableAdapter choices = new lw.Products.ChoicesTableAdapters.ItemChoicesTableAdapter();
			choices.UpdateChoice(ChoiceId, Choice);
		}
		public void DeleteChoice(int ChoiceId)
		{
			ChoicesTableAdapters.ItemChoicesTableAdapter choices = new lw.Products.ChoicesTableAdapters.ItemChoicesTableAdapter();
			DeleteOptionByChoice(ChoiceId);
			choices.DeleteChoice(ChoiceId);
		}
		public Choices.ItemChoicesDataTable GetChoices()
		{
			ChoicesTableAdapters.ItemChoicesTableAdapter choices = new lw.Products.ChoicesTableAdapters.ItemChoicesTableAdapter();
			return choices.GetData();
		}
		public Choices.ItemChoicesDataTable GetChoicesByItem(int ItemId)
		{
			ChoicesTableAdapters.ItemChoicesTableAdapter choices = new lw.Products.ChoicesTableAdapters.ItemChoicesTableAdapter();
			return choices.GetChoicesByItem(ItemId);
		}
		public void SaveChoicesSort(string choices)
		{
			string[] ids = choices.Split('|');
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < ids.Length; i++)
			{
				sb.Append(string.Format(
					"Update ItemChoices set Sorting={0} where ChoiceId={1};",
					i,
					ids[i]
				));
			}
			DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
		}
		#endregion

		#region Options

		public int AddOption(int ChoiceId, string Option)
		{
			Choices.ItemChoicesOptionsDataTable opts = GetOptions("'" + StringUtils.SQLEncode(Option) + "'");
			if (opts.Count > 0)
				return opts[0].OptionId;

			ChoicesTableAdapters.ItemChoicesOptionsTableAdapter options = new ChoicesTableAdapters.ItemChoicesOptionsTableAdapter();
			int? OptionId = 0;
			options.AddOption(Option, ChoiceId, ref OptionId);
			return OptionId.Value;
		}

		public void UpdateOption(int OptionId, string option)
		{
			ChoicesTableAdapters.ItemChoicesOptionsTableAdapter options = new lw.Products.ChoicesTableAdapters.ItemChoicesOptionsTableAdapter();
			options.UpdateOption(OptionId, option);
		}

		public void DeleteOption(int OptionId)
		{
			ChoicesTableAdapters.ItemChoicesOptionsTableAdapter options = new lw.Products.ChoicesTableAdapters.ItemChoicesOptionsTableAdapter();
			options.DeleteOption(OptionId);
		}
		public void DeleteOptionByChoice(int ChoiceId)
		{
			ChoicesTableAdapters.ItemChoicesOptionsTableAdapter options = new lw.Products.ChoicesTableAdapters.ItemChoicesOptionsTableAdapter();
			options.DeleteByChoice(ChoiceId);
		}

		public Choices.ItemChoicesOptionsDataTable GetOptions(string values)
		{
			ExtendChoices  options = new ExtendChoices();
			return options.GetOptionsByValues(values);
		}

		public Choices.ItemChoicesOptionsDataTable GetOptions(int ChoiceId)
		{
			ChoicesTableAdapters.ItemChoicesOptionsTableAdapter options = new lw.Products.ChoicesTableAdapters.ItemChoicesOptionsTableAdapter();
			return options.GetDataByChoice(ChoiceId);
		}
		public Choices.ItemChoicesOptionsDataTable GetOptions()
		{
			ChoicesTableAdapters.ItemChoicesOptionsTableAdapter options = new lw.Products.ChoicesTableAdapters.ItemChoicesOptionsTableAdapter();
			return options.GetData();
		}
		public void SaveOptionsSort(string options)
		{
			string[] ids = options.Split('|');
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < ids.Length; i++)
			{
				sb.Append(string.Format(
					"Update ItemChoicesOptions set Sorting={0} where OptionId={1};",
					i,
					ids[i]
				));
			}
			DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
		}
		#endregion

		#region Items VS Options
		public DataTable GetItemOptions(int ItemId)
		{
			string sql = string.Format("select * from ItemOptions where ItemId={0}",
				ItemId);
			return DBUtils.GetDataSet(sql, cte.lib).Tables[0];
		}
		public string GetJsonItemOptions(int ItemId)
		{
			ChoicesTableAdapters.GetItemOptionsTableAdapter adp = new lw.Products.ChoicesTableAdapters.GetItemOptionsTableAdapter();
			Choices.GetItemOptionsDataTable table = adp.GetData(ItemId);
			if (table.Rows.Count > 0)
				return table.Rows[0]["Options"].ToString();
			return "";
		}
		public void ItemOptions(int ItemId, string options)
		{
			ItemOptions(ItemId, options, "");
		}
		public void ItemOptions(int ItemId, string options, string skus)
		{
			ItemOptions(ItemId, options, skus, true);
		}
		public void ItemOptions(int ItemId, string options, string skus, bool DoDelete)
		{
			DataView optionsView = new DataView(this.GetItemOptions(ItemId));

			StringBuilder sb = new StringBuilder();

			string[] oIds = options.Split(new Char[] {'|', ',', ';', '-'});
			string[] skusArray = skus.Split(new Char[] { '|', ',', ';'});
			int i = 0;
			foreach (string optionId in oIds)
			{
				if(optionId == "")
					continue;

				int oId = Int32.Parse(optionId);

				optionsView.RowFilter = string.Format("OptionId={0}",
					oId);
				if (optionsView.Count > 0)
					continue;
				sb.Append(string.Format("insert into ItemOptions (ItemId, OptionId, DisplayOnSite, SKU) values ({0}, {1}, 0, '{2}');",
					ItemId, oId, StringUtils.SQLEncode(skusArray[i])));
				i++;
			}

			if (false && DoDelete)
			{
				if (options.Trim() != "")
				{
					sb.Append(string.Format(@"delete from ItemOptions 
					where ItemId={0} and OptionId in 
					(select OptionId from ItemChoicesOptions where 
					OptionId not in ({1}))",
						ItemId, options.Replace("|", ",")));
				}
				else
				{
					sb.Append(string.Format(@"delete from ItemOptions 
					where ItemId={0}", ItemId));
				}
			}

			if (sb.Length > 0)
			{
				DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
			}
			CreateInvntoryRecords(ItemId);
		}
		public void ItemOptionsforImport(int ItemId, string options, bool DoDelete)
		{
			DataView optionsView = new DataView(this.GetItemOptions(ItemId));

			StringBuilder sb = new StringBuilder();

			string[] oIds = options.Split('|');
			foreach (string optionId in oIds)
			{
				if (optionId == "")
					continue;

				int oId = Int32.Parse(optionId);

				optionsView.RowFilter = string.Format("OptionId={0}",
					oId);
				if (optionsView.Count > 0)
					continue;
				sb.Append(string.Format("insert into ItemOptions (ItemId, OptionId, DisplayOnSite) values ({0}, {1}, 0);",
					ItemId, oId));
			}

			if (false && DoDelete)
			{
				if (options.Trim() != "")
				{
					sb.Append(string.Format(@"delete from ItemOptions 
					where ItemId={0} and OptionId in 
					(select OptionId from ItemChoicesOptions where 
					OptionId not in ({1}))",
						ItemId, options.Replace("|", ",")));
				}
				else
				{
					sb.Append(string.Format(@"delete from ItemOptions 
					where ItemId={0}", ItemId));
				}
			}

			if (sb.Length > 0)
			{
				DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
			}
		}
		public Choices.ItemOptionsViewDataTable GetItemOptionsView(int ItemId)
		{
			ChoicesTableAdapters.ItemOptionsViewTableAdapter adp = new lw.Products.ChoicesTableAdapters.ItemOptionsViewTableAdapter();
			return adp.GetByItem(ItemId);
		}
		public Choices.ItemOptionsInventoryViewDataTable GetItemOptionInventory(int ItemId)
		{
			ChoicesTableAdapters.ItemOptionsInventoryViewTableAdapter adp = new lw.Products.ChoicesTableAdapters.ItemOptionsInventoryViewTableAdapter();
			return adp.GetByItem(ItemId);
		}
		public Choices.ItemOptionsInventoryViewDataTable GetItemOptionInventory(int ItemId, string key)
		{
			ChoicesTableAdapters.ItemOptionsInventoryViewTableAdapter adp = new lw.Products.ChoicesTableAdapters.ItemOptionsInventoryViewTableAdapter();
			return adp.GetDataByKey(key, ItemId);
		}
		public void ItemOptions(int ItemId, HttpRequest req)
		{
			Choices.ItemChoicesOptionsDataTable options = GetOptions();

			ItemsMgr iMgr = new ItemsMgr();
			DataRow item = iMgr.GetItem(ItemId);
			string folder = Path.Combine(WebContext.Server.MapPath(WebContext.Root + "/" + lw.CTE.Folders.ProductsImages), item["UniqueName"].ToString());
			folder = Path.Combine(folder, "Choices");

			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			Config cfg = new Config();

			Dimension ThumbSize = new Dimension(cfg.GetKey(Settings.ProductImage_ThumbSize));
			Dimension MediumSize = new Dimension(cfg.GetKey(Settings.ProductImage_MediumSize));
			Dimension LargeSize = new Dimension(cfg.GetKey(Settings.ProductImage_LargeSize));

			foreach (DataRow option in options.Rows)
			{
				if(!String.IsNullOrWhiteSpace(req.Form[string.Format("o_{0}", option["OptionId"])]))
				{
					string sql = string.Format(@"
if not exists (select * from ItemOptions where OptionId={1} and ItemId={0})
	Insert into ItemOptions (ItemId, OptionId, DisplayOnSite, Picture, SKU) values 
						({0}, {1}, {2}, '', '{3}') else 
	update ItemOptions set SKU='{3}',DisplayOnSite={2}  where ItemId={0} and OptionId={1};select * from ItemOptions where ItemId={0} and OptionId={1};
;",
												ItemId,
												option["OptionId"],
												!String.IsNullOrWhiteSpace(req[string.Format("display_o_{0}", option["OptionId"])])? 1: 0,
												StringUtils.SQLEncode(req[string.Format("sku_o_{0}", option["OptionId"])]));

					DataRow optionRow = DBUtils.GetDataSet(sql, cte.lib).Tables[0].Rows[0];


					bool deleteOldPicture = !String.IsNullOrWhiteSpace(req[string.Format("delete_o_{0}", option["OptionId"])]);
					string picture = optionRow["Picture"].ToString();

					HttpPostedFile pictureFile = req.Files[string.Format("picture_o_{0}", option["OptionId"])];

					if (deleteOldPicture || pictureFile.ContentLength > 0)
					{
						string temp = Path.Combine(folder, picture).ToLower();
						if (File.Exists(temp))
							File.Delete(temp);
						temp = temp.Replace(".jpg", "_m.jpg");
						if (File.Exists(temp))
							File.Delete(temp);

						temp = temp.Replace("_m.jpg", "_s.jpg");
						if (File.Exists(temp))
							File.Delete(temp);

					}

					

					if (pictureFile.ContentLength > 0)
					{
						picture = StringUtils.ToURL(option["value"].ToString() + "_" + (new Random().Next(9999999)).ToString()) + ".Jpg";


						deleteOldPicture = false;

						string temp = Path.Combine(folder, picture).ToLower();
						pictureFile.SaveAs(temp);

						string large = temp;
						temp = temp.Replace(".jpg", "_m.jpg");

						if (MediumSize.Valid)
							lw.GraphicUtils.ImageUtils.Resize(large, temp, MediumSize.IntWidth, MediumSize.IntHeight);
						else
							File.Copy(large, temp, true);

						temp = temp.Replace("_m.jpg", "_s.jpg");
						if (ThumbSize.Valid)
							lw.GraphicUtils.ImageUtils.CreateThumb(large, temp, ThumbSize.IntWidth, ThumbSize.IntHeight, true);
						else
							File.Copy(large, temp, true);

						if (LargeSize.Valid)
							lw.GraphicUtils.ImageUtils.Resize(large, large, LargeSize.IntWidth, LargeSize.IntHeight);

						sql = string.Format("Update ItemOptions set Picture = '{2}' where ItemId={0} and OptionId={1}",
							ItemId, option["OptionId"], StringUtils.SQLEncode(picture));

						DBUtils.ExecuteQuery(sql, cte.lib);
					}

					if (deleteOldPicture)
					{
						sql = string.Format("Update ItemOptions set Picture = '' where ItemId={0} and OptionId={1}",
							ItemId, option["OptionId"]);

						DBUtils.ExecuteQuery(sql, cte.lib);
					}
				}
			}
			CreateInvntoryRecords(ItemId);
		}
		#endregion

		#region INVENTORY

		/*
		 * the complicated part
		 * The point is to create all possible combinations between different
		 * options relating a different choices
		 * so for example we might have 
		 * Colors: red, blue
		 * Sizes: L, XL
		 * Style: Long, Short
		 * Results will be red,L,Long - red,L,Short - red, Xl, long, red, xl,short...
		 * then the same for blue
		 * Inside the database and to minimize the content we will use their ids instead 
		 * of their names
		 		
		*/

		public void CreateInvntoryRecords(int ItemId)
		{
			ChoicesTableAdapters.ItemChoicesTableAdapter choicesAdp = new lw.Products.ChoicesTableAdapters.ItemChoicesTableAdapter();
			Choices.ItemChoicesDataTable choices = choicesAdp.GetChoicesByItem(ItemId);

			ChoicesTableAdapters.ItemOptionsViewTableAdapter itemOptionsViewAdp = new lw.Products.ChoicesTableAdapters.ItemOptionsViewTableAdapter();
			Choices.ItemOptionsViewDataTable options = itemOptionsViewAdp.GetByItem(ItemId);


			/* Loop through choices,
			 * Each Step is considered as a level
			 * We get the options for the choice
			 * then we loop through options of previous level
			 * Ex: 
			 * Step 1: (Colors)
			 * red		1	==> ID = RedID
			 * blue		1	==> ID = BlueID
			 * Setp 2: (Sizes)
			 * red, S		2	==>	ID = RedID,SID
			 * red, L		2
			 * blue, S		2
			 * blue, L		2
			 * Step 3: (Style)
			 * red, S, Long		3	==> ID = RedId,SID,LongID
			 * red, S, Short	3
			 * red, L, Long		3
			 * ...
			 * ...
			 * With each step the complexity multiplies
			 
			 * Sorting by ids is the best choice
			 * We will be using the itterator i as the level
			 
			 */

			Choices.ItemOptionsInventoryTempDataTable inventory = new Choices.ItemOptionsInventoryTempDataTable();
			DataView choicesView = new DataView(choices, "", "ChoiceId", DataViewRowState.CurrentRows);

			for (int i = 0; i < choicesView.Count; i++)
			{
				DataView optionsView = new DataView(options,
					string.Format("ChoiceId={0}", choicesView[i]["ChoiceId"]),
					"OptionId", 
					DataViewRowState.CurrentRows);
				for (int j = 0; j < optionsView.Count; j++)
				{
					//First level.. 
					if (i == 0)
					{
						DataRow row = inventory.NewRow();
						row["Key"] = optionsView[j]["OptionId"];
						row["Level"] = i;
						inventory.Rows.Add(row);
					}
					else
					{
						DataView inventoryView = new DataView(inventory,
							String.Format("Level={0}", i - 1),
							"",
							DataViewRowState.CurrentRows);
						for (int k = 0; k < inventoryView.Count; k++)
						{
							DataRow row = inventory.NewRow();
							row["Key"] = string.Format("{0},{1}",
								inventoryView[k]["Key"],
								optionsView[j]["OptionId"]);
							row["Level"] = i;
							inventory.Rows.Add(row);
						}
					}
				}
			}

			DataView iView = new DataView(inventory,
				string.Format("Level={0}", choices.Count - 1),
				"",
				DataViewRowState.CurrentRows);
			StringBuilder sb = new StringBuilder();
			foreach(DataRowView drv in iView)
			{
				sb.Append(string.Format(
					@"exec('UpdateItemOptionsInventory ""{0}"", ""{1}"", ""0"", ""0"", ""0""');",
					ItemId,
					drv["Key"]));
			}
			if(sb.Length > 0)
				DBUtils.ExecuteQuery(sb.ToString(), cte.lib);
		}


		public Choices.ItemOptionsInventoryDataTable GetItemOptionsInventory(int ItemId)
		{
			ChoicesTableAdapters.ItemOptionsInventoryTableAdapter adp = new lw.Products.ChoicesTableAdapters.ItemOptionsInventoryTableAdapter();
			return adp.GetDataByItem(ItemId);
		}

		public void UpdateOptionsInventory(int ItemId, string key, float inventory, bool isDefault)
		{
			ChoicesTableAdapters.ItemOptionsInventoryTableAdapter adp = new lw.Products.ChoicesTableAdapters.ItemOptionsInventoryTableAdapter();
			adp.UpdateInventory((double)inventory, isDefault, ItemId, key);
		}

		public void ResetDefault(int ItemId, string id)
		{
			ChoicesTableAdapters.ItemOptionsInventoryTableAdapter adp = new lw.Products.ChoicesTableAdapters.ItemOptionsInventoryTableAdapter();
			adp.ResetDefaults(ItemId, id);
		}
		#endregion
	}
	class ExtendChoices : ChoicesTableAdapters.ItemChoicesOptionsTableAdapter
	{
		public Choices.ItemChoicesOptionsDataTable GetOptionsByValues(string values)
		{
			base.CommandCollection[0].CommandText = @"SELECT  OptionId, ChoiceId, value, Sorting
										FROM ItemChoicesOptions where [value] in (" + values +") order by OptionId";
			return base.GetData();
		}
	}
}
