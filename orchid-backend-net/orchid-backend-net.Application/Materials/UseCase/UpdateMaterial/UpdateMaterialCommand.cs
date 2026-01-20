using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Materials.UseCase.UpdateMaterial
{
    public record UpdateMaterialCommand(int Id, string? Name, string? Description, string? Category, string? Unit) : IRequest<string>;
    internal class UpdateMaterialCommandHandler(IMaterialRepository materialRepository) : IRequestHandler<UpdateMaterialCommand, string>
    {
        public async Task<string> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await materialRepository.FindAsync(m => m.ID == request.Id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy vật liệu này");
            material.Name = request.Name ?? material.Name;
            material.Description = request.Description ?? material.Description;
            material.Category = request.Category ?? material.Category;
            material.Unit = request.Unit ?? material.Unit;
            materialRepository.Update(material);
            return await materialRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? $"{material.ID}"
                : "Cập nhật thất bại";
        }
    }
}
