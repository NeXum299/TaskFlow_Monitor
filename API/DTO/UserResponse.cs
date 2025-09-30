namespace TaskFlow_Monitor.API.DTO
{
    public record class UserResponse(
        Guid id,
        string name,
        string email);
}
