using MediatR;
using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedure;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.GetById
{
    public record GetSafeProcedureByIdQuery(string Id) : IRequest<SafeProcedureDetailDto>;
    internal class GetSafeProcedureByIdQueryHandler(ISafeProcedureRepository safeProcedureRepository) : IRequestHandler<GetSafeProcedureByIdQuery, SafeProcedureDetailDto>
    {
        public async Task<SafeProcedureDetailDto> Handle(GetSafeProcedureByIdQuery request, CancellationToken cancellationToken)
        {
            var safeProcedure = await safeProcedureRepository.FindProjectToAsync<SafeProcedureDetailDto>(
                query => query.Where(sp => sp.ID == request.Id),
                cancellationToken)
                ?? throw new KeyNotFoundException($"SafeProcedure with Id {request.Id} not found.");
            return safeProcedure;
        }
    }
}
