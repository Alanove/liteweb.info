
using lw.Content.Controls;
using lw.CTE.Enum;

namespace lw.Members.Controls
{
	public class MemberBox : DisplayContainer
	{
		bool _loggedIn = false;
		bool _userEnabled = false;
		bool _bound = false;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			Display = _loggedIn ? Security.User.LoggedIn : !Security.User.LoggedIn;
			if (_userEnabled)
			{
				if (WebTools.WebContext.Profile.CurrentUserStatus != (int)UserStatus.Enabled)
					Display = false;
			}
			base.DataBind();
		}

		/// <summary>
		/// Same as visible when logged in
		/// </summary>
		public bool LoggedIn
		{
			get { return _loggedIn; }
			set { _loggedIn = value; }
		}

		/// <summary>
		/// Will render the control and its children's visibility depending on login status
		/// True: Visible When Logged In
		/// False: Not Visible When LoggedIn
		/// </summary>
		public bool VisibleWhenLoggedIn
		{
			get { return _loggedIn; }
			set { _loggedIn = value; }
		}


		/// <summary>
		/// Will render the control and its children's visibility depending on user status
		/// True: Visible When User Status is Enabled
		/// False: Not Visible When User Status is different than enabled
		/// </summary>
		public bool VisibleWhenUserEnabled
		{
			get { return _userEnabled; }
			set { _userEnabled = value; }
		}
	}

}
