using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceCategoryRepository : IServiceCategoryRepository
    {
        private readonly KnarrDbContext _context;

        public ServiceCategoryRepository(KnarrDbContext context)
        {
            _context = context;
        }
        public void Add(ServiceCategory serviceCategory)
        {
            _context.Add(serviceCategory);
            _context.SaveChanges();
        }

        public void AddServiceCategory(ServiceServiceTypes serviceCategory)
        {
            _context.Add(serviceCategory);
            _context.SaveChanges();
        }

        public void DeleteServiceServiceTypesByServiceId(int serviceId)
        {
            var records = _context.ServiceServiceTypes.Where(s => s.ServiceId == serviceId).ToList();
            foreach (var r in records)
            {
                _context.ServiceServiceTypes.Remove(r);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<ServiceCategory>> GetServiceCategoriesAsync()
        {
            return await _context.ServiceCategories.ToListAsync();
        }

        public async Task<IEnumerable<ServiceCategory>> GetServiceCategoriesByServiceTypeAsync(int serviceTypeId)
        {
            return await _context.ServiceCategories
                .Include(x => x.ServiceType)
                .Where(x => x.ServiceTypeId == serviceTypeId)
                .ToListAsync();
        }

        

        public async Task<ServiceCategory> GetServiceCategoryAsync(int id)
        {
            return await _context.ServiceCategories.FindAsync(id);
        }
    }
}
