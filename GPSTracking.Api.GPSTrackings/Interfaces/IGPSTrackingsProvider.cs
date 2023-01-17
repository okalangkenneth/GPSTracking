using GPSTracking.Api.GPSTrackings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.GPSTrackings.Interfaces
{
    public interface IGPSTrackingsProvider
    {
        Task<(bool IsSuccess, IEnumerable<GPSTrackingV> GPSTrackings, string ErrorMessage)> GetGPSTrackingsAsync();
        Task<(bool IsSuccess, GPSTrackingV GPSTracking, string ErrorMessage)> GetGPSTrackingAsync(int id);
    }
}
