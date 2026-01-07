using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class MethodStages : BaseIntEntity
    {
        public required int MethodId { get; set; }
        [ForeignKey(nameof(MethodId))]
        public virtual Methods Method { get; set; }
        public required int StageDefinitionId { get; set; }
        [ForeignKey(nameof(StageDefinitionId))]
        public virtual StageDefinition StageDefinition { get; set; }
        public required string Name { get; set; }
        public int DurationsDays { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public virtual IEnumerable<StageMaterials> StageMaterials { get; set; } = [];
        public virtual IEnumerable<StageChemicals> StageChemicals { get; set; } = [];
        public virtual IEnumerable<SamplesRequirements> SamplesRequirements { get; set; } = [];
    }
}