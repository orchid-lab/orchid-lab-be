using FluentValidation;

namespace orchid_backend_net.Application.Sample.UseCase.UpdateSampleInformation
{
    public class UpdateSampleInformationCommandValidator : AbstractValidator<UpdateSampleInformationCommand>
    {
        public UpdateSampleInformationCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id không được để trống")
                .NotNull().WithMessage("Id không được để trống");
        }
    }
}
