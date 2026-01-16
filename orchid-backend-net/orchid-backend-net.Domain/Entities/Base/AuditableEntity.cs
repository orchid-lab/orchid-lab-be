namespace orchid_backend_net.Domain.Entities.Base
{
    public abstract class AuditableEntity : BaseGuidEntity
    {
        public DateTime CreatedDate { get; set; }
        public required string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
