using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Batch.Dto.Batch
{
    public class BatchDto : IMapFrom<Batches>
    {
        public int Id { get; set; }
        public int LabRoomId { get; set; }  
        public string LabRoomName { get; set; } = default!;
        public string BatchName { get; set; } = default!;
        public decimal BatchSizeWidth { get; set; } = default!;
        public decimal BatchSizeHeight { get; set; } = default!;
        public string WidthUnit { get; set; } = default!;
        public string HeightUnit { get; set; } = default!;
        public BatchStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Batches, BatchDto>()
                .ForMember(dest => dest.LabRoomName,
                opt => opt.MapFrom(src => src.LabRoom.Name))
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status));
        }
    }
}
