using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class DefaultCalendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime DefaultDates { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }

        [ForeignKey("ServiceProviderId")]
        public ServiceProvider ServiceProvider { get; set; }
    }
}
