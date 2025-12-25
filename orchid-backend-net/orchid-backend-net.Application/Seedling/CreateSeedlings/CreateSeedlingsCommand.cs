using MediatR;
using orchid_backend_net.Application.Common.Constant;
using orchid_backend_net.Application.Common.E;
using orchid_backend_net.Application.Common.Enum;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.CreateSeedlings
{
    public class CreateSeedlingsCommand(
        string localName, 
        string scientificName, 
        string? description,
        string? parentAId, 
        string? parentBId, 
        List<CreateSeedlingTraistDto> createSeedlingTraistDtos) : IRequest<string>
    {
        public required string LocalName { get; set; } = localName;
        public required string ScientificName { get; set; } = scientificName;
        public string? Description { get; set; } = description;
        public string? ParentAId { get; set; } = parentAId;
        public string? ParentBId { get; set; } = parentBId;
        public required List<CreateSeedlingTraistDto> SeedlingsTraits { get; set; } = createSeedlingTraistDtos;
    }
    public class CreateSeedlingTraistDto
    {
        public required string CharacteristicId { get; set; }
        public required decimal Value { get; set; }
    }

    internal class CreateSeedlingsCommandHandler(ISeedlingRepository seedlingRepository,
        ISeedlingTraitRepository seedlingTraitRepository, 
        ICurrentUserService currentUserService) : IRequestHandler<CreateSeedlingsCommand, string>
    {
        public async Task<string> Handle(CreateSeedlingsCommand request, CancellationToken cancellationToken)
        {
            var seedling = new Seedlings
            {
                LocalName = request.LocalName,
                ScientificName = request.ScientificName,
                Description = request.Description,
                ParentAId = request.ParentAId,
                ParentBId = request.ParentBId,
                CreatedBy = currentUserService.UserId,
                CreatedDate = TimeZoneEnum.VietnamTimeZone,
            };
            seedlingRepository.Add(seedling);

            var seedlingTraits = request.SeedlingsTraits.Select(trait => new SeedlingsTraits
            {
                SeedlingId = seedling.ID,
                CharacteristicId = trait.CharacteristicId,
                Value = trait.Value,
            }).ToList();

            seedlingTraitRepository.AddRange(seedlingTraits);
            return await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Tạo cây giống thành công" : "Tạo cây giống thất bại";
        }
    }
}
