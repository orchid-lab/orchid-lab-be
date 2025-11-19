using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigTaskAttribute : IEntityTypeConfiguration<TaskAttributes>
    {
        public void Configure(EntityTypeBuilder<TaskAttributes> builder)
        {
            builder.HasOne(x => x.Task)
                .WithMany(x => x.Attributes)
                .HasForeignKey(x => x.TaskID);
        }
    }
}
