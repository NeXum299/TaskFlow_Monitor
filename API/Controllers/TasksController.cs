using Microsoft.AspNetCore.Mvc;
using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Domain.Interfaces.Services;
using TaskFlow_Monitor.API.DTO;
using TaskFlow_Monitor.API.Interfaces.Metrics;

namespace TaskFlow_Monitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ICustomMetrics _metrics;
        private readonly ITasksService _tasksService;
        private readonly ITaskHistoriesService _taskHistoriesService;
        private readonly IUsersService _usersService;

        public TasksController(
            ITasksService tasksService,
            ITaskHistoriesService taskHistoriesService,
            IUsersService usersService,
            ICustomMetrics metrics)
        {
            _tasksService = tasksService;
            _taskHistoriesService = taskHistoriesService;
            _usersService = usersService;
            _metrics = metrics;
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Add(TaskRequest request)
        {
            var userCheck = await _usersService.Get(request.assigneeId);
            if (userCheck == null) return NotFound();

            var newId = Guid.NewGuid();

            var newTask = new TaskEntity
            {
                Id = newId,
                CreatedAt = DateTime.UtcNow,
                Title = request.title,
                Description = request.description,
                Status = request.status,
                Priority = request.priority,
                AssigneeId = request.assigneeId
            };

            await _tasksService.Add(newTask);

            _metrics.RecordTaskCreated(request.priority);

            return Created(nameof(Guid), newId);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskResponse>> Get(Guid id)
        {
            var task = await _tasksService.Get(id);

            if (task == null) return NotFound();

            var response = new TaskResponse(
                id,
                task.CreatedAt,
                task.Title,
                task.Description ?? string.Empty,
                task.Status,
                task.Priority,
                task.AssigneeId);

            return Ok(response);
        }

        [HttpGet("{id:guid}/history")]
        public async Task<ActionResult<List<TaskHistoryResponse>>> GetHistory(Guid id)
        {
            var taskHistories = await _taskHistoriesService.GetList(id);
            var responseList = new List<TaskHistoryResponse>();

            foreach (var taskHistory in taskHistories)
            {
                responseList.Add(new TaskHistoryResponse
                {
                    Id = taskHistory.Id,
                    ChangeDescription = taskHistory.ChangeDescription,
                    ChangedAt = taskHistory.ChangedAt,
                    TaskId = taskHistory.TaskId
                });
            }

            return Ok(responseList);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, TaskRequest taskRequest)
        {
            await _tasksService.Update(
                id,
                taskRequest.title,
                taskRequest.description,
                taskRequest.status,
                taskRequest.priority);

            return Ok(id);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _tasksService.Delete(id);
            return Ok();
        }
    }
}
