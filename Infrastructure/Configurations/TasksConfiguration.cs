using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow_Monitor.Core.Models;

namespace TaskFlow_Monitor.Infrastructure.Configurations
{
    public class TasksConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder
                .HasMany(t => t.TaskHistories)
                .WithOne(th => th.Task);
            builder
                .HasOne(t => t.Recipient)
                .WithMany(u => u.Tasks);
        }   
    }
}
