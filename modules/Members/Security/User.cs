using System;
using System.Data;
using System.Web.UI;
using lw.Content;
using lw.CTE;
using lw.CTE.Enum;
using lw.Utils;
using lw.WebTools;

namespace lw.Members.Security
{
	/// <summary>
	/// Summary description for Manager.
	/// </summary>
	public class User : System.Web.HttpApplication
	{

		public User()
		{
		}

		public void InitUserSessions()
		{
		}
		public static bool TryLogin(string UserName, string Password)
		{
			return TryLogin(UserName, Password, false);
		}
		public static bool TryLogin(string UserName, string Password, bool persist)
		{
			return TryLogin(UserName, Password, false, true);
		}
		public static bool TryLogin(string UserName, string Password, bool persist, bool rememberName)
		{
			if (string.IsNullOrEmpty(UserName))
				return false;

			lw.Members.MembersManager mMgr = new lw.Members.MembersManager();

			string encPassword = mMgr.EncryptPassword(Password);

			DataTable members = mMgr.GetMembers(string.Format("UserName='{0}' Or Email='{0}' and Password='{1}'",
				StringUtils.SQLEncode(UserName).Trim(),
				StringUtils.SQLEncode(encPassword)));

			SignOut();

			if (members.Rows.Count > 0)
			{
				if (encPassword.Equals(members.Rows[0]["Password"].ToString(), StringComparison.Ordinal))
				{
					LoginUser(members.Rows[0], persist, rememberName);
					return true;
				}
			}
			return false;
		}
		public static void Logout()
		{
			SignOut();
		}
		public static void SignOut()
		{
			MembersManager.UpdateOnlineStatus(WebContext.Profile.UserId, false);
			WebContext.Profile.UserEmail = "";
			WebContext.Profile.dbUserName = "";
			WebContext.Profile.UserGuid = new Guid();
			WebContext.Profile.UserId = -1;
			WebContext.Profile.UserLogged = false;
			WebContext.Profile.UserRememberMe = false;
			WebContext.Profile.UserFullName = "";
			WebContext.Profile.FriendsCount = 0;
			WebContext.Profile.CurrentUserStatus = (int)UserStatus.None;
			WebContext.Profile.CurrentUserType = UserType.Guest;
			WebContext.Profile.CurrentUserStatus = (int)lw.CTE.Enum.UserStatus.Unknown;
		}
		public static bool LoggedIn
		{
			get
			{
				if (!WebContext.Profile.UserLogged)
					SignOut();
				return WebContext.Profile.UserLogged;
			}
		}

		public static void CheckAccess(UserStatus stat)
		{
			CheckAccess("", stat, null);
		}


		public static void CheckAccess()
		{
			CheckAccess("",
				UserStatus.Enabled | UserStatus.Incomplete | UserStatus.Modified | UserStatus.Pending,
				null);
		}
		public static void CheckAccess(string redirect)
		{
			CheckAccess("",
				UserStatus.Enabled | UserStatus.Incomplete | UserStatus.Modified | UserStatus.Pending,
				redirect);
		}

		public static void CheckAccess(string _params, UserStatus stat, string redirect)
		{
			if (String.IsNullOrEmpty(redirect))
			{
				Config cfg = new Config();
				redirect = cfg.GetKey(parameters.LoginURL);
			}

			if (!WebContext.Profile.UserLogged)
			{
				string url = WebContext.Request.ServerVariables["URL"];
				if (WebContext.Request.QueryString.Count > 0)
					url += string.Format("?{0}", WebContext.Request.QueryString);

				if (_params != "")
					_params = string.Format("&{0}", _params);

				WebContext.Response.Redirect(string.Format("{0}/{3}?return={1}{2}",
					WebContext.Root, WebContext.Server.UrlEncode(url), _params, redirect));
			}
			if (((int)WebContext.Profile.CurrentUserStatus & (int)stat) == 0)
			{
				ErrorContext.Add("permission", ContentManager.ErrorMsg(lw.CTE.Errors.InvalidPermission));
			}
		}

		/// <summary>
		/// Returns the current logged in user with his profile
		/// </summary>
		/// <param name="ctrl">Any control on the current page or the page itself</param>
		/// <returns>DataRow Logged In Member</returns>
		public static DataRow LoggedInUser(Control ctrl)
		{
			return LoggedInUser(ctrl, true);
		}


