using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.Delete
{
    public class DeleteDiseaseCommand(string id) : IRequest<string>, ICommand
    {
        public string Id { get; set; } = id;
    }

    internal class DeleteDiseaseCommandHandler(IDiseaseRepository diseaseRepository) : IRequestHandler<DeleteDiseaseCommand, string>
    {
        public async Task<string> Handle(DeleteDiseaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var disease = await diseaseRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken);
                disease.Status = false;
                diseaseRepository.Update(disease);
                return await diseaseRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                    ? $"Delete disease success with id: {disease.ID.ToString()}"
                    : throw new Exception("Failed to delete disease.");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
