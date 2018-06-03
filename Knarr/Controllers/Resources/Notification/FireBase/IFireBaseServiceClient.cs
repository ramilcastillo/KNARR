using System.Threading.Tasks;

namespace Knarr.Controllers.Resources.Notification.FireBase
{
    public interface IFireBaseServiceClient
    {
        Task<bool> SendNotification(NotificationInformation information);
    }
}