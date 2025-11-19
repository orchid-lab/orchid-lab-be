using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    public class ConfigTaskTemplates : IEntityTypeConfiguration<TaskTemplates>
    {
        public void Configure(EntityTypeBuilder<TaskTemplates> builder)
        {
            builder.HasOne(x => x.Stage)
                .WithMany(x => x.TaskTemplates)
                .HasForeignKey(x => x.StageID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
