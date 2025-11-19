using MediatR;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Application.Common.Interfaces;
using System.Threading.Tasks;
using orchid_backend_net.Domain.Common.Exceptions;

namespace orchid_backend_net.Application.Seedling.CreateSeedling
{
    public class CreateSeedlingCommand : IRequest<string>
    {
        public required string ScientificName { get; set; }
        public required string LocalName { get; set; }
        public required string Description { get; set; }
        public string? MotherID { get; set; }
        public string? FatherID { get; set; }
        public required DateOnly DoB { get; set; }
        public required List<CharacteristicsDTO> Characteristics { get; set; } = [];
    }

    internal class CreateSeedlingCommandHandler(ISeedlingRepository seedlingRepository, 
        ISeedlingAttributeRepository seedlingAttributeRepository, 
        ICharactersicticRepository charactersisticRepository,
        ICurrentUserService currentUserService) : IRequestHandler<CreateSeedlingCommand, string>
    {
        public async Task<string> Handle(CreateSeedlingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var seedling = new Seedlings
                {
                    LocalName = request.LocalName,
                    ScientificName = request.ScientificName,
                    Description = request.Description,
                    //Parent1 = request.MotherID,
                    //Parent2 = request.FatherID,
                    Dob = request.DoB,
                    Create_date = DateTime.UtcNow,
                    Create_by = currentUserService.UserName ?? "system"
                };
                if(await IsExitsSeedling(request.MotherID, cancellationToken) && await IsExitsSeedling(request.FatherID, cancellationToken))
                {
                    seedling.Parent1 = request.MotherID;
                    seedling.Parent1 = request.FatherID;
                }
                else if (await IsExitsSeedling(request.MotherID, cancellationToken) || await IsExitsSeedling(request.FatherID, cancellationToken))
                {
                    seedling.Parent1 = request.MotherID;
                    seedling.Parent1 = request.FatherID;
                }
                else
                {
                    throw new NotFoundException("not found parent");
                }


                    seedlingRepository.Add(seedling);

                var characteristicsEntities = new List<Characteristics>();

                foreach (var characteristic in request.Characteristics)
                {
                    // Find or create the attribute
                    var seedlingAttribute = await seedlingAttributeRepository.FindAsync(
                        x => x.Name.ToLower().Equals(characteristic.SeedlingAttribute.Name.ToLower()),
                        cancellationToken);

                    if (seedlingAttribute == null)
                    {
                        seedlingAttribute = new SeedlingAttributes
                        {
                            Name = characteristic.SeedlingAttribute.Name,
                            Description = characteristic.SeedlingAttribute.Description,
                            Status = true
                        };
                        seedlingAttributeRepository.Add(seedlingAttribute);
                    }
                    var characteristicsEntity = new Characteristics
                    {
                        Value = characteristic.Value,
                        Status = true,
                        SeedlingAttributeID = seedlingAttribute.ID,
                        SeedlingID = seedling.ID
                    };
                    characteristicsEntities.Add(characteristicsEntity);
                }

                // Batch insert characteristics
                foreach (var entity in characteristicsEntities)
                {
                    charactersisticRepository.Add(entity);
                }
                return await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Created Seedling with ID: {seedling.ID}" : "Failed to create Seedling.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }


        async Task<bool> IsExitsSeedling(string id, CancellationToken cancellationToken)
            => (await seedlingRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken));
    }
}
