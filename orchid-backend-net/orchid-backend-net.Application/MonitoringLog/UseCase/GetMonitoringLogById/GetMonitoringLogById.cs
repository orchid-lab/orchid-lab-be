using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Images.Dto.Img;
using orchid_backend_net.Application.MonitoringLog.Dto.MonitoringLog;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.GetMonitoringLogById
{
    public record GetMonitoringLogById(string Id) : IRequest<MonitoringLogDetailDto>;
    internal class GetMonitoringLogByIdHandler(
        IMonitoringLogRepository monitoringLogRepository,
        IImageRepository imageRepository,
        IMapper mapper) : IRequestHandler<GetMonitoringLogById, MonitoringLogDetailDto>
    {
        public async Task<MonitoringLogDetailDto> Handle(GetMonitoringLogById request, CancellationToken cancellationToken)
        {
            //Get monitoring log by id
            var monitoring = await monitoringLogRepository.FindProjectToAsync<MonitoringLogDetailDto>(
                queryOptions: q => q.Where(m => m.ID.Equals(request.Id)),
                cancellationToken)
                ?? throw new NotFoundException("Không thấy monitoring log này");

            var images = await imageRepository.GetImagesByTargetAsync(request.Id, Domain.Common.Enum.ImageTargetType.MonitoringLog, cancellationToken);
            monitoring.Images = mapper.Map<List<ImageDto>>(images);

            return monitoring;
        }
    }
}
