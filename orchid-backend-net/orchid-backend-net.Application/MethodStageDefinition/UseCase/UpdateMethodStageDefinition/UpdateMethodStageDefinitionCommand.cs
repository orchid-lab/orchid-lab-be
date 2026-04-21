using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.MethodStageDefinition.UseCase.UpdateMethodStageDefinition
{
    public record class UpdateMethodStageDefinitionCommand(int Id, string? Name, string? Description) : IRequest<string>;
    internal class UpdateMethodStageDefinitionCommandHandler(IMethodStageDefinitionRepository methodStageDefinitionRepository) : IRequestHandler<UpdateMethodStageDefinitionCommand, string>
    {
        public async Task<string> Handle(UpdateMethodStageDefinitionCommand request, CancellationToken cancellationToken)
        {
            var methodStageDefinition = await methodStageDefinitionRepository.FindAsync(x => x.Equals(request.Id), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy nội dung của phương thức này");
            methodStageDefinition.Name = request.Name ?? methodStageDefinition.Name;
            methodStageDefinition.Description = request.Description ?? methodStageDefinition.Description;
            methodStageDefinitionRepository.Update(methodStageDefinition);

            return await methodStageDefinitionRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? methodStageDefinition.ID.ToString()
                : "Cập nhật nội dung của phương thức thất bại";
        }
    }
}
