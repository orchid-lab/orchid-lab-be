using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.DeleteLabRoom
{
    public class DeleteLabRoomCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public DeleteLabRoomCommand(string ID)
        {
            this.ID = ID;
        }
    }

    internal class DeleteLabRoomCommandHandler(ILabRoomRepository labRoomRepository) : IRequestHandler<DeleteLabRoomCommand, string>
    {

        public async Task<string> Handle(DeleteLabRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var labroom = await labRoomRepository.FindAsync(x => x.ID.Equals(request.ID) && x.Status == true, cancellationToken);
                labroom.Status = false;
                labRoomRepository.Update(labroom);
                return await labRoomRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Deleted LabRoom with ID :{request.ID}" : $"Failed to delete LabRoom with ID : {request.ID}";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
