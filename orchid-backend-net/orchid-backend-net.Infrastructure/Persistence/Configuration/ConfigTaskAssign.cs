using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigTaskAssign : IEntityTypeConfiguration<TasksAssign>
    {
        public void Configure(EntityTypeBuilder<TasksAssign> builder)
        {
            builder.HasOne(x => x.Technician)
                .WithMany(x => x.Assigns)
                .HasForeignKey(x => x.TechnicianID);
            builder.HasOne(x => x.Task)
                .WithMany(x => x.Assigns)
                .HasForeignKey(x => x.TaskID);
        }
    }
}
