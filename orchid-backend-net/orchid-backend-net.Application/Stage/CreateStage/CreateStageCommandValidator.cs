using FluentValidation;

namespace orchid_backend_net.Application.Stage.CreateStage
{
    public class CreateStageCommandValidator : AbstractValidator<CreateStageCommand>
    {
        public CreateStageCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Stage name is required.")
                .MaximumLength(100).WithMessage("Stage name must not exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Stage description must not exceed 500 characters.");
            RuleFor(x => x.DateOfProcessing)
                .GreaterThan(0).WithMessage("Date of processing must be a positive integer.");
            RuleFor(x => x.Step)
                .GreaterThan(0).WithMessage("Step must be a positive integer.");
            RuleFor(x => x.ElementInStages)
                .NotEmpty().WithMessage("At least one element in stage is required.");
            RuleForEach(x => x.ElementInStages)
                .NotEmpty().WithMessage("Element ID cannot be empty.");
            RuleForEach(x => x.Referents)
                .NotNull().WithMessage("Referents cannot be null.")
                .SetValidator(new CreateReferentInStageValidator());
        }
    }

    public class CreateReferentInStageValidator : AbstractValidator<CreateReferentInStage>
    {
        public CreateReferentInStageValidator()
        {
            Configure();
        }
        private void Configure()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Referent name is required.")
               .MaximumLength(100).WithMessage("Referent name must not exceed 100 characters.");
            RuleFor(x => x.Unit)
                .NotEmpty()
                .NotNull()
                .WithMessage("Measurement Unit cannot be null or empty.");
            RuleFor(x => x.ValueFrom)
                .GreaterThanOrEqualTo(0).WithMessage("Value from must be non-negative.");
            RuleFor(x => x.ValueTo)
                .GreaterThanOrEqualTo(0).WithMessage("Value to must be non-negative.")
                .GreaterThan(x => x.ValueFrom).WithMessage("Value to must be greater than value from.");
        }
    }
}
