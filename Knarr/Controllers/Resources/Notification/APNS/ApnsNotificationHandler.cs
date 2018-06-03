using PushSharp.Common;
using System;

namespace Knarr.Controllers.Resources.Notification.APNS
{
    public class ApnsNotificationHandler
    {
        static void NotificationSent(object sender, INotification notification)
        {
            //Do something here
        }

        static void NotificationFailed(object sender,
        INotification notification, Exception notificationFailureException)
        {
            //Do something here
        }
    }
}
