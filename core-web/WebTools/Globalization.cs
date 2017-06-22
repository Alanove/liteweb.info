using System.Globalization;

namespace lw.WebTools
{
	public class Globalization
	{
		#region internal varialbles
		static string _defaultDBCulture = "en-us";
		static CultureInfo _defaultDBCultureInfo = null;
		#endregion

		/// <summary>
		/// Returns the default culture info to use when saving dates in the database
		/// </summary>
		public static CultureInfo DefaultDBCultureInfo
		{
			get
			{
				if (_defaultDBCultureInfo == null)
				{
					Config cfg = new Config();

					string key = cfg.GetKey(lw.CTE.parameters.DefaultDBCutureInfo);

					if (key != "")
					{
						_defaultDBCultureInfo = new CultureInfo(key);
					}
					else
					{
						_defaultDBCultureInfo = new CultureInfo(_defaultDBCulture);
					}
				}
				return _defaultDBCultureInfo;
			}
			set
			{
				_defaultDBCultureInfo = value;
			}
		}
	}
}
