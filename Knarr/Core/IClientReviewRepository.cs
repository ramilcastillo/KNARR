using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IClientReviewRepository
    {
        void AddClientReviewAsync(ClientReviews review);
        Task<IEnumerable<ClientReviews>> GetClientReviewAsync(string passengerId);
    }
}
