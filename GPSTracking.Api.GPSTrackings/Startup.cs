using AutoMapper;
using GPSTracking.Api.GPSTrackings.Db;
using GPSTracking.Api.GPSTrackings.Interfaces;
using GPSTracking.Api.GPSTrackings.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GPSTracking.Api.GPSTrackings
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

            services.AddScoped<IGPSTrackingsProvider, GPSTrackingsProvider>();
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<GPSTrackingsDbContext>(options =>

            {
              options.UseInMemoryDatabase("GPSTrackings");
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
