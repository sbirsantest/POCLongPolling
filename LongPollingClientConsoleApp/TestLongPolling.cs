using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LongPollingClientConsoleApp
{
    public class TestLongPolling : ITestLongPolling
    {
        private const string WebApiBaseUrl = "https://localhost:5001";
    
        private readonly ILogger<TestLongPolling> _logger;
        private readonly HttpClient _httpClient;

        public TestLongPolling(ILogger<TestLongPolling> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> GetStatusAsync(string orderNumber)
        {
            _logger.LogInformation($"Trying to get status of order with orderNumber = {orderNumber}");

            if (_httpClient == null) throw new ArgumentNullException(nameof(_httpClient));

            try
            {
                var response = await _httpClient.GetAsync($"{WebApiBaseUrl}/api/LongPolling?orderNumber={WebUtility.UrlEncode(orderNumber)}");
                response.EnsureSuccessStatusCode();

                _logger.LogInformation("Successfully get order status!");

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error!");
            }

            return string.Empty;
        }
    }
}
