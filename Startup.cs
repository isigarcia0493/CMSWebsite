using CMSWebsite.Data;
using CMSWebsite.Helpers;
using CMSWebsite.Models;
using CMSWebsite.Repositories;
using CMSWebsite.RepositoriesInterfaces;
using CMSWebsite.RepositoryInterfaces;
using CMSWebsite.ServiceInterfaces;
using CMSWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CMSWebsite
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
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddControllersWithViews();

            //Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICarouselRepository, CarouselRepository>();
            services.AddScoped<IFormMessageRepository, FormMessageRepository>();
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IEventRegistrationRepository, EventRegistrationRepository>();

            //Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICarouselService, CarouselService>();
            services.AddScoped<IFormMessageService, FormMessageService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IEventRegistrationService, EventRegistrationService>();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "AdminArea",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
