using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TissueCultureBatch.GetTissueCultureBatchInfor
{
    public class GetTissueCultureBatchInforQuery : IRequest<TissueCultureBatchDTO>, IQuery
    {
        public string ID { get; set; }
        public GetTissueCultureBatchInforQuery(string id)
        {
            ID = id;
        }
    }

    internal class GetTissueCultureBatchInforQueryHandler : IRequestHandler<GetTissueCultureBatchInforQuery, TissueCultureBatchDTO>
    {
        private readonly ITissueCultureBatchRepository _tissueCultureBatchRepository;
        public GetTissueCultureBatchInforQueryHandler(ITissueCultureBatchRepository tissueCultureBatchRepository, ILabRoomRepository labroomRepository)
        {
            this._tissueCultureBatchRepository = tissueCultureBatchRepository;
        }
        public async Task<TissueCultureBatchDTO> Handle(GetTissueCultureBatchInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var obj = await this._tissueCultureBatchRepository.FindProjectToAsync<TissueCultureBatchDTO>(query => query.Where(x => x.ID.Equals(request.ID)), cancellationToken: cancellationToken);
                return obj != null ? obj : throw new NotFoundException($"not found tissue culture batch with ID {request.ID}");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
