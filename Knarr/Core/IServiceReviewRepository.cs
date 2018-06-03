using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServiceReviewRepository
    {
        void AddReview(ServiceReview review);
        Task<IEnumerable<ServiceReview>> GetReviews(int serviceId);
    }
}
