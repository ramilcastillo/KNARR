using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private readonly KnarrDbContext _context;

        public ServiceTypeRepository(KnarrDbContext context)
        {
            _context = context;
        }
        public void Add(ServiceType serviceType)
        {
            _context.Add(serviceType);
        }

        public void Delete(ServiceType serviceType)
        {
            _context.Remove(serviceType);
        }

        public async Task<ServiceType> GetServiceTypeAsync(int id)
        {
            return await _context.ServiceTypes.FindAsync(id);
        }

        public async Task<IEnumerable<ServiceType>> GetServiceTypesAsync()
        {
            return await _context.ServiceTypes.ToListAsync();
        }
    }
}
