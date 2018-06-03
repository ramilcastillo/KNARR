using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Knarr.Persistence
{
    public class DistressedUsersRepository : IDistressedUsersRepository
    {
        private KnarrDbContext _context;

        public DistressedUsersRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public void Create(DistressedUsers details)
        {
            _context.Database.ExecuteSqlCommand(
                "CREATE_DISTRESSED_USERS @distressedUserId,@distressType,@Longitude,@Latitude,@isComfirmed,@isCompleted",
                new SqlParameter("@distressedUserId", SqlDbType.NVarChar) {Value = details.DistressedUserId},
                new SqlParameter("@distressType", SqlDbType.Int) {Value = details.DistressType},
                new SqlParameter("@Longitude", SqlDbType.Decimal) {Value = details.Longitude},
                new SqlParameter("@Latitude", SqlDbType.Decimal) {Value = details.Latitude},
                new SqlParameter("@isComfirmed", SqlDbType.Bit) {Value = details.IsConfirmed },
                new SqlParameter("@isCompleted", SqlDbType.Bit) {Value = details.IsCompleted });
        }

        public async Task<IEnumerable<DistressedUsers>> GetDistressedRequestAsync(string userId)
        {
            return await _context.DistressedUsers
                                .Include(x => x.User)
                                .Where(x => x.IsConfirmed && x.User.Id != userId)
                                .ToListAsync();
        }

        public async Task<IEnumerable<DistressedUsers>> GetOwnDistressedRequestsAsync(string userId)
        {
            return await _context.DistressedUsers
                .Include(x => x.User)
                .Where(x => x.IsConfirmed && x.DistressedUserId == userId && !x.IsCompleted)
                .ToListAsync();
        }

        public async Task<DistressedUsers> GetDistressedRequestDetailsAsync(int id)
        {
            return await _context.DistressedUsers
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id && x.IsConfirmed);
        }

        public async Task<bool> IsAnyIncompleteDistressedRequestByUserAsync(string userId)
        {
            bool status = false;

            var info  = await _context.DistressedUsers
                .Include(x => x.User)
                .Where(x => x.IsConfirmed && x.DistressedUserId == userId && !x.IsCompleted)
                .ToListAsync();

            if (info.Any())
            {
                status = true;
            }

            return status;
        }

        public async Task<IEnumerable<DistressedUsers>> GetAllNearestDistressedUsersAsync(SearchService search)
        {
            var distressedBeaconList = await _context.DistressedUsers
                .FromSql("GET_ALL_DISTRESSED_USERS @Longitude,@Latitude", new SqlParameter("@Longitude", SqlDbType.Decimal) { Value = search.Longitude }, new SqlParameter("@Latitude", System.Data.SqlDbType.Decimal) { Value = search.Latitude }).ToListAsync();

            return distressedBeaconList;
        }
    }
}
