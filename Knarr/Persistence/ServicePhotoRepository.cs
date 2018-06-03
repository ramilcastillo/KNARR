using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServicePhotoRepository : IServicePhotoRepository
    {
        private readonly KnarrDbContext _context;

        public ServicePhotoRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public void Delete(ServicePhoto servicePhoto)
        {
            _context.ServicePhotos.Remove(servicePhoto);
        }

        public async Task<ServicePhoto> GetPhoto(int id)
        {
            return await _context.ServicePhotos.FindAsync(id);
        }

        public async Task<IEnumerable<ServicePhoto>> GetPhotos(int serviceId)
        {
            return await _context.ServicePhotos
              .Where(p => p.ServiceId == serviceId)
              .ToListAsync();
        }
    }
}
