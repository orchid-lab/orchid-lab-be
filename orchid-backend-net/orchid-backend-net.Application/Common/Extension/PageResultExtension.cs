using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Common.Extension
{
    public static class PageResultExtensions
    {
        public static PageResult<T> ToAppPageResult<T>(this IPageResult<T> source)
        {
            return PageResult<T>.Create(
                totalCount: source.TotalCount,
                pageSize: source.PageSize,
                pageNumber: source.PageNo,
                pageCount: source.PageCount,
                data: source.ToList()
            );
        }
    }
}

