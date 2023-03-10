using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Notifications.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int DriverId { get; set;}
        public int Type { get; set; }
        public int GPSTrackingId { get; set; }
        public string Message { get; set; }
        public string Recipient { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsActive { get; set; }
        public List<NotificationType> Notifications { get; set; }
    }
}
