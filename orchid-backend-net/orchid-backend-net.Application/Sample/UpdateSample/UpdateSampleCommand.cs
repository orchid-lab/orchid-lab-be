using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UpdateSample
{
    public class UpdateSampleCommand(string id, string name, DateOnly dob, int sampleStatus, string? reason) : IRequest<string>
    {
        public string Id { get; set; } = id;
        public string Name { get; set; } = name;
        public DateOnly DoB { get; set; } = dob;
        public int SampleStatus { get; set; } = sampleStatus;
        public string? Reason { get; set; } = reason;
    }

    internal class UpdateSampleCommandHandler(ISampleRepository sampleRepository) : IRequestHandler<UpdateSampleCommand, string>
    {
        public async Task<string> Handle(UpdateSampleCommand request, CancellationToken cancellationToken)
        {
            var sample = await sampleRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken);
            sample.Name = request.Name;
            sample.Dob = request.DoB;
            sample.Status = request.SampleStatus;
            sample.Reason = request.Reason;
            sampleRepository.Update(sample);
            return await sampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Update sample with id: {request.Id} succeed" : "Failed to update";
        }
    }
}
