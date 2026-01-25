using MediatR;
using orchid_backend_net.Application.Sample.Dto.Sample;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.GetById
{
    public record GetSampleByIdQuery(string Id) : IRequest<SampleDetailDto>;
    internal class GetSampleByIdQuaryHandler(ISampleRepository sampleRepository) : IRequestHandler<GetSampleByIdQuery, SampleDetailDto>
    {
        public async Task<SampleDetailDto> Handle(GetSampleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await sampleRepository.FindProjectToAsync<SampleDetailDto>(
                queryOptions: q => q.Where(s => s.ID.Equals(request.Id)),
                cancellationToken: cancellationToken);
            return result ?? 
                throw new NotFoundException("Không tìm thấy sample này");
        }
    }
}
