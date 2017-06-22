using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using lw.Base;
using lw.WebTools;

namespace lw.cms
{
	public class CustomDataGrid : CMSPage
	{
		public System.Web.UI.WebControls.DataGrid MyDataGrid;
		public DataView MyDataSource;
		public string  SelectedPageGridValueBase;
		public System.Web.UI.HtmlControls.HtmlInputHidden SelectedPageGridBase;
		protected string SortExpression;
		protected TemplateColumn MyTemplateColumn;
		
		protected int startIndex = 0;

		public CustomDataGrid()
        {
		       
		}
		public override void DataBind()
		{
			base.DataBind();
			this.RegisterScriptFile("grid", WebContext.ManagerRoot + "/js/Grid.js");
		}


		protected void InitStylesEvents(string HeaderCssClass,string BodyCssClass,string AlternateCssClass,string ItemCssClass,string FooterCssClass,string PagingCssClass,string SelectedCssClass,int PageNumber,PagerMode _PagerMode,string NextPageText,string PrevPageText,int PageButtonCount,HorizontalAlign _HorizontalAlign)
		{
			
			//////Bloc Design Data Grid////////////////////////////
			GridCommonControls.InitCssClass((TableItemStyle)this.MyDataGrid.HeaderStyle, HeaderCssClass);
			//GridCommonControls.InitGeneralControlLayout((WebControl)this.MyDataGrid,Color.Gray,Color.BlueViolet,BorderStyle.Solid,"G",Unit.Pixel(2),"",true,Unit.Pixel(400),Unit.Pixel(200),Color.Black);
			GridCommonControls.InitCssClass((TableItemStyle)this.MyDataGrid.ItemStyle, ItemCssClass);
			GridCommonControls.InitCssClass((TableItemStyle)this.MyDataGrid.SelectedItemStyle, SelectedCssClass);
			GridCommonControls.InitCssClass((TableItemStyle)this.MyDataGrid.AlternatingItemStyle, AlternateCssClass);
			GridCommonControls.InitCssClass((TableItemStyle)this.MyDataGrid.FooterStyle, FooterCssClass);
			
			this.DataGridInitProperties(false,true,true,false,0,0,true,true,GridLines.Both ,HorizontalAlign.NotSet, false, true);
			this.PagerWork(PageNumber,_PagerMode,PagingCssClass,NextPageText,PrevPageText,PageButtonCount,_HorizontalAlign);
			
			this.MyDataGrid.CellPadding = 3;
			this.MyDataGrid.CellSpacing = 0;
			this.MyDataGrid.BorderWidth = 1;
			this.MyDataGrid.CssClass = "GridTable";

			////////////////////////////////////////////
			this.InitEvent();
		}


		DataView CreateDataSource() 
		{
			if(this.MyDataSource != null)
			{
				DataTable dt = new DataTable();
				foreach(DataColumn column in this.MyDataSource.Table.Columns)
				{
					dt.Columns.Add(new DataColumn(column.ColumnName,column.DataType));
				}
				DataRow dr;
				for (int i = startIndex; i < (startIndex + MyDataGrid.PageSize-1); i++) 
				{
					dr = dt.NewRow();

					foreach(DataColumn column in this.MyDataSource.Table.Columns)
					{
						dr[column.ColumnName] = this.MyDataSource[i][column.ColumnName];
					}

					dt.Rows.Add(dr);
				}

				DataView dv = new DataView(dt);
				return dv;
			}
			else
				return null;

		}
        
       
		protected void InitSource(DataView dataSource)
		{
			this.MyDataSource = dataSource;
		}
        public void DataGridInitProperties(bool AllowCustomPaging,bool AllowPaging,bool AllowSorting,bool AutoGenerateColumns,int CellPadding,int CellSpacing,bool Enabled,bool EnableViewState,System.Web.UI.WebControls.GridLines GridLines,System.Web.UI.WebControls.HorizontalAlign HorizontalAlign,bool ShowFooter,bool ShowHeader)
		{
			MyDataGrid.AllowCustomPaging=AllowCustomPaging;
			MyDataGrid.AllowPaging=AllowPaging;
			MyDataGrid.AllowSorting=AllowSorting;
			MyDataGrid.AutoGenerateColumns=AutoGenerateColumns;
			MyDataGrid.CellPadding=CellPadding;
			MyDataGrid.CellSpacing=CellSpacing;
			MyDataGrid.Enabled=Enabled;
			MyDataGrid.EnableViewState=EnableViewState;
			MyDataGrid.GridLines=GridLines;
			MyDataGrid.HorizontalAlign=HorizontalAlign;
			MyDataGrid.ShowFooter=ShowFooter;
			MyDataGrid.ShowHeader=ShowHeader;
        }
        ///////////////////////////////////
		protected string GetSelectedItems() 
		{
			string selectedItems = "";
			int count = 0;
			foreach (DataGridItem item in this.MyDataGrid.Items)
			{

				selectedItems = DetermineSelection(item, ref count, selectedItems);        

			}

			return selectedItems;
		}

