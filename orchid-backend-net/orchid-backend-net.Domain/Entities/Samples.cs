using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Samples : BaseGuidEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly Dob { get; set; }
        public int Status {  get; set; }
        public string? Reason { get; set; }
        public virtual ICollection<Linkeds> Linkeds { get; set; } = [];
        public virtual ICollection<Reports> Reports { get; set; } = [];
        public virtual InfectedSamples InfectedSamples { get; set; }
    }
}
