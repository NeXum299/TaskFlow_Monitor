using TaskFlow_Monitor.Core.Models;

namespace TaskFlow_Monitor.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<UserEntity?> Get(Guid userId);
        Task Add(UserEntity userEntity);
    }
}
