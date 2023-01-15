using GPSTracking.Api.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Interfaces
{
    public interface INotificationsService
    {
        Task<(bool IsSuccess, IEnumerable<Notification> Notifications, string ErrorMessage)> GetNotificationsAsync(int driverId);
    }
}
