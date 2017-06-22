using System;

using lw.Operators.Security;
using lw.WebTools;

namespace lw.cms
{
	/// <summary>
	/// This page must be reffered withing all the cms pages
	/// </summary>

	public class CMS2Page : lw.Base.CustomPage
	{
		public CMS2Page()
		{
			this.ClientIDMode = System.Web.UI.ClientIDMode.Static;
		}

		protected override void OnLoad(EventArgs e)
		{
			Manager.CheckAccess(this.Editable);
			base.OnLoad(e);
		}
		public override void DataBind()
		{
			RegisterStyleSheet("skin", Config.GetFromWebConfig("managerCss"));

			base.DataBind();
			RegisterScriptFile("jquery-ui", Config.GetFromWebConfig("jquery-ui"));
			try
			{
				RegisterScriptFile("inside-plugins", Config.GetFromWebConfig("insidePlugins"));
			}
			catch (Exception ex)
			{

			}
			RegisterScriptFile("inside-script", Config.GetFromWebConfig("insideScript"));

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