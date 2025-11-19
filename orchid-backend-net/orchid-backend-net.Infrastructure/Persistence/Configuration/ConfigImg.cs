using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;


namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigImg : IEntityTypeConfiguration<Imgs>
    {
        public void Configure(EntityTypeBuilder<Imgs> builder)
        {
            builder.HasOne(x => x.Report)
                .WithMany(x => x.Imgs)
                .HasForeignKey(x => x.ReportID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
