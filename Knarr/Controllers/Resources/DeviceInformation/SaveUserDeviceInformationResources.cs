using System.ComponentModel.DataAnnotations;

namespace Knarr.Controllers.Resources.DeviceInformation
{
    public class SaveUserDeviceInformationResources
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string DeviceType { get; set; }

        public string FcmToken { get; set; }
    }
}
