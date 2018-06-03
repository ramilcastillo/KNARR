using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly KnarrDbContext _context;
        public ServiceRequestRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public void CreateRideRequest(ServiceRequest request)
        {
            _context.ServiceRequests.Add(request);
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync(string userId)
        {
            return await _context.ServiceRequests.Where(x => x.PassengerId == userId && !x.IsDeleted)
                .Include(x => x.ApplicationUser)
                .Include(x => x.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsByServiceproviderAsync(int serviceProviderId)
        {
            return await _context.ServiceRequests.Where(x => x.Service.ServiceProviderId == serviceProviderId && !x.IsDeleted)
                .Include(x => x.ApplicationUser)
                .Include(x => x.Service)
                .ToListAsync();
        }

        public async Task<ServiceRequest> GetOnrideServcieRequestDetailsAsync(string userId)
        {
            return await _context.ServiceRequests
                .Include(x => x.Service)
                .Include(x => x.ApplicationUser)
                .Where(x => x.PassengerId == userId && x.OnRide).FirstOrDefaultAsync();
        }

        public async Task<ServiceRequest> GetOnrideServcieRequestDetailsAsyncByServiceProvider(int serviceProviderId)
        {
            return await _context.ServiceRequests
                .Include(x => x.Service)
                .Include(x => x.ApplicationUser)
                .Where(x => x.Service.ServiceProviderId == serviceProviderId && x.OnRide).FirstOrDefaultAsync();
        }

        public async Task<ServiceRequest> GetServiceRequestDetails(int requestId)
        {
            return await _context.ServiceRequests
                .Include(x => x.ApplicationUser)
                .Include(x => x.Service)
                .SingleOrDefaultAsync(x => x.Id == requestId);
        }

        public bool IsServiceProviderAlreadyOnRide(int serviceProviderId)
        {
            var obj = _context.ServiceRequests.Where(x => x.Service.ServiceProviderId == serviceProviderId && x.OnRide).SingleOrDefault();
            if(obj != null)
            {
                return true;
            }
            return false;
        }
    }
}
