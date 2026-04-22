using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Images.Dto.Img;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Images.UseCase.GetAll
{
    public class GetAllImageQuery : IRequest<PageResult<ImageDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? TargetId { get; set; }
    }

    internal class GetAllImageQueryHandler(IImageRepository imageRepository) : IRequestHandler<GetAllImageQuery, PageResult<ImageDto>>
    {
        public async Task<PageResult<ImageDto>> Handle(GetAllImageQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Imgs> query(IQueryable<Imgs> queryable)
            {
                if (!string.IsNullOrEmpty(request.TargetId))
                {
                    return queryable.Where(image => image.TargetId == request.TargetId);
                }
                return queryable;
            }

            var images = await imageRepository.FindAllProjectToAsync<ImageDto>(request.PageNumber, request.PageSize, queryOptions: query, cancellationToken);
            return images.ToAppPageResult();
        }
    }
}
