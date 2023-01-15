using GPSTracking.Api.Search.Interfaces;
using GPSTracking.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Services
{




    public class GPSTrackingsService : IGPSTrackingsService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<GPSTrackingsService> logger;

        public GPSTrackingsService(IHttpClientFactory httpClientFactory, ILogger<GPSTrackingsService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }


        public async Task<(bool IsSuccess, IEnumerable<GPSTrackingV> GPSTrackings, string ErrorMessage)> GetGPSTrackingsAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient(" GPSTrackinsService ");
                var response = await client.GetAsync("api/gPSTrackings");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<IEnumerable<GPSTrackingV>>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);


            }
            catch (Exception ex)
            {

                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
