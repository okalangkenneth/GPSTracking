using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.GPSTrackings.GPSTrackingProfile
{
    public class GPSTracking : AutoMapper.Profile
    {
        public GPSTracking()
        {
            CreateMap<Db.GPSTrackingV, Models.GPSTrackingV>();
        }
    }
}
