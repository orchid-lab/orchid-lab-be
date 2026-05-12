using MediatR;
using orchid_backend_net.Domain.IRepositories;
using DiseaseEntity = orchid_backend_net.Domain.Entities.Disease; 

namespace orchid_backend_net.Application.Disease.UseCase.CreateDisease
{
    internal class CreateDiseaseCommandHandler(IDiseaseRepository repo)
        : IRequestHandler<CreateDiseaseCommand, string>
    {
        public async Task<string> Handle(CreateDiseaseCommand request, CancellationToken ct)
        {
            if (await repo.ExistsByNameAsync(request.Name, ct))
                throw new ArgumentException($"Tên bệnh '{request.Name}' đã tồn tại.");

            if (await repo.ExistsByCodeAsync(request.Code, ct))
                throw new ArgumentException($"Mã bệnh '{request.Code}' đã tồn tại.");

            var disease = new DiseaseEntity
            {
                Name = request.Name.Trim(),
                Code = request.Code.Trim().ToUpper(),
                Description = request.Description?.Trim() ?? string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            repo.Add(disease);                              
            await repo.UnitOfWork.SaveChangesAsync(ct);

            return "Tạo bệnh thành công.";
        }
    }
}