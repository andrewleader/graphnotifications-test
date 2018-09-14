using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphNotificationsTest.Helpers
{
    public class GraphNotification
    {
        /// <summary>
        /// Represents the host name of the app to which the calling service wants to post the notification, for the given user.
        /// </summary>
        public string TargetHostName { get; set; } = "com.andrewleader.graphnotifications-test";

        /// <summary>
        /// The unique id set by the app server of a notification that is used to identify and target an individual notification.
        /// </summary>
        public string AppNotificationId { get; set; }

        /// <summary>
        /// Sets a UTC expiration time on a user notification - when time is up, the notification is removed from the Microsoft Graph notification feed store completely and is no longer part of notification history. Max value is 30 days.
        /// </summary>
        public DateTimeOffset ExpirationDateTime { get; set; }

        public GraphNotificationPayload Payload { get; set; }

        /// <summary>
        /// Sets how long (in seconds) this notification content will stay in each platform’s notification viewer. For example, when the notification is delivered to a Windows device, the value of this property is passed on to ToastNotification.ExpirationTime, which determines how long the toast notification will stay in the user’s Windows Action Center.
        /// </summary>
        public int DisplayTimeToLive { get; set; }
    }

    public class GraphNotificationPayload
    {
        /// <summary>
        /// The notification content of a raw user notification that will be delivered to and consumed by the app client receiving this notification.At least one of Payload.RawContent and Payload.VisualContent needs to be valid for a POST Notification request.
        /// </summary>
        public string RawContent { get; set; }

        public GraphNotificationPayloadVisual Visual { get; set; }
    }

    public class GraphNotificationPayloadVisual
    {
        public string Title { get; set; }

        public string Body { get; set; }
    }
}
