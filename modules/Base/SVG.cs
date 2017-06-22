using lw.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Threading;
using lw.WebTools;
using System.IO;

namespace lw.Base
{
	/// <summary>
	/// Reads and renders svg content inline
	/// </summary>
	public class SVG : HtmlControl
	{
		public static ReaderWriterLock _lock = new ReaderWriterLock();

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			string key = "LWSVG" + StringUtils.ToURL(_src);
			string svgContent = "";

			try
			{
				_lock.AcquireReaderLock(-1);

				object obj = WebContext.Cache[key];

				if (obj != null)
				{
					svgContent = obj.ToString();
				}
				else
				{
					_lock.ReleaseReaderLock();
					_lock.AcquireWriterLock(-1);

					System.Web.Caching.CacheDependency dep = null;

					if (_src.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || _src.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
					{
						svgContent = WebUtils.GetURLContent(_src);
					}
					else
					{
						_src = WebContext.Server.MapPath(_src);

						StreamReader sr = new StreamReader(_src);
						svgContent = sr.ReadToEnd();
						sr.Close();

						dep = new System.Web.Caching.CacheDependency(_src);
					}

					WebContext.Cache.Insert(key, svgContent, dep);
				}
			}
			finally
			{
				_lock.ReleaseLock();
			}

			writer.Write(svgContent);

			//base.Render(writer);
		}

		string _src = "";
		/// <summary>
		/// The source of the svg file (can also be external using http or https).
		/// </summary>
		public string SRC
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
	}
}
