using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigTissueCultureBatch : IEntityTypeConfiguration<TissueCultureBatches>
    {
        public void Configure(EntityTypeBuilder<TissueCultureBatches> builder)
        {
            builder.HasOne(x => x.LabRoom)
                .WithMany()
                .HasForeignKey(x => x.LabRoomID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
