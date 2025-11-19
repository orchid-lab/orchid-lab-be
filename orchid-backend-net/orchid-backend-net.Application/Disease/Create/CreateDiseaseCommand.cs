using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.Create
{
    public class CreateDiseaseCommand(string name, string desc,
        string solution, decimal infectedRate) : IRequest<string>, ICommand
    {
        public string Name { get; set; } = name;
        public string Description { get; set; }
        public string Solution { get; set; }
        public decimal InfectedRate { get; set; }
    }

    internal class CreateDiseaseCommandHandler(IDiseaseRepository diseaseRepository) : IRequestHandler<CreateDiseaseCommand, string>
    {
        public async Task<string> Handle(CreateDiseaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var disease = new Diseases
                {
                    Name = request.Name,
                    Description = request.Description,
                    Solution = request.Solution,
                    InfectedRate = request.InfectedRate,
                    Status = true // Assuming new diseases are active by default
                };
                diseaseRepository.Add(disease);
                return await diseaseRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                    ? $"Create disease success with id: {disease.ID.ToString()}"
                    : throw new Exception("Failed to create disease.");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
