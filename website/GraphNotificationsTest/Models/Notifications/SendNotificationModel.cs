using GraphNotificationsTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphNotificationsTest.Models.Notifications
{
    public class SendNotificationModel
    {
        public string Error { get; set; }

        public GraphNotification Notification { get; set; } = new GraphNotification();
    }
}
