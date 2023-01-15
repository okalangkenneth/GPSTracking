using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Drivers.Db
{
    public class DriversDbContext : DbContext
    {
        public DriversDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Driver> Drivers { get; set; }
    }
}
