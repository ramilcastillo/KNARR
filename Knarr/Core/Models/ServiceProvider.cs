using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ServiceProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Information { get; set; }

        [StringLength(255)]
        public string Information2 { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; }

        public bool? IsAvailable { get; set; }

        public int ServiceProviderRequestsStatusId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser {get;set;}

        public ICollection<ServiceProviderType> ServiceProviderTypes { get; set; }

        public ServiceProvider()
        {
            ServiceProviderTypes = new Collection<ServiceProviderType>();
        }
    }
}
