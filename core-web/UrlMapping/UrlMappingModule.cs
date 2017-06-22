using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Security;
using System.Web.UI;
using lw.CTE;

namespace lw.UrlMapping
{

	/// <summary>
	/// An HttpModule that, when added to the ASP.NET pipeline, intercepts incoming URLs
	/// and attempts to match them against a list of predefined static and dynamic
	/// URL templates; if a match is found, the UrlMappingModule redirects the request
	/// to an associated redirection URL.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The UrlMappingModule relies on a configured provider to supply the listing of
	/// URL templates and associated redirection mappings.  The UrlMappingModule assembly
	/// includes two concrete providers, <see cref="XmlUrlMappingProvider" /> and
	/// <see cref="SqlUrlMappingProvider" /> allowing URL mappings to be stored in an
	/// XML file or SQL Server database respectively.  The assembly also defines the
	/// <see cref="IUrlMappingProvider" /> interface for developers to create custom
	/// provider implementations.
	/// </para>
	/// <para>
	/// A URL mapping item minimally consists of an identifying name, a URL template, and a 
	/// corresponding redirection URL.  The URL template is interpreted as an application-relative
	/// URL, defined as either static references (such as <i>"Default.aspx"</i>) or dynamic
	/// references designed to be resolved at runtime.
	/// </para>
	/// <para>
	/// Fundamentally, a dynamic URL template is a regular expression, and developers who chose
	/// to implement IUrlMappingProvider may enjoy complete control over the expression syntax.
	/// To simplify template usage however, the built-in XmlUrlMappingProvider and
	/// SqlUrlMappingProvider classes employ the methods in the utility class 
	/// <see cref="UrlMappingHelper" /> to translate strings with embedded [squareBracketed] tokens
	/// into actual regular expressions.  This allows end users to define dynamic URL templates
	/// with a simpler syntax, using [Tokens] to substitute for portions of an incoming URL
	/// that should be captured and supplied to the redirection URL.
	/// </para>
	/// <para>
	/// For example, consider the following URL templates, and the corresponding redirection URLs:
	/// <list type="table">
	/// <listheader>
	/// <term>URL template</term>
	/// <description>Redirection</description>
	/// </listheader>
	/// <item>
	/// <term>Home.aspx</term>
	/// <description>Default.aspx</description>
	/// </item>
	/// <item>
	/// <term>MSDN.aspx</term>
	/// <description>http://www.msdn.com</description>
	/// </item>
	/// <item>
	/// <term>Customers/[ID]/[Action].aspx</term>
	/// <description>CustAction.aspx</description>
	/// </item>
	/// </list>
	/// In these examples for this web application, a request for <i>Home.aspx</i> will be
	/// redirected to <i>Default.aspx</i>.  A request for <i>MSDN.aspx</i> is likewise static
	/// and will redirect to an external resource.
	/// </para>
	/// <para>
	/// The third item however implies a pattern for dynamic, runtime matching, with two
	/// parameters - [ID] and [Action] - to be captured and appended as querystring parameters
	/// to the redirection URL.
	/// An incoming URL of <i>Customers/407/Display.aspx</i> is therefore re-routed to the
	/// redirection URL <i>CustAction.aspx?ID=407&amp;Action=Display</i>.
	/// </para>
	/// <para>
	/// To use the UrlMappingModule, include a reference to the UrlMappingModule.dll
	/// assembly in your web application project, or copy the assembly to your application's
	/// <i>bin</i> directory.  All implementations of the UrlMappingModule require the following
	/// entry in Web.config:
	/// <code>
	/// &lt;configuration&gt;
	///  &lt;system.web&gt; 
	///    &lt;httpModules&gt;
	///      &lt;add name="UrlMappingModule" 
	///           type="UNLV.IAP.UrlMapping.UrlMappingModule, UrlMappingModule"
	///           /&gt;
	///    &lt;/httpModules&gt;
	///  &lt;/system.web&gt;
	/// &lt;/configuration&gt;
	/// </code>
	/// </para>
	/// <para>
	/// In addition, a custom configuration section must be added to Web.config.  This section
	/// defines configuration settings specific to a provider and is therefore provider-dependent.
	/// For example, to use the XmlUrlMappingProvider with the UrlMappingModule, add the following
	/// to Web.config:
	/// <code>
	/// &lt;configuration&gt;
	///  &lt;configSections&gt;
	///    &lt;section name="urlMappingModule" 
	///             type="UNLV.IAP.UrlMapping.XmlUrlMappingProviderConfiguration, UrlMappingModule"
	///             /&gt;
	///  &lt;/configSections&gt;
	///    
	///  &lt;urlMappingModule providerType="UNLV.IAP.UrlMapping.XmlUrlMappingProvider, UrlMappingModule"
	///                    noMatchAction="..." 
	///                    noMatchRedirectUrl="..." 
	///                    incomingQueryStringBehavior="..." 
	///                    ignoreExtensions="..."
	///                    urlProcessingEvent="..."
	///                    automaticallyUpdateFormAction="..."
	///                    useDependency="..."
	///                    urlMappingFile="..." 
	///                    /&gt;
	///
	///&lt;/configuration&gt;
	/// </code>
	/// Consult the documentation for <see cref="XmlUrlMappingProvider" /> or
	/// <see cref="SqlUrlMappingProvider"/> for additional information on configuration and usage.
	/// For additional information on creating a custom provider, see the 
	/// <see cref="IUrlMappingProvider" /> documentation.
	/// </para>
	/// <para>
	/// Certain configuration attributes affect the behavior of the module itself rather than
	/// the provider.  These include <c>noMatchAction</c>, <c>noMatchRedirectionUrl</c>,
	/// <c>incomingQueryStringBehavior</c>, <c>ignoreExtensions</c>, 
	/// <c>urlProcessingEvent</c>, and <c>automaticallyUpdateFormAction</c>:
	/// <list type="bullet">
	/// <item>
	/// <c>noMatchAction</c> and the optional <c>noMatchRedirectUrl</c> specify
	/// the action taken when an incoming URL goes unmatched
	/// against the provider-supplied collection of mapping items.  
	/// See <see cref="NoMatchActionEnum" /> for more information.
	/// </item>
	/// <item>
	/// <c>incomingQueryStringBehavior</c> determines how the UrlMappingModule will process
	/// querystring arguments on incoming URLs.  See <see cref="IncomingQueryStringBehaviorEnum" />
	/// for more information.
	/// </item>
	/// <item>
	/// <c>ignoreExtensions</c> is an optional list of file extensions, requests for which
	/// should be ignored entirely by the UrlMappingModule (and thus no redirection is
	/// performed).  See the <see cref="UrlMappingProviderConfiguration.IgnoreExtensions">IgnoreExtensions</see>
	/// configuration property for more information.
	/// </item>
	/// <item>
	/// <c>urlProcessingEvent</c> optionally specifies at which point in the request
	/// cycle the UrlMappingModule performs its processing and redirection.
	/// See <see cref="UrlProcessingEventEnum" />
	/// for more information.
	/// </item>
	/// <item>
	/// <c>automaticallyUpdateFormAction</c> is an optional boolean value
	/// that determines whether the UrlMappingModule should automatically adjust
	/// the <c>action</c> attribute for rendered pages' <c>&lt;form&gt;</c>
	/// tags to match the original incoming URL rather than the redirected one
	/// (defaults to <c>true</c>).
	/// See the <see cref="UrlMappingProviderConfiguration.AutomaticallyUpdateFormAction">AutomaticallyUpdateFormAction</see>
	/// configuration property for more information.
	/// </item>
	/// </list>
	/// </para>
	/// <para>
	/// The UrlMappingModule also provides some static utility methods and properties
	/// for the developer to explicitly interface with an active module.  Among these is
	/// the <see cref="RefreshUrlMappings" /> method which the developer may call to have the 
	/// module's provider requery its persisted collection of mapping data and update its
	/// internal cache accordingly.
	/// Note that this and other static methods and properties require the configuration 
	/// section name to be <i>"urlMappingModule"</i>, as shown above.
	/// </para>
	/// </remarks>
	public class UrlMappingModule : IHttpModule
	{
		private const string kMAPPINGMODULE = "urlMappingModule";

