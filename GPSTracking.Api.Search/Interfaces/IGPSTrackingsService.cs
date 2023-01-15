using GPSTracking.Api.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Interfaces
{
    public interface IGPSTrackingsService
    {

        Task<(bool IsSuccess, IEnumerable<GPSTrackingV> GPSTrackings, string ErrorMessage)> GetGPSTrackingsAsync();
    }
}
