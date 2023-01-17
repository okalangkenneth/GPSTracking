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
                var notifications = new List<Notification>
        {
            new Notification { Id = 1, DriverId = 1,Type = 1, Message = "Vehicle is low on fuel", Recipient = "driver1@gmail.com", CreatedDate = DateTime.Now, DeliveryDate = DateTime.Now.AddDays(1), IsRead = false, GPSTrackingId = 1, IsActive = true },
            new Notification { Id = 2, DriverId = 2,Type = 2, Message = "Vehicle has exceeded speed limit", Recipient = "driver2@gmail.com", CreatedDate = DateTime.Now, DeliveryDate = DateTime.Now.AddDays(2), IsRead = false, GPSTrackingId = 2, IsActive = true },
            new Notification { Id = 3, DriverId = 3,Type = 3, Message = "Vehicle needs maintenance", Recipient = "driver3@gmail.com", CreatedDate = DateTime.Now, DeliveryDate = DateTime.Now.AddDays(3), IsRead = false, GPSTrackingId = 3, IsActive = true },
        };

                dbContext.Notifications.AddRange(notifications);
                dbContext.SaveChanges();
            }
        }

        //public async Task<(bool IsSuccess, Models.Notification Notification, string ErrorMessage)> GetNotificationAsync(int id)
        //{
        //    try
        //    {
        //        logger?.LogInformation("Querying notifications");
        //        var notification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == id);

        //        if (notification != null)
        //        {
        //            logger?.LogInformation("Notification found");
        //            var result = mapper.Map<Db.Notification, Models.Notification>(notification);
        //            return (true, result, null);
        //        }
        //        return (false, null, "Not found");

        //    }
        //    catch (Exception ex)
        //    {
        //        logger?.LogError(ex.ToString());
        //        return (false, null, ex.Message);
        //    }
        //}

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