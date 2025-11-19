using AutoMapper;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Element
{
    public static class ElementMappingExtentations
    {
        public static ElementDTO MapToElementDTO(this Elements element, IMapper mapper)
            => mapper.Map<ElementDTO>(element);
        public static List<ElementDTO> MapToElementDTOList(this IEnumerable<Elements> elementList, IMapper mapper)
            => elementList.Select(x => x.MapToElementDTO(mapper)).ToList();
    }
}
