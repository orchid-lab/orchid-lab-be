using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog
{
    public class GetSeedlingsNameDTO : IMapFrom<Seedlings>
    {
        public string Id { get; set; }
        public string LocalName { get; set; }
        public string ScientificName { get; set; }
        public static GetSeedlingsNameDTO Create(string localName, string scientificName, string id)
        {
            return new GetSeedlingsNameDTO
            {
                Id = id,
                LocalName = localName,
                ScientificName = scientificName
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seedlings, GetSeedlingsNameDTO>()
                .ReverseMap();
        }
    }
}
