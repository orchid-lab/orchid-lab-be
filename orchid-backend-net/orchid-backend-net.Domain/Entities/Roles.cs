using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Roles : BaseIntEntity
    {
        public required string Name { get; set; }
    }
}
