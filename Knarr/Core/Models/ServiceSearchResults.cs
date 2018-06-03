using System.Collections.Generic;

namespace Knarr.Core.Models
{
    public class ServiceSearchResults
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int RiderPerVessel { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsOnRide { get; set; }

        public decimal Rate { get; set; }

        public int ServiceProviderId { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public int? State { get; set; }

        public int? PostalCode { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public decimal Distance { get; set; }

        public decimal AvgRatingsPoint { get; set; }

        public List<string> ReviewComments { get; set; }

        public ServiceProvider ServiceProvider { get; set; }
    }
}
