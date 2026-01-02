namespace orchid_backend_net.Application.Tasks.Dto
{
    public class UpdateTaskAttributeDto
    {
        public required string TaskAttributesId { get; set; }
        public required string Unit { get; set; }
        public required decimal Value { get; set; }
        public int? ChemicalId { get; set; }
        public int? MaterialId { get; set; }
    }
}
