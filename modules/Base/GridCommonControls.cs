using System.Web.UI.WebControls;

namespace lw.Base
{
	public class GridCommonControls
	{
		public GridCommonControls()
		{
			
		}

		public static void InitGeneralControlLayout(
				System.Web.UI.WebControls.WebControl MyControl,
				System.Drawing.Color BackColor,
				System.Drawing.Color BorderColor,
				System.Web.UI.WebControls.BorderStyle BorderStyle,
				string AccessKey,
				System.Web.UI.WebControls.Unit BorderWidth,
				string MyCsClass,
				bool visible,
				System.Web.UI.WebControls.Unit Width,
				System.Web.UI.WebControls.Unit Height,
				System.Drawing.Color ForeColor
		)
		{
			MyControl.BackColor=BackColor;
			MyControl.BorderColor=BorderColor;
			MyControl.BorderStyle=BorderStyle;
			MyControl.AccessKey=AccessKey;
			MyControl.BorderWidth=BorderWidth;
			MyControl.CssClass=MyCsClass;
			MyControl.Visible=visible;
			MyControl.Width=Width;
			MyControl.Height=Height;
			MyControl.ForeColor=ForeColor;
                 
		}

		public static void InitGeneralTableLayout(System.Web.UI.WebControls.TableItemStyle TableItemStyle,System.Drawing.Color ForeColor,System.Drawing.Color BackColor,System.Drawing.Color BorderColor,System.Web.UI.WebControls.BorderStyle BorderStyle,System.Web.UI.WebControls.Unit BorderWidth,string MyCsClass,System.Web.UI.WebControls.HorizontalAlign HorizontalAlign,System.Web.UI.WebControls.VerticalAlign VerticalAlign,System.Web.UI.WebControls.Unit Width,System.Web.UI.WebControls.Unit Height)
		{
			TableItemStyle.BackColor=BackColor;
			TableItemStyle.BorderColor=BorderColor;
			TableItemStyle.BorderStyle=BorderStyle;
			TableItemStyle.BorderWidth=BorderWidth;
			TableItemStyle.CssClass=MyCsClass;
			TableItemStyle.HorizontalAlign=HorizontalAlign;
            TableItemStyle.VerticalAlign=VerticalAlign;
			TableItemStyle.ForeColor=ForeColor;
			TableItemStyle.Width=Width;
			TableItemStyle.Height=Height;
			
		}
		public static void InitCssClass(System.Web.UI.WebControls.TableItemStyle TableItemStyle,string CssClass)
		{
			TableItemStyle.CssClass= CssClass;
		}
	}
}
