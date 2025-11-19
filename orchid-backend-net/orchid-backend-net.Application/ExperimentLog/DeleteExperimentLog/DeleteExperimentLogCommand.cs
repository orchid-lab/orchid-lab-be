using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.DeleteExperimentLog
{
    public class DeleteExperimentLogCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public DeleteExperimentLogCommand(string ID)
        {
            this.ID = ID;
        }
    }

    internal class DeleteExperimentLogCommandHandler(IExperimentLogRepository experimentLogRepository) : IRequestHandler<DeleteExperimentLogCommand, string>
    {
        public async Task<string> Handle(DeleteExperimentLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var experimentLog = await experimentLogRepository.FindAsync(x => x.ID.Equals(request.ID), cancellationToken);
                experimentLog.Status = 2;
                return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Deleted ExperimentLog with ID :{request.ID}" : "Failed to delete ExperimentLog.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
