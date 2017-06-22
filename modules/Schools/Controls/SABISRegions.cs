using lw.Utils;
using lw.WebTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SABIS.Controls
{
	public class SABISRegions : Literal
	{

		public static ReaderWriterLock _lock = new ReaderWriterLock();

		public static String SABISRegionsHTMLElement
		{
			get
			{
				string _ret = "";
				try
				{

					_lock.AcquireReaderLock(2000);

					object obj = WebContext.Cache["SABISRegionsHTML_CACHE_KEY"];
					if (obj != null)
						_ret = obj.ToString();
					else
					{

						_lock.ReleaseReaderLock();
						_lock.AcquireWriterLock(2000);


						StringBuilder ret = new StringBuilder("<aside class=\"sabis-regions\"><div><h4>SABIS<sup>®</sup> Network</h4>");

						if (SABISRegionsXML != null)
						{
							var categories = from category in SABISRegionsXML.Descendants("Category")
											 select new
											 {
												 Title = category.Descendants("Title").First().Value,
												 Regions = from region in category.Descendants("Region")
														   select new
														   {
															   Title = region.Descendants("Title").First().Value,
															   Children = from child in region.Descendants("Child")
																		  select new
																		  {
																			  Title = child.Descendants("Title").First().Value,
																			  URL = child.Descendants("URL").First().Value
																		  }
														   }
											 };

							ret.AppendLine("<ol>");

							foreach (var category in categories)
							{
								ret.AppendFormat("<li class=\"{1}\"><h5>{0}</h5>", StringUtils.AddSup(category.Title), 
									category.Title == "SABIS® Network of Schools" ? "sabis-school-network" : StringUtils.ToURL(category.Title));
								ret.Append("<ol>");
								foreach (var region in category.Regions)
								{
									ret.AppendFormat("<li class=\"{0}\">", StringUtils.ToURL(StringUtils.AddSup(region.Title)));
									ret.AppendFormat("<h6>{0}</h6>", region.Title);
									ret.Append("<ol>");
									foreach (var child in region.Children)
									{
										ret.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", child.URL, StringUtils.AddSup(child.Title));
									}
									ret.Append("</ol>");
									ret.Append("</li>");
								}
								ret.Append("</ol>");
								ret.Append("</li>");
							}

							ret.Append("</ol><div>");
						}
						ret.Append("</aside>");
						_ret = ret.ToString();

						if (SABISRegionsXML != null)
							WebContext.Cache.Insert("SABISRegionsHTML_CACHE_KEY", ret.ToString());
					}
				}
				finally
				{
					_lock.ReleaseLock();
				}
				return _ret;
			}
		}

		public static XElement SABISRegionsXML
		{
			get
			{
				XElement _SABISRegions = null;
				string url = "http://www.sabis.net/feeds/regions.xml";
				try
				{
					string xml = lw.WebTools.WebUtils.GetURLContent(url);
					_SABISRegions = XElement.Parse(xml);
				}
				catch (Exception ex)
				{
					lw.WebTools.ErrorHandler.HandleError(ex);
				}
				return _SABISRegions;
			}
		}
		
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write(SABISRegionsHTMLElement);
			//base.Render(writer);
		}
	}
}
