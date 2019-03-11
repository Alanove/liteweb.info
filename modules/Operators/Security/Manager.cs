using System;
using System.Text.RegularExpressions;
using lw.CTE.Enum;
using lw.Members;
using lw.Utils;
using lw.WebTools;

namespace lw.Operators.Security
{
	/// <summary>
	/// Summary description for Manager.
	/// </summary>
	public class Manager : System.Web.HttpApplication
	{

		public Manager()
		{
		}

		public bool TryLogIn(string UserName,string Password)
		{
			OperatorsManager op = new OperatorsManager();
			System.Data.DataView dv = op.GetOperatorsV(string.Format("UserName='{0}' or Email='{0}'",
					StringUtils.SQLEncode(UserName)));
			if(dv.Count>0)
			{
				MembersManager mMgr = new MembersManager();
				string _Password = dv[0]["Password"].ToString();
				int stat = (int)UserStatus.Enabled|(int)UserStatus.Modified;
				if (_Password.Equals(mMgr.EncryptPassword(Password)) && 
					((int)dv[0]["Status"]&stat) != 0)
				{
					lw.Members.Security.User.LoginUser(mMgr.GetMember((int)dv[0]["MemberId"]));
					LoginOp(dv[0]["GroupName"].ToString(), dv[0]["GroupName"].ToString());
					return true;
				}
			}
			return false;
		}
		/*
		public bool StaticLogin(string UserName, string Password)
		{
			DataSet ds = XmlManager.GetDataSet(CTE.DataSets.StaticLogin);
			DataTable users = ds.Tables["Users"];
			DataView dv = new DataView(
				users, 
				string.Format("UserName='{0}' and Password='{1}'",
					Utils.StringUtils.SQLEncode(UserName),
					Utils.StringUtils.SQLEncode(Password)),
				"",
				DataViewRowState.CurrentRows);

			if (dv.Count > 0)
			{
				string _Password = dv[0]["Password"].ToString();
				if (_Password.Equals(Password))
				{
					LoginOp(UserName, "",
						"", "",
						dv[0]["Group"].ToString());
					return true;
				}
			}
			return false;
		}
		 * */
		public void LoginOp(string GroupName, string XmlFile)
		{
			WebContext.Profile.OperatorGroupName = GroupName;
			WebContext.Profile.OperatorGroupXmlFile = XmlFile;
			WebContext.Profile.CurrentUserType = UserType.Operator;


			string xmlPath = lw.WebTools.XmlManager.DataSetPath("cms-schemas/" + XmlFile + ".config");
			System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
			xml.Load(xmlPath);

			WebContext.Profile.OperatorGroupXml = xml.OuterXml;
			
			//MembersManager.UpdateOnlineStatus(WebContext.Profile.UserId, true);
			//Caching.UpdateUserCache();
		}

		public static string ManagerPermissionPath
		{
			get
			{
				string _managerPermissionPath;

				string path = WebContext.AbsolutePath;

				if (path.IndexOf("/") != 0)
					path = "/" + path;

				if (path.ToLower().IndexOf(WebContext.ManagerRoot.ToLower()) >= 0)
					path = path.Substring(WebContext.ManagerRoot.Length);

				
				string reqExpString = @"(\w|[-.])+$";

				Regex reg = new Regex(reqExpString, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				path = reg.Replace(path, "");

				if (path.ToLower().IndexOf("/sections") == 0)
					path = path.Substring(("/sections").Length);

				if (path.LastIndexOf("/") == path.Length - 1)
					path = path.Substring(0, path.Length - 1);


				_managerPermissionPath = path;

				return _managerPermissionPath;
			}
		}

		public static bool CheckIfLoggedIn()
		{
			return WebContext.Profile.UserLogged;
		}
		
		public static void CheckAccess()
		{
			CheckAccess(false);
		}

		public static void CheckAccess(bool editMode)
		{
			if (!(WebContext.Profile.UserLogged && WebContext.Profile.CurrentUserType == UserType.Operator))
			{
				string[] url = WebContext.Request.RawUrl.Split('?');
				string raw = WebContext.Server.UrlEncode(url[0]);
				string query = url.Length > 1 ? WebContext.Server.UrlEncode(url[1]) : "";

				string red = Config.GetFromWebConfig("cmslogin");

				if (String.IsNullOrWhiteSpace(red))
				{
					red = String.Format("{0}/login.aspx",
						WebContext.ManagerRoot
					);
				}


				WebContext.Response.Redirect(
					String.Format("{0}?return={1}&query={2}",
						red,
						raw,
						query
					)
				);
			}

			WebContext.Profile.LastManagerActivityDate = DateTime.Now;

			System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
			xml.LoadXml(WebContext.Profile.OperatorGroupXml);

			
			//TODO: Fix this with the new manager
			/*
			if (ManagerPermissionPath != "" && xml.SelectSingleNode("menu/main/" + ManagerPermissionPath) == null)
			{
				if(editMode)
					if (xml.SelectSingleNode("menu/main/Content") != null)
						return;

				WebContext.Response.Redirect(WebContext.ManagerRoot + "/AccessDenied.htm");
			}*/
		}

		public static void Logout()
		{
			lw.Members.Security.User.SignOut();
			WebContext.Profile.OperatorGroupName = "";
			WebContext.Profile.OperatorGroupXmlFile = "";
			WebContext.Profile.CurrentUserType = UserType.Guest;
		}

		/// <summary>
		/// Flag to check if an operator is currently logged in
		/// </summary>
		public static bool IsOpLoggedIn
		{
			get
			{
				return WebContext.Profile.UserLogged
					&&
					WebContext.Profile.OperatorGroupName != ""
					&&
					WebContext.Profile.OperatorGroupXmlFile != "";
			}
		}
	}
}