using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow_Monitor.Core.Models;

namespace TaskFlow_Monitor.Infrastructure.Configurations
{
    public class TaskHistoriesConfiguration : IEntityTypeConfiguration<TaskHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<TaskHistoryEntity> builder)
        {
            builder.HasKey(th => th.Id);
            builder
                .HasOne(th => th.Task)
                .WithMany(t => t.TaskHistories)
                .HasForeignKey(t => t.TaskId);
        }
    }
}
