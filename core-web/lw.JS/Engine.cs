using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Web.Routing;



using lw.WebTools;


namespace lw.js
{
	public class Engine
	{
		/// <summary>
		/// Returns the xml file that holds all js files information
		/// </summary>
		public static XElement XmlFile
		{
			get
			{
				//Getting xml from cach
				XElement xmlElement = WebContext.GetFromApplication(cte.XmlDoc_CachKey) as XElement;
				//if xml not found create, runs the first time
				if (xmlElement == null)
				{
					string fileName = XmlManager.DataSetPath(cte.XmlFile);

					using (Stream stream = 
						File.Exists(fileName)?  
							File.OpenRead(fileName):
							typeof(Engine).Assembly.GetManifestResourceStream("lw.js." + cte.XmlFile)
						)
					{
						using (StreamReader sr = new StreamReader(stream))
						{
							xmlElement = XElement.Load(sr);

							WebContext.AddToApplication(cte.XmlDoc_CachKey, xmlElement);

							/*
							var jsfiles = from nodes in xmlElement.Elements("js") select nodes;

							if (jsfiles != null)
							{
								foreach (var b in jsfiles)
								{
									string file = b.Element("file").ToString();
									string version = b.Element("version").ToString();
									RouteTable.Routes.MapPageRoute(
										file + "-" + version,
										"lw,
										routing["Redirection"].ToString().Trim()
									);
								}
							}
							 * */
						}
					}
				}
				return xmlElement;
			}
		}


		/// <summary>
		/// Returns a resource within the DLL
		/// </summary>
		/// <param name="filename">The file name of the resource</param>
		/// <returns>String Resource Content</returns>
		public static string GetResource(string filename)
		{
			string result;

			using (Stream stream = typeof(Engine).Assembly.
					   GetManifestResourceStream("lw.js." + filename))
			{
				using (var sr = new StreamReader(stream))
				{
					result = sr.ReadToEnd();
				}
			}
			return result;
		}
	}
}
