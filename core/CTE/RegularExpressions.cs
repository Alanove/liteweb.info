using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lw.CTE
{
	public class RegularExpressions
	{
		/// <summary>
		/// If called will remove all the query string and hash values from a URL
		/// </summary>
		public const string RemoveQueryStringFromURI = "(\\?.*|\\#.*)";


		/// <summary>
		/// Matches all hash tags in a text. ex: "#ab alsd #abc" will return ab and abc
		/// </summary>
		//public const string HashTagMatcher = "(#)((?:[A-Za-z0-9-_]*))";

        // naz: updated the hashtag not to take words that start with (#) and ends with (;) example: (&#39;) that its the ascii code for ('), the old tag takes (#39) as hashtag
        public const string HashTagMatcher = @"(?<=\s|^)#(\w*[A-Za-z_-]+\w*)";        
	}
}