		/// <summary>
		/// Returns the current logged in user with his profile
		/// </summary>
		/// <returns>DataRow Logged In Member</returns>
		public static DataRow LoggedInUser()
		{
			return LoggedInUser(null, true);
		}


		/// <summary>
		/// Returns the current logged in user with his profile
		/// </summary>
		/// <param name="WithProfile">Flag if set to true it will read from MemberView if false it will read from Members</param>
		/// <returns>DataRow Logged In Member</returns>
		public static DataRow LoggedInUser(bool WithProfile)
		{
			return LoggedInUser(null, WithProfile);
		}

		/// <summary>
		/// Returns the current logged in user with his profile
		/// </summary>
		/// <param name="ctrl">Any control on the current page or the page itself</param>
		/// <param name="GetWithProfile">Flag if set to true it will read from MemberView if false it will read from Members</param>
		/// <returns>DataRow Logged In Member</returns>
		public static DataRow LoggedInUser(Control ctrl, bool GetWithProfile)
		{
			lw.Base.CustomPage page = null;
			if (ctrl != null)
				page = ctrl.Page as lw.Base.CustomPage;

			if (page != null)
			{
				if (page.PageContext[cte.LoggedInUserContextKey] != null)
					return page.PageContext[cte.LoggedInUserContextKey] as DataRow;
			}

			if (WebContext.Profile.UserLogged)
			{
				MembersManager mMgr = new MembersManager();

				DataTable members;

				if (GetWithProfile)
					members = mMgr.GetMembersWithProfile(string.Format("MemberId={0}", WebContext.Profile.UserId));
				else
					members = mMgr.GetMembers(string.Format("MemberId={0}", WebContext.Profile.UserId));

				if (members.Rows.Count > 0)
				{
					if (page != null)
					{
						page.PageContext[cte.LoggedInUserContextKey] = members.Rows[0];
					}
					return members.Rows[0];
				}
			}
			else
				SignOut();
			return null;
		}

		public static int LoggedInUserId
		{
			get
			{
				return WebContext.Profile.UserId;
			}
		}

        public static int Roles
        {
            get
            {
                return WebContext.Profile.Roles;
            }
        }

		public static void LoginUser(DataRow member)
		{
			LoginUser(member, false, true);
		}
		public static void LoginUser(DataRow member, bool persist, bool rememberName)
		{
			WebContext.Profile.UserEmail = member["Email"].ToString();
			WebContext.Profile.UserGuid = (System.Guid)member["Geuid"];
			WebContext.Profile.UserId = (int)member["MemberId"];
			WebContext.Profile.UserLogged = true;
			WebContext.Profile.UserRememberMe = persist;
			WebContext.Profile.UserRememberName = rememberName;
			WebContext.Profile.UserFullName = string.Format("{0} {1}", member["FirstName"], member["LastName"]);
			WebContext.Profile.CurrentUserStatus = (int)MembersManager.GetUserStatus(member);
            WebContext.Profile.Roles = (int)member["Roles"];
			WebContext.Profile.CurrentUserType = UserType.User;
			WebContext.Profile.dbUserName = member["UserName"].ToString();
			//MembersManager.UpdateOnlineStatus((int)member["MemberId"], true);

			FriendsManager fMgr = new FriendsManager();
			WebContext.Profile.FriendsCount = fMgr.GetFriendsCount((int)member["MemberId"]);

			MemberLoginActivity mlMgr = new MemberLoginActivity();
			mlMgr.AddMemberLogin((int)member["MemberId"], WebContext.IPAddress + "," + WebTools.WebContext.Request.ServerVariables["LOCAL_ADDR"]);

			Caching.UpdateUserCache();
		}

		/// <summary>
		/// Checks if the user have permission to access the provided roles
		/// </summary>
		/// <param name="role">Can be one Role or serveral combination using the | operator</param>
		/// <returns>True = Have permission, false = don't have permission</returns>
		public static bool CheckPermissionAccess(Roles role)
		{
			return (WebContext.Profile.Roles & (int)role) > 0;
		}
	}
}