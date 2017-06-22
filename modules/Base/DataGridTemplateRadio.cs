using System.Web.UI;
using System.Web.UI.WebControls;

namespace lw.Base
{
	public class DataGridTemplateRadio : ITemplate
	{
		
		ListItemType templateType;
		string columnName;
		string _keyField;
   
		public DataGridTemplateRadio(ListItemType type, string colname,string keyField)
		{
			this._keyField=keyField;
			templateType = type;
			columnName = colname;
		}

		public void InstantiateIn(System.Web.UI.Control container)
		{
			Literal lc = new Literal();
			switch(templateType)
			{
				case ListItemType.Header:
					lc.Text = "<B>" + columnName + "</B>";
					container.Controls.Add(lc);
					break;
				case ListItemType.Item:
					GridCheckBox tb2 = new GridCheckBox();
					tb2.KeyField = this._keyField;
					tb2.Checked=false;
					//tb2.ID=DataBinder.Eval(tb2.NamingContainer,"DataItem.ClassGroupId").ToString();
                    container.Controls.Add(tb2);
					break;
				case ListItemType.EditItem:
					CheckBox tb3 = new CheckBox();
					tb3.Checked=false;
					container.Controls.Add(tb3);
					break;
				case ListItemType.Footer:
					lc.Text = "<I>" + columnName + "</I>";
					container.Controls.Add(lc);
					break;
			}
		}

		public static TemplateColumn LoadTemplate(string keyField)
		{
			TemplateColumn tc1 = new TemplateColumn();
			tc1.HeaderTemplate = new 
				DataGridTemplateRadio(ListItemType.Header, "",keyField);
			tc1.ItemTemplate = new 
				DataGridTemplateRadio(ListItemType.Item, "",keyField);
			tc1.EditItemTemplate = new 
				DataGridTemplateRadio(ListItemType.EditItem, "",keyField);
			tc1.FooterTemplate = new 
				DataGridTemplateRadio(ListItemType.Footer, "",keyField);
			return(tc1);
		}
		public static TemplateColumn LoadTemplate(string keyField,string DataGridId)
		{
			TemplateColumn tc1 = new TemplateColumn();
			tc1.HeaderTemplate = new 
				DataGridTemplateRadio(ListItemType.Header, "<input type=\"checkbox\" onclick=\"_CheckGridCheckBox(this, '" + DataGridId + "')\" DataGrid=\""+DataGridId+"\" id=\"Check_" + DataGridId + "\">",keyField);
			tc1.ItemTemplate = new
				DataGridTemplateRadio(ListItemType.Item, "<input type=\"checkbox\" onclick=\"GridCClick(this, '" + DataGridId + "')\" DataGrid=\"" + DataGridId + "\">", keyField);
			//tc1.EditItemTemplate = new
			//	DataGridTemplateRadio(ListItemType.EditItem, "<input type=\"checkbox\" onclick=\"_CheckGridCheckBox(this, '" + DataGridId + "')\" DataGrid=\"" + DataGridId + "\">", keyField);
			//tc1.FooterTemplate = new
			//	DataGridTemplateRadio(ListItemType.Footer, "<input type=\"checkbox\" onclick=\"_CheckGridCheckBox(this, '" + DataGridId + "')\" DataGrid=\"" + DataGridId + "\">", keyField);
			tc1.HeaderStyle.HorizontalAlign=HorizontalAlign.Center;
			tc1.ItemStyle.HorizontalAlign=HorizontalAlign.Center;
			tc1.FooterStyle.HorizontalAlign=HorizontalAlign.Center;
            return(tc1);
		}

	}


}
