using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class InactiveDaysRepository : IInactiveDaysRepository
    {
        private readonly KnarrDbContext _context;

        public InactiveDaysRepository(KnarrDbContext context)
        {
            _context = context;
        }
        public void Add(InactiveDay days)
        {
            _context.Add(days);
        }

        public void Delete(InactiveDay days)
        {
            _context.Remove(days);
        }

        public async Task<InactiveDay> GetInactiveDayAsync(int id)
        {
            return await _context.InactiveDays.FindAsync(id);
        }

        public async Task<IEnumerable<InactiveDay>> GetInactiveDayPerServiceAsync(int serviceId)
        {
            return await _context
                .InactiveDays
                .Include(i => i.Services)
                .Where(w => w.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InactiveDay>> GetInactiveDaysAsync()
        {
            return await _context.InactiveDays.Include(x => x.Services).ToListAsync();
        }
    }
}
