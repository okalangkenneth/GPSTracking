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
    public class NotificationsService : INotificationsService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<NotificationsService> logger;

        public NotificationsService(IHttpClientFactory httpClientFactory, ILogger<NotificationsService>logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }


        public async Task<(bool IsSuccess, IEnumerable<Notification> Notifications, string ErrorMessage)> GetNotificationsAsync(int driverId)
        {

            try
            {
                var client = httpClientFactory.CreateClient(" NotificationsService ");
                var response = await client.GetAsync( $"api/notifications/{driverId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<IEnumerable<Notification>>(content,options);
                    return (false, result,null);
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
