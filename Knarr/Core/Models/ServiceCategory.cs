using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ServiceCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ServiceTypeId { get; set; }

        [ForeignKey("ServiceTypeId")]
        public ServiceType ServiceType { get; set; }
    }
}
