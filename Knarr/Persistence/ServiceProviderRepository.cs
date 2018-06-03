using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceProviderRepository : IServiceProviderRepository
    {
        private readonly KnarrDbContext _context;

        public ServiceProviderRepository(KnarrDbContext context)
        {
            _context = context;
        }
        public void Add(ServiceProvider serviceProvider)
        {
            _context.Add(serviceProvider);
        }

        public void Delete(ServiceProvider serviceProvider)
        {
            _context.Remove(serviceProvider);
        }

        public async Task<ServiceProvider> GetServiceProviderAsync(int id)
        {
            return await _context.ServiceProviders.Include(sp => sp.ApplicationUser).SingleOrDefaultAsync(w=>w.Id == id);
        }

        public async Task<IEnumerable<ServiceProvider>> GetServiceProviderByUserAsync(string userId)
        {
            var result = await _context.ServiceProviders.Where(x => x.UserId == userId).ToListAsync();
            return result;
        }

        public async Task<ServiceProvider> GetServiceProviderDetailsByUserAsync(string userId)
        {
            var result = await _context.ServiceProviders.Where(x => x.UserId == userId).SingleOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<ServiceProvider>> GetServiceProvidersByStatusAsync(int serviceProviderStatusId)
        {
            var result = await _context.ServiceProviders
                    .Include(x =>x.ApplicationUser)
                .Where(x => x.ServiceProviderRequestsStatusId == serviceProviderStatusId).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ServiceProvider>> GetServiceProvidersAsync()
        {
            return await _context.ServiceProviders
                .Include(sp => sp.ApplicationUser)
                .ToListAsync();
        }
    }
}
