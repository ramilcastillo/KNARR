using System;

namespace Knarr.Core.Models
{
    public class FilterServices
    {
        public int ServiceTypeId { get; set; }

        public string[] ServiceCatagoryIds { get; set; }

        public decimal Distance { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ServiceProviderId { get; set; }
    }
}
