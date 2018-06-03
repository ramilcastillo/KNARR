using Knarr.Controllers.Resources.ServiceCategories;
using Knarr.Controllers.Resources.Services;
using Knarr.Controllers.Resources.ServiceTypes;

namespace Knarr.Controllers.Resources.ServiceServiceTypes
{
    public class ServiceServiceTypeResource
    {
        public int Id { get; set; }

        public ServicesResource Service { get; set; }

        public ServiceTypeResource ServiceType { get; set; }

        public ServiceCategoryResource ServiceCategory { get; set; }
        
    }
}
