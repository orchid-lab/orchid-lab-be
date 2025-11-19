using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Hybridzations
{
    public class CreateHybridzationCommand(List<string> hybridzations, string experimentLogId) : IRequest<Unit>, ICommand
    {
        public List<string> Hybridization { get; set; } = hybridzations;
        public string ExperimentLogID { get; set; } = experimentLogId;
    }

    internal class CreateHybridzationCommandHandler(IHybridizationRepository hybridizationRepository) : IRequestHandler<CreateHybridzationCommand, Unit>
    {
        public async Task<Unit> Handle(CreateHybridzationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var seedling in request.Hybridization)
                {
                    var parent = new Domain.Entities.Hybridizations()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ExperimentLogID = request.ExperimentLogID,
                        ParentID = seedling,
                        Status = true,
                    };
                    hybridizationRepository.Add(parent);
                }
                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error creating hybridization: {ex.Message}", ex);
            }
        }
    }
}
