using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.Images.Dto.Img
{
    public class ImageDto : IMapFrom<Domain.Entities.Imgs>
    {
        public string Id { get; set; } = null!;
        public ImageTargetType TargetType { get; set; }
        public string TargetId { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Imgs, ImageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.TargetType, opt => opt.MapFrom(src => src.TargetType))
                .ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => src.TargetId))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
