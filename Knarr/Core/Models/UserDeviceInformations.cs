using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class UserDeviceInformations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string DeviceType { get; set; }

        [Required]
        public string FcmToken { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser Users { get; set; }
    }
}
