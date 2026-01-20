using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Chemicals.UseCase.CreateChemicals
{
    public record CreateChemicalCommand(string Name, string Category, string Description, string Unit) : IRequest<string>;
    internal class CreateChemicalCommandHandler(IChemicalsRepository chemicalsRepository) : IRequestHandler<CreateChemicalCommand, string>
    {
        
        public async Task<string> Handle(CreateChemicalCommand request, CancellationToken cancellationToken)
        {
            var isDuplicated = await chemicalsRepository.AnyAsync(c => c.Name == request.Name, cancellationToken);
            if (isDuplicated)
            {
                throw new DuplicateException("Chemical with the same name already exists.");
            }

            var chemical = new Domain.Entities.Chemicals
            {
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ConcentrationUnit = request.Unit
            };
            chemicalsRepository.Add(chemical);
            return await chemicalsRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? chemical.ID.ToString()
                : throw new Exception("Create chemical failed.");
        }
    }
}
