using MediatR;
using orchid_backend_net.Application.Seedling.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.GetSeedlingsById
{
    public class GetSeedlingsByIdQuery : IRequest<SeedlingsDto>
    {
        public required string Id { get; set; }
        public GetSeedlingsByIdQuery(string id)
        {
            Id = id;
        }
        public GetSeedlingsByIdQuery() { }
    }

    internal class GetSeedlingsByIdQueryHandler(ISeedlingRepository seedlingRepository) : IRequestHandler<GetSeedlingsByIdQuery, SeedlingsDto>
    {
        public async Task<SeedlingsDto> Handle(GetSeedlingsByIdQuery request, CancellationToken cancellationToken)
        {
            var seedling = await seedlingRepository.FindProjectToAsync<SeedlingsDto>(
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
