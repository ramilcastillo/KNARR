using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Knarr.Controllers.Resources.Notification.FireBase
{
    public class FireBaseServiceClient : IFireBaseServiceClient
    {
        private const string ServerKey = "AAAAcM0PmFc:APA91bHcsXpvaD4B6O1UsLNucrOKg6UXykrnrwaLLkvoRAjKILhqGGtTbGbGUiaIfbqaFQ-5h64GzSLQVQ4vnyFBzu33Oofd8rVPdeyxaNtQB3k_vo00yfDl69QhMUdWcpL5DRL0pKBo";
        private const string SenderId = "484476688471";

        public async Task<bool> SendNotification(NotificationInformation information)
        {
            foreach (var details in information.DeviceInformations)
            {
                if (string.IsNullOrWhiteSpace(details.FcmToken))
                {
                    var fireUri = new Uri("https://fcm.googleapis.com/fcm/send");
                    var fireRequest = WebRequest.Create(fireUri);
                    fireRequest.Method = "POST";
                    fireRequest.Headers.Add("Authorization", $"key={ServerKey}");
                    fireRequest.Headers.Add("Sender", $"id={SenderId}");
                    fireRequest.ContentType = "application/json";

                    var payLoad = new
                    {
                        to = "dsfsdfsdfsd",//details.FcmToken,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {
                            title = information.NotiTitle,
                            body = information.NotiMessage
                        },
                    };
                    var data = JsonConvert.SerializeObject(payLoad);

                    var byteArray = Encoding.UTF8.GetBytes(data);
                    fireRequest.ContentLength = byteArray.Length;
                    using (var dataStream = fireRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (var fireResponse = await fireRequest.GetResponseAsync())
                        {
                            using (var dataStreamResponse = fireResponse.GetResponseStream())
                            {
                                if (dataStreamResponse != null) using (var fireReader = new StreamReader(dataStreamResponse))
                                {
                                    var sResponseFromServer = fireReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
