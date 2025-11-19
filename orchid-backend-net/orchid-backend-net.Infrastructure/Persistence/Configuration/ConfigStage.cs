using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigStage : IEntityTypeConfiguration<Stages>
    {
        public void Configure(EntityTypeBuilder<Stages> builder)
        {
            builder.HasOne(x => x.Method)
                .WithMany(x => x.Stages)
                .HasForeignKey(x => x.MethodID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
