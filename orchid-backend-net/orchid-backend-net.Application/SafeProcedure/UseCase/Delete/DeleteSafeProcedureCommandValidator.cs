using FluentValidation;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.Delete
{
    public class DeleteSafeProcedureCommandValidator : AbstractValidator<DeleteSafeProcedureCommand>
    {
        public DeleteSafeProcedureCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID của quy trình an toàn không được để trống.");
        }
    }
}
