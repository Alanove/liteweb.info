
using lw.Utils;

namespace lw.Members
{
	public enum MemberListBy
	{
		Friends, 
		PendingRequests, 
		SameNetwork,
		SameRegion,
		SameUniversity,
		All,
		Smart,
		Search,
        ChapterMembers
	}


	public enum PrivacyOptions
	{
		Everyone,

		Friends,
		
		[Description("Only Me")]
		OnlyMe
	}

	public enum Privacy
	{
		[Description("Allow visitors to access my profile information")]
		PublicProfile = 2,

		[Description("Display my email address")]
		ShowEmailAddress = 4,

		[Description("Display my phone number")]
		ShowPhone = 8,

		[Description("Display my profile picture")]
		ShowProfilePicture = 16
	}

	public enum FriendStatus
	{
		Pending = 0,
		Approved = 1,
		Blocked = 2,
		Deleted = 3,
		BlockDelete = 4
	}

	public enum Gender
	{
		Unknown = 0,
		
		[Name("M")]
		Male = 1,

		[Name("F")]
		Female = 2
	}


	public enum AddressType
	{
		[Description("Current")]
		Current = 1,

		[Description("Home Town")]
		HomeTown = 2,

		[Description("Work")]
		Work = 3,

		[Description("Billing Address")]
		BillingAddress = 4,

		[Description("Shipping Address")]
		ShippingAdrress = 5,

		[Description("Other")]
		Other = 6
	}
}