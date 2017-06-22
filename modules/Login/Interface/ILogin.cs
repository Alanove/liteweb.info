using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

using lw.Members;
using lw.Members.Security;

namespace lw.Login
{
	public class ILogin
	{
		string _userName;
		string _password;
		string _relateTo;
		bool _loggedIn;

		public ILogin(string relateTo)
		{
			this._relateTo = relateTo;
		}

		public bool Login(string userName, string Password)
		{
			if (Authenticate(userName, Password))
			{
				MembersManager mMgr = new MembersManager();
				DataRow dr = mMgr.GetMember(userName);
				if (dr == null)
				{
					dr = mMgr.GetMember(userName + "@sabis.net");
				}
				if (dr != null)
				{
					int status = (int)dr["Status"];
					string password = dr["Password"].ToString();
					string[] gid = dr["Geuid"].ToString().Split('-');

					if (status == (int)lw.CTE.Enum.UserStatus.Enabled)
					{
						string m = "m";
						string x = "x";
						string y = "y";
						string z = "z";
						string p = "p";
						string t = "t";
						string l = "l";
						string k = "k";
						if (password == lw.Utils.Cryptography.Encrypt(gid[gid.Length - 1], m + x + y + z + p + t + y + l + k))
						{
							User.LoginUser(dr);
							return true;
						}
					}
				}
			}
			return false;
		}

		public virtual bool Authenticate(string userName, string Password)
		{
			return _loggedIn;
		}


		#region Properties

		public string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				_userName = value;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		public string RelateTo
		{
			get
			{
				return _relateTo;
			}
			set
			{
				_relateTo = _userName;
			}
		}

		#endregion
	}



}
