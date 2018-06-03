using Knarr.Core.Models;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceLocationRepository
    {
        void AddServiceLocation(ServiceLocation location);
        Task<ServiceLocation> GetServiceCurrentLocationAsync(int id);
        void UpdateServiceLocation(ServiceLocation location);
    }
}
