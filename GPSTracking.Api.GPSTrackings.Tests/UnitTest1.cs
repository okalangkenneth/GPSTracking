using GPSTracking.Api.GPSTrackings.Db;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace GPSTracking.Api.GPSTrackings.Tests
{
    public class GPSTrafickingsServiceTest
    {
        [Fact]
        public void GetGPSTrakingsReturnsAllGPSTrakings()
        {
            var gPSTrackingsPovider = new DbContextOptionsBuilder<GPSTrackingsDbContext>();

        }
    }
}
