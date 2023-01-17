using AutoMapper;
using GPSTracking.Api.GPSTrackings.Db;
using GPSTracking.Api.GPSTrackings.Interfaces;
using GPSTracking.Api.GPSTrackings.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.GPSTrackings.Providers
{
    public class GPSTrackingsProvider : IGPSTrackingsProvider
    {
        private readonly GPSTrackingsDbContext dbContext;
        private readonly ILogger<GPSTrackingsProvider> logger;
        private readonly IMapper mapper;

        public GPSTrackingsProvider(GPSTrackingsDbContext dbContext,ILogger<GPSTrackingsProvider> logger,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.GPSTrackings.Any())
            {
                dbContext.GPSTrackings.Add(new Db.GPSTrackingV() { Id = 1, Vehicle = "Benz", Latitude = 37.788022, Longitude = -122.399797, Speed = 67, TimeStamp = DateTime.Now });
                dbContext.GPSTrackings.Add(new Db.GPSTrackingV() { Id = 2, Vehicle = "Toyota", Latitude = 60.497872, Longitude = -12.3546, Speed = 0, TimeStamp = DateTime.Now });
                dbContext.GPSTrackings.Add(new Db.GPSTrackingV() { Id = 3, Vehicle = "Lexus", Latitude = 3.578686, Longitude = -45.5768, Speed = 5, TimeStamp = DateTime.Now });
                dbContext.GPSTrackings.Add(new Db.GPSTrackingV() { Id = 4, Vehicle = "Opel", Latitude = 10.67855, Longitude = -1.399797, Speed = 50, TimeStamp = DateTime.Now });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.GPSTrackingV> GPSTrackings,
            string ErrorMessage)> GetGPSTrackingsAsync()
        {
            try
            {
                var gpsTrackings = await dbContext.GPSTrackings.ToListAsync();
                    if(gpsTrackings != null && gpsTrackings.Any())
                {
                   var result = mapper.Map<IEnumerable<Db.GPSTrackingV>, IEnumerable<Models.GPSTrackingV>>(gpsTrackings);
                    return (true, result, null);
                }
                return (false, null, "Not found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return(false,null,ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.GPSTrackingV GPSTracking, string ErrorMessage)> GetGPSTrackingAsync(int id)
        {
            try
            {
                var gpsTracking = await dbContext.GPSTrackings.FirstOrDefaultAsync(p => p.Id == id);

                if (gpsTracking != null)
                {
                    var result = mapper.Map<Db.GPSTrackingV, Models.GPSTrackingV>(gpsTracking);
                    return (true, result, null);
                }
                return (false, null, "Not found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
