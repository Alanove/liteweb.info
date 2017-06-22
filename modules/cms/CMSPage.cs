using System;
using lw.Operators.Security;
using lw.WebTools;

namespace lw.cms
{
	/// <summary>
	/// This page must be reffered withing all the cms pages
	/// </summary>

	public class CMSPage : lw.Base.CustomPage
	{
		protected override void OnLoad(EventArgs e)
		{
			Manager.CheckAccess(this.Editable);
			base.OnLoad(e);
		}
		public override void DataBind()
		{

			base.DataBind();
			RegisterLoadScript("registerself", _registerself, true);
			RegisterScriptFile("Ajax", WebContext.ManagerRoot + "/js/ajax.js");
			RegisterScriptFile("Validator", WebContext.ManagerRoot + "/js/Validators.js");
			RegisterScriptFile("Calendar", WebContext.ManagerRoot + "/js/calendar.js");
			RegisterStyleSheet("Base", WebContext.ManagerRoot + "/skins/base.css");
			RegisterStyleSheet("ext-styles", WebContext.ManagerRoot + "/resources/ext/css/ext-all.css");

			string file = XmlManager.GetFromWebConfig("InternalCssFile");
			if(string.IsNullOrWhiteSpace(file))
				file = WebContext.ManagerRoot + "/skins/inside.css";


			RegisterStyleSheet("Inside", file);
			
			
			RegisterScriptFile("ext-js", WebContext.ManagerRoot + "/resources/ext.js");
			RegisterScriptFile("ext-extensions", WebContext.ManagerRoot + "/resources/ext-extensions.js");
		}

		#region consts
		const string _registerself = @"(function()
{
	try
	{
		top.cms.registerframe(window);
	}
	catch(e)
	{
	}
})();";
		#endregion
	}
}