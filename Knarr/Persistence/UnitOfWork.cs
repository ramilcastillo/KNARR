using Knarr.Core;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KnarrDbContext _context;

        public UnitOfWork(KnarrDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
