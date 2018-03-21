using lw.WebTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace lw.js.Controls
{
	/// <summary>
	/// Combine script files into a one cached script file
	/// </summary>
	public class CombinedScripts : WebControl
	{
		/// <summary>
		/// splice the script files and add them into an array
		/// </summary>
		string[] scriptsArray = null;

		/// <summary>
		/// Boolean indicating if the current tag is bounded or not.
		/// </summary>
		bool _bound = false;
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			

			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (string.IsNullOrWhiteSpace(scripts))
			{
				throw new Exception("Scripts attribute cannot be empty.");
			}

			CacheKey = lw.Utils.Hash.GetHash(scripts, Utils.Hash.HashType.MD5);

			scriptsArray = Scripts.Split(new Char[] { ',', '|', ';' }, StringSplitOptions.RemoveEmptyEntries);

			string fileLocation = WebContext.Server.MapPath(FileLocation);
			if (!File.Exists(fileLocation))
			{
				using (FileStream streamCache = File.OpenWrite(fileLocation))
				{
					StringBuilder output = new StringBuilder();
					output.AppendLine("/* Script generated at: " + DateTime.Now.ToString() + "*/");
					foreach (string file in scriptsArray)
					{
						output.AppendLine("/* File Name: " + file + "*/");

						if (file.EndsWith(".lwjs"))
						{
							string tempFile = file.Replace(".lwjs", "").Replace("../", "");
							IEnumerable<JsXml> query = Engine.XmlFile.Descendants("js").Select(p => new JsXml
							{
								file = p.Element("file").Value,
								version = decimal.Parse(p.Element("version").Value),
								dependency = p.Element("dependency").Value,
								depversion = p.Element("depversion").Value
							});
							var jsXmls = query as JsXml[] ?? query.ToArray();
							var test = from a in jsXmls
									   where a.file == tempFile
									   select a;

							if (!test.Any())
							{
								throw new Exception("Script not found: " + tempFile);
							}


							query = from a in jsXmls
									where a.file == tempFile
									orderby a.version descending
									select a;
							string version = query.First().version.ToString();

							version = "_" + version.Replace(".", "._");

							string resourceKey = "lw.js.script." + tempFile.Replace("-", "_") + "." + version + "." + tempFile + ".min.js";

							string result = "";
							using (Stream stream = typeof(Engine).Assembly.
								GetManifestResourceStream(resourceKey))
							{
								using (var sr = new StreamReader(stream))
								{
									result = sr.ReadToEnd();
								}
							}
							output.AppendLine(result);
						}
						else if(file.IndexOf("//") == 0 || file.IndexOf("http") == 0){
							output.AppendLine(lw.WebTools.WebUtils.GetURLContent(file));
						}
						else
						{
							string fileName = WebContext.Server.MapPath(WebContext.Root + file);
							string minFileName = fileName.Replace(".js", ".min.js");
							if (File.Exists(minFileName))
							{
								fileName = minFileName;
							}
							using (Stream stream = File.OpenRead(fileName))
							{
								using (StreamReader sr = new StreamReader(stream))
								{
									output.AppendLine(sr.ReadToEnd());
								}
							}
						}
					}
					using (StreamWriter s = new StreamWriter(streamCache))
						s.WriteLine(output.ToString());
				}
			}

			writer.Write("<script src=\"" + FileLocation + "\"></script>");
			//base.Render(writer);
		}

		/// <summary>
		/// Returns the file location of the current cached item
		/// </summary>
		public string FileLocation
		{
			get
			{
				return String.Format("{0}/{1}/{2}.js",
					WebContext.Root,
					CachedScriptsLocation,
					CacheKey);
			}
		}

		string cachedScriptsLocation = "";
		/// <summary>
		/// Gets or sets the cached scripts location
		/// The default is const cte.DefaultCachedScriptsLocation (js/cache)
		/// Override 1: App.config CachedScriptsLocation
		/// Override 2: Hard Code the location inside the tag itself
		/// </summary>
		public string CachedScriptsLocation
		{
			get
			{
				if (string.IsNullOrWhiteSpace(cachedScriptsLocation))
				{
					cachedScriptsLocation = Config.GetFromWebConfig(cte.CachedScriptsConfig);
					if (string.IsNullOrWhiteSpace(cachedScriptsLocation))
					{
						cachedScriptsLocation = cte.DefaultCachedScriptsLocation;
					}
				}
				return cachedScriptsLocation;
			}
			set
			{
				cachedScriptsLocation = value;
			}
		}

		string scripts = "";
		/// <summary>
		/// Gets or sets the value of the comma seperated values of the scripts location
		/// </summary>
		public string Scripts
		{
			get
			{
				return scripts;
			}
			set
			{
				scripts = value;
			}
		}

		string cacheKey = "";
		/// <summary>
		/// Gets or sets the cache key for the scripts
		/// The cache key is unique and is the actual file name of the combined and cached script file
		/// </summary>
		public string CacheKey
		{
			get
			{
				return cacheKey;
			}
			set
			{
				cacheKey = value;
			}
		}
	}
}
