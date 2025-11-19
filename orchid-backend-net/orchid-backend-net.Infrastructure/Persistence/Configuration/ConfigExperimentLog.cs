using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigExperimentLog : IEntityTypeConfiguration<ExperimentLogs>
    {
        public void Configure(EntityTypeBuilder<ExperimentLogs> builder)
        {
            builder.HasOne(x => x.TissueCultureBatch)
                .WithMany(x => x.ExperimentLogs)
                .HasForeignKey(x => x.TissueCultureBatchID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Method)
                .WithMany(x => x.ExperimentLogs)
                .HasForeignKey(x => x.MethodID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
