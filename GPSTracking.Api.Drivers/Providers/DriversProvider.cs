using AutoMapper;
using GPSTracking.Api.Drivers.Db;
using GPSTracking.Api.Drivers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Drivers.Providers
{
    public class DriversProvider : IDriversProvider
    {
        private readonly DriversDbContext dbContext;
        private readonly ILogger<DriversProvider> logger;
        private readonly IMapper mapper;

        public DriversProvider(DriversDbContext dbContext, ILogger<DriversProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()

        {
            if (!dbContext.Drivers.Any())
            {
                dbContext.Drivers.Add(new Db.Driver() { Id = 1, Name = "Bengt", LicenseNumber = "123456",LicenseExpiration= DateTime.Now.AddYears(1), PhoneNumber="555-555-5555", Email = "john.smith@email.com" });
                dbContext.Drivers.Add(new Db.Driver() { Id = 2, Name = "John", LicenseNumber = "475634", LicenseExpiration = DateTime.Now.AddYears(1), PhoneNumber = "378888686", Email = "ken.doe@email.com" });
                dbContext.Drivers.Add(new Db.Driver() { Id = 3, Name = "Peter", LicenseNumber = "487411", LicenseExpiration = DateTime.Now.AddYears(1), PhoneNumber = "89875111", Email = "peter.smith@email.com" });
                dbContext.Drivers.Add(new Db.Driver() { Id = 4, Name = "Kacey", LicenseNumber = "687537", LicenseExpiration = DateTime.Now.AddYears(1), PhoneNumber = "59000000", Email = "john.pius@email.com" });

                dbContext.SaveChanges();
            }


        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Driver> Drivers, string ErrorMessage)> GetDriversAsync()
        {
            try
            {
                logger?.LogInformation("Querying drivers");
                var drivers = await dbContext.Drivers.ToListAsync();
                if (drivers != null && drivers.Any())
                {
                    logger?.LogInformation($"{drivers.Count} driver(s) found");
                    var result = mapper.Map<IEnumerable<Db.Driver>, IEnumerable<Models.Driver>>(drivers);
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

        public async Task<(bool IsSuccess, Models.Driver Driver, string ErrorMessage)> GetDriverAsync(int id)
        {
            try
            {
                logger?.LogInformation("Querying drivers");
                var driver = await dbContext.Drivers.FirstOrDefaultAsync(d => d.Id == id);

                if (driver != null)
                {
                    logger?.LogInformation("Driver found");
                    var result = mapper.Map<Db.Driver, Models.Driver>(driver);
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
