using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceServiceTypeRepository
    {
        void AddServiceServiceType(ServiceServiceTypes serviceServiceTypes);
        Task<IEnumerable<ServiceServiceTypes>> GetServiceServiceTypesAsync();
        Task<ServiceServiceTypes> GetServiceServiceTypeAsync(int id);
        Task<IEnumerable<ServiceServiceTypes>> GetServiceServiceTypeByServiceIdAsync(int serviceId);
        void DeleteServiceServiceType(ServiceServiceTypes serviceServiceTypes);
    }
}
