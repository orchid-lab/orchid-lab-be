using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Disease
{
    public class DiseaseDTO : IMapFrom<Diseases>
    {
        public string ID {get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
        public decimal InfectedRate { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Diseases, DiseaseDTO>()
                .ReverseMap();
        }
    }
}
