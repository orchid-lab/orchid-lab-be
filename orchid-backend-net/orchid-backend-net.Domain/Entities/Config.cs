using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Config : BaseGuidEntity
    {
        public string ConfigName { get; set; } = default!;
        public string Key { get; set; } = default!;
        public decimal Value { get; set; } = default!;
    }
}