		string DetermineSelection(DataGridItem item, ref int count, string selectedItems)
		{
			CheckBox selection = (CheckBox)item.FindControlRecursive("SelectCheckBox");

			if (selection != null)
			{

				if (selection.Checked)
				{
					
					string s = this.MyDataGrid.DataKeys[item.ItemIndex].ToString();
					selectedItems += ", " + s + " ";
					count++;
				}

			}
			if(selectedItems!="")
			{
				return selectedItems.Substring(1);
			}
			else
				return selectedItems;
             
		}
      ///////////////////////////////
      ///

		protected void InitDataSource(DataView dataSource,string SelectedPageGridValue,System.Web.UI.HtmlControls.HtmlInputHidden SelectedPageGrid)
		{
            this.MyDataSource=dataSource;
			this.SelectedPageGridValueBase=SelectedPageGridValue;
			this.SelectedPageGridBase=SelectedPageGrid; 
		}
		protected void SetDataSource(DataTable dataSource, string SelectedPageGridValue, System.Web.UI.HtmlControls.HtmlInputHidden SelectedPageGrid)
		{
			SetDataSource(dataSource.DefaultView, SelectedPageGridValue, SelectedPageGrid);
		}
		protected void SetDataSource(DataView dataSource,string SelectedPageGridValue,System.Web.UI.HtmlControls.HtmlInputHidden SelectedPageGrid)
		{
            this.SelectedPageGridValueBase=SelectedPageGridValue;
			this.SelectedPageGridBase=SelectedPageGrid;
			if(MyDataGrid.AllowCustomPaging)
			{
				this.MyDataSource=dataSource;
				if((this.SelectedPageGridValueBase!="")&&(this.SelectedPageGridValueBase!=null))
				{
					MyDataGrid.CurrentPageIndex = Int32.Parse(this.SelectedPageGridValueBase);
				    this.SelectedPageGridBase.Value=SelectedPageGridValue;
				}
				MyDataGrid.VirtualItemCount=dataSource.Count;
				this.MyDataGrid.DataSource=this.CreateDataSource();
			}
			else
			{   
				this.MyDataSource=dataSource;
				if((this.SelectedPageGridValueBase!="")&&(this.SelectedPageGridValueBase!=null))
				{
				 	MyDataGrid.CurrentPageIndex = Int32.Parse(this.SelectedPageGridValueBase);
				    this.SelectedPageGridBase.Value=SelectedPageGridValue;
				}
				this.MyDataGrid.DataSource=dataSource;
			}
			this.MyDataGrid.DataBind();
		}
		
		protected void InitEvent()
		{
			this.MyDataGrid.PageIndexChanged += new DataGridPageChangedEventHandler(this.MyDataGrid_Page);
			this.MyDataGrid.SelectedIndexChanged += new EventHandler(this.IndexChange_SelectCommand);
			this.MyDataGrid.SortCommand += new DataGridSortCommandEventHandler(this.Sort_Grid);
		}


