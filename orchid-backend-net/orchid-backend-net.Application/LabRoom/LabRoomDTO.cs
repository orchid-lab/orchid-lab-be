using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.LabRoom
{
    public class LabRoomDTO : IMapFrom<LabRooms>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public LabRoomDTO() { }
        public LabRoomDTO(string iD, string name, string description, bool status)
        {
            ID = iD;
            Name = name;
            Description = description;
            Status = status;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LabRooms, LabRoomDTO>();
        }
    }
}
