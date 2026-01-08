using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigMethod : IEntityTypeConfiguration<Methods>
    {
        public void Configure(EntityTypeBuilder<Methods> builder)
        {
            builder.HasMany(m => m.MethodStages)
                .WithOne(s => s.Method)
                .HasForeignKey(m => m.MethodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
