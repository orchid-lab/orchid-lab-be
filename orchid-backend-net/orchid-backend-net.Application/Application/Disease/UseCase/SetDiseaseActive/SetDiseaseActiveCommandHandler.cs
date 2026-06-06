using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;
namespace orchid_backend_net.Application.Disease.UseCase.SetDiseaseActive
{
    internal class SetDiseaseActiveCommandHandler(IDiseaseRepository repo)
        : IRequestHandler<SetDiseaseActiveCommand, string>
    {
        public async Task<string> Handle(SetDiseaseActiveCommand request, CancellationToken ct)
        {
            var disease = await repo.FindAsync(d => d.ID == request.Id, ct)
                ?? throw new NotFoundException($"Không tìm thấy bệnh với id {request.Id}");

            disease.IsActive = request.IsActive;

            repo.Update(disease);
            await repo.UnitOfWork.SaveChangesAsync(ct);

            return request.IsActive ? "Đã kích hoạt bệnh thành công." : "Đã vô hiệu hóa bệnh thành công.";
        }
    }
}