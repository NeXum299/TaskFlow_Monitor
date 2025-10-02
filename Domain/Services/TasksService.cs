using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Domain.Interfaces.Services;

namespace TaskFlow_Monitor.Domain.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _tRepository;

        public TasksService(
            ITasksRepository tasksRepository)
        {
            _tRepository = tasksRepository;
        }

        public async Task Add(TaskEntity taskEntity)
        {
            await _tRepository.Add(taskEntity);
        }

        public async Task Delete(Guid taskId) =>
            await _tRepository.Delete(taskId);

        public async Task<TaskEntity?> Get(Guid taskId) =>
            await _tRepository.Get(taskId);

        public async Task Update(
            Guid id,
            string title,
            string description,
            string status,
            string priority)
        {
            var task = await _tRepository.Get(id);
            var oldStatus = task!.Status;

            if (task == null) throw new ArgumentNullException(nameof(task));

            await _tRepository.Update(id, title, description, status, priority);
        }
    }
}
