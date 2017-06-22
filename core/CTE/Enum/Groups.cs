using System;
using System.Collections.Generic;
using System.Text;

using lw.Utils;

namespace lw.CTE.Enum
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
		Open = 0,

		[Description("Anyone can see the group and who's in it. Only members see posts.")]
		Closed = 1,

		[Description("Only members see the group, who's in it, and what members post.")]
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

		[Description("President")]
		President = 2,

		[Description("Vice President")]
		VicePresident = 4,

		[Description("Secretary")]
		Secretary = 8,

		[Description("Event Officer")]
		EventOfficer = 16
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
