using System.ComponentModel.DataAnnotations;

namespace Knarr.Controllers.Resources.ServiceLocation
{
    public class SaveServiceLocationResource
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public decimal Latitude { get; set; }
    }
}
