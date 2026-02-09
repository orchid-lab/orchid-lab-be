using FluentValidation;

namespace orchid_backend_net.Application.LabConfig.UseCase.Update
{
    public class UpdateConfigCommandValidator : AbstractValidator<UpdateConfigCommand>
    {
        public UpdateConfigCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id không được để trống");
        }
    }
}