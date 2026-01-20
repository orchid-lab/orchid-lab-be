using FluentValidation;
using orchid_backend_net.Application.Batch.Policy;

namespace orchid_backend_net.Application.Batch.UseCase.UpdateBatch
{
    public class UpdateBatchCommandValidator : AbstractValidator<UpdateBatchCommand>
    {
        public UpdateBatchCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0");
            RuleFor(x => x.WidthUnit)
                .Must(unit => unit is null || BatchPolicy.IsValidUnit(unit))
                .WithMessage("Unit không hợp lệ");
            RuleFor(x => x.HeightUnit)
                .Must(unit => unit is null || BatchPolicy.IsValidUnit(unit))
                .WithMessage("Unit không hợp lệ");
        }
    }
}
