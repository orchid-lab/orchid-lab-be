using System.ComponentModel.DataAnnotations;

namespace orchid_backend_net.Domain.Entities.Base
{
    public class BaseGuidEntity : BaseEntity<string>
    {
        protected BaseGuidEntity()
        {
            ID = Guid.NewGuid().ToString();
        }
        [Key]
        public override string ID { get; set; }
    }
}
