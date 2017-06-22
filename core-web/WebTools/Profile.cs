using System;
using System.Collections.Generic;
using System.Web.Profile;
using lw.CTE.Enum;

namespace lw.WebTools
{
	public class lwProfile: ProfileBase
	{
		public lwProfile()
			: base()
		{
		}
		#region Users...
		[SettingsAllowAnonymous(true)]
		public virtual string dbUserName
		{
			get { return base.GetPropertyValue("dbUserName") as string; }
			set { base.SetPropertyValue("dbUserName", value); }
		}

		[SettingsAllowAnonymous(true)]
		[CustomProviderData("UserLogged;Boolean")]
		public virtual Boolean UserLogged
		{
			get
			{
				Boolean logged = false;
				if (base["UserLogged"] != null)
				{
					logged = (Boolean)base["UserLogged"];
					if (logged)
					{
						TimeSpan dif = DateTime.Now - LastUserActivityDate;
						if (dif.TotalMinutes > 60*3 && !UserRememberMe)
							logged = false;
					}
					if(!logged)
						SignOut();
				}

				return logged;
			}
			set
			{
				if (value)
				{
					LastUserActivityDate = DateTime.Now;
				}
				base.SetPropertyValue("UserLogged", value);
			}
		}
		
		[SettingsAllowAnonymous(true)]
		public virtual Boolean UserRememberMe
		{
			get { return (bool)base.GetPropertyValue("UserRememberMe"); }
			set { base.SetPropertyValue("UserRememberMe", value); }
		}

		[SettingsAllowAnonymous(true)]
		public virtual Boolean UserRememberName
		{
			get { return (bool)base.GetPropertyValue("UserRememberName"); }
			set { base.SetPropertyValue("UserRememberName", value); }
		}

		[SettingsAllowAnonymous(true)]
		public virtual DateTime LastUserActivityDate
		{
			get { return (DateTime)base.GetPropertyValue("LastUserActivityDate"); }
			set { base.SetPropertyValue("LastUserActivityDate", value); }
		}

		[SettingsAllowAnonymous(true)]
		public virtual string UserFullName
		{
			get { return base.GetPropertyValue("UserFullName") as string; }
			set { base.SetPropertyValue("UserFullName", value); }
		}

		[SettingsAllowAnonymous(true)]
		public virtual string UserEmail
		{
			get { return base.GetPropertyValue("UserEmail") as string; }
			set { base.SetPropertyValue("UserEmail", value); }
		}
		[SettingsAllowAnonymous(true)]
		public virtual int UserId
		{
			get { return (int)base.GetPropertyValue("UserId"); }
			set { base.SetPropertyValue("UserId", value); }
		}
		[SettingsAllowAnonymous(true)]
		public System.Guid UserGuid
		{
			get { return (System.Guid)base.GetPropertyValue("UserGuid"); }
			set { base.SetPropertyValue("UserGuid", value); }
		}
		[SettingsAllowAnonymous(true)]
		public int CurrentUserStatus
		{
			get
			{
				int status = (int)UserStatus.Unknown;
				if ((int)base.GetPropertyValue("CurrentUserStatus") != 0)
				{
					status = (int)base.GetPropertyValue("CurrentUserStatus");
				}
				return status;
			}
			set
			{
				base.SetPropertyValue("CurrentUserStatus", value);
			}
		}

        [SettingsAllowAnonymous(true)]
        public int Roles
        {
            get
            {
                int status = (int)lw.CTE.Enum.Roles.Visitor;
                if ((int)base.GetPropertyValue("CurrentUserStatus") != 0)
                {
                    status = (int)base.GetPropertyValue("Roles");
                }
                return status;
            }
            set
            {
                base.SetPropertyValue("Roles", value);
            }
        }

		

		[SettingsAllowAnonymous(true)]
		[CustomProviderData("SerizalizeAs;Binary")]
		public lw.CTE.ChatProfile ChatProfile
		{
			get
			{
				lw.CTE.ChatProfile cp;
				if (base.GetPropertyValue("ChatProfile") != null)
					cp = (lw.CTE.ChatProfile)base.GetPropertyValue("ChatProfile");
				else
					cp = new lw.CTE.ChatProfile();

				return cp;
			}
			set
			{
				base.SetPropertyValue("ChatProfile", value);
			}
		}


		[SettingsAllowAnonymous(true)]
		[CustomProviderData("SerizalizeAs;Binary")]
		public Boolean HasFriends
		{
			get
			{
				return FriendsCount > 0;
			}
		}


		[SettingsAllowAnonymous(true)]
		[CustomProviderData("SerizalizeAs;Binary")]
		public int FriendsCount
		{
			get { return (int)base.GetPropertyValue("FriendsCount"); }
			set { base.SetPropertyValue("FriendsCount", value); }
		}

		[SettingsAllowAnonymous(true)]
		[CustomProviderData("SerizalizeAs;Binary")]
		public SessionBasket Basket
		{
			get
			{
				object basket = GetPropertyValue("Basket");
				if (basket == null)
					return new SessionBasket();
				return basket as SessionBasket;
			}
			set
			{
				SetPropertyValue("Basket", value);
			}
		}
		#endregion

		[SettingsAllowAnonymous(true)]
		public UserType CurrentUserType
		{
			get
			{
				UserType type = UserType.Guest;
				if ((int)base.GetPropertyValue("CurrentUserType") != 0)
				{
					type = (UserType)base.GetPropertyValue("CurrentUserType");
				}
				return type;
			}
			set
			{
				base.SetPropertyValue("CurrentUserType", value);
			}
		}

		#region Operators...
		
		[SettingsAllowAnonymous(true)]
		public virtual DateTime LastManagerActivityDate
		{
			get { return (DateTime)base.GetPropertyValue("LastManagerActivityDate"); }
			set { base.SetPropertyValue("LastManagerActivityDate", value); }
		}
		
		[SettingsAllowAnonymous(true)]
		public virtual string OperatorGroupName
		{
			get { return base.GetPropertyValue("OperatorGroupName") as string; }
			set { base.SetPropertyValue("OperatorGroupName", value); }
		}
		[SettingsAllowAnonymous(true)]
		public virtual string OperatorGroupXmlFile
		{
			get { return base.GetPropertyValue("OperatorGroupXmlFile") as string; }
			set { base.SetPropertyValue("OperatorGroupXmlFile", value); }
		}
		[SettingsAllowAnonymous(true)]
		public virtual string OperatorGroupXml
		{
			get { return base.GetPropertyValue("OperatorGroupXml") as string; }
			set { base.SetPropertyValue("OperatorGroupXml", value); }
		}
		#endregion

		/// <summary>
		/// Collection holding the saved forms data.
		/// </summary>
		[SettingsAllowAnonymous(true)]
		[CustomProviderData("SerizalizeAs;Binary")]
		public List<lw.CTE.FormsData> FormsData
		{
			get
			{
				List<lw.CTE.FormsData> cp;
				if (base.GetPropertyValue("FormsData") != null)
					cp = (List<lw.CTE.FormsData>)base.GetPropertyValue("FormsData");
				else
					cp = new List<lw.CTE.FormsData>();

				return cp;
			}
			set
			{
				base.SetPropertyValue("FormsData", value);
			}
		}

		

		#region functions
		public static void SignOut()
		{
			WebContext.Profile.UserLogged = false;
			WebContext.Profile.UserEmail = "";
			if(!WebContext.Profile.UserRememberName)
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
            WebContext.Profile.Roles = (int)lw.CTE.Enum.Roles.Visitor;
		}
		#endregion
	}
}