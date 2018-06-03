using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ServiceReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public string UserId { get; set; }

        [Required]
        public int RatingPoint { get; set; }

        public string Comments { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
