namespace TaskFlow_Monitor.API.DTO
{
    public record class TaskResponse(
        Guid id,
        DateTime createAt,
        string title,
        string description,
        string status,
        string priority,
        Guid assignedId);
}
