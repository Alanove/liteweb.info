using System;

using System.Text;
using System.IO;
using System.Web.UI;

namespace lw.Base
{
	public class UserControl : System.Web.UI.Control
	{
		string _control = "";
		
		bool _relative = false;


		public UserControl()
		{
		}
		public UserControl(string ctrl)
		{
			_control = ctrl;
		}



		protected override void OnInit(EventArgs e)
		{
			string path = Control;

			if (path.IndexOf("~") != 0)
			{
				if (!_relative)
					path = string.Format("{0}/{1}", lw.CTE.Folders.UserControlsFolder, Control);
			}

			if (!path.EndsWith(".ascx"))
				path = path + ".ascx";

			System.Web.UI.Control ctrl = this.Page.LoadControl(path);

			this.Controls.Add(ctrl);
			
			base.OnInit(e);
		}

		public override void DataBind()
		{
			int i = 0;
			base.DataBind();
		}

		

		#region Properties

		/// <summary>
		/// String presenting the path of the Control
		/// 1 - By Default all controls must be placed in the controls folder
		/// 2 - Set Relative=true, Controls will be loaded relative to the page.
		/// 3 - Start Control path with "~" the control will be loaded from its absolute path.
		/// </summary>
		public string Control
		{
			get
			{
				return _control;
			}
			set
			{
				_control = value;
			}
		}

		/// <summary>
		/// Returns the HTML Content of the control
		/// Usefull for ajax call backs, if you want to render this specific control
		/// </summary>
		public string HTMLContent
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				StringWriter tw = new StringWriter(sb);
				HtmlTextWriter hw = new HtmlTextWriter(tw);

				RenderControl(hw);
				return sb.ToString();
			}
		}

		


		/// <summary>
		/// Default: false (Controls will be loaded from _controls folder)
		/// If set to true the control will be loaded relative to the page
		/// Take caution that routed pages might not work well with Relative=true as the path will change depending on Route
		/// </summary>
		public bool Relative
		{
			get
			{
				return _relative;
			}
			set
			{
				_relative = value;
			}
		}


		#endregion
	}
}