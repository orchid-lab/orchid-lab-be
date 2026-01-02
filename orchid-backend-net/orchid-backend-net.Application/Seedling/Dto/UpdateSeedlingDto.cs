namespace orchid_backend_net.Application.Seedling.Dto
{
    /// <summary>
    /// parameter object for constructor of UpdateSeedlingsCommand
    /// </summary>
    public class UpdateSeedlingCommandDto
    {
        public required string Id { get; set; }
        public string? LocalName { get; set; }
        public string? ScientificName { get; set; }
        public string? Description { get; set; }
        public string? ParentAId { get; set; }
        public string? ParentBId { get; set; }
        public List<CreateSeedlingTraistDto>? CreateSeedlingsTraits { get; set; }
        public List<UpdateSeedlingsTraitsDto>? UpdateSeedlingsTraits { get; set; }
    }
}
