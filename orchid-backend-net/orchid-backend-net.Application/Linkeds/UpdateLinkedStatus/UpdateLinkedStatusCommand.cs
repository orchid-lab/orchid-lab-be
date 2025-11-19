using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Linkeds.UpdateLinkedStatus
{
    public class UpdateLinkedStatusCommand(string experimentLogId, string sampleId, int status) : IRequest<string>
    {
        public string ExperimentLogId { get; set; } = experimentLogId;
        public string SampleId { get; set; } = sampleId;
        public int Status { get; set; } = status;
    }

    internal class UpdateLinkedStatusCommandHandler(ILinkedRepository linkedRepository) : IRequestHandler<UpdateLinkedStatusCommand, string>
    {
        public async Task<string> Handle(UpdateLinkedStatusCommand request, CancellationToken cancellationToken)
        {
            var linkeds = linkedRepository.FindAllAsync(x => x.ExperimentLogID.Equals(request.ExperimentLogId) && x.SampleID.Equals(request.SampleId) && x.ProcessStatus != 2, cancellationToken);
            foreach (var linked in linkeds.Result)
            {
                linked.ProcessStatus = request.Status;
                linkedRepository.Update(linked);
            }
            return await linkedRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Linked status updated successfully."
                : "Failed to update linked status.";
        }
    }
}
