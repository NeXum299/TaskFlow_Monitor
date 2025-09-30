namespace TaskFlow_Monitor.API.DTO
{
    public class TaskHistoryResponse
    {
        public Guid Id { get; set; }
        public string? ChangeDescription { get; set; }
        public string ChangeStatus { get; set; } = "Average";
        public DateTime ChangedAt { get; set; }
        public Guid TaskId { get; set; }
    }
}
