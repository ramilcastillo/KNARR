using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceRequestRepository
    {
        void CreateRideRequest(ServiceRequest request);
        Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync(string userId);
        Task<ServiceRequest> GetServiceRequestDetails(int requestId);
        bool IsServiceProviderAlreadyOnRide(int serviceProviderId);
        Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsByServiceproviderAsync(int serviceProviderId);
        Task<ServiceRequest> GetOnrideServcieRequestDetailsAsyncByServiceProvider(int serviceProviderId);
        Task<ServiceRequest> GetOnrideServcieRequestDetailsAsync(string userId);

    }
}
