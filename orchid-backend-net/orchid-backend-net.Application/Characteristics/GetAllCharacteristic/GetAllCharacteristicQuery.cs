using MediatR;
using orchid_backend_net.Application.Characteristics.Dto;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Characteristics.GetAllCharacteristic
{
    public class GetAllCharacteristicQuery : IRequest<PageResult<CharacteristicDto>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public GetAllCharacteristicQuery() { }
        public GetAllCharacteristicQuery(int pageNo, int pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
        }
    }

    internal class GetAllCharacteristicQueryHandler(ICharacteristicRepository characteristicRepository) : IRequestHandler<GetAllCharacteristicQuery, PageResult<CharacteristicDto>>
    {
        public async Task<PageResult<CharacteristicDto>> Handle(GetAllCharacteristicQuery request, CancellationToken cancellationToken)
        {
            var characteristics = await characteristicRepository.FindAllProjectToAsync<CharacteristicDto>(pageNo: request.PageNo, pageSize: request.PageSize, cancellationToken: cancellationToken);
            return characteristics.ToAppPageResult();
        }
    }
}
