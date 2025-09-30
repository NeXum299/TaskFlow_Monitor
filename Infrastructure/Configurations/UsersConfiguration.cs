using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow_Monitor.Core.Models;

namespace TaskFlow_Monitor.Infrastructure.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);
            builder
                .HasMany(u => u.Tasks)
                .WithOne(t => t.Recipient)
                .HasForeignKey(t => t.AssigneeId);
        }
    }
}
