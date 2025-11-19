using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.GetSampleInfor
{
    public class GetSampleInforQuery : IRequest<SampleDTO>, IQuery
    {
        public string ID { get; set; }
        public GetSampleInforQuery(string iD)
        {
            ID = iD;
        }
        public GetSampleInforQuery() { }
    }

    internal class GetSampleInforQueryHandler : IRequestHandler<GetSampleInforQuery, SampleDTO>
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IMapper _mapper;
        public GetSampleInforQueryHandler(ISampleRepository sampleRepository, IMapper mapper)
        {
            _sampleRepository = sampleRepository;
            _mapper = mapper;
        }

        public async Task<SampleDTO> Handle(GetSampleInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sampleRepository.FindAsync(x => x.ID.Equals(request.ID), cancellationToken);
                return result.MapToSampleDTO(_mapper);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
