using System;

namespace Knarr.Controllers.Resources.Services
{
    public class SaveServicesResource
    {

        public int Id { get; set; }


        public string Title { get; set; }


        public int RiderPerVessel { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public decimal Rate { get; set; }

        public bool IsOnRide { get; set; }

        public int ServiceProviderId { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public int? State { get; set; }

        public int? PostalCode { get; set; }

        public int? ServiceTypeId { get; set; }
    }
}
