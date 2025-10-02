namespace TaskFlow_Monitor.API.Interfaces.Metrics
{
    public interface ITaskMetricsService
    {
        Task UpdateTasksByStatusMetricsAsync();
    }
}