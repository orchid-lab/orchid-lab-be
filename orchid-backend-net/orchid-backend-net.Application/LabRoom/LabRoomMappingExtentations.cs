using AutoMapper;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.LabRoom
{
    public static class LabRoomMappingExtentations
    {
        public static LabRoomDTO MapToLabRoomDTO(this LabRooms labRoom, IMapper mapper)
            => mapper.Map<LabRoomDTO>(labRoom);
        public static List<LabRoomDTO> MapToLabRoomDTOList(this IEnumerable<LabRooms> labRoomList, IMapper mapper)
            => labRoomList.Select(x => x.MapToLabRoomDTO(mapper)).ToList();
    }
}
