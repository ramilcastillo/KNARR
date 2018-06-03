using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ServiceLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }

        //public GeographyPoint Location { get; set; } 

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
    }
}
