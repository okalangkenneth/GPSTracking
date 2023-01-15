using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Drivers.Db
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseExpiration { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
