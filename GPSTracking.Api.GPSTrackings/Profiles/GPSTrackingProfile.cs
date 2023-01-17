using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.GPSTrackings.Profiles
{
    public class GPSTrackingProfile : AutoMapper.Profile
    {
        public GPSTrackingProfile()
        {
            CreateMap<Db.GPSTrackingV, Models.GPSTrackingV>();
        }
    }
}
