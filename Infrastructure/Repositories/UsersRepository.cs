using Microsoft.EntityFrameworkCore;
using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Domain.Interfaces.Repositories;
using TaskFlow_Monitor.Infrastructure.Contexts;

namespace TaskFlow_Monitor.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly MyDbContext _dbContext;

        public UsersRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(UserEntity userEntity)
        {
            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserEntity?> Get(Guid userId)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
