using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Materials.UseCase.DeleteMaterial
{
    public record DeleteMaterialCommand(int Id) : IRequest<string>;
    public class DeleteMaterialCommandHandler(IMaterialRepository materialRepository) : IRequestHandler<DeleteMaterialCommand, string>
    {
        public async Task<string> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await materialRepository.FindAsync(x => x.ID == request.Id, cancellationToken);
            if (material is null)
                return "Material không tồn tại";
            materialRepository.Remove(material);
            return await materialRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Xóa thành công"
                : "Xóa thất bại";
        }
    }
}
