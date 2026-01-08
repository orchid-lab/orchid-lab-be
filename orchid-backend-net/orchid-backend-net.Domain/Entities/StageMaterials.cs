using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class StageMaterials : BaseGuidEntity
    {
        public int MaterialId { get; set; }
        [ForeignKey(nameof(MaterialId))]
        public virtual Materials Material { get; set; }
        public int StageId { get; set; }
        [ForeignKey(nameof(StageId))]
        public virtual MethodStages MethodStage { get; set; }
    }
}