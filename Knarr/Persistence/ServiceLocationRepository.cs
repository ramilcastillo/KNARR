using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceLocationRepository : IServiceLocationRepository
    {
        private readonly KnarrDbContext _context;

        public ServiceLocationRepository(KnarrDbContext context)
        {
            _context = context;
        }

        public void AddServiceLocation(ServiceLocation location)
        {
            _context.Database.ExecuteSqlCommand("INSERT_SERVICE_LOCATION @serviceId,@Longitude,@Latitude", new SqlParameter("@serviceId", System.Data.SqlDbType.Int) { Value = location.ServiceId },
                new SqlParameter("@Longitude", System.Data.SqlDbType.Decimal) { Value = location.Longitude },
                new SqlParameter("@Latitude", System.Data.SqlDbType.Decimal) { Value = location.Latitude });
        }

        public async Task<ServiceLocation> GetServiceCurrentLocationAsync(int id)
        {
            return await _context.ServiceLocations
               .Include(x => x.Service)
               .Where(x => x.ServiceId == id).FirstOrDefaultAsync();
        }

        public void UpdateServiceLocation(ServiceLocation location)
        {
            _context.Database.ExecuteSqlCommand("UPDATE_SERVICE_LOCATION @serviceId,@Longitude,@Latitude", new SqlParameter("@serviceId", System.Data.SqlDbType.Int) { Value = location.ServiceId },
              new SqlParameter("@Longitude", System.Data.SqlDbType.Decimal) { Value = location.Longitude },
              new SqlParameter("@Latitude", System.Data.SqlDbType.Decimal) { Value = location.Latitude });
        }
    }
}
