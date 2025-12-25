using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Characteristic : BaseGuidEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        //using for reference in sample requirements etc.
        public required string Code { get; set; }
        public required string Unit { get; set; }
    }
}