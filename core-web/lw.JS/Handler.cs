using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Collections;
using System.Reflection;


using lw.CTE;
using lw.WebTools;

namespace lw.js
{
	/// <summary>
	/// The handler than returns all js files.
	/// Usage ex: lw.lwjs	will return the latest version of lw.js 
	/// or lw.4.065.lwjs	will return the version 4.065 of lw.js
	/// </summary>
	public class Handler : IHttpHandler 
	{
		public virtual void ProcessRequest(HttpContext context)
		{
			//string[] resources = typeof(Engine).Assembly.GetManifestResourceNames();

			context.Response.ContentType = "text/javascript";

			string min = context.Request.QueryString["minified"];

			if(String.IsNullOrWhiteSpace(min))
				min = WebTools.WebUtils.GetFromWebConfig(lw.CTE.parameters.LwJSMinify);



			if (!string.IsNullOrWhiteSpace(min) && (min.ToLower() == "true" || min == "1"))
				min = ".min";
			else
				min = "";

			string url = context.Request.Url.AbsolutePath;

			if (url.IndexOf("/", System.StringComparison.Ordinal) >= 0)
			{
				string[] t = url.Split('/');
				url = t[t.Length - 1];
			}
			string[] temp = url.Split('.');

			string script = temp[0];
			string version = WebTools.WebUtils.GetFromWebConfig(script + "-version");

			if(string.IsNullOrWhiteSpace(version))
			{
				if (temp.Length > 2)
				{
					version = "_" + temp[1];

					int i = 2;

					if (temp.Length != 3)
					{
						for (; i < temp.Length - 1; i++)
						{
							version += "._" + temp[i];
						}
					}
				}
				else
				{
					//Latest version
					IEnumerable<JsXml> query = Engine.XmlFile.Descendants("js").Select(p => new JsXml
					{
						file = p.Element("file").Value,
						version = decimal.Parse(p.Element("version").Value),
						dependency = p.Element("dependency").Value,
						depversion = p.Element("depversion").Value
					});
					var jsXmls = query as JsXml[] ?? query.ToArray();
					var test = from a in jsXmls
						where a.file == script
						select a;

					if (!test.Any())
					{
						throw new Exception("Script not found: " + script);
					}


					query = from a in jsXmls
						where a.file == script
						orderby a.version descending
						select a;
					version = query.First().version.ToString();

					version = "_" + version.Replace(".", "._");
				}
			}
			else
			{
				version = "_" + version.Replace(".", "._");
			}


			string resourceKey = "lw.js.script." + script.Replace("-", "_") + "." + version + "." + script + min + ".js";

			string result = "";
			using (Stream stream = typeof(Engine).Assembly.
				GetManifestResourceStream(resourceKey))
			{
				using (var sr = new StreamReader(stream))
				{
					result = sr.ReadToEnd();
				}
			}
			context.Response.Write(result);
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
