using MediatR;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.GetFailedExperimentLogDetails
{
    internal class GetFailedExperimentLogDetailsQueryHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        IMethodStageDefinitionRepository methodStageDefinitionRepository
    ) :
        IRequestHandler<GetFailedExperimentLogDetailsQuery, PagedFailedExperimentLogResult>
    {
        public async Task<PagedFailedExperimentLogResult> Handle(
            GetFailedExperimentLogDetailsQuery request,
            CancellationToken cancellationToken)
        {
            // Verify method exists
            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken);
            if (method == null)
                throw new NotFoundException($"Method with ID {request.MethodId} not found.");

            // Get all experiment logs
            var allExperimentLogs = await experimentLogRepository.FindAllAsync(cancellationToken);

            // Filter failed experiments for this method
            var failedExperiments = allExperimentLogs
                .Where(x => x.MethodId == request.MethodId &&
                           (x.Status == ExperimentLogStatus.Destroyed ||
                            x.Status == ExperimentLogStatus.Cancelled))
                .ToList();

            // Calculate total count before pagination
            var totalCount = failedExperiments.Count;

            // Sort by newest first (EndDate descending) and apply pagination
            var paginatedFailedExperiments = failedExperiments
                .OrderByDescending(x => x.EndDate)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToList();

            // Transform to DTOs
            var detailedResults = new List<FailedExperimentLogDetailDto>();

            foreach (var experiment in paginatedFailedExperiments)
            {
                // Get the method stage where it failed
                var failedStage = method.MethodStages.FirstOrDefault(ms => ms.Order == experiment.CurrentStageOrder);

                // Get stage definition
                string? stageName = null;
                if (failedStage != null)
                {
                    var stageDefinition = await methodStageDefinitionRepository.FindAsync(
                        sd => sd.ID == failedStage.MethodStageDefinitionId,
                        cancellationToken);

                    if (stageDefinition == null)
                        throw new DomainException($"MethodStageDefinition with ID {failedStage.MethodStageDefinitionId} not found.");

                    stageName = stageDefinition.Name;
                }

                // Build DTO
                var dto = new FailedExperimentLogDetailDto
                {
                    ExperimentLogId = experiment.ID.ToString(), // Ensure ID is treated as a string (GUID)
                    ExperimentLogName = experiment.Name,
                    FailedAtStageOrder = experiment.CurrentStageOrder,
                    FailedAtStageName = stageName ?? "Unknown",
                    SeedlingLocalName = experiment.SeedlingParent?.LocalName ?? "Unknown",
                    SeedlingScientificName = experiment.SeedlingParent?.ScientificName ?? "Unknown",
                    Status = experiment.Status.ToString(),
                    Reason = experiment.Reason ?? "No reason provided",
                    Issues = experiment.Issues ?? "No issues recorded",
                    Recommendations = experiment.Recommendations ?? "No recommendations",
                    FailedDate = experiment.EndDate
                };

                detailedResults.Add(dto);
            }

            return new PagedFailedExperimentLogResult
            {
                TotalCount = totalCount,
                Items = detailedResults,
                Skip = request.Skip,
                Take = request.Take
            };
        }
    }
}
