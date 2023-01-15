using GPSTracking.Api.Search.Interfaces;
using GPSTracking.Api.Search.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search
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

            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<INotificationsService, NotificationsService>();
            services.AddScoped<IDriversService, DriversService>();
            services.AddScoped<IGPSTrackingsService, GPSTrackingsService>();

            services.AddHttpClient("NotificationsService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Notifications"]) ;

            });
            services.AddHttpClient("GPSTrackingsService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:GPSTrackings"]);

            });
            services.AddHttpClient("DriversService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Drivers"]);

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
