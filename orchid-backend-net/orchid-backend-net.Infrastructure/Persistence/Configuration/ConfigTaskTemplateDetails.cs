using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    public class ConfigTaskTemplateDetails : IEntityTypeConfiguration<TaskTemplateDetails>
    {
        public void Configure(EntityTypeBuilder<TaskTemplateDetails> builder)
        {
            builder.HasOne(x => x.TaskTemplate)
                .WithMany(x => x.TemplateDetails)
                .HasForeignKey(x => x.TaskTemplateID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
