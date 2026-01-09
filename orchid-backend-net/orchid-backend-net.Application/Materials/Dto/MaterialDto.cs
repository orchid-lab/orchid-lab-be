using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Materials.Dto
{
    public class MaterialDto : IMapFrom<Domain.Entities.Materials>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public string? Description { get; set; }
        public required string Unit { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Materials, MaterialDto>();
        }
    }
}
