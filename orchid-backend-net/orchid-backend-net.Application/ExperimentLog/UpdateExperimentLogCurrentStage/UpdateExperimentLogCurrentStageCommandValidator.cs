using FluentValidation;

namespace orchid_backend_net.Application.ExperimentLog.UpdateExperimentLogCurrentStage
{
    public class UpdateExperimentLogCurrentStageCommandValidator : AbstractValidator<UpdateExperimentLogCurrentStageCommand>
    {
        public UpdateExperimentLogCurrentStageCommandValidator()
        {
            RuleFor(x => x.ExperimentLogId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Experiment Log ID cannot be empty.");
            RuleFor(x => x.CurrentStage)
                .NotNull()
                .NotEmpty()
                .WithMessage("Current Stage cannot be empty.");
        }
    }
}
