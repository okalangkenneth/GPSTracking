using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Notifications.Interfaces
{
    public interface INotificationsProvider
    {

        Task<(bool IsSuccess, IEnumerable<Models.Notification> Notifications, string ErrorMessage)> GetNotificationsAsync();
        Task<(bool IsSuccess, Models.Notification Notification, string ErrorMessage)> GetNotificationAsync(int id);
    }
}
