using mshtml;

namespace lw.Utils
{
	/// <summary>
	/// Provides helper class to deal with HTML documents
	/// </summary>
	public class HTMLDom
	{
		/// <summary>
		/// Converts and HTML string read from and html file or database into <see cref=" mshtml.IHTMLDocument2">HTML document</see>
		/// </summary>
		/// <param name="htmlString">The entry html string</param>
		/// <returns><see cref=" mshtml.IHTMLDocument2">HTML document</see></returns>
		public static mshtml.IHTMLDocument2 GetHtmlDocument(string htmlString)
		{
			if (htmlString.Length > 0)
			{
				//reads the html into an html document to enable parsing
				IHTMLDocument2 doc = new HTMLDocumentClass();
				doc.write(new object[] { htmlString });
				doc.close();

				return doc;
			}

			return null;
		}
	}
}
