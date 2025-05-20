using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Events;

namespace TaskManagement.Logger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void LogEvent([FromBody] LogEvent logEvent)
        {
            _logger.LogInformation($"��������� ������� {logEvent.Event} ���������� � {logEvent.OldValue?.ToString()} �� {logEvent.NewValue?.ToString()}");
        }
    }
}
