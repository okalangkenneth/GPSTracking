using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Interfaces
{
    public interface ISearchService
    {
        Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int driverId);
    }
}
