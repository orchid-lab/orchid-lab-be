using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Seedlings : BaseSoftDelete
    {
        public string LocalName { get; set; }
        public string ScientificName { get; set; }
        public string Description { get; set; }
        public string? Parent1 {  get; set; }
        public string? Parent2 { get; set; }
        public DateOnly Dob {  get; set; }
        public virtual ICollection<Characteristics> Characteristics { get; set; } = [];
        public virtual ICollection<Hybridizations> Hybridizations { get; set; } = [];
    }
}
