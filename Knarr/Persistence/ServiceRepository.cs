using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Persistence
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly KnarrDbContext _context;

        public ServiceRepository(KnarrDbContext context)
        {
            _context = context;
        }
        public void Add(Service service)
        {
            _context.Add(service);
        }

        public void Delete(Service service)
        {
            _context.Remove(service);
        }

        public async Task<Service> GetServiceAsync(int id)
        {
            return await _context
                .Services
                .Include(c => c.States)
                .Include(c => c.ServiceProvider)
                .ThenInclude(c => c.ApplicationUser)
                .SingleOrDefaultAsync(w=> w.Id == id);
        }

        public async Task<IEnumerable<Service>> GetServicesAsync()
        {
            return await _context
                .Services
                .Include(c=>c.ServiceProvider)
                .ThenInclude(c => c.ApplicationUser)
                .Include(c => c.States)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> FilterServicesAsync(FilterServices filterService)
        {
            var serviceCatagoryTbl = new DataTable("ServiceCatagoriesType");
            serviceCatagoryTbl.Columns.Add("ServiceCatagoryId",typeof(int));
            var filterResultList = new List<Service>();

            foreach (var serviceCatagory in filterService.ServiceCatagoryIds)
            {
                serviceCatagoryTbl.Rows.Add(Convert.ToInt32(serviceCatagory));
            }

            var filterDatesTbl = new DataTable("FilterDateRangeTable");
            filterDatesTbl.Columns.Add("dates");

            for(var day = filterService.StartDate; day <= filterService.EndDate; day = day.AddDays(1))
            {
                filterDatesTbl.Rows.Add(day);
            }
            var connection = _context.Database.GetDbConnection();

           await connection.OpenAsync();
            var command = connection.CreateCommand();

                    command.CommandText = "SEARCH_NEAREST_SERVICES";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@serviceTypeId", filterService.ServiceTypeId));
                    command.Parameters.Add(new SqlParameter("@serviceCatagoryTempTable", serviceCatagoryTbl));
                    command.Parameters.Add(new SqlParameter("@distance", filterService.Distance));
                    command.Parameters.Add(new SqlParameter("@Longitude", filterService.Longitude));
                    command.Parameters.Add(new SqlParameter("@latitude", filterService.Latitude));
                    command.Parameters.Add(new SqlParameter("@datesTempTable", filterDatesTbl));


                    var dr = await command.ExecuteReaderAsync();
                    while (dr.Read())
                    {
                        var s = new Service {Id = Convert.ToInt32(dr["ServiceId"])};

                        filterResultList.Add(s);
                    }
                    dr.Close();
                
            return filterResultList;
        }

        public async Task<IEnumerable<Service>> GetAllNearestAvailableServicesAsyc(SearchService search)
        {
            var serviceList = await _context.Services
                .FromSql("GET_ALL_NEAREST_SERVICES @Longitude,@Latitude", new SqlParameter("@Longitude", System.Data.SqlDbType.Decimal) { Value = search.Longitude },new SqlParameter("@Latitude", System.Data.SqlDbType.Decimal) { Value = search.Latitude }).ToListAsync();

            return serviceList;
        }

        public async Task<IEnumerable<Service>> SerachServiceByNameAsync(string name)
        {
            return await _context.Services
                .Include(x => x.ServiceProvider)
                .Include(x => x.ServiceProvider.ApplicationUser)
                .Include(x => x.States)
                .Where(x => (x.ServiceProvider.Name.Contains(name) || x.ServiceProvider.ApplicationUser.FirstName.Contains(name) || x.ServiceProvider.ApplicationUser.LastName.Contains(name)) && x.IsActive && !x.IsDeleted && !x.IsOnRide)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetServicesByServiceProviderIdAsync(int serviceProviderId)
        {
            return await _context
                .Services
                .Include(x => x.Photos)
                .Include(c => c.ServiceProvider)
                .Include(c => c.States)
                .Where(x => x.ServiceProviderId == serviceProviderId && x.IsDeleted == false)
                .ToListAsync();
        }
    }
}
