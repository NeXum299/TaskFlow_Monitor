using TaskFlow_Monitor.Core.Models;

namespace TaskFlow_Monitor.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        Task<UserEntity?> Get(Guid userId);
        Task Add(UserEntity userEntity);
    }
}
