namespace orchid_backend_net.Domain.Entities.Base
{
    public class BaseSoftDelete : BaseGuidEntity
    {
        public string? Create_by { get; set; }
        public DateTime? Create_date { get; set; }
        public string? Update_by { get; set; }
        public DateTime? Update_date { get; set; }
        public string? Delete_by { get; set; }
        public DateTime? Delete_date { get; set; }
    }
}
