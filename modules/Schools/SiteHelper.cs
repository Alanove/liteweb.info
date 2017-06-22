using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Threading;
using System.Text;


using lw.CTE.Enum;
using lw.WebTools;
using System.Xml.Serialization;
using System.IO;
using lw.Utils;


namespace SABIS
{

	/// <summary>
	/// Summary description for SiteHelper
	/// </summary>
	public class SiteHelper
	{
		static List<Languages> _siteLanguages;
		static ListItemCollection _languageCollection;

		public SiteHelper()
		{
		}

		public static ReaderWriterLock _lock = new ReaderWriterLock();

		public static List<School> SchoolList
		{
			get
			{
				List<School> _schools = null;

				try
				{

					_lock.AcquireReaderLock(2000);

					object obj = WebContext.Cache["SABIS_School_List"];
					if (obj != null)
						_schools = obj as List<School>;
					else
					{
						_lock.ReleaseReaderLock();
						_lock.AcquireWriterLock(2000);

						string url = "http://www.sabis.net/feeds/schools.ashx";


						string xml = lw.WebTools.WebUtils.GetURLContent(url);

						XmlReader reader = XmlReader.Create(new StringReader(xml));

						XmlSerializer deserializer = new XmlSerializer(typeof(List<School>));
						_schools = (List<School>)deserializer.Deserialize(reader);

						WebContext.Cache.Insert("SABIS_School_List", _schools);
					}
				}
				finally
				{
					_lock.ReleaseLock();
				}

				return _schools;
			}
		}
	}
}