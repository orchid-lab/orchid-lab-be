namespace orchid_backend_net.Application.Seedling.Dto
{
    /// <summary>
    /// using this dto to update seedlings trait
    /// </summary>
    public class UpdateSeedlingsTraitsDto
    {
        public required string Id { get; set; }
        public required decimal Value { get; set; }
    }
}
