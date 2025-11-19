using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigReportAttribute : IEntityTypeConfiguration<ReportAttributes>
    {
        public void Configure(EntityTypeBuilder<ReportAttributes> builder)
        {
            builder.HasOne(x => x.Referent)
                .WithMany(x => x.ReportAttributes)
                .HasForeignKey(x => x.ReferentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Report)
                .WithMany(x => x.ReportAttributes)
                .HasForeignKey(x => x.ReportID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
