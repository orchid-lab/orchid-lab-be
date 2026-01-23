using FluentValidation;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.CreateExperimentLog
{
    public class CreateExperimentLogCommandValidator : AbstractValidator<CreateExperimentLogCommand>
    {
        public CreateExperimentLogCommandValidator()
        {
            Configre();
        }

        private void Configre()
        {
            RuleFor(x => x.MethodId)
                .NotNull()
                .NotEmpty().WithMessage("Phương pháp không được để trống.");
            RuleFor(x => x.BatchesId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Lô không được để trống.");
            RuleFor(x => x.AssignedToTechnicianId)
                .NotNull()
                .NotEmpty().WithMessage("Người thực hiện không được để trống.");
            RuleFor(x => x.ParentAId)
                .NotNull()
                .NotEmpty().WithMessage("Cây giống không được để trống.");
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(100).WithMessage("Tên phải ít hơn 100 ký tự");
            RuleFor(x => x.ExpectedSampleCount)
                .GreaterThan(0).WithMessage("Số lượng mẫu dự kiến phải lớn hơn 0.");
        }
    }
}
