using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Tasks.Dto.Task;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.GetAllTask
{
    public class GetAllTaskQuery : IRequest<PageResult<TaskDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? TechnicianId { get; set; }
        public string? ResearcherId { get; set; }
        /// <summary>
        /// search term only for names
        /// </summary>
        public string? SearchTerm { get; set; }
        public int? StageId { get; set; }
    }

    internal class GetAllTaskQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetAllTaskQuery, PageResult<TaskDto>>
    {
        public async Task<PageResult<TaskDto>> Handle(GetAllTaskQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Tasks> queryOption(IQueryable<Domain.Entities.Tasks> query)
            {
                if (!string.IsNullOrWhiteSpace(request.ResearcherId))
                {
                    query = query.Where(t => t.ResearcherId!.Equals(request.ResearcherId));
                }
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    query = query.Where(t => t.Name.ToLower().Contains(request.SearchTerm.ToLower()));
                }
                if (request.StageId is not null)
                {
                    query = query.Where(t => t.StageId!.Equals(request.StageId));
                }
                if(!string.IsNullOrWhiteSpace(request.TechnicianId))
                {
                    query = query.Where(t => t.TaskAssignment.TechnicianId!.Equals(request.TechnicianId));
                }
                return query.OrderByDescending(x => x.CreatedDate);
            }

            var list = await taskRepository.FindAllProjectToAsync<TaskDto>(
                pageNo: request.PageNumber,
                pageSize: request.PageSize,
                queryOptions: queryOption,
                cancellationToken: cancellationToken);
            return list.ToAppPageResult();
        }
    }
}
