using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;

namespace orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedureStep
{
    public class SafeProcedureStepDto : IMapFrom<Domain.Entities.SafeProcedureStep>
    {
        public string Id { get; set; } = default!;
        public string SafeProcedureStepName { get; set; } = default!;
        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SafeProcedureStep, SafeProcedureStepDto>();
        }
    }
}
