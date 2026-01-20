using FluentValidation;
using orchid_backend_net.Application.Batch.Policy;

namespace orchid_backend_net.Application.Batch.UseCase.CreateBatch
{
    public class CreateBatchCommandValidator : AbstractValidator<CreateBatchCommand>
    {
        public CreateBatchCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.LabRoomId)
                .GreaterThan(0).WithMessage("LabRoomId phải lớn hơn 0");
            RuleFor(x => x.BatchName)
                .NotEmpty().WithMessage("BatchName không được để trống")
                .MaximumLength(100).WithMessage("BatchName không được vượt quá 100 ký tự");
            RuleFor(x => x.BatchSizeWidth)
                .GreaterThan(0).WithMessage("BatchSizeWidth phải lớn hơn 0");
            RuleFor(x => x.BatchSizeHeight)
                .GreaterThan(0).WithMessage("BatchSizeHeight phải lớn hơn 0");
            RuleFor(x => x.WidthUnit)
                .NotEmpty().WithMessage("WidthUnit không được để trống")
                .MaximumLength(50).WithMessage("WidthUnit không được vượt quá 50 ký tự")
                .Must(unit => BatchPolicy.IsValidUnit(unit))
                .WithMessage("Unit không hợp lệ");
            RuleFor(x => x.HeightUnit)
                .NotEmpty().WithMessage("HeightUnit không được để trống")
                .MaximumLength(50).WithMessage("HeightUnit không được vượt quá 50 ký tự")
                .Must(unit => BatchPolicy.IsValidUnit(unit))
                .WithMessage("Unit không hợp lệ");
        }
    }
}
