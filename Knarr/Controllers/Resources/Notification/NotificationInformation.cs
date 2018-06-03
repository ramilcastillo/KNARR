using Knarr.Core.Models;
using System.Collections.Generic;

namespace Knarr.Controllers.Resources.Notification
{
    public class NotificationInformation
    {
        public IEnumerable<UserDeviceInformations> DeviceInformations { get; set; }

        public string NotiTitle { get; set; }

        public string NotiMessage { get; set; }

        public string NotiIcon { get; set; }
    }
}
