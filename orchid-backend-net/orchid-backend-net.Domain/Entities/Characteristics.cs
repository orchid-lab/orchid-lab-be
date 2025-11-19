using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Characteristics : BaseGuidEntity
    {
        public string SeedlingAttributeID { get; set; }
        [ForeignKey(nameof(SeedlingAttributeID))]
        public virtual SeedlingAttributes SeedlingAttribute {  set; get; }
        public string SeedlingID { get; set; }
        [ForeignKey(nameof(SeedlingID))]
        public virtual Seedlings Seedling {  set; get; }
        public decimal Value {  get; set; }
        //public enum Unit
        public bool Status {  get; set; }
    }
}