	  protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
      { 
        SortExpression=e.SortExpression.ToString();
		this.MyDataSource.Sort=SortExpression;
        this.MyDataGrid.DataSource=this.MyDataSource;
	    this.MyDataGrid.DataBind();
      }



		protected void IndexChange_SelectCommand(Object sender, EventArgs e) 
		{
			 string s;
			s= "You selected " +
				this.MyDataGrid.SelectedItem.Cells[1].Text +
				".<br>" + 
				this.MyDataGrid.SelectedItem.Cells[1].Text +
				" has an index number of " +
				this.MyDataGrid.SelectedIndex.ToString() + ".";

		}

		protected void MyDataGrid_Page(Object sender, DataGridPageChangedEventArgs e) 
		{
			
			
			if(MyDataGrid.AllowCustomPaging)
			{
				MyDataGrid.CurrentPageIndex = e.NewPageIndex;
				this.SelectedPageGridBase.Value=e.NewPageIndex.ToString();
				startIndex = MyDataGrid.CurrentPageIndex * MyDataGrid.PageSize;
				this.MyDataGrid.DataSource =this.CreateDataSource();
				this.MyDataGrid.DataBind();
			}
			else
			{
				MyDataGrid.CurrentPageIndex = e.NewPageIndex;
				this.SelectedPageGridBase.Value=e.NewPageIndex.ToString();
				this.MyDataGrid.DataSource =this.MyDataSource;
				this.MyDataGrid.DataBind();
			}
        }
		protected void PagerWork(int PageSize,System.Web.UI.WebControls.PagerMode pager,System.Drawing.Color BackColor,System.Drawing.Color BorderColor,string MyClass)
		{
			this.PagerWork(PageSize,pager,Color.Gray,Color.Black,Unit.Pixel(12),MyClass,Unit.Pixel(12),HorizontalAlign.Right,"Next",2,PagerPosition.Bottom,"Previous",VerticalAlign.Middle,true,true);
		}
		protected void PagerWork(int PageSize,System.Web.UI.WebControls.PagerMode pager,System.Drawing.Color BackColor,System.Drawing.Color BorderColor,System.Web.UI.WebControls.Unit BorderWidth,string MyClass,System.Web.UI.WebControls.Unit Height,System.Web.UI.WebControls.HorizontalAlign HorizontalAlign,string NextPageText,int PageButtonCount,System.Web.UI.WebControls.PagerPosition PagerPosition,string PrevPageText,System.Web.UI.WebControls.VerticalAlign VerticalAlign,bool Visible,bool Wrap)
		{
			MyDataGrid.PageSize=PageSize; 
			MyDataGrid.PagerStyle.Mode=pager;
			MyDataGrid.PagerStyle.BackColor=BackColor;
			MyDataGrid.PagerStyle.BorderColor=BorderColor;
			MyDataGrid.PagerStyle.BorderWidth=BorderWidth;
			MyDataGrid.PagerStyle.CssClass=MyClass;
			MyDataGrid.PagerStyle.Height=Height;
			MyDataGrid.PagerStyle.HorizontalAlign=HorizontalAlign;
			MyDataGrid.PagerStyle.NextPageText=NextPageText;
			MyDataGrid.PagerStyle.PageButtonCount=PageButtonCount;
			MyDataGrid.PagerStyle.Position=PagerPosition;
			MyDataGrid.PagerStyle.PrevPageText=PrevPageText;
			MyDataGrid.PagerStyle.VerticalAlign=VerticalAlign;
			MyDataGrid.PagerStyle.Visible=Visible;
			MyDataGrid.PagerStyle.Wrap=Wrap;
			
		}
		protected void PagerWork(int PageSize,System.Web.UI.WebControls.PagerMode pager,string CssClass,string NextPageText,string PrevPageText,int PageButtonCount,HorizontalAlign _HorizontalAlign)
		{
			MyDataGrid.PageSize=PageSize; 
			MyDataGrid.PagerStyle.Mode=pager;
			MyDataGrid.PagerStyle.CssClass=CssClass;
			MyDataGrid.PagerStyle.NextPageText=NextPageText;
			MyDataGrid.PagerStyle.PrevPageText=PrevPageText;
			MyDataGrid.PagerStyle.PageButtonCount=PageButtonCount;
			MyDataGrid.PagerStyle.HorizontalAlign=_HorizontalAlign;
		}

