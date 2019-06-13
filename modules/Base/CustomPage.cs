using lw.CTE;
using lw.CTE.Enum;
using lw.WebTools;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using lw.js.Controls;

namespace lw.Base
{
	/// <summary>
	/// CustomPage is the holder of every control ever created using the lw library.
	/// All pages must inherit CustomPage in order for the controls and everything else to work.
	/// </summary>
	public class CustomPage : System.Web.UI.Page
	{
		#region variables
		//javascript holders
		protected NameValueCollection ScriptFiles = new NameValueCollection(),
			HeaderScripts = new NameValueCollection(),
			LoadScripts = new NameValueCollection(),
			UnloadScripts = new NameValueCollection(),
			CssFiles = new NameValueCollection(),
			Styles = new NameValueCollection(),
			HeaderScriptFiles = new NameValueCollection();

		System.Web.UI.HtmlControls.HtmlHead _head;
		private System.Web.UI.HtmlControls.HtmlControl _body = null;

		StringBuilder loggedTime = null;

		/// <summary>
		/// Represents the description of the page that is added to the meta tag "description"
		/// The description is automatically read from the Parameters Config files if not declared within the page
		/// </summary>
		public string Description = "";

		/// <summary>
		/// Represents the keywords of the page that is added to the meta tag "keywords"
		/// The keywords are automatically read from the Parameters Config files if not declared within the page
		/// </summary>
		public string Keywords = "";

		/// <summary>
		/// Represents the title of the page that is added to the meta tag "title"
		/// The title is automatically read from the Parameters Config files if not declared within the page
		/// </summary>
		public string CustomTitle = "";

		/// <summary>
		/// Represents the image of the page that is added to the meta tag "og:image"
		/// The image is used to help facebook/social media identify the correct image for the shared page
		/// </summary>
		public string Image = "";

		/// <summary>
		/// Represents the url of the page that is added to the meta tag "og:url" and used for the "canonical" link rel
		/// The url is used to help facebook/social media identify the correct URL of the object that will be used as its permanent ID in the graph
		/// </summary>
		public string Url = "";

		/// <summary>
		/// Represents the security of the page
		/// The IsSecure  is used to open https pages when set to True
		/// </summary>
		public bool isSecure = false;

		int _scriptsRendered = 1;

		/// <summary>
		/// Automatically checks if the user is logged in.
		/// </summary>
		public bool UserLoggedIn = false;


		/// <summary>
		/// Check if CMS inside Page
		/// </summary>
		public bool CMSInside = false;

		/// <summary>
		/// Check if Page Properties
		/// </summary>
		public bool PageProperties = false;



		//Page Context
		/// <summary>
		/// PageContext is a dictionary variable that lives with the page and destroyed as soon as the page gets destroyed.
		/// </summary>
		public HybridDictionary PageContext = new HybridDictionary();

		//Page Warnings
		protected ArrayList PageWarnings = new ArrayList();

		//Page Errors
		protected ArrayList PageErrors = new ArrayList();


		CombinedScripts _headerCombinedScripts, _footerCombinedScripts;

		/// <summary>
		/// Returns the body tag of the page
		/// </summary>
		public System.Web.UI.HtmlControls.HtmlControl BodyControl
		{
			get
			{
				if (_body == null)
				{
					Control parentControl = this;
					if (parentControl.Controls.Count == 1)
						parentControl = parentControl.Controls[0];

					if (parentControl.Controls.Count > 0)
					{
						foreach (Control control in parentControl.Controls)
						{
							var ctrl = control as HtmlControl;
							if (ctrl != null)
							{
								if (ctrl.TagName.ToLower() == "body")
								{
									this._body = ctrl;
									break;
								}
							}
						}
					}
				}
				return this._body;
			}
		}


		/// <summary>
		/// Returns the Head element of the page
		/// </summary>
		protected System.Web.UI.HtmlControls.HtmlHead Head
		{
			get
			{
				if (this.Header != null)
					return this.Header;

				if (_head == null)
				{
					_head = new System.Web.UI.HtmlControls.HtmlHead();
					try
					{
						//this.Controls.AddAt(0, _Head);
					}
					catch (Exception Ex)
					{
					}
				}
				return this._head;
			}
		}

		protected CombinedScripts HeaderCombinedScripts
		{
			get
			{
				this._headerCombinedScripts = this.FindControlRecursive(this, cte.HeaderCombinedScriptsID) as CombinedScripts;
				if (_headerCombinedScripts == null)
				{
					_headerCombinedScripts = new CombinedScripts();
					if (Head != null)
					{
						Head.Controls.AddAt(0, _headerCombinedScripts);
					}
				}
				return _headerCombinedScripts;
			}
		}

		protected CombinedScripts FooterCombinedScripts
		{
			get
			{
				this._footerCombinedScripts = this.FindControlRecursive(this, cte.FooterCombinedScriptsID) as CombinedScripts;
				if (_footerCombinedScripts == null)
				{
					_footerCombinedScripts = new CombinedScripts();
					if (BodyControl != null)
					{
						BodyControl.Controls.Add(_footerCombinedScripts);
					}
					else
					{
						if (Head != null)
						{
							Head.Controls.Add(_footerCombinedScripts);
						}
					}
				}
				return _footerCombinedScripts;
			}
		}

