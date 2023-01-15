using GPSTracking.Api.Search.Interfaces;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly INotificationsService notificationsService;
        private readonly IDriversService driversService;
        private readonly IGPSTrackingsService gPSTrackingsService;

        public SearchService(INotificationsService notificationsService,IGPSTrackingsService gPSTrakingsService, IDriversService driversService)
        {
            this.notificationsService = notificationsService;
            this.driversService = driversService;
            this.gPSTrackingsService = gPSTrackingsService;
        }


        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int driverId)
        {
            var driversResult = await driversService.GetDriverAsync(driverId);
            var notificationsResult = await notificationsService.GetNotificationsAsync(driverId);
            var gPSTrackingResult = await gPSTrackingsService.GetGPSTrackingsAsync();

            if (notificationsResult.IsSuccess )
{
                foreach (var notifications in notificationsResult.Notifications)
                {
                   // notification.GPSTracking = gPSTrackingResult.GPSTrackings.FirstOrDefault(p => p.Id == notification.GPSTrackingId)?.Vehicle;
                }

                var result = new
                {
                    Driver = driversResult.IsSuccess ?
                             driversResult.Driver :
                             new { Name = "Driver information is not available" },
                    Notifications = notificationsResult.Notifications

                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
