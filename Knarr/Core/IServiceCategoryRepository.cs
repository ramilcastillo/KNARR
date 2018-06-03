using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceCategoryRepository
    {
        void DeleteServiceServiceTypesByServiceId(int serviceId);
        void Add(ServiceCategory serviceCategory);
        void AddServiceCategory(ServiceServiceTypes serviceCategory);
        Task<IEnumerable<ServiceCategory>> GetServiceCategoriesAsync();
        Task<ServiceCategory> GetServiceCategoryAsync(int id);
        Task<IEnumerable<ServiceCategory>> GetServiceCategoriesByServiceTypeAsync(int serviceTypeId);

    }
}
