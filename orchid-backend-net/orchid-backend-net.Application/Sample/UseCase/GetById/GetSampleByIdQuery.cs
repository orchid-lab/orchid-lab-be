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
            if (result.SampleStageDto?.Any() == true)
            {
                var allStageIds = result.SampleStageDto.Select(s => s.Id).ToArray();

                // Batch load all images
                var allImages = await sampleRepository.GetLatestImagesByTargetsAsync(
                    allStageIds,
                    cancellationToken);

                // Map images to corresponding stages (only if we have images)
                if (allImages.Count > 0) //avoid iteratedly searching for images if there are none
                {
                    var imageDict = allImages
                        .Where(img => img.TargetType == ImageTargetType.SampleStage)
                        .ToDictionary(img => img.TargetId, img => img.Url);  // ← Optimize O(n) lookup

                    foreach (var stage in result.SampleStageDto)
                    {
                        stage.LatestImageUrl = imageDict.GetValueOrDefault(stage.Id);
                    }
                }
            }

            return result;
        }
    }
}
