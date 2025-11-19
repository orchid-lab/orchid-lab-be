using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.GetAllLabRoom
{
    public class GetAllLabRoomQuery : IRequest<PageResult<LabRoomDTO>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetAllLabRoomQuery() { }
        public GetAllLabRoomQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    internal class GetAllLabRoomQueryHandler(ILabRoomRepository labRoomRepository, IMapper mapper) : IRequestHandler<GetAllLabRoomQuery, PageResult<LabRoomDTO>>
    {

        public async Task<PageResult<LabRoomDTO>> Handle(GetAllLabRoomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await labRoomRepository.FindAllAsync(request.PageNumber, request.PageSize, cancellationToken);
                if (result == null)
                    throw new NotFoundException("Not found any LabRoom in the system.");
                return PageResult<LabRoomDTO>.Create(
                                totalCount: result.TotalCount,
                                pageCount: result.PageSize,
                                pageNumber: request.PageNumber,
                                pageSize: request.PageSize,
                                data: result.MapToLabRoomDTOList(mapper)
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
