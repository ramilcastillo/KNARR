using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceServiceTypeRepository : IServiceServiceTypeRepository
    {
        private readonly KnarrDbContext _context;

        public ServiceServiceTypeRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public void AddServiceServiceType(ServiceServiceTypes serviceServiceTypes)
        {
            _context.Add(serviceServiceTypes);
        }

        public void DeleteServiceServiceType(ServiceServiceTypes serviceServiceTypes)
        {
            _context.Remove(serviceServiceTypes);
        }

        public async Task<ServiceServiceTypes> GetServiceServiceTypeAsync(int id)
        {
            return await _context.ServiceServiceTypes.FindAsync(id);
        }

        public async Task<IEnumerable<ServiceServiceTypes>> GetServiceServiceTypeByServiceIdAsync(int serviceId)
        {
            return await _context.ServiceServiceTypes
                .Include(x => x.Service)
                .Include(x => x.ServiceType)
                .Include(x => x.ServiceCategories)
                .Where(x => x.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceServiceTypes>> GetServiceServiceTypesAsync()
        {
            return await _context.ServiceServiceTypes.ToListAsync();
        }
    }
}
