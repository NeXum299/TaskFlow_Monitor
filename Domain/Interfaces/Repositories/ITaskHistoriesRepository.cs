using TaskFlow_Monitor.Core.Models;

namespace TaskFlow_Monitor.Domain.Interfaces.Repositories
{
    public interface ITaskHistoriesRepository
    {
        Task<List<TaskHistoryEntity>> GetList(Guid taskId);
    }
}
