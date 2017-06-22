using System.Collections.Generic;

namespace lw.UrlMapping
{
    /// <summary>
    /// A typed list of <see cref="UrlMappingItem" /> objects.  
    /// </summary>
    /// <remarks>
    /// A concrete provider (a class that implements the <see cref="IUrlMappingProvider" /> interface)
    /// supplies a list of URL mapping definitions to the <see cref="UrlMappingModule" />
    /// in the form of a UrlMappingItemCollection.  The UrlMappingModule uses this provided list
    /// in its supplied order to match incoming URLs for potential redirection.  
    /// </remarks>
    public class UrlMappingItemCollection : List<UrlMappingItem>
    {
    }
}
