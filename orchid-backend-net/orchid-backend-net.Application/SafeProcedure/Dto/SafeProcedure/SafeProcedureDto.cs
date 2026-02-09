using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedureStep;

namespace orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedure
{
    public class SafeProcedureDto : IMapFrom<Domain.Entities.SafeProcedure>
    {
        public required string Id { get; set; } = default!;
        public string ProcedureName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ProcedureType { get; set; } = default!;

        public List<SafeProcedureStepDto> SafeProcedureSteps { get; set; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SafeProcedure, SafeProcedureDto>()
                .ForMember(dest => dest.SafeProcedureSteps, 
                opt => opt.MapFrom(src => src.SafeProcedureSteps));
        }
    }
}
