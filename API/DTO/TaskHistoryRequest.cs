namespace TaskFlow_Monitor.API.DTO
{
    public class TaskHistoryRequest
    {
        public string? ChangeDescription { get; set; }
        public string ChangeStatus { get; set; } = "Average";
        public DateTime ChangedAt { get; set; }
    }
}
