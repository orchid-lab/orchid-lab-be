using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.TaskTemplate.GetAllTaskTemplate
{
    public class GetAllTaskTemplateQuery : IRequest<PageResult<TaskTemplateDTO>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Filter { get; set; }
        public GetAllTaskTemplateQuery() { }
        public GetAllTaskTemplateQuery(int pageNumber, int pageSize, string? filter)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            Filter = filter;
        }
    }
    internal class GetAllTaskTemplateQueryHandler : IRequestHandler<GetAllTaskTemplateQuery, PageResult<TaskTemplateDTO>>
    {
        private readonly ITaskTemplatesRepository _tasksRepository;
        private readonly ITaskTemplateDetailsRepository _detailsRepository;
        public GetAllTaskTemplateQueryHandler(ITaskTemplatesRepository tasksRepository, ITaskTemplateDetailsRepository detailsRepository)
        {
            _tasksRepository = tasksRepository;
            _detailsRepository = detailsRepository;
        }

        public async Task<PageResult<TaskTemplateDTO>> Handle(GetAllTaskTemplateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Domain.Entities.TaskTemplates> queryOptions(IQueryable<Domain.Entities.TaskTemplates> query)
                {
                    query = query.Where(x => x.Status);
                    if (!string.IsNullOrEmpty(request.Filter))
                    {
                        query = query.Where(x => x.StageID.Equals(request.Filter) && x.Status);
                    }
                    return query;
                }

                var template = await this._tasksRepository.FindAllProjectToAsync<TaskTemplateDTO>(
                    request.PageNumber,
                    request.PageSize,
                    queryOptions: queryOptions,
                    cancellationToken);


                var list = await this._tasksRepository.FindAllAsync(x => x.Status == true, request.PageNumber, request.PageSize, cancellationToken);

                return template.ToAppPageResult();

            }
            catch (Exception ex) 
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
