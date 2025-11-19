using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigElementStage : IEntityTypeConfiguration<ElementInStage>
    {
        public void Configure(EntityTypeBuilder<ElementInStage> builder)
        {
            builder.HasOne(x => x.Element)
                .WithMany(x => x.ElementInStages)
                .HasForeignKey(x => x.ElementID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Stage)
                .WithMany(x => x.ElementInStages)
                .HasForeignKey(x => x.StageID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
