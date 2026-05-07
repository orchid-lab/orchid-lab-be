using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.MethodStageDefinition.UseCase.DeleteMethodStageDefinition
{
    public class DeleteMethodStageDefinitionCommandValidator : AbstractValidator<DeleteMethodStageDefinitionCommand>
    {
        public DeleteMethodStageDefinitionCommandValidator() 
        {
            Configuration();    
        }
        private void Configuration()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Id phải lớn hơn 0");
        }
    }
}
