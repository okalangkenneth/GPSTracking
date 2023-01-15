using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.GPSTrackings.Db
{
    public class GPSTrackingsDbContext : DbContext
    {
        public GPSTrackingsDbContext(DbContextOptions options) : base(options) 
        {

        }
        public DbSet<GPSTrackingV> GPSTrackings { get; set; }

        

    }
}
