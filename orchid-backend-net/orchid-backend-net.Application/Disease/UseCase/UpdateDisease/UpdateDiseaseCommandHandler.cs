using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.UseCase.UpdateDisease
{
    internal class UpdateDiseaseCommandHandler(IDiseaseRepository repo)
        : IRequestHandler<UpdateDiseaseCommand, string>
    {
        public async Task<string> Handle(UpdateDiseaseCommand request, CancellationToken ct)
        {
            var disease = await repo.FindAsync(d => d.ID == request.Id, ct)
                ?? throw new NotFoundException($"Không tìm thấy bệnh với id {request.Id}");

            if (await repo.AnyAsync(
                d => d.Name.ToLower() == request.Name.ToLower() && d.ID != request.Id, ct))
                throw new ArgumentException($"Tên bệnh '{request.Name}' đã tồn tại.");

            if (await repo.AnyAsync(
                d => d.Code.ToLower() == request.Code.ToLower() && d.ID != request.Id, ct))
                throw new ArgumentException($"Mã bệnh '{request.Code}' đã tồn tại.");

            disease.Name = request.Name.Trim();
            disease.Code = request.Code.Trim().ToUpper();
            disease.Description = request.Description?.Trim() ?? string.Empty;
            disease.OnnxClassName = request.OnnxClassName?.Trim();

            repo.Update(disease);                          
            await repo.UnitOfWork.SaveChangesAsync(ct);

            return "Cập nhật thành công.";
        }
    }
}