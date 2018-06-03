using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Knarr.Persistence
{
    public class UserDeviceInformationRepository : IUserDeviceInformationRepository
    {
        private readonly KnarrDbContext _context;

        public UserDeviceInformationRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public void Add(UserDeviceInformations information)
        {
            _context.Add(information);
        }

        public async Task<IEnumerable<UserDeviceInformations>> GetUserDeviceInformationsAsync(string userId)
        {
            return await _context.UserDeviceInformations.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
