using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class InfectedSamples : BaseGuidEntity
    {
        public string SampleID {  get; set; }
        [ForeignKey(nameof(SampleID))]
        public virtual Samples Sample { get; set; }
        public string DiseaseID {  get; set; }
        [ForeignKey(nameof(DiseaseID))]
        public virtual Diseases Disease { get; set; }
    }
}