		private const string kCONTEXTITEMS_RAWURLKEY = "__UrlMappingModule_RawUrl__";
		private const string kCONTEXTITEMS_ADDEDQSKEY = "__UrlMappingModule_AddedQS__";

		private IUrlMappingProvider _provider;
		private NoMatchActionEnum _noMatchAction;
		private string _noMatchRedirectPage;
		private bool _automaticallyUpdateFormAction;
		private IncomingQueryStringBehaviorEnum _qsBehavior;
		private string[] _ignoreExtensions;
		private UrlProcessingEventEnum _processingEvent;
		private bool _authorizeRedirectionUrl;
		private string _authorizeFailureRedirectUrl;


		#region Static Utility Methods and Properties

		/// <summary>
		/// Returns the active UrlMappingModule, or null if none
		/// </summary>
		/// <returns>the active UrlMappingModule</returns>
		public static UrlMappingModule Module
		{
			get
			{
				HttpContext context = HttpContext.Current;
				if (context != null)
				{
					HttpApplication app = context.ApplicationInstance;
					if (app != null)
					{
						return (app.Modules[kMAPPINGMODULE] as UrlMappingModule);
					}
				}

				// otherwise, return null
				return null;
			}
		}


		/// <summary>
		/// The provider object supplying URL mapping items to this module.
		/// </summary>
		public static IUrlMappingProvider Provider
		{
			get
			{
				UrlMappingModule mod = UrlMappingModule.Module;
				if (mod != null)
					return mod.GetProvider();
				else
					return null;
			}
		}




