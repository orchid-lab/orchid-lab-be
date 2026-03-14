using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class DiseaseIncidentAction : BaseGuidEntity
    {
        public required string DiseaseIncidentId { get; set; }
        [ForeignKey(nameof(DiseaseIncidentId))]
        public virtual DiseaseIncident DiseaseIncident { get; set; } = null!;

        public required string ActionDescription { get; set; }
        public required string PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public string? Result { get; set; }
    }
}
