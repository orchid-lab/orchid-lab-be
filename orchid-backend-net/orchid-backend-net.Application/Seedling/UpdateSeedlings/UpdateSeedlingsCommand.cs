using MediatR;
using orchid_backend_net.Application.Common.Enum;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Seedling.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
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
        ISeedlingTraitRepository seedlingTraitRepository,
        ICharacteristicRepository characteristicRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateSeedlingsCommand, string>
    {
        public async Task<string> Handle(UpdateSeedlingsCommand request, CancellationToken cancellationToken)
        {
            List<SeedlingsTraits> traitsToAdd = new List<SeedlingsTraits>();
            List<SeedlingsTraits> traitToUpdate = new List<SeedlingsTraits>();
            var seedlings = await seedlingRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken)
                ?? throw new NotFoundException("Cây giống không tồn tại.");

            seedlings.LocalName = request.LocalName ?? seedlings.LocalName;
            seedlings.ScientificName = request.ScientificName ?? seedlings.ScientificName;
            seedlings.Description = request.Description ?? seedlings.Description;
            seedlings.ParentAId = request.ParentAId ?? seedlings.ParentAId;
            seedlings.ParentBId = request.ParentBId ?? seedlings.ParentBId;
            seedlings.UpdatedDate = TimeZoneEnum.VietnamTimeZone;
            seedlings.UpdatedBy = currentUserService.UserId;


            request.UpdateSeedlingsTraits?.ForEach(async x =>
             {
                 var seedlingTrait = await seedlingTraitRepository.FindAsync(st => st.ID.Equals(x.Id), cancellationToken);
                 if (seedlingTrait is not null)
                 {
                     seedlingTrait.Value = x.Value;
                     traitToUpdate.Add(seedlingTrait);
                 }
             });

            request.CreateSeedlingsTraits?.ForEach(async x =>
            {
                var characteristic = await characteristicRepository.FindAsync(c => c.ID.Equals(x.CharacteristicId), cancellationToken);
                if (characteristic is null)
                    throw new NotFoundException($"Đặc điểm với ID {x.CharacteristicId} không tồn tại.");

                var isSeedlingsTraitDuplicated = await seedlingTraitRepository.AnyAsync(t => t.CharacteristicId.Equals(x.CharacteristicId) && t.SeedlingId.Equals(seedlings.ID));

                if (isSeedlingsTraitDuplicated)
                    throw new DuplicateException($"Đặc điểm với ID {x.CharacteristicId} đã tồn tại trong cây giống này.");

                var newSeedlingTrait = new SeedlingsTraits()
                {
                    CharacteristicId = x.CharacteristicId,
                    SeedlingId = seedlings.ID,
                    Value = x.Value
                };
                traitsToAdd.Add(newSeedlingTrait);
            });

            seedlingRepository.Update(seedlings);
            seedlingTraitRepository.UpdateRange(traitToUpdate);
            seedlingTraitRepository.AddRange(traitsToAdd);
            return await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Cập nhật cây giống thành công." : "Cập nhật cây giống thất bại.";
        }
    }

    /// <summary>
    /// parameter object for constructor of UpdateSeedlingsCommand
    /// </summary>
    public class UpdateSeedlingCommandParameter
    {
        public required string Id { get; set; }
        public string? LocalName { get; set; }
        public string? ScientificName { get; set; }
        public string? Description { get; set; }
        public string? ParentAId { get; set; }
        public string? ParentBId { get; set; }
        public List<CreateSeedlingTraistDto>? CreateSeedlingsTraits { get; set; }
        public List<UpdateSeedlingsTraitsDto>? UpdateSeedlingsTraits { get; set; }
    }
}
