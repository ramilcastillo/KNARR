using Knarr.Controllers.Resources.ServiceTypes;

namespace Knarr.Controllers.Resources.ServiceCategories
{
    public class ServiceCategoryResource
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public int ServiceTypeId { get; set; }

        public ServiceTypeResource ServiceType { get; set; }
    }
}
