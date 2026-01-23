using FluentValidation;


namespace orchid_backend_net.Application.ExperimentLog.UseCase.UpdateExperimentLogInformation
{
    public class UpdateExperimentLogInformationCommandValidator : AbstractValidator<UpdateExperimentLogInformationCommand>
    {
        public UpdateExperimentLogInformationCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id của thí nghiệm không được để trống.");
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Tên phải ít hơn 100 kí tự.");
            RuleFor(x => x.ExpectedSampleCount)
                .GreaterThan(0).When(x => x.ExpectedSampleCount.HasValue)
                .WithMessage("Số lượng mẫu mong muốn phải lớn hơn 0.");
        }
    }
}
