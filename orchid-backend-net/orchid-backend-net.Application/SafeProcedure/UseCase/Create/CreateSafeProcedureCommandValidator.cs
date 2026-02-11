using FluentValidation;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.Create
{
    public class CreateSafeProcedureCommandValidator : AbstractValidator<CreateSafeProcedureCommand>
    {
        public CreateSafeProcedureCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.ProcedureName)
                .NotEmpty().WithMessage("ProcedureName không được để trống.")
                .MaximumLength(200).WithMessage("ProcedureName không được vượt quá 200 ký tự.");
            RuleFor(x => x.ProcedureType)
                .NotEmpty().WithMessage("ProcedureType không được để trống.")
                .MaximumLength(100).WithMessage("ProcedureType không được vượt quá 100 ký tự.");
        }
    }
}
