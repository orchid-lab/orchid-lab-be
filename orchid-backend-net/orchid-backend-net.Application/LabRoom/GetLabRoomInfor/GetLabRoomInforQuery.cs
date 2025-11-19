using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.GetLabRoomInfor
{
    public class GetLabRoomInforQuery : IRequest<LabRoomDTO>, IQuery
    {
        public string ID { get; set; }
        public GetLabRoomInforQuery(string iD)
        {
            ID = iD;
        }
    }

    internal class GetLabRoomInforQueryHandler(ILabRoomRepository labRoomRepository, IMapper mapper) : IRequestHandler<GetLabRoomInforQuery, LabRoomDTO>
    {
        public async Task<LabRoomDTO> Handle(GetLabRoomInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var labroom = await labRoomRepository.FindAsync(x => x.ID.Equals(request.ID) && x.Status == true, cancellationToken);
                return labroom.MapToLabRoomDTO(mapper);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
