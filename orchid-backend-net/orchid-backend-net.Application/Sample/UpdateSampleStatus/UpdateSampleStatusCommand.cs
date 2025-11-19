using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UpdateSampleStatus
{
    public class UpdateSampleStatusCommand(Guid sampleId, int status) : IRequest<string>
    {
        public Guid SampleId { get; set; } = sampleId;
        public int Status { get; set; } = status;
    }

    internal class UpdateSampleStatusCommandHandler(ISampleRepository sampleRepository) : IRequestHandler<UpdateSampleStatusCommand, string>
    {
        public async Task<string> Handle(UpdateSampleStatusCommand request, CancellationToken cancellationToken)
        {
            var sample = await sampleRepository.FindAsync(x => x.ID.Equals(request.SampleId), cancellationToken);
            sample.Status = request.Status;
            sampleRepository.Update(sample);
            return await sampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? sample.ID.ToString() : string.Empty;
        }
    }
}
