using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.MethodStageDefinition.UseCase.DeleteMethodStageDefinition
{
    public record class DeleteMethodStageDefinitionCommand(int Id) : IRequest<string>;
    internal class DeleteMethodStageDefinitionCommandHandler(IMethodStageDefinitionRepository methodStageDefinitionRepository) : IRequestHandler<DeleteMethodStageDefinitionCommand, string>
    {
        public async Task<string> Handle(DeleteMethodStageDefinitionCommand request, CancellationToken cancellationToken)
        {
            var methodStageDefinition = await methodStageDefinitionRepository.FindAsync(c => c.ID == request.Id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy nội dung của phương thức này");
            methodStageDefinitionRepository.Remove(methodStageDefinition);
            return await methodStageDefinitionRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? methodStageDefinition.ID.ToString()
                : "Xóa nội dung của phương thức thất bại";
        }
    }


}
