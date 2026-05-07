using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class UserSkill : BaseGuidEntity
    {
        public required string UserId { get; set; }
        public virtual Users? User { get; set; }
        public required int SkillId { get; set; }
        public virtual Skills? Skill { get; set; }
    }
}
