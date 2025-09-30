namespace TaskFlow_Monitor.API.DTO
{
    public class TaskHistoryRequest
    {
        public string ChangeDescription { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }
}
