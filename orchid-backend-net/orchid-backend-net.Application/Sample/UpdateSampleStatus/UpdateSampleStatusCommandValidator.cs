using FluentValidation;

namespace orchid_backend_net.Application.Sample.UpdateSampleStatus
{
    public class UpdateSampleStatusCommandValidator : AbstractValidator<UpdateSampleStatusCommand>
    {
        public UpdateSampleStatusCommandValidator()
        {
            RuleFor(x => x.SampleId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Sample ID cannot be empty.");
            RuleFor(x => x.Status)
                .NotNull()
                .NotEmpty()
                .InclusiveBetween(0, 3).WithMessage("Status must be between 0 and 3.");
        }
    }
}
