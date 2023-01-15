using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.GPSTrackings.Db
{
    public class GPSTrackingV
    {
        public int Id { get; set; }
        public string Vehicle { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