		/// <summary>
		/// Returns the date and time the provider most recently refreshed
		/// its URL mappings data
		/// </summary>
		public static DateTime LastRefreshTime
		{
			get
			{
				UrlMappingModule mod = UrlMappingModule.Module;
				if (mod != null)
				{
					IUrlMappingProvider prov = mod.GetProvider();
					if (prov != null)
					{
						return prov.GetLastRefreshTime();
					}
				}

				// otherwise return the minvalue
				return DateTime.MinValue;
			}
		}


		/// <summary>
		/// Instructs the provider associated with this UrlMappingModule
		/// to refresh its internally-cached collection of URL mappings data
		/// </summary>
		public static void RefreshUrlMappings()
		{
			UrlMappingModule mod = UrlMappingModule.Module;
			if (mod != null)
			{
				IUrlMappingProvider prov = mod.GetProvider();
				if (prov != null)
				{
					prov.RefreshUrlMappings();
				}
			}
		}

		#endregion



		/// <summary>
		/// Returns the IUrlMappingProvider instance associated with the UrlMappingModule
		/// </summary>
		public IUrlMappingProvider GetProvider()
		{
			return _provider;
		}


		#region Initialization

		private void Initialize()
		{
			// get the configuration section
			UrlMappingProviderConfiguration config
				= (UrlMappingProviderConfiguration)ConfigurationManager.GetSection(kMAPPINGMODULE);

			if (config == null)
				throw new ProviderException("The configuration section for the UrlMappingModule is missing from Web.config.");

			if (!string.IsNullOrEmpty(config.ProviderTypeString))
			{
				// instantiate a concrete provider given the provider type string through reflection            
				Type t = Type.GetType(config.ProviderTypeString);

				// if the type couldn't be retrieved, it could be because the user has it defined in APP_CODE; 
				// try the BuildManager
				if (t == null)
					t = BuildManager.GetType(config.ProviderTypeString, false, true);

				// if we still have an error, throw the exception
				if (t == null)
					throw new ProviderException("Cannot locate the type '" + config.ProviderTypeString + "' for the UrlMappingModule.  Check your web.config settings.");

				// attempt to instantiate the provider given its type
				_provider = (IUrlMappingProvider)Activator.CreateInstance(t);
			}
			else
				_provider = null;

			if (_provider == null)
				throw new ProviderException("Invalid provider for UrlMappingModule.  This must be a type that implements IUrlMappingProvider.  Check your web.config settings, section 'urlMappingModule', attribute 'providerType'");


			// remember other configuration properties
			_noMatchAction = config.NoMatchAction;
			_noMatchRedirectPage = config.NoMatchRedirectUrl;
			_automaticallyUpdateFormAction = config.AutomaticallyUpdateFormAction;
			_qsBehavior = config.IncomingQueryStringBehavior;
			_processingEvent = config.UrlProcessingEvent;
			_authorizeRedirectionUrl = config.AuthorizeRedirectionUrl;
			_authorizeFailureRedirectUrl = config.AuthorizeFailureRedirectUrl;

			// test configuration combinations
			if (_authorizeRedirectionUrl == true && _processingEvent != UrlProcessingEventEnum.AuthorizeRequest)
				throw new ConfigurationErrorsException("When the <urlMappingModule> 'authorizeRedirectionUrl' attribute is 'true', the 'processEvent' attribute must be 'AuthorizeEvent'.");



			// remember the list of extensions to ignore
			_ignoreExtensions = config.IgnoreExtensions.Split(new char[] { ' ', ';', ',' });
			for (int i = 0; i < _ignoreExtensions.Length; i++)
				_ignoreExtensions[i] = _ignoreExtensions[i].Trim().ToLower();

			// allow the provider to initialize itself given the configuration section
			if (_provider != null)
				_provider.Initialize(config);

		}

