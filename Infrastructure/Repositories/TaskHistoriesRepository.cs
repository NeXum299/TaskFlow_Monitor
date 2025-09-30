using Microsoft.EntityFrameworkCore;
using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Infrastructure.Contexts;

namespace TaskFlow_Monitor.Infrastructure.Repositories
{
    public class TaskHistoriesRepository : ITaskHistoriesRepository
    {
        private readonly MyDbContext _dbContext;

        public TaskHistoriesRepository(MyDbContext dbContext) => _dbContext = dbContext;

        public async Task<List<TaskHistoryEntity>> GetList(Guid taskId)
        {
            return await _dbContext.TaskHistories
                .AsNoTracking()
                .Where(t => t.Id == taskId)
                .ToListAsync();
        }
    }
}
