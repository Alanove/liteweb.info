using System;

using lw.Operators.Security;
using lw.WebTools;

namespace lw.cms
{
	/// <summary>
	/// This page must be reffered withing all the cms 2016 version inside pages
	/// </summary>
	public class PageProperties : lw.Base.CustomPage
	{
		/// <summary>
		/// Constructor
		/// </summary> 
		public PageProperties()
		{
			this.ClientIDMode = System.Web.UI.ClientIDMode.Static;
			this.CMSInside = false;
			this.PageProperties = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			Manager.CheckAccess(this.Editable);
			base.OnLoad(e);
		}
		public override void DataBind()
		{
			RegisterStyleSheet("font-awesome", Config.GetFromWebConfig("fontAwesome"));
			RegisterStyleSheet("DatePickerCSS", Config.GetFromWebConfig("DatePickerCSS"));

			base.DataBind();
			RegisterScriptFile("bootstrap", Config.GetFromWebConfig("bootstrap"));
			RegisterScriptFile("MomentJS", Config.GetFromWebConfig("MomentJS"));
			RegisterScriptFile("DatePickerJS", Config.GetFromWebConfig("DatePickerJS"));

			RegisterLoadScript("registerself", _registerself, true);
		}

		#region consts
		const string _registerself = @"(function()
{
	try
	{
		top.lw.manager.registerframe(window);
	}
	catch(e)
	{
	}
})();";
		#endregion
	}
}