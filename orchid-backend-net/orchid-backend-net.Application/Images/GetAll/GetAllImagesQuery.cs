using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Images.GetAll
{
    public class GetAllImagesQuery(int pageNo, int pageSize, string? reportId) : IRequest<PageResult<ImagesDTO>>, IQuery
    {
        public int PageNumber { get; set; } = pageNo;
        public int PageSize { get; set; } = pageSize;

        /// <summary>
        /// based on report ID or image URI
        /// </summary>
        public string? ReportId { get; set; } = reportId;
    }

    internal class GetAllImagesQueryHandler(IImageRepository imageRepository) : IRequestHandler<GetAllImagesQuery, PageResult<ImagesDTO>>
    {
        public async Task<PageResult<ImagesDTO>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Imgs> queryOptions(IQueryable<Imgs> query)
            {
                query = query.Where(x => x.Status == true);
                if(!string.IsNullOrWhiteSpace(request.ReportId))
                    query = query.Where(x => x.ReportID.ToLower().Contains(request.ReportId.ToLower()) && x.Status);
                return query;
            }

            var images = await imageRepository.FindAllProjectToAsync<ImagesDTO>(
                request.PageNumber,
                request.PageSize,
                queryOptions,
                cancellationToken: cancellationToken);

            return images.ToAppPageResult();
        }
    }
}
