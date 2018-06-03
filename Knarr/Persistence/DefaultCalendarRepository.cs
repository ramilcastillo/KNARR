using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class DefaultCalendarRepository : IDefaultCalendarRepository
    {
        private readonly KnarrDbContext _context;

        public DefaultCalendarRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public void Add(DefaultCalendar calendar)
        {
            _context.Add(calendar);
        }

        public void Delete(DefaultCalendar calendar)
        {
            _context.Remove(calendar);
        }

        public async Task<DefaultCalendar> GetdefaultCalendarAsync(int id)
        {
            return await _context.DefaultCalendars.FindAsync(id);
        }

        public async Task<IEnumerable<DefaultCalendar>> GetDefaultCalenderByServiceProviderAsync(int serviceProviderId)
        {
            return await _context.DefaultCalendars
                .Include(c => c.ServiceProvider)
                .Where(x => x.ServiceProviderId == serviceProviderId)
                .ToListAsync();
        }
    }
}
