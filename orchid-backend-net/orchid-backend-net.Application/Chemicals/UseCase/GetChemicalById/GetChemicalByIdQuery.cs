using MediatR;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Chemicals.UseCase.GetChemicalById
{
    public class GetChemicalByIdQuery : IRequest<ChemicalDto?>
    {

        public required int ChemicalId { get; set; }

        public GetChemicalByIdQuery(int chemicalId)
        {
            ChemicalId = chemicalId;
        }

        public GetChemicalByIdQuery()
        {
        }
    }

    internal class GetChemicalByIdQueryHandler(IChemicalsRepository chemicalsRepository) : IRequestHandler<GetChemicalByIdQuery, ChemicalDto?>
    {
        public async Task<ChemicalDto?> Handle(GetChemicalByIdQuery request, CancellationToken cancellationToken)
        {
            var chemical = await chemicalsRepository.FindProjectToAsync<ChemicalDto>(
                queryOptions: q => q.Where(ch => ch.ID == request.ChemicalId),
                cancellationToken);
            return chemical;
        }
    }
}
