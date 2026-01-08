using MediatR;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Application.Method.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.CreateMethod
{
    public record CreateMethodCommand(string Name, string? Description, List<CreateMethodDto> CreateMethodDtos) : IRequest<string>;
    internal class CreateMethodCommandHandler(IMethodRepository methodRepository) : IRequestHandler<CreateMethodCommand, string>
    {
        public async Task<string> Handle(CreateMethodCommand request, CancellationToken cancellationToken)
        {
            var isMethodDuplicated = await methodRepository
                .AnyAsync(m => m.Name.ToLower().Equals(request.Name.ToLower()), cancellationToken);
            if (isMethodDuplicated)
            {
                throw new DuplicateException("Method này đã trùng");
            }
            var newMethod = new Methods() 
            { 
                Name = request.Name, 
                Description = request.Description 
            };
            MethodHelper.AddMethodWithResourceHelper(newMethod, request.CreateMethodDtos);
            methodRepository.Add(newMethod);
            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 
                ? "Tạo method thành công." :
                "Tạo method thất bại.";
        }
    }
}
