using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Recipient { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<NotificationType> Notifications { get; set; }

    }
}
