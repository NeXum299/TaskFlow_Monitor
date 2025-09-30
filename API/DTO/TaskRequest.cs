namespace TaskFlow_Monitor.API.DTO
{
    public record class TaskRequest(
        string title,
        string description,
        string status,
        string priority,
        Guid assigneeId);
}
