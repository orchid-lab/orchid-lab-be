using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigReferent : IEntityTypeConfiguration<Referents>
    {
        public void Configure(EntityTypeBuilder<Referents> builder)
        {
            builder.HasOne(x => x.Stage)
                .WithMany()
                .HasForeignKey(x => x.StageID);
        }
    }
}
