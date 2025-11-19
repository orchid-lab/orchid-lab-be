using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.GetExperimentLogInfor
{
    public class GetExperimentLogInforQuery : IRequest<ExperimentLogDTO>, IQuery
    {
        public required string ID { get; set; }
        public GetExperimentLogInforQuery(string id)
        {
            ID = id;
        }
        public GetExperimentLogInforQuery() { }
    }

    internal class GetExperimentLogInforQueryHandler(IExperimentLogRepository experimentLogRepository) : IRequestHandler<GetExperimentLogInforQuery, ExperimentLogDTO>
    {

        public async Task<ExperimentLogDTO> Handle(GetExperimentLogInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var obj = await experimentLogRepository.FindProjectToAsync<ExperimentLogDTO>(query => query.Where(x => x.ID.Equals(request.ID)), cancellationToken: cancellationToken);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
