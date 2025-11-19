using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskAttribute.CreateTaskAttribute
{
    public class CreateTaskAttributeCommandValidator : AbstractValidator<CreateTaskAttributeCommand>
    {
        public CreateTaskAttributeCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name cannot be null or empty.");
            RuleFor(x => x.MeasurementUnit)
                .NotEmpty()
                .NotNull()
                .WithMessage("Measurement Unit cannot be null or empty.");
            RuleFor(x => x.Value)
                .NotEmpty()
                .NotNull()
                .WithMessage("Value cannot be null or empty");
        }
    }
}
