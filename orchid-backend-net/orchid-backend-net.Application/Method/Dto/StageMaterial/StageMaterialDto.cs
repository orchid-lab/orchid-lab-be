using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Materials.Dto;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Dto.StageMaterial
{
    public class StageMaterialDto : IMapFrom<StageMaterials>
    {
        public string Id { get; set; }
        public MaterialDto Material { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<StageMaterials, StageMaterialDto>();
        }
    }
}