		#endregion


		#region IHttpModule Members

		/// <summary>
		/// Releases resources used by this module.
		/// </summary>
		public void Dispose()
		{
			if (_provider != null) _provider.Dispose();
		}

		/// <summary>
		/// Initializes the module in a given application context
		/// </summary>
		/// <param name="context">the application context</param>
		public void Init(HttpApplication context)
		{
			Initialize();

			// wire up the processing event
			switch (_processingEvent)
			{
				case UrlProcessingEventEnum.BeginRequest:
					context.BeginRequest += new EventHandler(ProcessUrl);
					break;

				case UrlProcessingEventEnum.AuthenticateRequest:
					context.AuthenticateRequest += new EventHandler(ProcessUrl);
					break;

				case UrlProcessingEventEnum.AuthorizeRequest:
					context.AuthorizeRequest += new EventHandler(ProcessUrl);
					break;
			}


			// if we want to automatically update action attributes for form tags,
			// wire up the PostMapRequestHandler event to capture when the Page
			// handler is instantiated
			if (_automaticallyUpdateFormAction)
				context.PostMapRequestHandler += new EventHandler(OnPostMapRequestHandler);
		}

		/// <summary>
		/// Assigns code to the PreRender event for Web Forms pages
		/// to rewrite the <i>action</i> attribute of the rendered
		/// &lt;form&gt; tag.
		/// </summary>
		/// <param name="sender">the application object</param>
		/// <param name="e">event arguments</param>
		protected void OnPostMapRequestHandler(object sender, EventArgs e)
		{
			// test to see if the handler for this request is a WebForm page
			HttpApplication app = (sender as HttpApplication);
			if (app != null)
			{
				Page page = (app.Context.Handler as Page);
				if (page != null)
				{
					// a page handler is responding to the request;
					// add the hook to update the form action for the page
					if (HttpContext.Current.Items[kCONTEXTITEMS_RAWURLKEY] != null)
						page.PreRenderComplete += new EventHandler(OnPagePreRenderComplete);
				}
			}
		}

		/// <summary>
		/// For redirected pages, adjusts the URL path such that
		/// the <i>action</i> attribute of the rendered &lt;form&gt; tag
		/// is written appropriately for the incoming URL.
		/// </summary>
		/// <param name="sender">the object</param>
		/// <param name="e">event arguments</param>
		protected void OnPagePreRenderComplete(object sender, EventArgs e)
		{
			// make sure the page being rerouted has the proper form action
			if (HttpContext.Current.Items[kCONTEXTITEMS_RAWURLKEY] != null)
			{
				string rawPath = HttpContext.Current.Items[kCONTEXTITEMS_RAWURLKEY].ToString();

				// was there a query string in the original unmapped request?
				string qs = "";
				if (rawPath.Contains("?"))
				{
					int index = rawPath.IndexOf("?");
					qs = (_qsBehavior == IncomingQueryStringBehaviorEnum.Ignore
							  ? ""
							  : qs = rawPath.Substring(index + 1)
						  );
					rawPath = rawPath.Remove(index);
				}
				HttpContext.Current.RewritePath(rawPath, "", qs, false);
			}
		}

