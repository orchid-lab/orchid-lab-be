using MediatR;
using orchid_backend_net.Application.Sample.Dto.Sample;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.GetById
{
    public record GetSampleByIdQuery(string Id) : IRequest<SampleDetailDto>;
    
    internal class GetSampleByIdQuaryHandler(ISampleRepository sampleRepository) 
        : IRequestHandler<GetSampleByIdQuery, SampleDetailDto>
    {
        public async Task<SampleDetailDto> Handle(GetSampleByIdQuery request, CancellationToken cancellationToken)
        {
            // get sample with stage info
            var result = await sampleRepository.FindProjectToAsync<SampleDetailDto>(
                queryOptions: q => q.Where(s => s.ID.Equals(request.Id)),
                cancellationToken: cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy sample này");

            // only load images if sample stage exists
            if (result.SampleStageDto != null)
            {
                var allImages = await sampleRepository.GetLatestImagesByTargetsAsync(
                    new[] { result.SampleStageDto.Id },  // ← Chỉ lấy SampleStage ID
                    cancellationToken);

                result.SampleStageDto.LatestImageUrl = allImages
                    .FirstOrDefault(img => img.TargetId == result.SampleStageDto.Id
                        && img.TargetType == ImageTargetType.SampleStage)?.Url;
            }

            return result;
        }
    }
}
