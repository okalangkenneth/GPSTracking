using GPSTracking.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GPSTracking.Api.Search.Services
{
    public class DriversService : IDriversService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<DriversService> logger;

        public DriversService(IHttpClientFactory httpClientFactory, ILogger<DriversService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<(bool IsSuccess, dynamic Driver, string ErrorMessage)> GetDriverAsync(int id)
        {

            try
            {
                var client = httpClientFactory.CreateClient("DriversService");
                var response = await client.GetAsync($"api/drivers/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<dynamic>(content, options);
                    return (false, result, null);
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
