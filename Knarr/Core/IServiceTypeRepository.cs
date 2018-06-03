using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceTypeRepository
    {
        void Add(ServiceType serviceType);
        Task<ServiceType> GetServiceTypeAsync(int id);
        Task<IEnumerable<ServiceType>> GetServiceTypesAsync();
        void Delete(ServiceType serviceType);
    }
}
