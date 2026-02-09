using MediatR;
using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedure;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.GetById
{
    public record GetSafeProcedureByIdQuery(string Id) : IRequest<SafeProcedureDto>;
    internal class GetSafeProcedureByIdQueryHandler(ISafeProcedureRepository safeProcedureRepository) : IRequestHandler<GetSafeProcedureByIdQuery, SafeProcedureDto>
    {
        public async Task<SafeProcedureDto> Handle(GetSafeProcedureByIdQuery request, CancellationToken cancellationToken)
        {
            var safeProcedure = await safeProcedureRepository.FindProjectToAsync<SafeProcedureDto>(
                query => query.Where(sp => sp.ID == request.Id),
                cancellationToken)
                ?? throw new KeyNotFoundException($"SafeProcedure with Id {request.Id} not found.");
            return safeProcedure;
        }
    }
}
