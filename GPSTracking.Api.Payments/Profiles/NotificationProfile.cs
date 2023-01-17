using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Notifications.Profiles
{
    public class NotificationProfile : AutoMapper.Profile
    {
        public NotificationProfile()
        {
            CreateMap<Db.Notification, Models.Notification>();
            CreateMap<Db.NotificationType, Models.NotificationType>();
        }
    }
}
