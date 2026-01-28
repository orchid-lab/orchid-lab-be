using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult;
using orchid_backend_net.Application.MonitoringLog.Dto.Disease;
using orchid_backend_net.Application.MonitoringLog.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.Analyze
{
    public record AnalyzeOrchidImageCommand(string FileName, byte[] FileStream) : IRequest<AnalyticResultAfterAnalysisDto>;

    internal class AnalyzeOrchidImageCommandHandler(
        IOrchidAnalyzerService orchidAnalyzerService,
        IAnalyticResultRepository analyticResultRepository,
        IDiseaseRepository diseaseRepository) : IRequestHandler<AnalyzeOrchidImageCommand, AnalyticResultAfterAnalysisDto>
    {
        public async Task<AnalyticResultAfterAnalysisDto> Handle(AnalyzeOrchidImageCommand request, CancellationToken cancellationToken)
        {
            var analyticResult = await orchidAnalyzerService.AnalyzeAsync(request.FileStream, cancellationToken);
            if (analyticResult.Disease is null) throw new ArgumentException("Kết quả phân tích bệnh bị thiếu", nameof(request));

            var analyticDisease = await diseaseRepository.FindProjectToAsync<DiseaseDto>(
                q => q.Where(d => d.Code.Equals(analyticResult.Disease.Predict)), 
                cancellationToken)
                ?? throw new NotFoundException("Không thấy bệnh này");

            var analyticResultEntity = OrchidAnalysisMapper.ToAnalyticResult(analyticResult);

            analyticResultRepository.Add(analyticResultEntity);

            var resultObject = new AnalyticResultAfterAnalysisDto()
            {
                StageName = analyticResult.Stage,
                Disease = analyticDisease,
                AnalyticResult = AnalyticResultDto.Create(analyticResultEntity)
            };
           
            return await analyticResultRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? resultObject
                : throw new InvalidOperationException("Phân tích thất bại");
        }
    }
}
