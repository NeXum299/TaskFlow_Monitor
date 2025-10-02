using TaskFlow_Monitor.API.Interfaces.Metrics;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;

namespace TaskFlow_Monitor.API.Metrics
{
    public class TaskMetricsService : ITaskMetricsService
    {
        private readonly CustomMetrics _metrics;
        private readonly ITasksRepository _tasksRepository;

        public TaskMetricsService(CustomMetrics metrics, ITasksRepository tasksRepository)
        {
            _metrics = metrics;
            _tasksRepository = tasksRepository;
        }

        public async Task UpdateTasksByStatusMetricsAsync()
        {
            var tasksByStatus = await _tasksRepository.GetTasksCountByStatusAsync();
            _metrics.UpdateTasksByStatusMetrics(tasksByStatus);
        }
    }
}