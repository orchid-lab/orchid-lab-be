using FluentValidation;

namespace orchid_backend_net.Application.Batch.UseCase.DeleteBatch
{
    public class DeleteBatchCommandValidator : AbstractValidator<DeleteBatchCommand>
    {
        public DeleteBatchCommandValidator()
        {
            Configure();
        }
        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0");
        }
    }
}
