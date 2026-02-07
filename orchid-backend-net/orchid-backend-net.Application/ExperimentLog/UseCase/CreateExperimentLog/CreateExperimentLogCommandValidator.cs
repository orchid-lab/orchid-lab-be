using FluentValidation;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.CreateExperimentLog
{
    public class CreateExperimentLogCommandValidator : AbstractValidator<CreateExperimentLogCommand>
    {
        private readonly IConfigRepository _configRepository;
        public CreateExperimentLogCommandValidator(IConfigRepository configRepository)
        {
            _configRepository = configRepository; 
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
            RuleFor(x => x.ExpectedSampleCount)
                .MustAsync(async (sampleCount, cancellationToken) => await IsValidSampleCount(sampleCount, cancellationToken))
                .WithMessage("Số lượng mẫu dự kiến vượt quá giới hạn cho phép.");
        }

        private async Task<bool> IsValidSampleCount(int sampleCount, CancellationToken cancellationToken)
        {
            var config = await _configRepository.FindAsync(c => c.ConfigName == "MaxSampleCountPerExperimentLog", cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy config để có thể tạo");

            var maxSampleCount = (int)config.Value;
            return sampleCount <= maxSampleCount;
        }
    }
}
