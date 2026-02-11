using FluentValidation;

namespace orchid_backend_net.Application.LabConfig.UseCase.Delete
{
    public class DeleteConfigCommandValidator : AbstractValidator<DeleteConfigCommand>
    {
        public DeleteConfigCommandValidator()
        {
            Configure();
        }
        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id là bắt buộc.");
        }
    }
}
