using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.UpdateSeedling
{
    public class UpdateSeedlingCommand : IRequest<string>
    {
        public required string SeedlingId { get; set; }
        public string? ScientificName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Parent1 { get; set; }
        public string? Parent2 { get; set; }
        public DateOnly? DoB { get; set; }
        public List<CharacteristicsDTO>? Characteristics { get; set; } = [];
    }

    internal class UpdateSeedlingCommandHandler(ISeedlingRepository seedlingRepository,
        ISeedlingAttributeRepository seedlingAttributeRepository,
        ICharactersicticRepository charactersisticRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateSeedlingCommand, string>
    {
        public async Task<string> Handle(UpdateSeedlingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var seedling = await seedlingRepository.FindAsync(x => x.ID.Equals(request.SeedlingId), cancellationToken);

                // Update seedling properties
                seedling.LocalName = request.Name ?? seedling.LocalName;
                seedling.ScientificName = request.ScientificName ?? seedling.ScientificName;
                seedling.Description = request.Description ?? seedling.Description;
                seedling.Parent1 = request.Parent1 ?? seedling.Parent1;
                seedling.Parent2 = request.Parent2 ?? seedling.Parent2;
                seedling.Dob = request.DoB ?? seedling.Dob;
                seedling.Update_date = DateTime.UtcNow;
                seedling.Update_by = currentUserService.UserName ?? "system";
                seedlingRepository.Update(seedling);
                if(request.Characteristics == null || request.Characteristics.Count() == 0)
                    return await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                    ? $"Updated Seedling with ID: {seedling.ID}"
                    : "Failed to update Seedling.";
                foreach (var characteristic in request.Characteristics)
                {
                    // Find the attribute by name (case-insensitive)
                    var seedlingAttribute = await seedlingAttributeRepository.FindAsync(
                        x => x.Name.ToLower().Equals(characteristic.SeedlingAttribute.Name.ToLower()),
                        cancellationToken);

                    // If the attribute does not exist, create and save it
                    if (seedlingAttribute == null)
                    {
                        seedlingAttribute = new SeedlingAttributes
                        {
                            Name = characteristic.SeedlingAttribute.Name,
                            Description = characteristic.SeedlingAttribute.Description,
                            Status = true
                        };
                        seedlingAttributeRepository.Add(seedlingAttribute);
                        await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    }

                    // Find the existing characteristic by seedling and attribute
                    var existingCharacteristic = await charactersisticRepository.FindAsync(
                        x => x.SeedlingID.ToLower().Equals(seedling.ID.ToLower())
                            && x.SeedlingAttributeID.ToLower().Equals(seedlingAttribute.ID.ToLower()),
                        cancellationToken);

                    if (existingCharacteristic == null)
                    {
                        // Add new characteristic if not exists
                        var newCharacteristic = new Characteristics
                        {
                            SeedlingID = seedling.ID,
                            SeedlingAttributeID = seedlingAttribute.ID,
                            Value = characteristic.Value,
                            Status = true
                        };
                        charactersisticRepository.Add(newCharacteristic);
                    }
                    else if (existingCharacteristic.Value != characteristic.Value)
                    {
                        // Update value if changed
                        existingCharacteristic.Value = characteristic.Value;
                        charactersisticRepository.Update(existingCharacteristic);
                    }
                }

                return await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                    ? $"Updated Seedling with ID: {seedling.ID}"
                    : "Failed to update Seedling.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
