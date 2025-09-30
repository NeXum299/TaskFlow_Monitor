using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Domain.Interfaces.Services;

namespace TaskFlow_Monitor.Domain.Services
{
    public class TaskHistoriesService : ITaskHistoriesService
    {
        private readonly ITaskHistoriesRepository _thRepository;

        public TaskHistoriesService(ITaskHistoriesRepository thRepository) => _thRepository = thRepository;
        
        public async Task<List<TaskHistoryEntity>> GetList(Guid taskId) =>
            await _thRepository.GetList(taskId);
    }
}
