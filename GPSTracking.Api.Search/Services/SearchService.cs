using GPSTracking.Api.Search.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly INotificationsService notificationsService;
        private readonly IDriversService driversService;
        private readonly IGPSTrackingsService gPSTrackingsService;

        public SearchService(INotificationsService notificationsService,
            IGPSTrackingsService gPSTrakingsService, IDriversService driversService)
        {
            this.notificationsService = notificationsService;
            this.gPSTrackingsService = gPSTrackingsService;
            this.driversService = driversService;
            
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
                    foreach (var notification in notifications.Notifications)
                    {
                        notification.Name = gPSTrackingResult.IsSuccess ?
                           gPSTrackingResult.GPSTrackings.FirstOrDefault(p => p.Id == notification.GPSTrackingId)?.Vehicle :
                           "GPSTracking information is not available";
                    }

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
