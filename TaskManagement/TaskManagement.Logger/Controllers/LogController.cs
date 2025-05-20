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
            _logger.LogInformation($"Произошло событие {logEvent.Event} изменилось с {logEvent.OldValue?.ToString()} до {logEvent.NewValue?.ToString()}");
        }
    }
}
