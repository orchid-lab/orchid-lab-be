using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.MethodStageDefinition.UseCase.UpdateMethodStageDefinition
{
    public class UpdateMethodStageDefinitionCommandValidator : AbstractValidator<UpdateMethodStageDefinitionCommand>
    {
        public UpdateMethodStageDefinitionCommandValidator() 
        {
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(command => !string.IsNullOrEmpty(command.Name))
                .WithMessage("Tên phải dưới 100 ký tự");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(command => !string.IsNullOrEmpty(command.Description))
                .WithMessage("Chú thích phải dưới 500 ký tự");
        }
    }
}
