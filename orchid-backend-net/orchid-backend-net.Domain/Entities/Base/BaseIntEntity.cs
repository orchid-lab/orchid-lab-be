using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities.Base
{
    public class BaseIntEntity : BaseEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public override int ID { get; set; }
    }
}