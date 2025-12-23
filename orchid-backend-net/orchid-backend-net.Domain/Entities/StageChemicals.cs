using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class StageChemicals : BaseGuidEntity
    {
        public int ChemicalId { get; set; }
        [ForeignKey(nameof(ChemicalId))]
        public virtual Chemicals Chemical { get; set; }
        public int StageId { get; set; }
        [ForeignKey(nameof(StageId))]
        public virtual Stages Stage { get; set; }
    }
}