using Knarr.Controllers.Resources.ServiceCategories;
using Knarr.Controllers.Resources.ServiceProviders;

namespace Knarr.Controllers.Resources.ServiceProviderTypes
{
    public class ServiceProviderTypeResource
    {
        public int Id { get; set; }

        public ServiceProviderResource ServiceProvider { get; set; }

        public ServiceCategoryResource ServiceCategory { get; set; }
    }
}
