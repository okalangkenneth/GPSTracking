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
            dbContext.Notifications.Add(new Db. Notification()
            {
                Id = 1,
                Type = "Geofencing",
                Message = "Vehicle entered geofence area",
                Recipient = "driver@email.com",
                CreatedDate = DateTime.Now,
                DeliveryDate = DateTime.Now,
                IsRead = false,
                GPSTrackingId = 1,
                IsActive = true
            });
            dbContext.Notifications.Add(new Db.Notification()
            {
                Id = 2,
                Type = "Speed Limit",
                Message = "Vehicle exceeded speed limit",
                Recipient = "admin@email.com",
                CreatedDate = DateTime.Now,
                DeliveryDate = DateTime.Now,
                IsRead = false,
                GPSTrackingId = 2,
                IsActive = true
            });
            dbContext.Notifications.Add(new Db. Notification()
            {
                Id = 3,
                Type = "Maintenance",
                Message = "Vehicle needs maintenance",
                Recipient = "driver@email.com",
                CreatedDate = DateTime.Now,
                DeliveryDate = DateTime.Now,
                IsRead = false,
                GPSTrackingId = 3,
                IsActive = true
            });
            dbContext.SaveChanges();
        }

        public async Task<(bool IsSuccess, Models.Notification Notification, string ErrorMessage)> GetNotificationAsync(int id)
        {
            try
            {
                logger?.LogInformation("Querying notifications");
                var notification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == id);

                if (notification != null)
                {
                    logger?.LogInformation("Notification found");
                    var result = mapper.Map<Db.Notification, Models.Notification>(notification);
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

        public async  Task<(bool IsSuccess, IEnumerable<Models.Notification> Notifications,string ErrorMessage)> GetNotificationsAsync()
        {
            try
            {
                logger?.LogInformation("Querying notifications");
                var notifications = await dbContext.Notifications.ToListAsync();
                if (notifications != null && notifications.Any())
                {
                    logger?.LogInformation($"{notifications.Count} driver(s) found");
                    var result = mapper.Map<IEnumerable<Db.Notification>, IEnumerable<Models.Notification>>(notifications);
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