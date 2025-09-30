using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Domain.Interfaces.Services;

namespace TaskFlow_Monitor.Domain.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _uRepository;

        public UsersService(IUsersRepository uRepository) =>
            _uRepository = uRepository;
        
        public async Task Add(UserEntity userEntity) =>
            await _uRepository.Add(userEntity);

        public async Task<UserEntity?> Get(Guid userId) =>
            await _uRepository.Get(userId);
    }
}
