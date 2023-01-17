using AutoMapper;
using GPSTracking.Api.GPSTrackings.Db;
using GPSTracking.Api.GPSTrackings.Profiles;
using GPSTracking.Api.GPSTrackings.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GPSTracking.Api.GPSTrackings.Tests
{
    public class GPSTrafickingsServiceTest
    {
        [Fact]
        public async Task GetGPSTrackingsReturnsAllGPSTrackings()
        {
            var options = new DbContextOptionsBuilder<GPSTrackingsDbContext>()
                 .UseInMemoryDatabase(nameof(GetGPSTrackingsReturnsAllGPSTrackings))
                 .Options;
            var dbContext = new GPSTrackingsDbContext(options);
            CreateGPSTrackings(dbContext);

            var gPSTrackingProfile = new GPSTrackingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(gPSTrackingProfile));
            var mapper = new Mapper(configuration);

            var gPSTrackingsProvider = new GPSTrackingsProvider(dbContext, null, mapper);

            var gPSTracking = await gPSTrackingsProvider.GetGPSTrackingsAsync();
            Assert.True(gPSTracking.IsSuccess);
            Assert.True(gPSTracking.GPSTrackings.Any());
            Assert.Null(gPSTracking.ErrorMessage);

        }
        [Fact]
        public async Task GetGPSTrakingsReturnsGPSTrackingsUsingValidIdAsync()
        {
            var options = new DbContextOptionsBuilder<GPSTrackingsDbContext>()
                 .UseInMemoryDatabase(nameof(GetGPSTrakingsReturnsGPSTrackingsUsingValidIdAsync))
                 .Options;
            var dbContext = new GPSTrackingsDbContext(options);
            CreateGPSTrackings(dbContext);

            var gPSTrackingProfile = new GPSTrackingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(gPSTrackingProfile));
            var mapper = new Mapper(configuration);

            var gPSTrackingsProvider = new GPSTrackingsProvider(dbContext, null, mapper);

            var gPSTracking = await gPSTrackingsProvider.GetGPSTrackingAsync(1);
            Assert.True(gPSTracking.IsSuccess);
            Assert.NotNull(gPSTracking.GPSTracking);
            Assert.True(gPSTracking.GPSTracking.Id == 1);
            Assert.Null(gPSTracking.ErrorMessage);

        }
        [Fact]
        public async Task GetGPSTrackingsReturnsGPSTrackingsUsingInvalid()
        {
            var options = new DbContextOptionsBuilder<GPSTrackingsDbContext>()
                 .UseInMemoryDatabase(nameof(GetGPSTrackingsReturnsGPSTrackingsUsingInvalid))
                 .Options;
            var dbContext = new GPSTrackingsDbContext(options);
            CreateGPSTrackings(dbContext);

            var gPSTrackingProfile = new GPSTrackingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(gPSTrackingProfile));
            var mapper = new Mapper(configuration);

            var gPSTrackingsProvider = new GPSTrackingsProvider(dbContext, null, mapper);

            var gPSTracking = await gPSTrackingsProvider.GetGPSTrackingAsync(-1);
            Assert.False(gPSTracking.IsSuccess);
            Assert.Null(gPSTracking.GPSTracking);
            Assert.NotNull(gPSTracking.ErrorMessage);

        }
      
        private void CreateGPSTrackings(GPSTrackingsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.GPSTrackings.Add(new GPSTrackingV()
                {
                    Id = i,
                    Vehicle = Guid.NewGuid().ToString(),
                    Latitude = i + 10,
                    Longitude = i + 10,
                    Speed = i + 10,
                    TimeStamp = DateTime.Now

                });
            }
            dbContext.SaveChanges();

        }
    }
}
