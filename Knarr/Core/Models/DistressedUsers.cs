using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Core.Models
{
    public class DistressedUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string DistressedUserId { get; set; }

        [Required]
        public int DistressType { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsCompleted { get; set; }

        [ForeignKey("DistressedUserId")]
        public ApplicationUser User { get; set; }
    }
}
