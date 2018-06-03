using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceProviderTypeRepository
    {
        void Add(ServiceProviderType serviceProviderType);
        Task<IEnumerable<ServiceProviderType>> GetServiceProviderTypesAsync();
        Task<IEnumerable<ServiceProviderType>> GetServiceProviderTypeAsync(int id);
        Task<ServiceProviderType> GetServiceProviderTypeByIdAsync(int id);
        void Delete(ServiceProviderType serviceProviderType);
    }
}
