using AutoMapper;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Sample
{
    public static class SampleMappingExtentations
    {
        public static SampleDTO MapToSampleDTO(this Samples obj, IMapper mapper)
            => mapper.Map<SampleDTO>(obj);
        public static List<SampleDTO> MapToSampleDTOList(this IEnumerable<Samples> objList, IMapper mapper)
            => objList.Select(x => x.MapToSampleDTO(mapper)).ToList();
    }
}
