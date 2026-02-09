using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;

namespace orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedure
{
    public class SafeProcedureDto : IMapFrom<Domain.Entities.SafeProcedure>
    {
        public required string Id { get; set; } = default!;
        public string ProcedureName { get; set; } = default!;
        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SafeProcedure, SafeProcedureDto>();
        }
    }
}
