using MediatR;
using orchid_backend_net.Application.LabRoom.Dto;
using orchid_backend_net.Application.SampleRequirementDefinition.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.LabRoom.UseCase.GetLabRoomByIdQuery
{
    public class GetLabRoomByIdQuery : IRequest<LabRoomDto>
     {
        public int Id { get; set; }
        public GetLabRoomByIdQuery(int id)
        {
            Id = id;
        }
        public GetLabRoomByIdQuery()
        {
        }
    }
    internal class GetLabRoomByIdQueryHandler(ILabRoomRepository labRoomRepository) : IRequestHandler<GetLabRoomByIdQuery, LabRoomDto>
    {
        public async Task<LabRoomDto> Handle(GetLabRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var labroom = await labRoomRepository.FindProjectToAsync<LabRoomDto>(
                queryOptions: q => q.Where(s => s.ID == request.Id),
                cancellationToken);
            if (labroom is null)
                throw new NotFoundException("Không tìm thấy phòng lab này.");
            return labroom;
        }
    }
}
