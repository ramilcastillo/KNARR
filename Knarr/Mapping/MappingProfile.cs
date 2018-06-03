using AutoMapper;
using Knarr.Controllers.Resources.ClientReviews;
using Knarr.Controllers.Resources.DefaultCalendars;
using Knarr.Controllers.Resources.DeviceInformation;
using Knarr.Controllers.Resources.DistressedUsers;
using Knarr.Controllers.Resources.InactiveDays;
using Knarr.Controllers.Resources.ServiceCategories;
using Knarr.Controllers.Resources.ServiceLocation;
using Knarr.Controllers.Resources.ServiceProviders;
using Knarr.Controllers.Resources.ServiceProviderTypes;
using Knarr.Controllers.Resources.ServiceRequests;
using Knarr.Controllers.Resources.ServiceReviews;
using Knarr.Controllers.Resources.Services;
using Knarr.Controllers.Resources.ServiceServiceTypes;
using Knarr.Controllers.Resources.ServicesPhotos;
using Knarr.Controllers.Resources.ServiceTypes;
using Knarr.Controllers.Resources.States;
using Knarr.Core.Models;

namespace Knarr.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to API Resource

            CreateMap<Service, ServicesResource>()
                .ForMember(s => s.State, opt => opt.MapFrom(x => new StatesResources { Id = x.States.Id, State = x.States.State, Abbreviation = x.States.Abbreviation }))
                .ForMember(sr => sr.ServiceProvider, opt => opt.MapFrom(s => new ServiceProviderResource { Id = s.ServiceProvider.Id, Information = s.ServiceProvider.Information, Information2 = s.ServiceProvider.Information2, Name = s.ServiceProvider.Name, ApplicationUser = new ApplicationUser {
                    Id = s.ServiceProvider.ApplicationUser.Id,
                    FirstName = s.ServiceProvider.ApplicationUser.FirstName,
                    LastName = s.ServiceProvider.ApplicationUser.LastName,
                    AccessFailedCount = s.ServiceProvider.ApplicationUser.AccessFailedCount,
                    Address = s.ServiceProvider.ApplicationUser.Address,
                    Country = s.ServiceProvider.ApplicationUser.Country,
                    State = s.ServiceProvider.ApplicationUser.State,
                    PhoneNumber = s.ServiceProvider.ApplicationUser.PhoneNumber,
                    ProfilePictureFileName = s.ServiceProvider.ApplicationUser.ProfilePictureFileName,
                    ConcurrencyStamp = s.ServiceProvider.ApplicationUser.ConcurrencyStamp,
                    Email = s.ServiceProvider.ApplicationUser.Email,
                    EmailConfirmed = s.ServiceProvider.ApplicationUser.EmailConfirmed,
                    LockoutEnabled = s.ServiceProvider.ApplicationUser.LockoutEnabled,
                    LockoutEnd = s.ServiceProvider.ApplicationUser.LockoutEnd,
                    NormalizedEmail = s.ServiceProvider.ApplicationUser.NormalizedEmail,
                    NormalizedUserName = s.ServiceProvider.ApplicationUser.NormalizedUserName,
                    PasswordHash = s.ServiceProvider.ApplicationUser.PasswordHash,
                    UserName = s.ServiceProvider.ApplicationUser.UserName,
                    PhoneNumberConfirmed = s.ServiceProvider.ApplicationUser.PhoneNumberConfirmed,
                    SecurityStamp =  s.ServiceProvider.ApplicationUser.SecurityStamp,
                    ServiceProviders = s.ServiceProvider.ApplicationUser.ServiceProviders,
                    TwoFactorEnabled = s.ServiceProvider.ApplicationUser.TwoFactorEnabled
                    
                } }));
            CreateMap<InactiveDay, InactiveDaysResource>()
                .ForMember(ir => ir.Services, opt => opt.MapFrom(i => new ServicesResource { Id = i.Services.Id, Title = i.Services.Title, Quantity = i.Services.Quantity, Rate = i.Services.Rate, IsActive = i.Services.IsActive, IsDeleted = i.Services.IsDeleted, RiderPerVessel = i.Services.RiderPerVessel }));
            CreateMap<ServiceRequest, ServiceRequestResource>()
                .ForMember(srr => srr.ApplicationUser, opt => opt.MapFrom(sr => new ApplicationUser { Id = sr.ApplicationUser.Id, Email = sr.ApplicationUser.Email, FirstName = sr.ApplicationUser.FirstName, LastName = sr.ApplicationUser.LastName}))
                .ForMember(srr => srr.Services, opt => opt.MapFrom(sr => new ServicesResource { Id = sr.Service.Id, IsActive = sr.Service.IsActive , IsDeleted = sr.Service.IsDeleted, IsOnRide = sr.Service.IsOnRide, Quantity = sr.Service.Quantity, Rate = sr.Service.Rate, RiderPerVessel = sr.Service.RiderPerVessel, Title = sr.Service.Title, City = sr.Service.City, StreetAddress = sr.Service.StreetAddress, PostalCode = sr.Service.PostalCode }));

            CreateMap<ServiceProvider, ServiceProviderResource>()
                .ForMember(spr => spr.ApplicationUser, opt => opt.MapFrom(sp => new ApplicationUser { Id = sp.ApplicationUser.Id, Email = sp.ApplicationUser.Email, FirstName = sp.ApplicationUser.FirstName, LastName = sp.ApplicationUser.LastName, Country = sp.ApplicationUser.Country, Address = sp.ApplicationUser.Address, State = sp.ApplicationUser.State, PhoneNumber = sp.ApplicationUser.PhoneNumber }));
            CreateMap<ServiceType, ServiceTypeResource>();

            CreateMap<ServiceProviderType, ServiceProviderTypeResource>()
                .ForMember(sp => sp.ServiceProvider, opt => opt.MapFrom(s => new ServiceProviderResource { Id = s.ServiceProviders.Id, Name = s.ServiceProviders.Name, Information = s.ServiceProviders.Information, Information2 = s.ServiceProviders.Information2 }))
                .ForMember(sp => sp.ServiceCategory, opt => opt.MapFrom(s => new ServiceCategoryResource { Id = s.ServiceCategory.Id, Name = s.ServiceCategory.Name }));

            CreateMap<DefaultCalendar, DefaultCalendarResource>()
                .ForMember(dc => dc.ServiceProvider, opt => opt.MapFrom(s => new ServiceProviderResource { Id = s.ServiceProvider.Id, Name = s.ServiceProvider.Name, Information = s.ServiceProvider.Information, Information2 = s.ServiceProvider.Information2 }));
            CreateMap<ServiceReview, ServiceReviewResource>()
                .ForMember(sr => sr.Service, opt => opt.MapFrom(s => new ServicesResource { Id = s.Service.Id, IsActive = s.Service.IsActive, IsDeleted = s.Service.IsDeleted, Quantity = s.Service.Quantity, RiderPerVessel = s.Service.RiderPerVessel, IsOnRide = s.Service.IsOnRide, City = s.Service.City, StreetAddress = s.Service.StreetAddress, PostalCode = s.Service.PostalCode }));
            CreateMap<ClientReviews, ClientReviewsRerource>()
                .ForMember(cr => cr.ServiceProvider, opt => opt.MapFrom(s => new ServiceProviderResource { Id = s.ServiceProvider.Id, Name = s.ServiceProvider.Name, Information = s.ServiceProvider.Information, Information2 = s.ServiceProvider.Information2 }));

           
            CreateMap<ServiceLocation, ServiceLocationResource>()
                .ForMember(sl => sl.Service, opt => opt.MapFrom(slr => new ServicesResource { Id = slr.Service.Id, IsActive = slr.Service.IsActive, IsDeleted = slr.Service.IsDeleted, Quantity = slr.Service.Quantity, RiderPerVessel = slr.Service.RiderPerVessel, Title = slr.Service.Title, Rate = slr.Service.Rate, IsOnRide = slr.Service.IsOnRide,  City = slr.Service.City, StreetAddress = slr.Service.StreetAddress, PostalCode = slr.Service.PostalCode }));
            CreateMap<ServiceServiceTypes, ServiceServiceTypeResource>()
                .ForMember(sst => sst.Service, opt => opt.MapFrom(s => new ServicesResource { Id = s.Service.Id, IsActive = s.Service.IsActive, IsDeleted = s.Service.IsDeleted, Quantity = s.Service.Quantity, RiderPerVessel = s.Service.RiderPerVessel, Rate = s.Service.Rate, Title = s.Service.Title, IsOnRide = s.Service.IsOnRide, City = s.Service.City, StreetAddress = s.Service.StreetAddress, PostalCode = s.Service.PostalCode }))
                .ForMember(sst => sst.ServiceType, opt => opt.MapFrom(st => new ServiceTypeResource { Id = st.ServiceType.Id, Name = st.ServiceType.Name }))
                .ForMember(sst => sst.ServiceCategory, opt => opt.MapFrom(sc => new ServiceCategoryResource { Id = sc.Id, Name = sc.ServiceCategories.Name,  ServiceTypeId = sc.ServiceCategories.ServiceTypeId}));

            CreateMap<ServicePhoto, ServicePhotoResource>();

            CreateMap<States, StatesResources>();
            CreateMap<UserDeviceInformations,SaveUserDeviceInformationResources>();
            CreateMap<DistressedUsers, DistressedUsersResource>();

            //API Resource to Domain

            CreateMap<SaveServiceProviderResource, ServiceProvider>()
                .ForMember(sp => sp.Id, opt => opt.Ignore());

            CreateMap<SaveServiceTypeResource, ServiceType>()
                .ForMember(st => st.Id, opt => opt.Ignore());
            CreateMap<SaveServiceProviderTypeResource, ServiceProviderType>()
                .ForMember(spt => spt.Id, opt => opt.Ignore());

            CreateMap<SaveInactiveDaysResource, InactiveDay>()
                .ForMember(id => id.Id, opt => opt.Ignore());
            CreateMap<SaveDefaultCalendarResource, DefaultCalendar>()
                 .ForMember(sd => sd.Id, opt => opt.Ignore());
            CreateMap<SaveServiceReviewResource, ServiceReview>()
                .ForMember(s => s.Id, opt => opt.Ignore());
            
            CreateMap<SaveServiceLocationResource, ServiceLocation>()
                .ForMember(ssl => ssl.Id, opt => opt.Ignore());

            CreateMap<SaveServiceServiceTypeResource, ServiceServiceTypes>()
                .ForMember(sst => sst.Id, opt => opt.Ignore());
            CreateMap<SaveServiceCategoryResource, ServiceCategory>()
                .ForMember(s => s.Id, opt => opt.Ignore());
            CreateMap<SaveServicesResource, Service>()
                .ForMember(s => s.Id, opt => opt.Ignore());
            CreateMap<SaveServiceRequestResource, ServiceRequest>()
                .ForMember(s => s.Id, opt => opt.Ignore());

            CreateMap<SaveClientReviewsResource, ClientReviews>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<ServicePhotoResource, ServicePhoto>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<SaveUserDeviceInformationResources, UserDeviceInformations>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<SaveDistressedUsersResource, DistressedUsers>()
                .ForMember(x => x.Id, opt => opt.Ignore());

        }
    }
}
