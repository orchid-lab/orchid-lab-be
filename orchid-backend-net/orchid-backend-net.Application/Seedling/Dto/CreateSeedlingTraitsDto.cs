namespace orchid_backend_net.Application.Seedling.Dto
{
    /// <summary>
    /// using this dto to create seedlings trait
    /// </summary>
    public class CreateSeedlingTraistDto
    {
        public required string CharacteristicId { get; set; }
        public required decimal Value { get; set; }
    }
}
