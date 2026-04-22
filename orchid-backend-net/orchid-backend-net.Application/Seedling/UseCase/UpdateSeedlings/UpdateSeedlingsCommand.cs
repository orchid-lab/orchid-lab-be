using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Seedling.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.UseCase.UpdateSeedlings
{
    public class UpdateSeedlingsCommand(string id,
        string? localName,
        string? scientificName,
        string? description,
        string? parentAId,
        List<CreateSeedlingTraistDto>? createSeedlingsTraits,
        List<UpdateSeedlingsTraitsDto>? updateSeedlingsTraits) : IRequest<string>
    {
        public required string Id { get; set; } = id;
        public string? LocalName { get; set; } = localName;
        public string? ScientificName { get; set; } = scientificName;
        public string? Description { get; set; } = description;
        public string? ParentAId { get; set; } = parentAId;
        public List<CreateSeedlingTraistDto>? CreateSeedlingsTraits { get; set; } = createSeedlingsTraits;
        public List<UpdateSeedlingsTraitsDto>? UpdateSeedlingsTraits { get; set; } = updateSeedlingsTraits;
    }

    internal class UpdateSeedlingCommandHandler(ISeedlingRepository seedlingRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateSeedlingsCommand, string>
    {
        public async Task<string> Handle(UpdateSeedlingsCommand request, CancellationToken cancellationToken)
        {
            var seedlings = await seedlingRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken)
                ?? throw new NotFoundException("Cây giống không tồn tại.");

            seedlings.LocalName = request.LocalName ?? seedlings.LocalName;
            seedlings.ScientificName = request.ScientificName ?? seedlings.ScientificName;
            seedlings.Description = request.Description ?? seedlings.Description;
            seedlings.ParentAId = request.ParentAId ?? seedlings.ParentAId;
            seedlings.UpdatedDate = DateTime.UtcNow;
            seedlings.UpdatedBy = currentUserService.UserId;


            request.UpdateSeedlingsTraits?.ForEach(x =>
            {
                seedlings.UpdateTrait(x.Id, x.Value);
            });

            request.CreateSeedlingsTraits?.ForEach(x =>
            {
                seedlings.AddTrait(x.CharacteristicId, x.Value);
            });

            seedlingRepository.Update(seedlings);
            return await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Cập nhật cây giống thành công." : "Cập nhật cây giống thất bại.";
        }
    }
}
