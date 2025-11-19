using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class ReportAttributes : BaseGuidEntity
    {
        public string ReferentID { get; set; }
        [ForeignKey(nameof(ReferentID))]
        public virtual Referents Referent { get; set; }
        public string ReportID { get; set; }
        [ForeignKey(nameof(ReportID))]
        public virtual Reports Report { get; set; }
        public string Name { get; set; } // Name of the attribute
        public  decimal Value { get; set; } // Value of the attribute
        public int Status { get; set; }

    }
}
