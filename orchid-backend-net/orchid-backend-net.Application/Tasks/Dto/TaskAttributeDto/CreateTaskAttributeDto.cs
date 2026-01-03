namespace orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto
{
    public class CreateTaskAttributeDto
    {
        public int? ChemicalId { get; set; }
        public int? MaterialId { get; set; }
        public required string Unit { get; set; }
        public required decimal Value { get; set; }
    }
}
