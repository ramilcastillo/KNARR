using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Knarr.Core.Models;

namespace Knarr.Core
{
    public interface IDistressedUsersRepository
    {
        void Create(DistressedUsers details);

        Task<IEnumerable<DistressedUsers>> GetDistressedRequestAsync(string userId);

        Task<IEnumerable<DistressedUsers>> GetOwnDistressedRequestsAsync(string userId);

        Task<DistressedUsers> GetDistressedRequestDetailsAsync(int id);

        Task<bool> IsAnyIncompleteDistressedRequestByUserAsync(string userId);

        Task<IEnumerable<DistressedUsers>> GetAllNearestDistressedUsersAsync(SearchService search);
    }
}
