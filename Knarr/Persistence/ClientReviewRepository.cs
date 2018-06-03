using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ClientReviewRepository : IClientReviewRepository
    {
        private readonly KnarrDbContext _context;

        public ClientReviewRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public void AddClientReviewAsync(ClientReviews review)
        {
            _context.Add(review);
        }

        public async Task<IEnumerable<ClientReviews>> GetClientReviewAsync(string passengerId)
        {
            return await _context.ClientReviews
                .Include(x => x.ServiceProvider)
                .Include(x => x.Passenger)
                .Where(x => x.PassengerId == passengerId)
                .OrderByDescending(x => x.RatingPoint)
                .ToListAsync();
        }
    }
}
