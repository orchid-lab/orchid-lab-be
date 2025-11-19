using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigInfectedSample : IEntityTypeConfiguration<InfectedSamples>
    {
        public void Configure(EntityTypeBuilder<InfectedSamples> builder)
        {
            builder.HasOne(x => x.Sample)
                .WithOne(x => x.InfectedSamples)
                .HasForeignKey<InfectedSamples>(x => x.SampleID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Disease)
                .WithMany()
                .HasForeignKey(x => x.DiseaseID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