		#endregion

		#region Constructor

		public CustomPage()
		{
			//int i = 0;
		}

		#endregion

		#region internals
		void RenderStyles()
		{
			System.Web.UI.WebControls.Literal l;
			var sb = new StringBuilder();
			if (Styles.Count > 0)
			{
				sb.Append("<style type=\"text/css\">");
				foreach (string key in Styles.Keys)
				{
					sb.Append("/* " + key + " */" + Environment.NewLine);
					sb.Append(Styles[key] + Environment.NewLine);
				}
				sb.Append("</style>");
				l = new System.Web.UI.WebControls.Literal { Text = sb.ToString() };

				this.Head.Controls.AddAt(_scriptsRendered++, l);
			}
		}
		void RenderScripts()
		{
			bool cacheJsFiles = true;

			if (!String.IsNullOrWhiteSpace(lw.WebTools.WebUtils.GetFromWebConfig(cte.CacheJsFiles)))
				cacheJsFiles = bool.Parse(lw.WebTools.WebUtils.GetFromWebConfig(cte.CacheJsFiles));

			System.Web.UI.WebControls.Literal l;
			var sb = new StringBuilder();
			if (ScriptFiles.Count > 0)
			{
				if (CurrentTemplate != cte.NeutreTemplate)
				{
					foreach (string key in HeaderScriptFiles.Keys)
					{
						if (!cacheJsFiles)
						{
							sb.Append(string.Format("<script type=\"text/javascript\" id=\"{0}\" src=\"{1}\"></script>{2}",
								key, HeaderScriptFiles[key], System.Environment.NewLine));
						}
						else
						{
							sb.Append(",");
							sb.Append(HeaderScriptFiles[key]);
						}
					}
					if (cacheJsFiles)
						HeaderCombinedScripts.Scripts += sb.ToString();
					else
					{
						l = new System.Web.UI.WebControls.Literal { Text = sb.ToString() };

						Head.Controls.AddAt(_scriptsRendered++, l);
					}
					sb = new StringBuilder();

					foreach (string key in ScriptFiles.Keys)
					{
						if (!cacheJsFiles)
						{
							sb.Append(string.Format("<script type=\"text/javascript\" id=\"{0}\" src=\"{1}\"></script>{2}",
								key, ScriptFiles[key], System.Environment.NewLine));
						}
						else
						{
							sb.Append(",");
							sb.Append(ScriptFiles[key]);
						}
					}
					if (cacheJsFiles)
						FooterCombinedScripts.Scripts += sb.ToString();
					else
					{
						l = new System.Web.UI.WebControls.Literal { Text = sb.ToString() };

						if (BodyControl != null)
							BodyControl.Controls.Add(l);
						else
							Head.Controls.AddAt(_scriptsRendered++, l);
					}
				}
				else
				{
					sb.Append("lw.SideLoadScripts([");
					string sep = "";
					foreach (string key in ScriptFiles.Keys)
					{
						sb.Append(sep);
						sb.Append(string.Format("{{{0}: \"{1}\"}}",
							key, ScriptFiles[key]));
						sep = ",";
					}
					sb.Append("]);");

					//RegisterLoadScript("sideloadedscripts", sb.ToString());
				}
			}
			string temp = this.GetScriptString(HeaderScripts);
			if (temp != "")
			{
				l = new System.Web.UI.WebControls.Literal
				{
					Text = string.Format("<script type=\"text/javascript\">{0}</script>", WebCompress.JsCompress(temp))
				};

				if (BodyControl != null)
					BodyControl.Controls.Add(l);
				else
					Head.Controls.AddAt(_scriptsRendered++, l);
			}
			temp = this.GetScriptString(LoadScripts);
			l = new System.Web.UI.WebControls.Literal();

			if (CurrentTemplate != cte.NeutreTemplate)
			{

				l.Text = string.Format(@"<script type=""text/javascript"">
//<![CDATA[
lw.AppendInit(function(){{{0}}});
jQuery(document).ready(function(){{lw.init('{1}', jQuery, '{2}', {3});}});
//]]>
</script>", temp,
		  WebContext.Root,
		  (new Config()).GetKey("SiteName"),
		  Editable.ToString().ToLower());

				if (BodyControl != null)
					BodyControl.Controls.Add(l);
				else
					Head.Controls.AddAt(_scriptsRendered++, l);
			}
			else
			{
				l.Text = string.Format(@"<script type=""text/javascript"">
lw.SideLoad(function () {{{0}}});
</script>", WebCompress.JsCompress(temp));
				this.Controls.AddAt(0, l);
			}

			temp = this.GetScriptString(UnloadScripts);
			if (temp != "")
			{
				l = new System.Web.UI.WebControls.Literal
				{
					Text = string.Format(@"<script type=""text/javascript"">
//<![CDATA[
window.onunload = function lw_UnLoad(){{{0}}}</script>
//]]>
", WebCompress.JsCompress(temp))
				};

				if (BodyControl != null)
					BodyControl.Controls.Add(l);
				else
					Head.Controls.AddAt(_scriptsRendered++, l);
			}
		}
		void RenderCSS()
		{
			System.Web.UI.WebControls.Literal l;
			var sb = new StringBuilder();
			if (CssFiles.Count > 0)
			{
				foreach (string key in CssFiles.Keys)
				{
					sb.Append("\r\n");
					sb.Append(string.Format("<link rel=\"stylesheet\" type=\"text/css\" id=\"{0}\" href=\"{1}\" />", key, CssFiles[key]));
					sb.Append("\r\n");
				}
				l = new System.Web.UI.WebControls.Literal();
				l.Text = sb.ToString();

				this.Head.Controls.AddAt(_scriptsRendered++, l);
			}
		}
		string GetScriptString(NameValueCollection scriptCollection)
		{
			var sb = new StringBuilder();
			if (scriptCollection.Count > 0)
			{
				foreach (string key in scriptCollection.Keys)
				{
					if (CurrentTemplate != cte.NeutreTemplate)
					{
						sb.Append("\r\n");
						sb.Append(string.Format("/*#{0}#*/", key));
						sb.Append("\r\n");
					}
					sb.Append(scriptCollection[key]);
					if (CurrentTemplate != cte.NeutreTemplate)
						sb.Append("\r\n");
				}
			}
			if (sb.Length > 0)
				return sb.ToString();
			return "";
		}
		#endregion

		#region override


		protected override void OnPreRender(EventArgs e)
		{
			if (LogLoadTime)
			{
				loggedTime.AppendLine("OnPreRender: " + DateTime.Now.ToString());
			}

			RenderCSS();
			RenderScripts();
			RenderStyles();

			base.OnPreRender(e);
		}

		/// <summary>
		/// Can be overrided to display ajax posback entries
		/// </summary>
		protected virtual void AjaxPostBack()
		{

		}

		protected override void Render(HtmlTextWriter writer)
		{
			AjaxPostBack();
			base.Render(writer);

			//if (Request.Browser.Cookies)
			//{
			//	try
			//	{
			//		var cookie = new HttpCookie(lw.CTE.Variables.TestCookieId, lw.CTE.Variables.TestCookieValue);
			//		Response.Cookies.Add(cookie);
			//	}
			//	catch
			//	{
			//	}
			//}
		}

		public override void DataBind()
		{

			string doNotAutoIncludeLwJsFiles = WebUtils.GetFromWebConfig(lw.CTE.parameters.DoNotAutoIncludeLwJsFiles);

			if (String.IsNullOrWhiteSpace(doNotAutoIncludeLwJsFiles) || doNotAutoIncludeLwJsFiles == "false")
			{
				//this.RegisterScriptFile(Files.html5shiv, Files.html5shivFile);
				this.RegisterHeaderScriptFile(Files.jQuery, Files.jQueryFile);
				this.RegisterHeaderScriptFile(Files.utils, Files.utilsFile);
				this.RegisterHeaderScriptFile(Files.lw, Files.lwFile);
				this.RegisterScriptFile(Files.jQueryFormPlugin, Files.jQueryFormPluginFile);
				this.RegisterScriptFile(Files.lwPlugins, Files.lwPluginsFile);
				this.RegisterScriptFile(Files.Forms, Files.FormsFile);
			}
			else
			{
				//this.RegisterScriptFile(Files.html5shiv, Files.Site_html5shivFile);
				this.RegisterHeaderScriptFile(Files.jQuery, Files.Site_jQueryFile);
				this.RegisterScriptFile(Files.utils, Files.Site_utilsFile);
				this.RegisterScriptFile(Files.lw, Files.Site_lwFile);
				this.RegisterScriptFile(Files.jQueryFormPlugin, Files.Site_jQueryFormPluginFile);
				this.RegisterScriptFile(Files.lwPlugins, Files.Site_lwPluginsFile);
				this.RegisterScriptFile(Files.Forms, Files.Site_FormsFile);
			}


			if (UserLoggedIn)
			{
				if (InlineEditingEnabled && !String.IsNullOrWhiteSpace(WebContext.Profile.OperatorGroupName) && !CMSInside)
				{
					string ver = Config.GetFromWebConfig("CMSVersion");

					if (string.IsNullOrEmpty(ver))
					{
						CssFiles.Add("EditingCSS", string.Format("{0}/skins/inline-cms/inline.min.css", WebContext.Root));
                        ScriptFiles.Add("ckeditor", string.Format("{0}/js/ckeditor/ckeditor.js", WebContext.Root));
                        ScriptFiles.Add("inner pages script", string.Format("{0}/_cms/inline/js/page-script.min.js", WebContext.Root));
					}
					else
					{
						if (!PageProperties)
						{
							CssFiles.Add("EditingCSS", string.Format("{0}/_cms/inline/css/{1}", WebContext.Root, Config.GetFromWebConfig("InlineCMSStyle")));
                            ScriptFiles.Add("ckeditor", string.Format("{0}/js/ckeditor/ckeditor.js", WebContext.Root));
                            ScriptFiles.Add("inner pages script", string.Format("{0}/_cms/inline/js/page-script.min.js", WebContext.Root));
						}
					}
				}
			}
			foreach (Control ctrl in this.Controls)
				ctrl.DataBind();

			base.DataBind(true);

			var cfg = new Config();
			string siteName = cfg.GetKey(lw.CTE.Settings.SiteName);

			if (this.Language != Languages.Default)
			{
				string temp = cfg.GetKey(lw.CTE.Settings.SiteName + "_" + this.Language.ToString());
				if (!String.IsNullOrWhiteSpace(temp))
				{
					siteName = temp;
				}
			}

			if (this.Header != null)
			{
				if (String.IsNullOrWhiteSpace(CustomTitle))
				{
					this.Title = this.Title == "Untitled Page" || String.IsNullOrEmpty(this.Title) ?
							siteName :
								this.Title.IndexOf(siteName, System.StringComparison.Ordinal) < 0 ?
									this.Title + " - " + siteName : this.Title;
				}
				else
					this.Title = CustomTitle;

				if (String.IsNullOrWhiteSpace(this.Description))
					this.Description = cfg.GetKey(lw.CTE.Settings.SiteDescription);
				if (String.IsNullOrWhiteSpace(this.Keywords))
					this.Keywords = cfg.GetKey(lw.CTE.Settings.SiteKeywords);
			}

		}

		protected override void OnLoad(EventArgs e)
		{
			/*
			string err = Request.QueryString["err"];
			if (!String.IsNullOrEmpty(err))
			{
				string error = lw.Content.PagesManager.ErrorMsg(err);
				if (!String.IsNullOrEmpty(error))
				{
					ErrorContext.Add(err, error);
				}
			}
			 * */

			try
			{
				if (UserLoggedIn)
				{
					WebContext.Profile.LastUserActivityDate = DateTime.Now;

					if (!String.IsNullOrWhiteSpace(WebContext.Profile.OperatorGroupName))
					{
						InitEditing(true);
					}
					if (Request.RawUrl != WebContext.Request.Url.ToString())
					{
						if (Request.RawUrl.IndexOf(lw.CTE.parameters.Edit_Param + "=true", System.StringComparison.Ordinal) >= 0)
						{
							InitEditing(true);
						}
					}
					else
					{
						if (WebContext.Request.QueryString[lw.CTE.parameters.Edit_Param] == lw.CTE.parameters.Edit_Param_Value)
						{
							InitEditing(true);
						}
					}
				}

			}
			catch
			{
			}

			DataBind();

			if (this.Header != null)
			{
				if (Description != "")
				{
					HtmlMeta metaDescription = new HtmlMeta();
					metaDescription.Content = Description;
					metaDescription.Name = "description";
					this.Header.Controls.Add(metaDescription);

					// Facebook
					HtmlMeta ogDescription = new HtmlMeta();
					ogDescription.Attributes.Add("property", "og:description");
					ogDescription.Content = Description;
					this.Header.Controls.Add(ogDescription);

					// Google Plus
					HtmlMeta gpDescription = new HtmlMeta();
					gpDescription.Attributes.Add("itemprop", "description");
					gpDescription.Content = Description;
					this.Header.Controls.Add(gpDescription);

				}

				if (Keywords != "")
				{
					HtmlMeta metaKeywords = new HtmlMeta();
					metaKeywords.Content = Keywords;
					metaKeywords.Name = "keywords";
					this.Header.Controls.Add(metaKeywords);
				}

				if (Image != "")
				{
					// Facebook
					HtmlMeta ogImage = new HtmlMeta();
					ogImage.Attributes.Add("property", "og:image");
					ogImage.Content = Image;
					this.Header.Controls.Add(ogImage);

					// Google Plus
					HtmlMeta gpImage = new HtmlMeta();
					gpImage.Attributes.Add("itemprop", "image");
					gpImage.Content = Image;
					this.Header.Controls.Add(gpImage);
				}


				if (CustomTitle != "")
				{
					// Facebook
					HtmlMeta ogTitle = new HtmlMeta();
					ogTitle.Attributes.Add("property", "og:title");
					ogTitle.Content = CustomTitle;
					this.Header.Controls.Add(ogTitle);

					// Google Plus
					HtmlMeta gpTitle = new HtmlMeta();
					gpTitle.Attributes.Add("itemprop", "name");
					gpTitle.Content = CustomTitle;
					this.Header.Controls.Add(gpTitle);
				}


				if (Url != "")
				{
					// Facebook
					HtmlMeta ogUrl = new HtmlMeta();
					ogUrl.Attributes.Add("property", "og:url");
					ogUrl.Content = Url;
					this.Header.Controls.Add(ogUrl);

					HtmlLink canonical = new HtmlLink();
					canonical.Attributes.Add("rel", "canonical");
					canonical.Href = Url;
					this.Header.Controls.Add(canonical);
				}


			}

			CheckDomain();
			base.OnLoad(e);
		}

		protected override void OnUnload(EventArgs e)
		{
			base.OnUnload(e);
		}


		bool? isSecureConnection = null;
		/// <summary>
		/// Returns true if the connection is secrure
		/// Taking into consideration all available variables that might come from CloudeFlare and not just Request.IsSecureConnection;
		/// </summary>
		public bool IsSecureConnection
		{
			get
			{
				if (isSecureConnection == null)
				{
					isSecureConnection = Request.IsSecureConnection;
					if (!isSecureConnection.Value)
					{
						isSecureConnection = Request.ServerVariables["HTTP_X_FORWARDED_PROTO"] == "https" ||
							Request.ServerVariables["HTTP_CF_VISITOR"].IndexOf("https") > 0;
					}
				}
				return isSecureConnection.Value;
			}
		}

		public void CheckDomain()
		{
			//return;

			string defaultDomain = WebTools.Config.GetFromWebConfig(lw.CTE.parameters.Domain);
			string secureDomain = WebTools.Config.GetFromWebConfig(lw.CTE.parameters.SecureDomain);
			string currentDomain = WebContext.ServerName;
			
			string exceptionString = "default.aspx";


			// if secure domain exist

			if (!string.IsNullOrWhiteSpace(secureDomain))
			{
				// (check if match)

				if (secureDomain.Equals(currentDomain, StringComparison.InvariantCultureIgnoreCase)) // domain match 
				{
					// (check if secure)

					// if not secure [redirect to secure]

					if (!IsSecureConnection)
					{
						string newLocation = Request.Url.AbsoluteUri.Replace("http://", "https://");

						if (newLocation.ToLower().EndsWith(exceptionString))
						{
							newLocation = newLocation.Remove(newLocation.LastIndexOf(exceptionString, StringComparison.CurrentCultureIgnoreCase), exceptionString.Length);
						}

						Response.Redirect(newLocation);
					}

				}
				else // domain not match [change and redirect]
				{
					Response.StatusCode = 301;
					Response.Status = "301 Moved Permanently";

					string newLocation = string.Format("https://{0}{1}{2}",
					 secureDomain,
					 WebContext.Request.ServerVariables["URL"],
					 Request.QueryString.Keys.Count == 0 ? "" : "?" + Request.QueryString);

					if (newLocation.ToLower().EndsWith(exceptionString))
					{
						newLocation = newLocation.Remove(newLocation.LastIndexOf(exceptionString, StringComparison.CurrentCultureIgnoreCase), exceptionString.Length);
					}

					Response.RedirectLocation = newLocation;
					Response.End();
				}

			}
			else // secure domain doesn't exist
			{
				// (check if default domain exist)
				if (!string.IsNullOrWhiteSpace(defaultDomain))
				{

					// (check if match)
					if (defaultDomain.Equals(currentDomain, StringComparison.InvariantCultureIgnoreCase)) // domain match
					{
						//continue

						// we can still check if the page is secure in the database and redirect
					}
					else // domain not match [change and redirect]
					{
						Response.StatusCode = 301;
						Response.Status = "301 Moved Permanently";

						string newLocation = string.Format("http://{0}{1}{2}",
							defaultDomain,
							WebContext.Request.ServerVariables["URL"],
							Request.QueryString.Keys.Count == 0 ? "" : "?" + Request.QueryString);

						if (newLocation.ToLower().EndsWith(exceptionString))
						{
							newLocation = newLocation.Remove(newLocation.LastIndexOf(exceptionString, StringComparison.CurrentCultureIgnoreCase), exceptionString.Length);
						}
						Response.RedirectLocation = newLocation;
						Response.End();
					}
				}
			}


			//	if ((!String.IsNullOrWhiteSpace(secureDomain) && !secureDomain.Equals(currentDomain, StringComparison.InvariantCultureIgnoreCase)) || secureProtocol == "off")
			//	{


			//		Response.StatusCode = 301;
			//		Response.Status = "301 Moved Permanently";

			//		string newLocation = string.Format("https://{0}{1}{2}",
			//		 secureDomain,
			//		 WebContext.Request.ServerVariables["URL"],
			//		 Request.QueryString.Keys.Count == 0 ? "" : "?" + Request.QueryString);

			//		string exceptionString = "Default.aspx";
			//		if (newLocation.EndsWith("Default.aspx") || newLocation.EndsWith("default.aspx"))
			//		{
			//			newLocation = newLocation.Remove(newLocation.LastIndexOf(exceptionString, StringComparison.CurrentCultureIgnoreCase), exceptionString.Length);
			//			//newLocation.Substring(0, newLocation.Length - exceptionString.Length);
			//		}

			//		Response.RedirectLocation = newLocation;
			//		//Response.Redirect(newLocation, true);
			//		Response.End();
			//	}

			//}
			//else
			//{

			//	if ((!String.IsNullOrWhiteSpace(defaultDomain) && !defaultDomain.Equals(currentDomain, StringComparison.InvariantCultureIgnoreCase)) || secureProtocol == "on")
			//	{

			//		Response.StatusCode = 301;
			//		Response.Status = "301 Moved Permanently";

			//		string newLocation = string.Format("http://{0}{1}{2}",
			//			defaultDomain,
			//			WebContext.Request.ServerVariables["URL"],
			//			Request.QueryString.Keys.Count == 0 ? "" : "?" + Request.QueryString);

			//		string exceptionString = "Default.aspx";
			//		if (newLocation.EndsWith("Default.aspx") || newLocation.EndsWith("default.aspx"))
			//		{
			//			newLocation = newLocation.Remove(newLocation.LastIndexOf(exceptionString, StringComparison.CurrentCultureIgnoreCase), exceptionString.Length);
			//			//newLocation.Substring(0, newLocation.Length - exceptionString.Length);
			//		}
			//		Response.RedirectLocation = newLocation;
			//		//Response.Redirect(newLocation, true);
			//		Response.End();
			//	}
			//}
		}


		bool _hasPostBack = false;
		public bool HasPostBack
		{
			set
			{
				_hasPostBack = value;
			}
			get
			{
				return _hasPostBack;
			}
		}

		/// <summary>
		/// The Page_PreInit method is called at the beginning of the page initialization stage.
		/// After the Page_PreInit method is called, personalization information is loaded and the page theme, if any, is initialized. This is also the preferred stage to dynamically define a PageTheme or MasterPage for the Page.
		/// Raising an event invokes the event handler through a delegate. For more information, see Handling and Raising Events.
		/// The Page_PreInit method also allows derived classes to handle the event without attaching a delegate. This is the preferred technique for handling the event in a derived class.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void Page_PreInit(object sender, EventArgs e)
		{
			if (LogLoadTime)
			{
				loggedTime = new StringBuilder();
				loggedTime.AppendLine("Page_PreInit: " + DateTime.Now.ToString());
			}

			if (!string.IsNullOrWhiteSpace(CurrentTemplate))
				MasterPageFile = CurrentTemplate;

			if (!String.IsNullOrEmpty(MasterPageFile))
			{
				if (MasterPageFile.IndexOf(".master", System.StringComparison.Ordinal) < 0)
					MasterPageFile = MasterPageFile + ".master";
				if (MasterPageFile.IndexOf("~", System.StringComparison.Ordinal) != 0 && MasterPageFile.IndexOf("/", System.StringComparison.Ordinal) != 0)
					this.MasterPageFile = string.Format("{0}/{1}", Settings.TemplatesFolder, MasterPageFile);
			}
			try
			{
				UserLoggedIn = WebContext.Profile.UserLogged;

			}
			catch
			{

			}
		}

		protected virtual void Page_Init(object sender, EventArgs e)
		{
			if (LogLoadTime)
			{
				loggedTime.AppendLine("Page_Init: " + DateTime.Now.ToString());
			}
		}

		protected virtual void Page_InitComplete(object sender, EventArgs e)
		{
			if (LogLoadTime)
			{
				loggedTime.AppendLine("Page_InitComplete: " + DateTime.Now.ToString());
			}
		}

		protected virtual void OnPreLoad(object sender, EventArgs e)
		{
			if (LogLoadTime)
			{
				loggedTime.AppendLine("OnPreLoad: " + DateTime.Now.ToString());
			}
		}
		//protected virtual void Page_Load(object sender, EventArgs e)
		//{
		//	if (LogLoadTime)
		//	{
		//		loggedTime.AppendLine("Page_Load: " + DateTime.Now.ToString());
		//	}
		//}

		protected virtual void Page_LoadComplete(object sender, EventArgs e)
		{
			if (LogLoadTime)
			{
				loggedTime.AppendLine("Page_LoadComplete: " + DateTime.Now.ToString());
			}
		}

		protected virtual void Page_UnLoad(object sender, EventArgs e)
		{
			if (LogLoadTime)
			{
				loggedTime.AppendLine("Page_UnLoad: " + DateTime.Now.ToString());
				lw.Error.Handler.Log("Load Time" + Environment.NewLine + loggedTime.ToString());
			}
		}

		/// <summary>
		/// Recursively finds a control within the root control inside the page. 
		/// </summary>
		/// <param name="root">The root control, from where should start the search</param>
		/// <param name="id">The ID of the control to find.</param>
		/// <returns>The found control or null if none found.</returns>
		public Control FindControlRecursive(Control root, string id)
		{
			if (root.ID == id)
				return root;

			foreach (Control ctl in root.Controls)
			{
				Control foundCtl = FindControlRecursive(ctl, id);

				if (foundCtl != null)
					return foundCtl;
			}

			return null;
		}
		/// <summary>
		/// Finds a control within the the page. 
		/// </summary>
		/// <param name="id">The ID of the control to find.</param>
		/// <returns>The found control or null if none found.</returns>
		public override Control FindControl(string id)
		{
			Control ctrl = base.FindControl(id);
			if (ctrl != null)
				return ctrl;
			return FindControlRecursive(Page, id);
		}
		public override void VerifyRenderingInServerForm(Control control)
		{
			try
			{
				base.VerifyRenderingInServerForm(control);
			}
			catch
			{
			}
		}

		/// <summary>
		/// Automatically called when an error occurs on the page.
		/// It handles the error by adding an entry in the error log files.
		/// It also sends an email to the error email of the site.
		/// </summary>
		/// <param name="e">The event</param>
		protected override void OnError(EventArgs e)
		{
			Exception ex = lw.Error.Handler.HandleError();

			string errorPage = new Config().GetKey(CTE.Settings.ErrorPage);

			if (String.IsNullOrWhiteSpace(errorPage))
				throw (ex);
			else
				Response.Redirect(errorPage, true);

		}
		#endregion

		#region Language
		Languages _language = Languages.Default;

		public Languages Language
		{
			get { return _language; }
			set { _language = value; }
		}

		#endregion

		#region public methods

		/// <summary>
		/// Adds description to the meta name="description" tag in the page.
		/// </summary>
		/// <param name="desc">The description of the page</param>
		public void AddDescription(string desc)
		{
			this.Description = desc;
		}

		/// <summary>
		/// Adds keywords to the meta name="keywords" tag in the page.
		/// </summary>
		/// <param name="desc">The keywords of the page</param>
		public void AddKeywords(string desc)
		{
			this.Keywords = desc;
		}


		/// <summary>
		/// Registers a script file into the page and place it in the <![CDATA[<head>]]> section of the page.
		/// Important: For this to work the head tag must have runat="server" <![CDATA[<head runat="server">]]> 
		/// </summary>
		/// <param name="key">The key of the file, having 2 files registered with same key will have only the last file registered.</param>
		/// <param name="file">The file location. Can start with "~"</param>
		public void RegisterHeaderScriptFile(string key, string file)
		{
			string _file = XmlManager.GetFromWebConfig(key);
			if (!String.IsNullOrEmpty(_file))
			{
				file = _file;
			}
			RegisterScript(HeaderScriptFiles, key, file, true);
		}


		/// <summary>
		/// Registers a script file into the page and place it at the bottom of the body before the <![CDATA[</body>]]> closing tag for fast loading.
		/// Important: For this to work the body tag must have runat="server" <![CDATA[<body runat="server">]]> 
		/// Fallback: if <![CDATA[<body runat=server>]]> not found, the scripts will be added to the head.
		/// </summary>
		/// <param name="key">The key of the file, having 2 files registered with same key will have only the last file registered.</param>
		/// <param name="file">The file location. Can start with "~"</param>
		public void RegisterScriptFile(string key, string file)
		{
			string _file = XmlManager.GetFromWebConfig(key);
			if (!String.IsNullOrEmpty(_file))
			{
				file = _file;
			}
			RegisterScript(ScriptFiles, key, file, true);
		}

		/// <summary>
		/// Registers a stylesheet file into the page and place it in the <![CDATA[<head>]]> section of the page.
		/// Important: For this to work the head tag must have runat="server" <![CDATA[<head runat="server">]]> 
		/// </summary>
		/// <param name="key">The key of the file, having 2 files registered with same key will have only the last file registered.</param>
		/// <param name="file">The file location. Can start with "~"</param>
		public void RegisterStyleSheet(string key, string file)
		{
			RegisterScript(CssFiles, key, file, true);
		}

		/// <summary>
		/// Adds inline styles block to the page from the code behind.
		/// Can be used with tags which must include some styles, or selected menu, etc...
		/// </summary>
		/// <param name="key">The key of the insereted styles (must be unique)</param>
		/// <param name="style">The style parameters</param>
		/// <param name="Override">This tells the style of override existing styles with same keys</param>
		public void AddStyles(string key, string style, bool Override)
		{
			RegisterScript(Styles, key, style, Override);
		}

		/// <summary>
		/// Adds inline script block to the page from the code behind.
		/// Can be used with tags which must include some scripts, or selected menus, etc...
		/// </summary>
		/// <param name="key">The key of the insereted styles (must be unique)</param>
		/// <param name="script">The style parameters</param>
		/// <param name="Override">This tells the script of override existing scripts with same keys</param>
		public void RegisterHeaderScript(string key, string script, bool Override)
		{
			RegisterScript(HeaderScripts, key, script, Override);
		}

		/// <summary>
		/// Adds inline script block that runs with the page load from the code behind.
		/// Can be used with tags which must include some styles, or selected menu, etc...
		/// </summary>
		/// <param name="key">The key of the insereted styles (must be unique)</param>
		/// <param name="script">The style parameters</param>
		public void RegisterLoadScript(string key, string script)
		{
			RegisterScript(LoadScripts, key, script, true);
		}

		/// <summary>
		/// Adds inline script block that runs with the page load from the code behind.
		/// Can be used with tags which must include some styles, or selected menu, etc...
		/// </summary>
		/// <param name="key">The key of the insereted styles (must be unique)</param>
		/// <param name="script">The style parameters</param>
		/// <param name="Override">Default: true. This tells the script of override existing scripts with same keys</param>
		public void RegisterLoadScript(string key, string script, bool Override)
		{
			RegisterScript(LoadScripts, key, script, Override);
		}

		/// <summary>
		/// Register a client script block that runs on page.unload 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="script"></param>
		/// <param name="Override"></param>
		public void RegisterUnLoadScript(string key, string script, bool Override)
		{
			RegisterScript(UnloadScripts, key, script, Override);
		}
		public void RegisterScript(NameValueCollection scriptCollection, string key, string script, bool Override)
		{
			if (scriptCollection[key] != null && !Override)
				return;

			scriptCollection[key] = script.IndexOf("~", System.StringComparison.Ordinal) == 0 ? ResolveClientUrl(script) : script;
		}

		/// <summary>
		/// Adds a text warning to the PageWarnings Context.
		/// </summary>
		/// <param name="warning"></param>
		public void AddWarning(string warning)
		{
			if (this.PageWarnings == null)
				this.PageWarnings = new ArrayList();

			PageWarnings.Add(warning);
		}

		/// <summary>
		/// Adds an object to the PageContext Object.
		/// </summary>
		/// <param name="key">The key of the object</param>
		/// <param name="v"></param>
		/// <param name="Override">Specifies if the new object should override old existing objects with same key</param>
		public void AddContext(string key, object v, bool Override)
		{
			if (this.PageContext == null)
				PageContext = new HybridDictionary();

			if (this.PageContext[key] != null && !Override)
				return;

			PageContext[key] = v;
		}

		/// <summary>
		/// Adds an error message to the page's PageErrors Dictionnary.
		/// This can be used for validation or if you want to display some errors to the user.
		/// </summary>
		/// <param name="error"></param>
		public void AddError(string error)
		{
			if (this.PageErrors == null)
				this.PageErrors = new ArrayList();

			PageErrors.Add(error);
		}

		#endregion

		bool _logLoadTime = false;
		/// <summary>
		/// Logs the load time needed for every step of the page life cycle and adds the total time to the log file
		/// </summary>
		public bool LogLoadTime
		{
			get
			{
				return _logLoadTime;
			}
			set
			{
				_logLoadTime = value;
			}
		}

		#region Properties
		string _currentTemplate = null;
		/// <summary>
		/// Gets of sets the current template (Master Page) of the page.
		/// By default it reads from the page's querystring: template
		/// </summary>
		public string CurrentTemplate
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_currentTemplate))
					_currentTemplate = Request.QueryString["template"];
				return _currentTemplate;
			}
			set
			{
				_currentTemplate = value;
			}
		}
		#endregion

		#region edit mode

		bool _editable = false;

		bool _editingInitialized = false;

		bool? _inlineEditingEnabled = null;
		/// <summary>
		/// If true the inline editing will be enabled
		/// This is set in Web.Config App for key "InlineEditingEnabled"
		/// </summary>
		public bool InlineEditingEnabled
		{
			get
			{
				if (this.GetQueryValue("preview") == "true" || this.GetQueryValue("original") == "true")
					_inlineEditingEnabled = false;

				if (_inlineEditingEnabled == null)
				{
					if (Config.GetFromWebConfig(lw.CTE.cms.InlineEditingEnabled) != null)
						_inlineEditingEnabled = Config.GetFromWebConfig(lw.CTE.cms.InlineEditingEnabled).ToLower() == "true";
					else
						_inlineEditingEnabled = false;
				}
				return _inlineEditingEnabled.Value;
			}
		}

		bool _hasDraft = false;
		/// <summary>
		/// Set to true if the page contains a draft version
		/// </summary>
		public bool HasDraft
		{
			get
			{
				return _hasDraft;
			}
			set
			{
				_hasDraft = value;
			}
		}

		/// <summary>
		/// Initialize the inline editing tools.
		/// </summary>
		/// <param name="MakeEditable"></param>
		public void InitEditing(bool MakeEditable)
		{
			if (!InlineEditingEnabled)
				return;

			_editable = MakeEditable;
			if (_editingInitialized)
				return;
			_editingInitialized = true;
		}
		public bool Editable
		{
			get { return _editable; }
			set { _editable = value; }
		}
		#endregion

		#region Routing
		NameValueCollection _routingQuery;

		/// <summary>
		/// Transforms Routing VirtualPath into a virtual QueryString
		/// ex: 
		///	<UrlRouting>
		///		<Description>downloads</Description>
		///		<Route>downloads</Route>
		///		<Redirection>~/_Routing/page.aspx?section=downloads&amp;sub=default</Redirection>
		///	</UrlRouting>
		///	section and sub will not be returned as usual QueryString, instead you can get their values using the RoutingQuery collection
		/// </summary>
		public NameValueCollection RoutingQuery
		{
			get
			{
				if (RouteData != null)
				{
					if (_routingQuery == null)
					{
						_routingQuery = new NameValueCollection();
						PageRouteHandler h = RouteData.RouteHandler as PageRouteHandler;
						if (h != null)
						{
							if (h.VirtualPath.IndexOf("?") > 0)
							{
								_routingQuery = HttpUtility.ParseQueryString(h.VirtualPath.Split('?')[1]);
							}
						}
					}
				}
				return _routingQuery;
			}
		}


		#endregion

		#region Query Interraction

		/// <summary>
		/// returns a value from 
		/// 1 - Query String
		/// 2 - RouteData
		/// 3 - RoutingQuery <seealso cref="RoutingQuery"/>
		/// </summary>
		/// <param name="key">The key</param>
		/// <returns>The value if found otherwise null</returns>
		public string GetQueryValue(string key)
		{
			key = WebUtils.GetParameter(key);


			string ret = WebContext.Request[key];
			if (!String.IsNullOrWhiteSpace(ret))
				return ret;

			var obj = this.RouteData.Values[key];
			return obj != null ? obj.ToString() : RoutingQuery[key];
		}

		#endregion
	}


}