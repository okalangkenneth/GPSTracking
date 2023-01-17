using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Notifications.Db
{
    public class NotificationType
    {
        public int Id { get; set; }
        public int NotificationId { get; set;}
        public string Name { get; set; }
        public int GPSTrackingId { get; set; }
        public string Description { get; set; }
    }
}
