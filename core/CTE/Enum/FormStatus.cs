namespace lw.CTE.Enum
{
    using lw.Utils;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a role enumeration or a group of Form Status.
    /// </summary>

    public enum FormStatus
    {
        /// <summary>
        /// Pending.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Open.
        /// </summary>
        [Description("In Progress")]
        InProgress = 1,

        /// <summary>
        /// Closed.
        /// </summary>
        Complete = 2
    }
}
