using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ServiceProviderType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ServiceProviderId { get; set; }

        public int ServiceCategoryId { get; set; }

        [ForeignKey("ServiceProviderId")]
        public ServiceProvider ServiceProviders { get; set; }

        [ForeignKey("ServiceCategoryId")]
        public ServiceCategory ServiceCategory { get; set; }
    }
}
