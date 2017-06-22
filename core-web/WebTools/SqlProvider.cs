using System;
using System.Reflection;
using System.Web.Profile;
using lw.CTE;

namespace lw.WebTools
{
	public class SqlProvider : SqlProfileProvider
	{
		public SqlProvider():base()
		{
		}
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			base.Initialize(name, config);

			Config cfg = new Config();

			string _isGodaddy = cfg.GetKey(lw.CTE.parameters.IsGodaddyHosting);

			if (String.IsNullOrEmpty(_isGodaddy))
				_isGodaddy = "false";

			if (!bool.Parse(_isGodaddy))
			{
				var connectionStringField = GetType().BaseType.GetField("_sqlConnectionString", BindingFlags.Instance | BindingFlags.NonPublic);

				string conn = Config.GetConnectionString(Config.GetFromWebConfig(AppConfig.ProfileConnection));

				connectionStringField.SetValue(this, conn);
			}
		}
	}
}
