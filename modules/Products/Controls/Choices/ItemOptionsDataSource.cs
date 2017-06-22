using System;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using lw.CTE;
using lw.WebTools;

namespace lw.Products.Controls
{
	public class ItemOptionsDataSource : lw.DataControls.CustomDataSource
	{
		bool _bound = false;

		public ItemOptionsDataSource()
		{
			this.DataLibrary = cte.lib;
		}
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			string sql = @"
Select Distinct ICO.* from ItemChoicesOptions ICO
Inner Join ItemChoices IC
	on IC.ChoiceId = ICO.ChoiceId
Inner Join ItemOptions IO
	on IO.OptionId = ICO.OptionId
";

			if (!String.IsNullOrWhiteSpace(Choice))
			{
				sql += string.Format(" and ICO.ChoiceId in (select ChoiceId from ItemChoices where Name='{0}')",
					Choice);
			}

			if(ChoiceId!= null)
				sql += string.Format(" and ICO.ChoiceId={0}", ChoiceId);
				

			if (!String.IsNullOrWhiteSpace(_boundTo))
			{
				ItemsSource source = this.Page.FindControlRecursive(_boundTo) as ItemsSource;
				if (source != null)
				{
					string items = source.ItemIds;
					if (!String.IsNullOrWhiteSpace(items))
						sql += " and IO.ItemId in (" + items.TrimEnd(',') + ")";
				}
			}

			if (ReadFromParent != null && ReadFromParent.Value && ItemId != null)
				sql += string.Format(" and ICO.OptionId in (select OptionId from ItemOptions where ItemId = {0})", ItemId);

			this.SelectCommand = sql + " Order By ICO.Sorting Asc,ICO.Value";

			base.DataBind();
		}

		public override object Data
		{
			get
			{
				if (ReadFromParent != null && ReadFromParent.Value && ItemId != null)
				{
					DataTable itemOptionsInventory = null;
					if (MyPage.PageContext["ItemsOptionsInvenroty" + ItemId.ToString()] != null)
					{
						itemOptionsInventory = MyPage.PageContext["ItemsOptionsInvenroty" + ItemId.ToString()] as DataTable;
					}
					else
					{
						ChoicesMgr cMgr = new ChoicesMgr();
						itemOptionsInventory = cMgr.GetItemOptionsInventory(ItemId.Value);
						MyPage.PageContext["ItemsOptionsInvenroty" + ItemId.ToString()] = itemOptionsInventory;
					}

					if (itemOptionsInventory != null)
					{
						DataTable _data = base.Data as DataTable;

						Dictionary<string, bool> availableOptions = new Dictionary<string, bool>();
						Dictionary<string, bool> toDeleteOptions = new Dictionary<string, bool>();

						foreach (DataRow dr in itemOptionsInventory.Rows)
						{
							string key = dr["key"].ToString();
							string[] temp = key.Split(',');

							if (float.Parse(dr["Inventory"].ToString()) == 0)
							{
								foreach (string optionId in temp)
								{
									if (!availableOptions.ContainsKey(optionId))
									{
										toDeleteOptions[optionId] = true;
									}
								}
							}
							else
							{
								foreach (string optionId in temp)
								{
									if (!availableOptions.ContainsKey(optionId))
										availableOptions[optionId] = true;

									if (toDeleteOptions.ContainsKey(optionId))
										toDeleteOptions.Remove(optionId);
								}
							}
						}

						foreach (string optionId in toDeleteOptions.Keys)
						{
							DataRow[] col = _data.Select("OptionId=" + optionId);
							foreach (DataRow r in col)
								r.Delete();
						}
						_data.AcceptChanges();
					}
				}
				return base.Data;
			}
			set
			{
				base.Data = value;
			}
		}

		string _choice = "";
		/// <summary>
		/// Gets or Sets the Choice Group
		/// Ex: Color, Size, ...
		/// </summary>
		public string Choice
		{
			get
			{
				return _choice;
			}
			set
			{
				_choice = value;
			}
		}

		int? _choiceId = null;
		public int? ChoiceId
		{
			get
			{
				if (_choiceId == null)
				{
					try
					{
						object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "ChoiceId");
						if (obj != null)
							_choiceId = (int)obj;
					}
					catch
					{

					}
				}
				return _choiceId;
			}
		}

		string _boundTo = "";
		/// <summary>
		/// Bounds the tag to an Items DataSource
		/// The categories returned by this source will definetly have items from this data source
		/// Good for using filters
		/// </summary>
		public string BoundTo
		{
			get
			{
				return _boundTo;
			}
			set
			{
				_boundTo = value;
			}
		}

		bool? _ReadFromParent = null;
		/// <summary>
		/// Determines if the source should read from a parent container
		/// </summary>
		public bool? ReadFromParent
		{
			get
			{
				return _ReadFromParent;
			}
			set
			{
				_ReadFromParent = value;
			}

		}

		int? _itemId = null;
		public int? ItemId
		{
			get
			{
				object temp = ControlUtils.GetBoundedDataField(this.NamingContainer, "ItemId");
				if (temp != null)
					_itemId = (int)temp;
				return _itemId;
			}
		}
	}
}