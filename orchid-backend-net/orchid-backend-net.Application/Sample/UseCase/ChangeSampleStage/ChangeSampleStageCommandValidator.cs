using FluentValidation;

namespace orchid_backend_net.Application.Sample.UseCase.ChangeSampleStage
{
    public class ChangeSampleStageCommandValidator : AbstractValidator<ChangeSampleStageCommand>
    {
        public ChangeSampleStageCommandValidator()
        {
            RuleFor(x => x.SampleId)
                .NotEmpty().WithMessage("SampleId không được để trống");
        }
    }
}
