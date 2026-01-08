using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.Policy
{
    public static class MethodPolicy
    {
        public static async Task ValidateCreateMethod(
           IEnumerable<int> stageDefinitionIds,
           IEnumerable<int> materialIds,
           IEnumerable<int> chemicalIds,
           IStageDefinitionRepository stageRepo,
           IMaterialRepository materialRepo,
           IChemicalsRepository chemicalRepo,
           CancellationToken cancellationToken)
        {
            // StageDefinition
            var stageCount = await stageRepo
                .CountAsync(s => stageDefinitionIds.Contains(s.ID), cancellationToken);

            if (stageCount != stageDefinitionIds.Distinct().Count())
                throw new DomainException("Có StageDefinition không tồn tại.");

            // Material
            if (materialIds.Any())
            {
                var materialCount = await materialRepo
                    .CountAsync(m => materialIds.Contains(m.ID), cancellationToken);

                if (materialCount != materialIds.Distinct().Count())
                    throw new DomainException("Có Material không tồn tại.");
            }

            // Chemical
            if (chemicalIds.Any())
            {
                var chemicalCount = await chemicalRepo
                    .CountAsync(c => chemicalIds.Contains(c.ID), cancellationToken);

                if (chemicalCount != chemicalIds.Distinct().Count())
                    throw new DomainException("Có Chemical không tồn tại.");
            }
        }

        public static async Task EnsureStageExistsAsync(IMethodRepository methodRepository, int methodId, int stageId, CancellationToken ct)
        {
            var exists = await methodRepository.AnyAsync(
                m => m.ID == methodId && m.MethodStages.Any(s => s.ID == stageId), ct);

            if (!exists)
                throw new NotFoundException("Stage không tồn tại trong method.");
        }

        public static async Task EnsureMaterialExistsAsync(IMaterialRepository materialRepository, int materialId, CancellationToken ct)
        {
            if (!await materialRepository.AnyAsync(m => m.ID == materialId, ct))
                throw new NotFoundException("Material không tồn tại.");
        }

        public static async Task EnsureChemicalExistsAsync(IChemicalsRepository chemicalRepository, int chemicalId, CancellationToken ct)
        {
            if (!await chemicalRepository.AnyAsync(c => c.ID == chemicalId, ct))
                throw new NotFoundException("Chemical không tồn tại.");
        }

        public static async Task EnsureSampleRequirementExistsAsync(ISampleRequirementDefinitionRepository sampleRequirementDefinitionRepository, string requirementId, CancellationToken ct)
        {
            if (!await sampleRequirementDefinitionRepository.AnyAsync(r =>  r.ID == requirementId, ct))
                throw new NotFoundException("Sample requirement không tồn tại.");
        }
    }
}
