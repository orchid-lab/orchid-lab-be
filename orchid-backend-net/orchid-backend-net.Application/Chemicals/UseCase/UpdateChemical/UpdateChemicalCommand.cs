using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Chemicals.UseCase.UpdateChemical
{
    public record UpdateChemicalCommand(int Id,
        string? Name,
        string? Category,
        string? Description,
        string? Unit) : IRequest<string>;

    internal class UpdateChemicalCommandHandler(IChemicalsRepository chemicalsRepository) : IRequestHandler<UpdateChemicalCommand, string>
    {
        public async Task<string> Handle(UpdateChemicalCommand request, CancellationToken cancellationToken)
        {
            var chemical = await chemicalsRepository.FindAsync(c => c.ID == request.Id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy hóa chất này");
            chemical.Name = request.Name ?? chemical.Name;
            chemical.Category = request.Category ?? chemical.Category;
            chemical.Description = request.Description ?? chemical.Description;
            chemical.ConcentrationUnit = request.Unit ?? chemical.ConcentrationUnit;
            chemicalsRepository.Update(chemical);

            return await chemicalsRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? chemical.ID.ToString()
                : "Cập nhật hóa chất thất bại";
        }
    }
}
