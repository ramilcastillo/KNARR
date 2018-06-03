using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ClientReviews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }

        [Required]
        public string PassengerId { get; set; }

        [Required]
        public int RatingPoint { get; set; }

        public string Comments { get; set; }

        [ForeignKey("ServiceProviderId")]
        public ServiceProvider ServiceProvider { get; set; }

        [ForeignKey("PassengerId")]
        public ApplicationUser Passenger { get; set; }
    }
}
