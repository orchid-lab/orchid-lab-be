using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.UseCase.SetDiseaseActive;

public record SetDiseaseActiveCommand(int Id, bool IsActive) : IRequest<string>;

internal class SetDiseaseActiveCommandHandler(IDiseaseRepository diseaseRepository)
    : IRequestHandler<SetDiseaseActiveCommand, string>
{
    public async Task<string> Handle(SetDiseaseActiveCommand request, CancellationToken cancellationToken)
    {
        var disease = await diseaseRepository.FindAsync(
            d => d.ID == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Không tìm thấy bệnh với id: {request.Id}");

        disease.IsActive = request.IsActive;
        diseaseRepository.Update(disease);

        var isSaved = await diseaseRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
        if (!isSaved)
            throw new InvalidOperationException("Cập nhật thất bại");

        return request.IsActive ? "Đã kích hoạt bệnh" : "Đã vô hiệu hóa bệnh";
    }
}