using Microsoft.EntityFrameworkCore;
using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Infrastructure.Configurations;

namespace TaskFlow_Monitor.Infrastructure.Contexts
{
    public class MyDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskHistoryEntity> TaskHistories { get; set; }

        public MyDbContext() { }
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new TaskHistoriesConfiguration());
            modelBuilder
                .ApplyConfiguration(new TasksConfiguration());
            modelBuilder
                .ApplyConfiguration(new UsersConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
