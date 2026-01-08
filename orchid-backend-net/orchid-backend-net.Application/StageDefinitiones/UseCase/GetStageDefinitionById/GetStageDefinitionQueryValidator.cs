using FluentValidation;

namespace orchid_backend_net.Application.StageDefinitiones.UseCase.GetStageDefinitionById
{
    public class GetStageDefinitionQueryValidator : AbstractValidator<GetStageDefinitionByIdQuery>
    {
        public GetStageDefinitionQueryValidator()
        {
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.StageID)
                .NotEmpty()
                .NotNull()
                .WithMessage("Id không được bỏ trống.");
        }
    }
}
