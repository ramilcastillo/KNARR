using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class InactiveDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime InactiveDate { get; set; }

        [Required]
        public int ServiceId { get; set; }
        
        [ForeignKey("ServiceId")]
        public Service Services { get; set; }
    }
}
