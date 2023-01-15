using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.GPSTracking.Db
{
    public class GPSTrackingContext : DbContext
    {
        public GPSTrackingContext(DbContextOptions<GPSTrackingContext> options) : base(options) { }

        public DbSet<GPSTracking> GPSTrackings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GPSTracking>().ToTable("GPSTracking");
        }
    }
}
