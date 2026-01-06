using MediatR;
using orchid_backend_net.Application.Materials.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Materials.GetMaterialById
{
    public class GetMaterialByIdQuery : IRequest<MaterialDto?>
    {
        public int MaterialId { get; set; }

        public GetMaterialByIdQuery(int materialId)
        {
            MaterialId = materialId;
        }

        public GetMaterialByIdQuery()
        {
        }
    }

    internal class GetMaterialByIdQueryHandler(IMaterialRepository materialRepository) : IRequestHandler<GetMaterialByIdQuery, MaterialDto?>
    {
        public async Task<MaterialDto?> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
        {
            var material = await materialRepository.FindProjectToAsync<MaterialDto>(
                queryOptions: q => q.Where(ch => ch.ID == request.MaterialId),
                cancellationToken);
            return material;
        }
    }
}
