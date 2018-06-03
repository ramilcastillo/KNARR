using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceReviewRepository : IServiceReviewRepository
    {
        private readonly KnarrDbContext _context;
        public ServiceReviewRepository(KnarrDbContext context)
        {
            _context = context;
        }
        public void AddReview(ServiceReview review)
        {
            _context.ServiceReviews.Add(review);
        }

        public async Task<IEnumerable<ServiceReview>> GetReviews(int serviceId)
        {
            return await _context.ServiceReviews
                .Include(x => x.ApplicationUser)
                .Include(x => x.Service)
                .Where(x => x.ServiceId == serviceId).OrderByDescending(x => x.RatingPoint)
                .ToListAsync();
        }
    }
}