		/// <summary>
		/// Process incoming URLs and determine if redirection is appropriate
		/// </summary>
		/// <param name="sender">the application object</param>
		/// <param name="e">event arguments</param>
		protected void ProcessUrl(object sender, EventArgs e)
		{
			HttpApplication app = (sender as HttpApplication);
			if (app != null)
			{
				// should the module ignore this request, based on the extension?
				bool bIgnore = false;
				for (int i = 0; i < _ignoreExtensions.Length; i++)
				{
					if (_ignoreExtensions[i] != "" && app.Request.FilePath.ToLower().EndsWith(_ignoreExtensions[i]))
					{
						bIgnore = true;
						break;
					}
				}

				if (!bIgnore)
				{
					string appPath = app.Request.ApplicationPath.ToLower();

					// if we want to include the queryString in pattern matching, use RawUrl
					// otherwise use Path
					string rawUrl = (_qsBehavior == IncomingQueryStringBehaviorEnum.Include ? app.Request.RawUrl : app.Request.Path);

					rawUrl = rawUrl.ToLower();

					// remember the incoming querystring values
					NameValueCollection incomingQS = app.Request.QueryString;

					// identify the string to pattern match; this should not include
					// "~/" or "/" at the front but otherwise should be an application-relative
					// reference
					string urlRequested = "";
					
					if (appPath != "/" && rawUrl.IndexOf(appPath) == 0)
						urlRequested = rawUrl.Substring(appPath.Length);
					else
						urlRequested = rawUrl;

					if (urlRequested.StartsWith("/"))
						urlRequested = urlRequested.Substring(1);
					
					Regex removeVD = new Regex("^" + appPath + "/", RegexOptions.IgnoreCase);
					urlRequested = removeVD.Replace(rawUrl, "");

					urlRequested = urlRequested.Replace(parameters.Edit_Param + "=" + parameters.Edit_Param_Value, "");

					Regex r = new Regex("(\\?|\\&)$");
					urlRequested = r.Replace(urlRequested, "");
					urlRequested = urlRequested.Replace("?&", "?");


					// inspect the request and perform redirection as necessary
					// start by getting the mapping items from the provider
					// (it is up to the provider to cache these if feasible)
					UrlMappingItemCollection coll
						= (_provider != null ? _provider.GetUrlMappings() : new UrlMappingItemCollection());


					bool matchFound = false;
					foreach (UrlMappingItem item in coll)
					{
						Match match = item.UrlTarget.Match(urlRequested);
						if (match.Success)
						{
							string newPath = item.Redirection;

							// do we want to add querystring parameters for dynamic mappings?
							NameValueCollection qs = new NameValueCollection();
							if (match.Groups.Count > 1)
							{
								for (int i = 1; i < match.Groups.Count; i++)
									qs.Add(item.UrlTarget.GroupNameFromNumber(i), match.Groups[i].Value);
							}

							RerouteRequest(app, newPath, qs, incomingQS);

							// exit the loop
							matchFound = true;
							break;
						}

					}

					// if we didn't find a match, take appropriate action
					if (!matchFound)
					{
						switch (_noMatchAction)
						{
							case NoMatchActionEnum.PassThrough:
								// do nothing; allow the request to continut to be processed normally;
								break;
							case NoMatchActionEnum.Redirect:
								RerouteRequest(app, _noMatchRedirectPage, null, incomingQS);
								break;
							case NoMatchActionEnum.Return404:
								app.Response.StatusCode = 404;
								app.Response.StatusDescription = "File not found.";
								break;
							case NoMatchActionEnum.ThrowException:
								throw new NoMatchFoundException("No UrlMappingModule match found for url '" + urlRequested + "'.");
						}
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Redirects the incoming request to a different URL.
		/// </summary>
		/// <param name="app">the web application object</param>
		/// <param name="newPath">the redirection path</param>
		/// <param name="qs">querystring parameters to apply to the redirection path</param>
		/// <param name="incomingQS">querystring parameters retrieved from the incoming URL</param>
		protected void RerouteRequest(HttpApplication app, string newPath, NameValueCollection qs, NameValueCollection incomingQS)
		{
			// signal to the future page handler that we rerouted from a different URL
			HttpContext.Current.Items.Add(kCONTEXTITEMS_RAWURLKEY, HttpContext.Current.Request.RawUrl);
			HttpContext.Current.Items.Add(kCONTEXTITEMS_ADDEDQSKEY, qs);

			// apply the querystring to the path
			newPath = ApplyQueryString(newPath, qs);

			// if configured, apply the incoming query string variables too
			if (_qsBehavior == IncomingQueryStringBehaviorEnum.PassThrough)
				newPath = ApplyQueryString(newPath, incomingQS);

			// perform the redirection
			if (newPath.StartsWith("~/"))
				RerouteRequestLocal(newPath);
			else if (newPath.StartsWith("/"))
				app.Response.Redirect(newPath);
			else if (newPath.StartsWith("http:") || newPath.StartsWith("https:"))
				app.Response.Redirect(newPath);
			else
				// otherwise, treat it as a local file and force the virtual path
				RerouteRequestLocal("~/" + newPath);
		}

		/// <summary>
		/// Internal method for rerouting to local, application relative paths.
		/// </summary>
		/// <param name="newLocalRelativePath"></param>
		protected void RerouteRequestLocal(string newLocalRelativePath)
		{
			// if we want to authorize the redirected url, do it now
			bool canAccess = true;
			if (_authorizeRedirectionUrl)
			{
				// drop any query string
				string testUrl = newLocalRelativePath.Split(new char[] { '?' })[0];
				canAccess = UrlAuthorizationModule.CheckUrlAccessForPrincipal(testUrl, HttpContext.Current.User, "GET");
			}

			// can we access the locally redirected path?
			if (canAccess)
				HttpContext.Current.RewritePath(newLocalRelativePath, false);
			else
			{
				// nope... authorization failed.
				if (_authorizeRedirectionUrl && !string.IsNullOrEmpty(_authorizeFailureRedirectUrl))
				{
					// the failure URL may contain a {0} placeholder; if it does,
					// substitute the original incoming url for it.    
					string failureUrl = _authorizeFailureRedirectUrl;
					if (_authorizeFailureRedirectUrl.Contains("{0}"))
					{
						string originalUrl = HttpContext.Current.Request.RawUrl;
						string originalEncoded = HttpUtility.UrlEncode(originalUrl);
						failureUrl = string.Format(_authorizeFailureRedirectUrl, originalEncoded);
					}

					HttpContext.Current.RewritePath(failureUrl, true);
					//HttpContext.Current.Response.Redirect(failureUrl);
				}
				else
					throw new NoMatchFoundException("Authorization failed on the redirected URL, and no redirection page was established in the <urlMappingModule> 'authorizeFailureRedirectUrl' attribute in web.config.");
			}

		}


		/// <summary>
		/// Append the given collection of querystring values to the given URL path
		/// </summary>
		/// <param name="path">the URL path, with or without querystring parameters</param>
		/// <param name="qs">querystring parameters to append</param>
		/// <returns></returns>
		protected string ApplyQueryString(string path, NameValueCollection qs)
		{
			// append the given querystring items to the given path
			if (qs != null)
			{
				for (int i = 0; i < qs.Count; i++)
				{
					if ((i == 0) && !path.Contains("?"))
						path += string.Format("?{0}={1}", qs.GetKey(i), qs[i]);
					else
						path += string.Format("&{0}={1}", qs.GetKey(i), qs[i]);
				}
			}

			return path;
		}


	}
}
