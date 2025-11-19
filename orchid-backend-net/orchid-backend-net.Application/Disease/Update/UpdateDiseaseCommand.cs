using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.Update
{
    public class UpdateDiseaseCommand(string id, string? name, string? description, string? solution, decimal? infectionRate, bool? isActive) : IRequest<string>, ICommand
    {
        public string Id { get; set; } = id;
        public string? Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public string? Solution { get; set; } = solution;
        public decimal? InfectionRate { get; set; } = infectionRate;
        public bool? IsActive { get; set; } = isActive;
    }

    internal class UpdateDiseaseCommandHandler(IDiseaseRepository diseaseRepository) : IRequestHandler<UpdateDiseaseCommand, string>
    {
        public async Task<string> Handle(UpdateDiseaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var disease = await diseaseRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken);
                disease.Name = request.Name ?? disease.Name;
                disease.Description = request.Description ?? disease.Description;
                disease.Solution = request.Solution ?? disease.Solution;
                disease.InfectedRate = request.InfectionRate ?? disease.InfectedRate;
                disease.Status = request.IsActive ?? disease.Status;
                diseaseRepository.Update(disease);
                return await diseaseRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Disease updated successfully." : "Failed to update disease.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }   
        }
    }
}
