using AutoMapper;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Materials.Dto;
using orchid_backend_net.Application.Method.Dto.SampleRequirement;
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
        public required List<MaterialDto> Materials { get; set; }
        public required List<ChemicalDto> Chemicals { get; set; }
        public required List<SampleRequirementDto> SampleRequirements { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MethodStages, MethodStageDetailDto>()
                .ForMember(src => src.Materials, 
                opt => opt.MapFrom(src => src.StageMaterials.Select(sm => sm.Material)))
                .ForMember(src => src.Chemicals,
                opt => opt.MapFrom(src => src.StageChemicals.Select(sm => sm.Chemical)));
        }
    }
}
