using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceProviderTypeRepository : IServiceProviderTypeRepository
    {
        private readonly KnarrDbContext _context;

        public ServiceProviderTypeRepository(KnarrDbContext context)
        {
            _context = context;
        }
        public void Add(ServiceProviderType serviceProviderType)
        {
            _context.Add(serviceProviderType);
        }

        public void Delete(ServiceProviderType serviceProviderType)
        {
            _context.Remove(serviceProviderType);
        }

        public async Task<IEnumerable<ServiceProviderType>> GetServiceProviderTypeAsync(int id)
        {
            return await _context.ServiceProviderTypes
                .Include(i => i.ServiceProviders)
                .Include(i => i.ServiceCategory)
                .Where(w=> w.ServiceProviderId == id)
                .ToListAsync();
        }

        public async Task<ServiceProviderType> GetServiceProviderTypeByIdAsync(int id)
        {
            return await _context.ServiceProviderTypes.FindAsync(id);
        }

        public async Task<IEnumerable<ServiceProviderType>> GetServiceProviderTypesAsync()
        {
            return await _context.ServiceProviderTypes
                .Include(s => s.ServiceProviders)
                .Include(s => s.ServiceCategory)
                .ToListAsync();
        }
    }
}
