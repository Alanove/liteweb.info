
using lw.Utils;

namespace lw.Groups
{

	public enum GroupStatus
	{
		Disabled = 0,
		Enabled = 1,
		Pending = 2
	}

	public enum GroupPrivacy
	{
		[Description("Anyone can see the group, who's in it, and what members post.")]
		[Name("Open")]
		Open = 0,

		[Description("Anyone can see the group and who's in it. Only members see posts.")]
		[Name("Closed")]
		Closed = 1,

		[Description("Only members see the group, who's in it, and what members post.")]
		[Name("Secret")]
		Secret = 2
	}

	public enum GroupType
	{
		Group = 1,
		Chapter = 2
	}

	public enum GroupSecurity
	{
		[Description("Only members can post in this group.")]
		Members = 0,

		[Description("Only administrators can post to the group.")]
		Administrators = 1
	}

	public enum GroupMemberLevel
	{
		[Description("Regular")]
		Regular = 0,

		[Description("Administrator")]
		Administrator = 2,

		[Description("President")]
		President = 4,

		[Description("Vice President")]
		VicePresident = 8,

		[Description("Secretary")]
		Secretary = 16,

		[Description("Events Officer")]
		EventsOfficer = 32,

		[Description("Treasurer")]
		Treasurer = 64,

		[Description("Membership Officer")]
		MembershipOfficer = 128
	}

	public enum GroupMemberStatus
	{
		[Description("Pending")]
		Pending = 0,

		[Description("Approved")]
		Approved = 1,

		[Description("Rejected")]
		Rejected = 2,

		[Description("Blocked")]
		Blocked = 3
	}
}
