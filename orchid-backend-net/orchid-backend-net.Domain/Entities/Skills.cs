using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Skills : BaseIntEntity
    {
        public required string SkillName { get; set; }
        public required string SkillDescription { get; set; }
        public virtual List<UserSkill> UserSkills { get; set; } = new();
    }
}
