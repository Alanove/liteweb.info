using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lw.Install
{
	/// <summary>
	/// Installs or upgrades to the latest version of the database
	/// </summary>
	public class InstallDB
	{
		public const string ChangeLogXmlFile = "ChangeLog.xml";

		string _DBName;
		/// <summary>
		/// Specifies the name of the database
		/// </summary>
		public string DBName
		{
			get { return _DBName; }
			set { _DBName = value; }
		}
	}
}
