namespace orchid_backend_net.Application.Common.Pagination
{
    public class PageResult<T>
    {
        public PageResult()
        {
            Data = null!;
        }

        public static PageResult<T> Create(
            int totalCount,
            int pageCount,
            int pageSize,
            int pageNumber,
            IEnumerable<T> data)
        {
            return new PageResult<T>
            {
                TotalCount = totalCount,
                PageCount = pageCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                Data = data,
            };
        }

        public int TotalCount { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}
