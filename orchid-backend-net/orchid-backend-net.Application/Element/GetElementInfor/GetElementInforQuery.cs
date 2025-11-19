using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.ExperimentLog;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Element.GetElementInfor
{
    public class GetElementInforQuery : IRequest<ElementDTO>, IQuery
    {
        public string ID { get; set; }
        public GetElementInforQuery(string id)
        {
            this.ID = id;
        }
        public GetElementInforQuery() { }
    }

    public class GetElementInforQueryHandler(IElementRepositoty elementRepositoty, IMapper mapper) : IRequestHandler<GetElementInforQuery, ElementDTO>
    {
        public async Task<ElementDTO> Handle(GetElementInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var element = await elementRepositoty.FindProjectToAsync<ElementDTO>(query => query.Where(x => x.ID.Equals(request.ID)), cancellationToken: cancellationToken);
                return element;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
