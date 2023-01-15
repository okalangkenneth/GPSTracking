using GPSTracking.Api.GPSTrackings.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.GPSTrackings.Controllers
{

    [ApiController]
    [Route("api/gPSTrackings")]
    public class GPSTrackingsController : ControllerBase
    {
        private readonly IGPSTrackingsProvider gPSTrackingsProvider;

        public GPSTrackingsController( IGPSTrackingsProvider gPSTrackingsProvider)
        {
            this.gPSTrackingsProvider = gPSTrackingsProvider;

        }

        [HttpGet]
        public async Task<IActionResult> GetGPSTrackingsAsync()
        {
            var result = await gPSTrackingsProvider.GetGPSTruckingsAsync();
            if (result.IsSuccess)
            {
                return Ok(result.GPSTrackings);
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGPSTrackingAsync(int id)
        {
            var result = await gPSTrackingsProvider.GetGPSTruckingAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.GPSTracking);
            }
            return NotFound();
        }



    }
}
