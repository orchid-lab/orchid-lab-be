using AutoMapper;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Dto.StageChemical
{
    public class StageChemicalDto : IMapFrom<StageChemicals>
    {
        public string Id { get; set;  }
        public ChemicalDto Chemical { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<StageChemicals, StageChemicalDto>();
        }
    }
}
