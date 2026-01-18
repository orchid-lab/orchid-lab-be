using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    public class ConfigExperimentLog : IEntityTypeConfiguration<ExperimentLogs>
    {
        public void Configure(EntityTypeBuilder<ExperimentLogs> builder)
        {
            builder.HasOne(e => e.Hybridzations)
                .WithOne(h => h.Experiment)
                .HasForeignKey<ExperimentLogs>(e => e.HybridzationId);
        }
    }
}
