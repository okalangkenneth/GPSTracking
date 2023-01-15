using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Drivers.Profiles
{
    public class DriverProfile :AutoMapper.Profile
    {
        public DriverProfile()
        {
            CreateMap<Db.Driver, Models.Driver>();
        }
    }
}
