using MediatR;
using orchid_backend_net.Application.Seedling.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.UseCase.GetSeedlingsById
{
    public class GetSeedlingsByIdQuery : IRequest<SeedlingsDetailDto>
    {
        public required string Id { get; set; }
        public GetSeedlingsByIdQuery(string id)
        {
            Id = id;
        }
        public GetSeedlingsByIdQuery() { }
    }

    internal class GetSeedlingsByIdQueryHandler(ISeedlingRepository seedlingRepository) : IRequestHandler<GetSeedlingsByIdQuery, SeedlingsDetailDto>
    {
        public async Task<SeedlingsDetailDto> Handle(GetSeedlingsByIdQuery request, CancellationToken cancellationToken)
        {
            var seedling = await seedlingRepository.FindProjectToAsync<SeedlingsDetailDto>(
                queryOptions: q => q.Where(s => s.ID.Equals(request.Id)),
                cancellationToken: cancellationToken);
            if(seedling == null)
            {
                throw new NotFoundException($"Không tìm thấy cây giống này.");
            }
            return seedling;
        }
    }
}
