using System.Collections.Generic;

namespace Knarr.Controllers.Resources.Notification
{
    public class SendNotification
    {
        public static void Send(IEnumerable<NotificationInformation> info)
        {
            foreach(var deviceNotiInfo in info)
            {
                foreach(var device in deviceNotiInfo.DeviceInformations)
                {
                    if(device.DeviceType == "ios")
                    {

                    }
                    if(device.DeviceType == "android")
                    {

                    }
                }
            }
        }
    }
}
