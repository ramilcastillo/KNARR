using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Knarr.Controllers.Resources.ServiceProviders;
using Knarr.Controllers.Resources.ServicesPhotos;
using Knarr.Controllers.Resources.States;
using Knarr.Core.Models;

namespace Knarr.Controllers.Resources.Services
{
    public class ServicesResource
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public int RiderPerVessel { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public decimal Rate { get; set; }

        public bool IsOnRide { get; set; }

        public ServiceProviderResource ServiceProvider { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public StatesResources State { get; set; }

        public int? PostalCode { get; set; }

        public int? ServiceTypeId { get; set; }

        public ServiceType ServiceType { get; set; }

        public ICollection<ServicePhotoResource> Photos { get; set; }

        public ServicesResource()
        {
            Photos = new Collection<ServicePhotoResource>();
        }
    }
}
