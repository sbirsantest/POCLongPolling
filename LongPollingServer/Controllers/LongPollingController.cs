using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LongPollingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LongPollingController : ControllerBase
    {
        private readonly ILogger<LongPollingController> _logger;

        public LongPollingController(ILogger<LongPollingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteLongPolling([FromQuery, Required] string orderNumber)
        {
            var longPolling = new LongPolling(orderNumber);
            var orderStatus = await longPolling.GetStatusAsync();
            return new ObjectResult(new { OrderStatus = (orderStatus != null ? orderStatus : "Long polling timeout!") });
        }

        [HttpPost]
        public void SimulateStateChange([FromQuery, Required] string orderNumber, string status)
        {
            LongPolling.SetStatus(orderNumber, status);
        }
    }
}
