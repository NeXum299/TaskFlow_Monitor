namespace TaskFlow_Monitor.Core.Models
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public Guid AssigneeId { get; set; }
        public UserEntity? Recipient { get; set; }
        public List<TaskHistoryEntity> TaskHistories { get; set; } = [];
    }
}
