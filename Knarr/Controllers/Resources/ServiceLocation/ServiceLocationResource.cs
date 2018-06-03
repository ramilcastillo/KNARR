using Knarr.Controllers.Resources.Services;
using System.ComponentModel.DataAnnotations;

namespace Knarr.Controllers.Resources.ServiceLocation
{
    public class ServiceLocationResource
    {
        public int Id { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        public ServicesResource Service { get; set; }
    }
}
