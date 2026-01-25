using FluentValidation;

namespace orchid_backend_net.Application.Sample.UseCase.DestroyBecauseOfDisease
{
    public class DestroySampleBecauseOfDiseaseCommandValidator : AbstractValidator<DestroySampleBecauseOfDiseaseCommand>
    {
        public DestroySampleBecauseOfDiseaseCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id không được để trống");
        }
    }
}
