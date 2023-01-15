using GPSTracking.Api.Drivers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GPSTracking.Api.Drivers.Controllers
{

    [ApiController]
    [Route("api/drivers")]
    public class DriversController : ControllerBase
    {

        private readonly IDriversProvider driversProvider;

        public DriversController(IDriversProvider driversProvider)
        {
            this.driversProvider = driversProvider;

        }

        [HttpGet]
        public async Task<IActionResult> GetDriversAsync()
        {
            var result = await driversProvider.GetDriversAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Drivers);
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverAsync(int id)
        {
            var result = await driversProvider.GetDriverAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Driver);
            }
            return NotFound();
        }
    }
}
