using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.LabRoom.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.UseCase.GetAllLabRoom
{
    public class GetAllLabRoomQuery : IRequest<PageResult<LabRoomDto>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public GetAllLabRoomQuery(int pageNo, int pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
        }
        public GetAllLabRoomQuery()
        {
        }
    }
    internal class GetAllLabRoomQueryHandler(ILabRoomRepository labRoomRepository) : IRequestHandler<GetAllLabRoomQuery, PageResult<LabRoomDto>>
    {
        public async Task<PageResult<LabRoomDto>> Handle(GetAllLabRoomQuery request, CancellationToken cancellationToken)
        {
            var labroom = await labRoomRepository.FindAllProjectToAsync<LabRoomDto>(
                request.PageNo,
                request.PageSize,
                null,
                cancellationToken);
            return labroom.ToAppPageResult();
        }
    }
}
