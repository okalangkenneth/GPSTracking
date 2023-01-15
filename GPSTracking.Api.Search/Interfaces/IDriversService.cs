using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Interfaces
{
    public interface IDriversService
    {
        Task<(bool IsSuccess, dynamic Driver, string ErrorMessage)> GetDriverAsync(int id);
    }
}
