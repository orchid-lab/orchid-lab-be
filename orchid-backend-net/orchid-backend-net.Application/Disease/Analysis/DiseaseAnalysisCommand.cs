using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.Analysis
{
    public class DiseaseAnalysisCommand : IRequest<AnalysisDTO>, ICommand
    {
        public required byte[] ImageBytes { get; set; }
    }

    internal class DiseaseAnalysisCommandHandler(IOrchidAnalyzerService orchidAnalyzerService, IDiseaseRepository diseaseRepository) : IRequestHandler<DiseaseAnalysisCommand, AnalysisDTO>
    {
        public async Task<AnalysisDTO> Handle(DiseaseAnalysisCommand request, CancellationToken cancellationToken)
        {
            try
            {
                OrchidAnalysisResult result = await orchidAnalyzerService.AnalyzeAsync(request.ImageBytes);
                var diseaseId = result.Disease.Probability.Keys.ToList();

                var disease = await diseaseRepository.FindAllToDictionaryAsync(filter => diseaseId.Contains(filter.ID),
                    key => key.ID, 
                    value => value.Name, 
                    cancellationToken: cancellationToken);
                
                var finalPropabilityMapping = result.Disease.Probability.ToDictionary(
                    prop => disease.GetValueOrDefault(prop.Key, prop.Key),
                    prop => prop.Value);

                return new AnalysisDTO(result.Stage, new DiseaseResultDTO()
                {
                    Predict = result.Disease.Predict,
                    Probability = finalPropabilityMapping,
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
