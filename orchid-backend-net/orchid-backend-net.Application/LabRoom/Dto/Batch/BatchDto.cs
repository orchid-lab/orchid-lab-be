using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.LabRoom.Dto.Batch
{
    public class BatchDto : IMapFrom<Batches>
    {
        public int Id { get; set; }
        public int LabRoomId { get; set; }  
        public string LabRoomName { get; set; }
        public string BatchName { get; set; }
        public int BatchSize { get; set; }
        public bool IsBatching { get; set; }    

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Batches, BatchDto>()
                .ForMember(dest => dest.LabRoomName,
                opt => opt.MapFrom(src => src.LabRoom.Name));
        }
    }
}
