namespace lw.CTE.Enum
{
    using lw.Utils;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a role enumeration or a group of roles.
    /// </summary>
 
    public enum Roles
    {
        /// <summary>
        /// User priviledges.
        /// </summary>
        Visitor = 0,

        /// <summary>
        /// Administrator priviledges.
        /// </summary>
		[PermissionGroup(31)]//0|1|2|4|8|16
		[Description("Administrator")]
        Administrator = 1,

        /// <summary>
        /// Member priviledges.
        /// </summary>
		[PermissionGroup(2)]//0|2
		[Description("Member")]
        Member = 2,

        /// <summary>
        /// Moderator priviledges.
        /// </summary>
		[PermissionGroup(6)]//0|2|4
        [Description("Moderator")]
        SchoolWebMaster = 4, //member

        /// <summary>
        /// WebMaster priviledges.
        /// </summary>
		[PermissionGroup(14)]//0|2|4|8
		[Description("Webmaster")]
        CorporateWebMaster = 8, //WebMaster  member

		///// <summary>
		///// Publisher
		///// </summary>
		//[PermissionGroup(2)]//0|2
		//Publisher = 16
    }
}
