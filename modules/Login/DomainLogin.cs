using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Web;

namespace lw.Login
{
	public class DomainLogin : ILogin
	{
		DirectoryEntry de = null;
		string _domain = "LDAP://sabis.net";

		public DomainLogin()
			: base("UserName")
		{

		}

		public override bool Authenticate(string userName, string Password)
		{
			de = GetDirectoryEntry(userName, Password, _domain);

			if (IsLogged(de))
			{
				return true;
			}
			return false;
		}

		public DirectoryEntry GetDirectoryEntry(string userName, string password, string domain)
		{
			DirectoryEntry de = new DirectoryEntry();
			de.Path = domain;
			de.AuthenticationType = AuthenticationTypes.Secure;

			de.Username = userName.Split('@')[0];// domain != _domain ? userName : "interedlb\\" + userName;
			de.Password = password;
			
			//de = GetDirectoryEntry(userName, password, domain);

			return de;
		}

		bool IsLogged(DirectoryEntry de)
		{
			try
			{
				System.Guid id = de.Guid;
			}
			catch (DirectoryServicesCOMException)
			{
				return false;
			}
			catch (System.Runtime.InteropServices.COMException)
			{
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}

		public SearchResult getOneUser(string userName)
		{
			DirectorySearcher search = new DirectorySearcher(de);

			search.Filter = "(&(objectClass=user)(objectCategory=person)(SAMAccountName=" + userName + "))";

			SearchResult result = search.FindOne();
			if (result != null)
				return result;
			else return null;
		}
	}
}
