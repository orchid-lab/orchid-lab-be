using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.GetMethodInfor
{
    public class GetMethodInforQuery : IRequest<MethodDTO>, IQuery
    {
        public string ID { get; set; }
        public GetMethodInforQuery(string id)
        {
            ID = id;
        }
    }
    internal class GetMethodInforQueryHandler : IRequestHandler<GetMethodInforQuery, MethodDTO>
    {
        private readonly IMethodRepository _methodRepository;
        private readonly IMapper _mapper;
        public GetMethodInforQueryHandler(IMapper mapper, IMethodRepository methodRepository)
        {
            _mapper = mapper;
            _methodRepository = methodRepository;
        }
        public async Task<MethodDTO> Handle(GetMethodInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var method = await this._methodRepository.FindAsync(x => x.ID.Equals(request.ID) && x.Status == true, cancellationToken);
                return method.MapToMethodDTO(_mapper);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
