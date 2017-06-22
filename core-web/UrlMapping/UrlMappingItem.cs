using System.Text.RegularExpressions;

namespace lw.UrlMapping
{

    /// <summary>
    /// One of potentially many items provided to a UrlMappingModule that describes how an incoming
    /// static or dynamic URL should be redirected.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The UrlMappingModule accepts a collection of UrlMappingItem objects from its configured 
    /// provider.  The collection in total defines all possible incomming URL templates the module
    /// may match and redirect.
    /// </para>
    /// <para>
    /// When constructing a UrlMappingItem in code, set the <see cref="Name"/> property to a value
    /// useful to the developer, or to a blank string; the UrlMappingModule does not require a value
    /// for <i>Name</i>.  
    /// </para>
    /// <para>
    /// Set the <see cref="UrlTarget" /> property to a regular expression that defines 
    /// a URL pattern relative to the web application that may be matched by the module.  
    /// The expression may imply a static URL, such as <i>Reports.aspx</i> 
    /// or a dynamic one, such as 
    /// <i>Reports/(?&lt;ID&gt;[a-zA-Z0-9_+%]*).aspx</i>.
    /// </para>
    /// <para>
    /// In the case of dynamic patterns, named captures should be used within the 
    /// regular expression (such as <i>&lt;ID&gt;</i> in the previous example).
    /// Upon redirection, the UrlMappingModule will append any matched named captures as querystring 
    /// parameters to the redirection string.  For example, given the regular expression
    /// <i>Reports/(?&lt;ID&gt;[a-zA-Z0-9_+%]*).aspx</i>
    /// with a redirection string of <i>DisplayReport.aspx</i>, the UrlMappingModule will
    /// match the incoming URL <i>Reports/24.aspx</i> and redirect to the application-relative URL
    /// <i>~/DisplayReport.aspx?ID=24</i>.
    /// </para>
    /// </remarks>
    public class UrlMappingItem
    {
        private string _name;

        /// <summary>
        /// An arbirary identifier for this mapping item.
        /// </summary>
        /// <remarks>
        /// While not required by the UrlMappingModule, the developer may choose
        /// to supply a meaningful Name for items for other inspection purposes.
        /// </remarks>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private Regex _urlTarget;

        /// <summary>
        /// A regular expression defining a pattern for the UrlMappingModule to use 
        /// to match incoming application URLs for possible redirection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The expression set in UrlTarget may imply a static URL, 
        /// such as <i>Reports.aspx</i> or a dynamic one, such as 
        /// <i>Reports/(?&lt;ID&gt;[a-zA-Z0-9_+%]*).aspx"</i>.
        /// </para>
        /// <para>
        /// In the case of dynamic patterns, named captures should be used within the 
        /// regular expression (such as <i>&lt;ID&gt;</i> in the previous example).
        /// Upon redirection, the UrlMappingModule will append any matched named captures as querystring 
        /// parameters to the redirection string.  For example, given the regular expression
        /// <i>Reports/(?&lt;ID&gt;[a-zA-Z0-9_+%]*).aspx"</i>
        /// with a redirection string of <i>DisplayReport.aspx</i>, the UrlMappingModule will
        /// match the incoming URL <i>Reports/24.aspx</i> and redirect to the application-relative URL
        /// <i>~/DisplayReport.aspx?ID=24</i>.
        /// </para>
        /// </remarks>
        public Regex UrlTarget
        {
            get { return _urlTarget; }
            set { _urlTarget = value; }
        }


        private string _redirection;

        /// <summary>
        /// The URL used for redirection upon matching its associated <see cref="UrlTarget" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The redirection string may be an absolute URL, such as <i>http://www.microsoft.com</i>,
        /// a server-relative URL beginning with a "/", such as <i>/AnotherAppOnThisServer/page.aspx</i>,
        /// or an application-relative URL to a concrete resource, optionally beginning with a "~/", such as
        /// <i>~/ReportSummary.aspx</i> or <i>ReportSummary.aspx</i>.
        /// </para>
        /// <para>
        /// The redirection string may contain its own querystring parameters, such as 
        /// <i>ReportSummary.aspx?category=1</i>.  Additional querystring parameters
        /// are automatically appended at runtime by the UrlMappingModule when dynamic 
        /// URL templates are matched.
        /// </para>
        /// </remarks>
        /// <seealso cref="UrlTarget" />
        public string Redirection
        {
            get { return _redirection; }
            set { _redirection = value; }
        }

        /// <summary>
        /// Default constructor for a URLMappingItem
        /// </summary>
        public UrlMappingItem()
        {
            _name = "";
            _urlTarget = null;
            _redirection = "";
        }

        /// <summary>
        /// Constructor for a URLMappingItem
        /// </summary>
        /// <param name="name">A name for this mapping item</param>
        /// <param name="urlTarget">A regular expression object defining a URL template for pattern matching</param>
        /// <param name="redirection">The redirection that the UrlMappingModule should apply when incoming URLs match the associated regular expression.</param>
        public UrlMappingItem(string name, Regex urlTarget, string redirection)
        {
            _name = name;
            _urlTarget = urlTarget;
            _redirection = redirection;
        }

    }
}
