using System;

using lw.Operators.Security;
using lw.WebTools;

namespace lw.cms
{
	/// <summary>
	/// This page must be reffered withing all the cms 2016 version inside pages
	/// </summary>

	public class CMS3Page : lw.Base.CustomPage
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public CMS3Page()
		{ 
			this.ClientIDMode = System.Web.UI.ClientIDMode.Static;
			this.CMSInside = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			Manager.CheckAccess(this.Editable);
			base.OnLoad(e);
		}
		public override void DataBind()
		{
			RegisterStyleSheet("CMS-Inside-CSS", Config.GetFromWebConfig("CMSInside"));
			RegisterStyleSheet("font-awesome", Config.GetFromWebConfig("fontAwesome"));
			RegisterStyleSheet("DatePickerCSS", Config.GetFromWebConfig("DatePickerCSS"));
			RegisterStyleSheet("ToasterCSS", Config.GetFromWebConfig("ToasterCSS"));

			base.DataBind();
			RegisterScriptFile("CMS-Inside-script", Config.GetFromWebConfig("CMSInsideScript"));
			RegisterScriptFile("bootstrap", Config.GetFromWebConfig("bootstrap"));
			RegisterScriptFile("MomentJS", Config.GetFromWebConfig("MomentJS"));
			RegisterScriptFile("DatePickerJS", Config.GetFromWebConfig("DatePickerJS"));
			RegisterScriptFile("ToasterJS", Config.GetFromWebConfig("ToasterJS"));

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