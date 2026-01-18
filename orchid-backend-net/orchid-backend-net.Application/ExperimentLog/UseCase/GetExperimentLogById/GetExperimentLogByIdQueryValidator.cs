using FluentValidation;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.GetExperimentLogById
{
    internal class GetExperimentLogByIdQueryValidator : AbstractValidator<GetExperimentLogByIdQuery>
    {
        public GetExperimentLogByIdQueryValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("Id không được để trống");
        }
    }
}
