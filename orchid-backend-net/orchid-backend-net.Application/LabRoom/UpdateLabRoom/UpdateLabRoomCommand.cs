using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.UpdateLabRoom
{
    public class UpdateLabRoomCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public UpdateLabRoomCommand(string ID, string? Name, string? Description)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
        }
    }

    internal class UpdateLabRoomCommandHandler(ILabRoomRepository labRoomRepository) : IRequestHandler<UpdateLabRoomCommand, string>
    {
        public async Task<string> Handle(UpdateLabRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var labroom = await labRoomRepository.FindAsync(x => x.ID.Equals(request.ID));
                if (labroom == null)
                    throw new Exception($"Not found labroom with ID : {request.ID}.");
                labroom.Name = request.Name ?? labroom.Name;
                labroom.Description = request.Description ?? labroom.Description;
                labRoomRepository.Update(labroom);
                return await labRoomRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Updated labroom with ID :{request.ID}" : $"Failed to update labroom with ID :{request.ID}";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
