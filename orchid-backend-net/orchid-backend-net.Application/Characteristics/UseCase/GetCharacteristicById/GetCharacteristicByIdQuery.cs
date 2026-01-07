using MediatR;
using orchid_backend_net.Application.Characteristics.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Characteristics.UseCase.GetCharacteristicById
{
    public class GetCharacteristicByIdQuery : IRequest<CharacteristicDto?>
    {
        public required string CharacteristicId { get; set; }
        public GetCharacteristicByIdQuery()
        {
        }


        public GetCharacteristicByIdQuery(string characteristicId)
        {
            CharacteristicId = characteristicId;
        }
    }

    internal class GetCharacteristicByIdQueryHandler(ICharacteristicRepository characteristicRepository) : IRequestHandler<GetCharacteristicByIdQuery, CharacteristicDto?>
    {
        public async Task<CharacteristicDto?> Handle(GetCharacteristicByIdQuery request, CancellationToken cancellationToken)
        {
            var characteristic = await characteristicRepository.FindProjectToAsync<CharacteristicDto>(
                queryOptions: q => q.Where(ch => ch.ID == request.CharacteristicId),
                cancellationToken);
            return characteristic;
        }
    }
}
