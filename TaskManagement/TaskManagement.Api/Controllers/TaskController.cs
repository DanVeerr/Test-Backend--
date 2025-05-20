using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Models;
using TaskManagement.Application.Service;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<TaskDto?> GetAsync([FromRoute] Guid id, CancellationToken ct = default)
        {
            return await _taskService.GetAsync(id, ct);
        }

        [HttpGet]
        public async Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _taskService.GetAllAsync(ct);
        }

        [HttpPost]
        public async Task<TaskDto> CreateAsync([FromBody] TaskCreateCommand taskCreateCommand, CancellationToken ct = default)
        {
            return await _taskService.CreateAsync(taskCreateCommand, ct);
        }

        [HttpPut]
        public async Task<TaskDto?> UpdateAsync([FromBody] TaskUpdateCommand taskUpdateCommand, CancellationToken ct = default)
        {
            return await _taskService.UpdateAsync(taskUpdateCommand, ct);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync([FromRoute] Guid id, CancellationToken ct = default)
        {
            return await _taskService.DeleteAsync(id, ct);
        }
    }
}
