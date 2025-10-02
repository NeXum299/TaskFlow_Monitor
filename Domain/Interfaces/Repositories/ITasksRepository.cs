using TaskFlow_Monitor.Core.Models;

namespace TaskFlow_Monitor.Domain.Interfaces.Repositories
{
    public interface ITasksRepository
    {
        Task<TaskEntity?> Get(Guid taskId);
        Task Add(TaskEntity taskEntity);
        Task Update(Guid id, string title, string description,
            string status, string priority);
        Task Delete(Guid taskId);
        Task<Dictionary<string, int>> GetTasksCountByStatusAsync();
    }
}
