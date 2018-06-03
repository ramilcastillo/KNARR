using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceRepository
    {
        void Add(Service service);
        Task<Service> GetServiceAsync(int id);
        Task<IEnumerable<Service>> GetServicesAsync();
        Task<IEnumerable<Service>> GetServicesByServiceProviderIdAsync(int serviceProviderId);
        void Delete(Service service);
        Task<IEnumerable<Service>> GetAllNearestAvailableServicesAsyc(SearchService search);
        Task<IEnumerable<Service>> FilterServicesAsync(FilterServices filterService);
        Task<IEnumerable<Service>> SerachServiceByNameAsync(string name);
    }
}
