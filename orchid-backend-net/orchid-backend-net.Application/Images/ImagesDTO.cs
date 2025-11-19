using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Images
{
    public class ImagesDTO : IMapFrom<Imgs>
    {
        public string ID { get; set; }
        public string Url { get; set; }
        public string ReportId { get; set; }
        public bool Status { get; set; }

        public ImagesDTO Create(string id, string url, string reportId, bool status)
        {
            return new ImagesDTO
            {
                ID = id,
                Url = url,
                ReportId = reportId,
                Status = status
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Imgs, ImagesDTO>()
                .ReverseMap();
        }
    }
}
