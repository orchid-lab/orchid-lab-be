using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.CreateLabRoom
{
    public class CreateLabRoomCommand : IRequest<string>, ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public CreateLabRoomCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    internal class CreateLabRoomCommandHandler(ILabRoomRepository labRoomRepository) : IRequestHandler<CreateLabRoomCommand, string>
    {
        public async Task<string> Handle(CreateLabRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checker = await labRoomRepository.FindAsync(x => x.Name.Equals(request.Name), cancellationToken);
                LabRooms obj = new()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Description = request.Description,
                    Status = true
                };
                labRoomRepository.Add(obj);
                return await labRoomRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Created LabRoom name :{obj.Name}" : "Failed create LabRoom.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
