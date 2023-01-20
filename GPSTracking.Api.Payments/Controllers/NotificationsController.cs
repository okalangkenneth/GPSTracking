using GPSTracking.Api.Notifications.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GPSTracking.Api.Notifications.Controllers
{

    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsProvider notificationsProvider;

        public NotificationsController(INotificationsProvider notificationsProvider)
        {
            this.notificationsProvider = notificationsProvider;

        }

        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetNotificationsAsync(int driverId)
        {
            var result = await notificationsProvider.GetNotificationsAsync(driverId);
            if (result.IsSuccess)
            {
                return Ok(result.Notifications);
            }
            return NotFound();
        }
        
    }
}
