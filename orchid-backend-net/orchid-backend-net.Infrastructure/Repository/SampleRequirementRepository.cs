using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class SampleRequirementRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<SamplesRequirementsDefinition, SamplesRequirementsDefinition, OrchidDbContext>(dbContext, mapper), ISampleRequirementDefinitionRepository
    {
    }
}
