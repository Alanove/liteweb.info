
using lw.Utils;

namespace lw.CTE.Enum
{
	public enum UserStatus
	{
		None = 0,
		Enabled = 2,
		Pending = 4,
		Incomplete = 8,
		Modified = 16,
		Reseller = 32,
		Unknown = 64,
		Disabled = 128,
		Deceased = 256,
		ChangePassword = 512,
		Old = 1024
	}

	/// <summary>
	/// Defines the current user type, defaults to guest
	/// </summary>
	/// <remarks>
	/// Note: when changing this check lwMenu.UserType
	/// to select more than one type use the "|" operator
	/// </remarks>
	public enum UserType
	{
		Unknown = 0, 
		Guest = 2,
		User = 4,
		Operator = 8
	}

	public enum UserEducation
	{ 
		[Name("University")]
		University = 1,

		[Name("School")]
		School = 2
	}


	public enum UserSocial
	{
		[Name("Facebook")]
		facebook = 1,

		[Name("Twitter")]
		twitter = 2,

		[Name("LinkedIn")]
		linkedin = 3,

		[Name("MSN")]
		msn = 4,

		[Name("Yahoo")]
		yahoo = 5,

		[Name("Skype")]
		skype = 6,

		[Name("Instagram")]
		instagram = 7,

		[Name("Google+")]
		googleplus = 8,

		[Name("Behance")]
		behance = 9
	}
}
