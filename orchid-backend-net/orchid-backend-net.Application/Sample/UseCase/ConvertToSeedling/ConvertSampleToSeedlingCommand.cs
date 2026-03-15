using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.ConvertToSeedling
{
    public record ConvertSampleToSeedlingCommand(
        string SampleId,
        string LocalName,
        string ScientificName,
        string? Description) : IRequest<string>;

    internal class ConvertSampleToSeedlingCommandHandler(
        ISampleRepository sampleRepository,
        ISeedlingRepository seedlingRepository,
        IMonitoringLogRepository monitoringLogRepository,
        ICharacteristicRepository characteristicRepository)
        : IRequestHandler<ConvertSampleToSeedlingCommand, string>
    {
        public async Task<string> Handle(
            ConvertSampleToSeedlingCommand request,
            CancellationToken cancellationToken)
        {
            var sample = await sampleRepository.FindAsync(
                s => s.ID == request.SampleId,
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy mẫu vật.");

            sample.ConvertToSeedling();

            var lastStage = sample.SampleStages
                .OrderByDescending(s => s.StartedAt)
                .First();

            var latestApprovedLog = await monitoringLogRepository
                .FindLatestApprovedLogWithDetailsBySampleStageIdAsync(
                    lastStage.ID, cancellationToken);

            var allCharacteristics = await characteristicRepository
                .FindAllAsync(cancellationToken);

            var characteristicByCode = allCharacteristics
                .Where(c => !string.IsNullOrWhiteSpace(c.Code))
                .ToDictionary(c => c.Code!, c => c.ID);

            var traits = BuildTraits(latestApprovedLog, characteristicByCode);

            var newSeedling = new Seedlings
            {
                LocalName = request.LocalName,
                ScientificName = request.ScientificName,
                Description = request.Description,
                ParentAId = sample.ExperimentLog?.SeedlingParentId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = sample.CreatedBy
            };

            foreach (var (characteristicId, value) in traits)
                newSeedling.AddTrait(characteristicId, value);

            sampleRepository.Update(sample);
            seedlingRepository.Add(newSeedling);

            await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return newSeedling.ID;
        }

        /// <summary>
        /// Maps LogDetails → SeedlingTraits via CharacteristicCode.
        /// Only LogDetails whose SamplesRequirementsDefinition has a non-null CharacteristicCode
        /// that matches a known Characteristic are included.
        /// When the same CharacteristicCode appears more than once, the last value wins.
        /// </summary>
        private static Dictionary<string, decimal> BuildTraits(
            MonitoringLogs? log,
            Dictionary<string, string> characteristicByCode)
        {
            if (log is null)
                return [];

            var traits = new Dictionary<string, decimal>();

            foreach (var logDetail in log.LogDetails)
            {
                var code = logDetail.StageRequirementDefinition
                    ?.SampleRequirementsDefinition
                    ?.CharacteristicCode;

                if (string.IsNullOrWhiteSpace(code))
                    continue;

                if (!characteristicByCode.TryGetValue(code, out var characteristicId))
                    continue;

                // Dictionary assignment naturally overrides duplicates (e.g. SURVIVAL_RATE)
                traits[characteristicId] = logDetail.MeasuredValue;
            }

            return traits;
        }
    }
}
