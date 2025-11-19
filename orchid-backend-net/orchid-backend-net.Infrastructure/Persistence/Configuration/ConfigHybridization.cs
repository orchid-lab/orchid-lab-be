using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigHybridization : IEntityTypeConfiguration<Hybridizations>
    {
        public void Configure(EntityTypeBuilder<Hybridizations> builder)
        {
            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Hybridizations)
                .HasForeignKey(x => x.ParentID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.ExperimentLog)
                .WithMany(x => x.Hybridizations)
                .HasForeignKey(x => x.ExperimentLogID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
