using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZetaProducerHtmlCompressor.Internal;
using Yahoo.Yui.Compressor;

namespace lw.WebTools
{
	/// <summary>
	/// A class handler to compress html and js content
	/// </summary>
	public class WebCompress
	{
		/// <summary>
		/// Compresses and minifies HTML Content
		/// </summary>
		/// <param name="HtmlInput">The original HTML Content</param>
		/// <returns>Compressed HTML</returns>
		public static string HTMLCompress(string HtmlInput)
		{
			string ret = HtmlInput;

			try
			{

				HtmlCompressor htmlCompressor = new HtmlCompressor();
				htmlCompressor.setRemoveComments(true);

				

				ret = htmlCompressor.compress(HtmlInput);
			}
			catch (Exception ex)
			{
				//throw ex;
			}
			return ret;
		}

		/// <summary>
		/// Compresses and minifies Javascript Content
		/// </summary>
		/// <param name="JsInput">The original JavaScript Content<</param>
		/// <returns>The minified JavaScript</returns>
		public static string JsCompress(string JsInput)
		{
			string ret = JsInput;

			//return ret;

			try
			{
				JavaScriptCompressor jCompressor = new JavaScriptCompressor();
				jCompressor.CompressionType = CompressionType.Standard;
				ret = jCompressor.Compress(JsInput);
			}
			catch { 
			}
			return ret;
		}
	}

}
