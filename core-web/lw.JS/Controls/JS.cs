using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;



namespace lw.js.Controls
{
	public class JS : WebControl
	{
		public override void DataBind()
		{
			IEnumerable<JsXml> query = from p in Engine.XmlFile.Descendants("js")
						select new JsXml
						{
							file = p.Element("file").Value,
							version = decimal.Parse(p.Element("version").Value),
							dependency = p.Element("dependency").Value,
							depversion = p.Element("depversion").Value
						};

			addFileWidthDependencies(query, _src, _version);

			base.DataBind();
		}

		void addFileWidthDependencies(IEnumerable<JsXml> query, string src, decimal? version)
		{
			var test = from a in query
					   where a.file == Src
					   select a;

			if (test.Count() == 0)
			{
				throw new Exception("Script not found: " + Src);
			}

			if (version == null)
			{
				query = from a in query
						where a.file == _src
						orderby a.version descending
						select a;
				version = query.First().version;
			}

			//if (_version != null)
			//{
			//	MyPage.RegisterScriptFile(src,
			//		string.Format("{0}.{1}.{2}{3}",
			//			src,
			//			version,
			//			cte.extension,
			//			_minified != null && _minified.Value ? "?minified=true" : ""
			//		)
			//	);
			//}
		}


		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			//base.Render(writer);
		}

		#region Properties

		string _src;
		/// <summary>
		/// The script src file.
		/// </summary>
		public string Src
		{
			get
			{
				return _src;
			}
			set
			{
				_src = value;
			}
		}

		decimal? _version;
		/// <summary>
		/// The version of the javascript file.
		/// Defaults to the latest version.
		/// </summary>
		public decimal? Version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		bool? _minified;
		/// <summary>
		/// If true the .min version of the file will be returned.
		/// Make sure that it is available
		/// </summary>
		public bool? Minified
		{
			get
			{
				return _minified;
			}
			set
			{
				_minified = value;
			}
		}

		#endregion
	}
}
