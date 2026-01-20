using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Materials.UseCase.CreateMaterial
{
    public record CreateMaterialCommand(string Name, string Category, string? Description, string Unit) : IRequest<string>;
    internal class CreateMaterialCommandHandler(IMaterialRepository materialRepository) : IRequestHandler<CreateMaterialCommand, string>
    {
        public async Task<string> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {
            var isDuplicated = await materialRepository.AnyAsync(m => m.Name == request.Name, cancellationToken);
            if (isDuplicated)
                throw new DuplicateException("Material đã tồn tại");

            var material = new Domain.Entities.Materials
            {
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                Unit = request.Unit
            };
            materialRepository.Add(material);
            return await materialRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? $"{material.ID}"
                : "Tạo thất bại";
        }
    }
}
