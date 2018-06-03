using Knarr.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Knarr.Persistence
{
    public class KnarrDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ServiceProviderType> ServiceProviderTypes { get; set; }
        public DbSet<ServiceReview> ServiceReviews { get; set; }
        public DbSet<ServicePhoto> ServicePhotos { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<DefaultCalendar> DefaultCalendars { get; set; }
        public DbSet<ServiceLocation> ServiceLocations { get; set; }
        public DbSet<InactiveDay> InactiveDays { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ClientReviews> ClientReviews { get; set; }
        public DbSet<ServiceServiceTypes> ServiceServiceTypes { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<UserDeviceInformations> UserDeviceInformations { get; set; }
        public DbSet<DistressedUsers> DistressedUsers { get; set; }
        

        public KnarrDbContext(DbContextOptions<KnarrDbContext> options) : base(options)
        {

        }
    }
}
