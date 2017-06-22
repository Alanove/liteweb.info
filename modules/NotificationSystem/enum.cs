using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lw.NotificationSystem
{
    public enum NotificationStatus
    {
        Pending = 0, // unread
        Sent = 1,
        Read = 2,
        Deleted = 3
    }

    public sealed class NotificationHubGroup
    {
        public static readonly NotificationHubGroup Friends = new NotificationHubGroup("Friends");
        public static readonly NotificationHubGroup Chapters = new NotificationHubGroup("Chapters");
        public static readonly NotificationHubGroup Groups = new NotificationHubGroup("Groups");

        private NotificationHubGroup(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
