using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigCharacteristic : IEntityTypeConfiguration<Characteristics>
    {
        public void Configure(EntityTypeBuilder<Characteristics> builder)
        {
            builder.HasOne(x => x.SeedlingAttribute)
                .WithMany()
                .HasForeignKey(x => x.SeedlingAttributeID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Seedling)
                .WithMany(x => x.Characteristics)
                .HasForeignKey(x => x.SeedlingID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
