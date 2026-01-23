using FluentValidation;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.UpdateExperimentLogStatus
{
    public class UpdateExperimentLogStatusCommandValidator : AbstractValidator<UpdateExperimentLogStatusCommand>
    {
        public UpdateExperimentLogStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id của thí nghiệm không được để trống.");
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái của thí nghiệm không được để trống.");
        }
    }
}
