using Knarr.Core.Models;

namespace Knarr.Controllers.Resources.ServiceProviders
{
    public class ServiceProviderResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public string Information2 { get; set; }

        public bool? IsAvailable { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int ServiceProviderRequestsStatusId { get; set; }
    }
}
