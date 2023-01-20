using AutoMapper;
using GPSTracking.Api.Notifications.Db;
using GPSTracking.Api.Notifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Notifications.Providers
{
    public class NotificationsProvider : INotificationsProvider
    {
        private readonly NotificationsDbContext dbContext;
        private readonly ILogger<NotificationsProvider> logger;
        private readonly IMapper mapper;

        public NotificationsProvider(NotificationsDbContext dbContext, ILogger<NotificationsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();

        }

        private void SeedData()
        {
            if (!dbContext.Notifications.Any())
            {
                dbContext.Notifications.AddRange(new Notification()
                {
                    Id = 1,
                    DriverId = 1,
                    Message = "Vehicle has exceeded speed limit",
                    CreatedDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddMinutes(5),
                    IsRead = false,
                    IsActive = true,
                    Notifications = new List<NotificationType>
            {
                new NotificationType()
                {
                    Id = 1,Name = "Speed Limit",GPSTrackingId = 1,Description = "Vehicle has exceeded the speed limit"}
            }
                },
                new Notification()
                {
                    Id = 2,
                    DriverId = 2,
                    Message = "Vehicle has entered a restricted area",
                    CreatedDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddMinutes(5),
                    IsRead = false,
                    IsActive = true,
                    Notifications = new List<NotificationType>
                    {
                new NotificationType()
                {
                    Id = 2,Name = "Restricted Area",GPSTrackingId = 2,Description = "Vehicle has entered a restricted area"
                }
                    }
                });

                dbContext.SaveChanges();
            }
        }


        public async  Task<(bool IsSuccess, IEnumerable<Models.Notification> Notifications,string ErrorMessage)> GetNotificationsAsync(int driverId)
        {
            try
            {

                var notifications = await dbContext.Notifications
                    .Where(o => o.DriverId == driverId)
                    .Include(o => o.Notifications)
                    .ToListAsync();
                if (notifications != null && notifications.Any())
                {
                    
                    var result = mapper.Map<IEnumerable<Db.Notification>,
                        IEnumerable<Models.Notification>>(notifications);
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