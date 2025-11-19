using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TissueCultureBatch.CreateTissueCultureBatch
{
    public class CreateTissueCultureBatchCommand : IRequest<string>, ICommand
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string LabRoom { get; set; }
        public CreateTissueCultureBatchCommand(string LabRoom, string name, string description)
        {

            this.LabRoom = LabRoom;
            this.Name = name;
            this.Description = description;
        }
    }

    internal class CreateTissueCultureBatchCommandHandler : IRequestHandler<CreateTissueCultureBatchCommand, string>
    {
        private readonly ITissueCultureBatchRepository _tissueCultureBatchRepository;
        private readonly ILabRoomRepository _labRoomRepository;
        public CreateTissueCultureBatchCommandHandler(ITissueCultureBatchRepository tissueCultureBatchRepository, ILabRoomRepository labRoomRepository)
        {
            this._tissueCultureBatchRepository = tissueCultureBatchRepository;
            this._labRoomRepository = labRoomRepository;
        }
        public async Task<string> Handle(CreateTissueCultureBatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if ((await this._labRoomRepository.FindAsync(x => x.ID.Equals(request.LabRoom), cancellationToken)) == null)
                    throw new NotFoundException($"Not found labroom with ID {request.LabRoom}");
                var tissue = new TissueCultureBatches()
                {
                    LabRoomID = request.LabRoom,
                    Name = request.Name,
                    Status = true,
                    Description = request.Description,
                };
                this._tissueCultureBatchRepository.Add(tissue);
                return (await this._tissueCultureBatchRepository.UnitOfWork.SaveChangesAsync(cancellationToken)) > 0 ? $"Created tissue cuture batch name : {tissue.Name}" : "Failed to create tissue culture batch";
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message}");
            }
        }
    }
}
