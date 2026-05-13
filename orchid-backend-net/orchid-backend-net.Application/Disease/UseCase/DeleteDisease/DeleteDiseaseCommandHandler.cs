using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.UseCase.DeleteDisease
{
    internal class DeleteDiseaseCommandHandler(IDiseaseRepository repo)
        : IRequestHandler<DeleteDiseaseCommand, string>
    {
        public async Task<string> Handle(DeleteDiseaseCommand request, CancellationToken ct)
        {
            var disease = await repo.FindAsync(d => d.ID == request.Id, ct)
                ?? throw new NotFoundException($"Không tìm thấy bệnh với id {request.Id}");

            disease.IsActive = false;

            repo.Update(disease);                           
            await repo.UnitOfWork.SaveChangesAsync(ct);

            return "Xóa thành công.";
        }
    }
}