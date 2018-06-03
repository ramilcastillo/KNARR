using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ServiceServiceTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        [Required]
        public int ServiceCategoryId { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        [ForeignKey("ServiceTypeId")]
        public ServiceType ServiceType { get; set; }

        [ForeignKey("ServiceCategoryId")]
        public ServiceCategory ServiceCategories { get; set; }
    }
}
