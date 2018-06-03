using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class StateRepository : IStateRepository
    {
        private readonly KnarrDbContext _context;

        public StateRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<States>> GetAllStatesAsync()
        {
            return await _context.States.ToListAsync();
        }
    }
}
