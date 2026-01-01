using MediatR;
using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Seedling.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.UpdateSeedlings
{
    public class UpdateSeedlingsCommand(UpdateSeedlingCommandParameter parameter) : IRequest<string>
    {
        public required string Id { get; set; } = parameter.Id;
        public string? LocalName { get; set; } = parameter.LocalName;
        public string? ScientificName { get; set; } = parameter.ScientificName;
        public string? Description { get; set; } = parameter.Description;
        public string? ParentAId { get; set; } = parameter.ParentAId;
        public string? ParentBId { get; set; } = parameter.ParentBId;
        public List<CreateSeedlingTraistDto>? CreateSeedlingsTraits { get; set; } = parameter.CreateSeedlingsTraits;
        public List<UpdateSeedlingsTraitsDto>? UpdateSeedlingsTraits { get; set; } = parameter.UpdateSeedlingsTraits;
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
            seedlings.ParentBId = request.ParentBId ?? seedlings.ParentBId;
            seedlings.UpdatedDate = TimeZoneHelper.VietnamTimeNow;
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
