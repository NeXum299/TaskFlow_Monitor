using TaskFlow_Monitor.Core.Models;

namespace TaskFlow_Monitor.Domain.Interfaces.Services
{
    public interface ITaskHistoriesService
    {
        Task<List<TaskHistoryEntity>> GetList(Guid taskId);
    }
}
