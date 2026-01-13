using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class SamplesRequirementsDefinition : BaseGuidEntity
    {
        //only using for reference, not foreign key
        //normally would link to Characteristic entity, but to reduce complexity and kdependencies, just store code here
        //not showing to user, just for internal linking
        public string? CharacteristicCode { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Unit { get; set; }

        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public decimal DefaultExpectedValue { get; set; }
    }
}