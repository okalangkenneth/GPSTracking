using GPSTracking.Api.Drivers.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Drivers.Interfaces
{
    public interface IDriversProvider
    {
        Task<(bool IsSuccess, IEnumerable<Models.Driver>Drivers, string ErrorMessage)> GetDriversAsync();
        Task<(bool IsSuccess, Models.Driver Driver, string ErrorMessage)> GetDriverAsync(int id);
    }
}
