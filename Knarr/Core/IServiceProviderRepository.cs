using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceProviderRepository
    {
        void Add(ServiceProvider serviceProvider);
        Task<ServiceProvider> GetServiceProviderAsync(int id);
        Task<IEnumerable<ServiceProvider>> GetServiceProvidersAsync();
        void Delete(ServiceProvider serviceProvider);
        Task<IEnumerable<ServiceProvider>> GetServiceProviderByUserAsync(string userId);
        Task<ServiceProvider> GetServiceProviderDetailsByUserAsync(string userId);
        Task<IEnumerable<ServiceProvider>> GetServiceProvidersByStatusAsync(int serviceProviderStatusId);
    }
}
