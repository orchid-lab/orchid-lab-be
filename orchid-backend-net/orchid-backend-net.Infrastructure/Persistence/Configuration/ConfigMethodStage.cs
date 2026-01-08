using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigMethodStage : IEntityTypeConfiguration<MethodStages>
    {
        public void Configure(EntityTypeBuilder<MethodStages> builder)
        {
            builder.HasMany(s => s.StageMaterials)
                .WithOne(sm => sm.MethodStage)
                .HasForeignKey(sm => sm.StageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.StageChemicals)
                .WithOne(sm => sm.MethodStage)
                .HasForeignKey(sm => sm.StageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.SamplesRequirements)
                .WithOne(sr => sr.MethodStages)
                .HasForeignKey(sm => sm.MethodStageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
