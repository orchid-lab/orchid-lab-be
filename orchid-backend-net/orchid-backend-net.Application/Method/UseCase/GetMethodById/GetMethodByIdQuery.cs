using MediatR;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.GetMethodById
{
    public class GetMethodByIdQuery : IRequest<MethodDetailDto>
    {
        public required int Id { get; set; }
        public GetMethodByIdQuery() { }

        public GetMethodByIdQuery(int id)
        {
            Id = id;
        }
    }

    internal class GetMethodByIdQueryHandler(IMethodRepository methodRepository) : IRequestHandler<GetMethodByIdQuery, MethodDetailDto>
    {
        public async Task<MethodDetailDto> Handle(GetMethodByIdQuery request, CancellationToken cancellationToken)
        {
            var method = await methodRepository.FindProjectToAsync<MethodDetailDto>(
                q => q.Where(m => m.ID == request.Id),
                cancellationToken);
            if (method is null)
                throw new NotFoundException("Không tìm thấy method này.");
            return method;
        }
    }
}
