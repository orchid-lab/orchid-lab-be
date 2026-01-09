using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Method.Dto.SampleRequirement;
using orchid_backend_net.Application.Method.Dto.StageChemical;
using orchid_backend_net.Application.Method.Dto.StageMaterial;
using orchid_backend_net.Application.StageDefinitiones.Dto;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Dto.MethodStage
{
    public class MethodStageDetailDto : IMapFrom<MethodStages>
    {
        public int Id { get; set; }
        public required int DurationsDays { get; set; }
        public required int Order { get; set; }
        public required StageDefinitionDto StageDefinition { get; set; }
        public required List<StageMaterialDto> StageMaterials { get; set; }
        public required List<StageChemicalDto> StageChemicals { get; set; }
        public required List<SampleRequirementDto> SampleRequirements { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MethodStages, MethodStageDetailDto>()
                .ForMember(dst => dst.StageMaterials,
                    opt => opt.MapFrom(src => src.StageMaterials))
                .ForMember(dst => dst.StageChemicals,
                    opt => opt.MapFrom(src => src.StageChemicals));
        }
    }
}
