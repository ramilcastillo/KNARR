using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.Notification.FireBase;
using Knarr.Core;
using Knarr.Core.Models;
using Knarr.Core.Models.AppSettings;
using Knarr.Extensions;
using Knarr.Persistence;
using Knarr.ServiceClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Knarr
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PhotoAppSettings>(Configuration.GetSection("Photo"));
            services.Configure<BraintreeAppSettings>(Configuration.GetSection("Braintree"));
            services.Configure<AwsAppSettings>(Configuration.GetSection("Aws"));

            services.AddScoped<IBraintreeConfiguration, BraintreeConfiguration>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
            services.AddScoped<IInactiveDaysRepository, InactiveDaysRepository>();
            services.AddScoped<IServiceProviderTypeRepository, ServiceProviderTypeRepository>();
            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
            services.AddScoped<IServicePhotoRepository, ServicePhotoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
            services.AddScoped<IServiceReviewRepository, ServiceReviewRepository>();
            services.AddScoped<IDefaultCalendarRepository, DefaultCalendarRepository>();
            services.AddScoped<IServiceReviewRepository, ServiceReviewRepository>();
            services.AddScoped<IClientReviewRepository, ClientReviewRepository>();
            services.AddScoped<IServiceLocationRepository, ServiceLocationRepository>();
            services.AddScoped<IServiceServiceTypeRepository, ServiceServiceTypeRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IUserDeviceInformationRepository, UserDeviceInformationRepository>();
            services.AddScoped<IAwsServiceClient, AwsServiceClient>();
            services.AddScoped<IFireBaseServiceClient, FireBaseServiceClient>();
            services.AddScoped<IDistressedUsersRepository, DistressedUsersRepository>();

            services.AddAutoMapper();

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 4;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<KnarrDbContext>().AddDefaultTokenProviders();


            services.AddDbContext<KnarrDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["JwtIssuer"],
                    ValidAudience = Configuration["JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };
            });

            services.AddMvc().AddJsonOptions(option =>
                {
                    option.SerializerSettings.ContractResolver =
                        new DefaultContractResolver();

                    option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();

            CreateRoles(serviceProvider).Wait();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Plane API");
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding customs roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = {"Admin", "Captain", "Passenger", "Company"};

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}