using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Linkeds.CreateLinkedsCommand
{
    public class CreateLinkedsCommand : IRequest<Unit>, ICommand
    {
        public string? ExperimentLogID { get; set; }
        public string? StageID { get; set; }
        public string? SampleID { get; set; }
        public string? TaskID { get; set; }
    }

    internal class CreateLinkedsCommandHandler(ILinkedRepository linkedRepository) : IRequestHandler<CreateLinkedsCommand, Unit>
    {
        public async Task<Unit> Handle(CreateLinkedsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var linkeds = new Domain.Entities.Linkeds
                {
                    ExperimentLogID = request.ExperimentLogID,
                    StageID = request.StageID,
                    SampleID = request.SampleID,
                    TaskID = request.TaskID,
                    ProcessStatus = 0,
                };
                linkedRepository.Add(linkeds);
                return Unit.Value;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }   
        }
    }
}
