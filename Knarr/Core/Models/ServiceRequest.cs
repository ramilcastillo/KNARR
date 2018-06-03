using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ServiceRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(450)]
        public string PassengerId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy HH:mm:ss}")]
        public DateTime? RequestDate { get; set; }
        public bool IsPending { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy HH:mm:ss}")]
        public DateTime? DeletedDate { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm:ss}")]
        public TimeSpan? TotalTime { get; set; }

        [ForeignKey("PassengerId")]
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy HH:mm:ss}")]
        public DateTime? CheckInStartTime { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }   
        
        public bool OnRide { get; set; }
    }
}
