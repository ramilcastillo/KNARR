using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int RiderPerVessel { get; set; }

        [Required]
        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsOnRide { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }

        [ForeignKey("ServiceProviderId")]
        public ServiceProvider ServiceProvider { get; set; }

        public ICollection<ServicePhoto> Photos { get; set; }

       
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public int? State { get; set; }

        public int? PostalCode { get; set; }
    
        public int? ServiceTypeId { get; set; }

        [ForeignKey("ServiceTypeId")]
        public ServiceType ServiceType { get; set; }

        [ForeignKey("State")]
        public States States { get; set; }
        public Service()
        {
            Photos = new Collection<ServicePhoto>();
        }

    }
}
