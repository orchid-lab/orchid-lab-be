using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.InfectedSample.CreateInfectedSampleCommand;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.DeleteSample
{
    public class DeleteSampleCommand(string id, string reason,
        string diseaseId) : IRequest<string>, ICommand
    {
        public string Id { get; set; } = id;
        /// <summary>
        /// In this string has one kind of message
        /// Nhiễm bệnh => infected sample
        /// </summary>
        public string Reason { get; set; } = reason;
        public string DiseaseId { get; set; } = diseaseId;
    }

    internal class DeleteSampleCommandHandler(ISampleRepository sampleRepository, ISender sender, ILinkedRepository linkedRepository) : IRequestHandler<DeleteSampleCommand, string>
    {
        public async Task<string> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sample = await sampleRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken);
                sample.Reason = request.Reason;
                sample.Status = 2; //deleted
                sampleRepository.Update(sample);
                var result = await sender.Send(new CreateInfectedSampleCommand
                {
                    SampleID = request.Id,
                    DiseaseID = request.DiseaseId
                }, cancellationToken);
                var allLinkeds = await linkedRepository.FindAllAsync(x => x.SampleID.Equals(request.Id), cancellationToken);
                foreach (var linked in allLinkeds)
                {
                    linked.ProcessStatus = 2; //deleted
                    linkedRepository.Update(linked);
                }
                if (!result) return $"Failed delete sample with ID :{request.Id}";
                return await sampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Deleted sample ID :{request.Id}" : $"Failed delete sample with ID :{request.Id}";
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