		public void AddColumn(string fieldName,string displayName,string format,string sort,HorizontalAlign _HorizontalAlign)
		{
			if(this.MyDataGrid.DataKeyField == null || this.MyDataGrid.DataKeyField == "")
			{
				throw new Exception("You must specify the dataKeyField before creating other fields.");
			}
			this.InitBoundColumn(false,displayName,fieldName,format,displayName,"",sort,true,_HorizontalAlign);
		}
		
		void InitBoundColumn(bool image,string HeaderText,string DataFieldName,string DataFormatString,string FooterText,string LocationImage,string SortExpressionFiledOrExpression,bool Visible,HorizontalAlign _HorizontalAlign)
		{
			System.Web.UI.WebControls.BoundColumn BoundColumToAdd = new BoundColumn();
            BoundColumToAdd.DataField=DataFieldName;
			BoundColumToAdd.DataFormatString=DataFormatString;
			BoundColumToAdd.HeaderText=HeaderText;
			BoundColumToAdd.FooterText=FooterText;
			if(image)
				BoundColumToAdd.HeaderImageUrl=LocationImage;
			BoundColumToAdd.SortExpression=SortExpressionFiledOrExpression;
			BoundColumToAdd.Visible=Visible;
			BoundColumToAdd.HeaderStyle.HorizontalAlign = _HorizontalAlign;
            BoundColumToAdd.ItemStyle.HorizontalAlign=_HorizontalAlign;
			BoundColumToAdd.FooterStyle.HorizontalAlign=_HorizontalAlign;
			this.MyDataGrid.Columns.Add(BoundColumToAdd);
		}
		protected void InitButtonColumn(bool image,ButtonColumnType type,string commandName,string text,string dataTextField,string dataFormatString,string footerText,string headerText,string LocationImage,string Sort,bool Visible)
		{
			System.Web.UI.WebControls.ButtonColumn ButtonColumToAdd = new ButtonColumn();
			ButtonColumToAdd.ButtonType = type;
			ButtonColumToAdd.CommandName = commandName;
			ButtonColumToAdd.DataTextField = dataTextField;
			ButtonColumToAdd.DataTextFormatString = dataFormatString;
			ButtonColumToAdd.HeaderText = headerText;
			ButtonColumToAdd.FooterText=footerText;
			if(image)
			ButtonColumToAdd.HeaderImageUrl=LocationImage;
			ButtonColumToAdd.SortExpression=Sort;
			ButtonColumToAdd.Visible=Visible;
			ButtonColumToAdd.Text = text;
		}
		protected virtual void Grid_Init(Object sender, EventArgs e) 
		{
			MyTemplateColumn = DataGridTemplateRadio.LoadTemplate(this.MyDataGrid.DataKeyField,this.MyDataGrid.ID);
			
			this.MyDataGrid.Columns.Add(MyTemplateColumn);	
		}
		protected void SetKeyField(string fieldName)
		{
			this.MyDataGrid.DataKeyField = fieldName;
		}
		protected void InitEditCommandColumn(bool image,ButtonColumnType type,string cancelText,string editText,string headerText,string footerText,string locationImage,string sort,string updateText,bool visible)
		{
			System.Web.UI.WebControls.EditCommandColumn column = new EditCommandColumn();
			column.ButtonType = type;
			column.CancelText = cancelText;
			column.EditText = editText;
			column.FooterText = footerText;
			column.HeaderText = headerText;
			if(image)
			column.HeaderImageUrl=locationImage;
			column.SortExpression = sort;
			column.UpdateText = updateText;
			column.Visible = visible;
		}




	}



}
