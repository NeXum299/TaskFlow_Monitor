namespace TaskFlow_Monitor.Core.Models
{
    public class TaskHistoryEntity
    {
        public Guid Id { get; set; }
        public string? ChangeDescription { get; set; }
        public string ChangeStatus { get; set; } = "Average";
        public DateTime ChangedAt { get; set; }
        public Guid TaskId { get; set; }
        public TaskEntity? Task { get; set; }
    }
}
