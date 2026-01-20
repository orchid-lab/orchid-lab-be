using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Chemicals.UseCase.DeleteChemical
{
    public record DeleteChemicalCommand(int Id) : IRequest<string>;
    internal class DeleteChemicalCommandHandler(IChemicalsRepository chemicalsRepository) : IRequestHandler<DeleteChemicalCommand, string>
    {
        public async Task<string> Handle(DeleteChemicalCommand request, CancellationToken cancellationToken)
        {
            var chemical = await chemicalsRepository.FindAsync(c => c.ID == request.Id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy hóa chất này");
            chemicalsRepository.Remove(chemical);
            return await chemicalsRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? chemical.ID.ToString()
                : "Xóa hóa chất thất bại";
        }
    }
}
