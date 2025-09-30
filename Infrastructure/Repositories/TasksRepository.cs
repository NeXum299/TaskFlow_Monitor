using Microsoft.EntityFrameworkCore;
using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Infrastructure.Contexts;

namespace TaskFlow_Monitor.Infrastructure.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly MyDbContext _dbContext;

        public TasksRepository(MyDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(TaskEntity taskEntity)
        {
            await _dbContext.Tasks.AddAsync(taskEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid taskId)
        {
            await _dbContext.Tasks
                .Where(t => t.Id == taskId)
                .ExecuteDeleteAsync();
        }

        public async Task<TaskEntity?> Get(Guid taskId)
        {
            return await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task Update(Guid id, string title, string description,
            string status, string priority)
        {
            var taskEntity = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            var taskHistory = new TaskHistoryEntity
            {
                Id = Guid.NewGuid(),
                ChangeDescription = taskEntity!.Description,
                ChangedAt = DateTime.UtcNow,
                TaskId = taskEntity.Id,
                Task = taskEntity
            };

            await _dbContext.TaskHistories.AddAsync(taskHistory);

            await _dbContext.Tasks
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(options => options
                    .SetProperty(t => t.Title, title)
                    .SetProperty(t => t.Description, description)
                    .SetProperty(t => t.Status, status)
                    .SetProperty(t => t.Priority, priority));
        }
    }
}
