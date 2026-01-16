using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog
{
    internal class ExperimentLogDto : IMapFrom<ExperimentLogs>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CurrentStageOrder { get; set; }
        public string MethodName { get; set; }
        public string BatcheName { get; set; }
        public int ExpectedSampleCount { get; set; }
        public string CreatedBy { get; set; }   
        public DateTime CreatedDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExperimentLogs, ExperimentLogDto>()
                .ForMember(dest => dest.MethodName, 
                opt => opt.MapFrom(src => src.Method.Name))
                .ForMember(dest => dest.BatcheName,
                opt => opt.MapFrom(src => src.Batch.BatchName));
        }
    }
}
