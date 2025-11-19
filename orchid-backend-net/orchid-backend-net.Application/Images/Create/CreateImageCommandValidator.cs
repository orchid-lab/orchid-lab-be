using FluentValidation;

namespace orchid_backend_net.Application.Images.Create
{
    public class CreateImageCommandValidator : AbstractValidator<CreateImageCommand>
    {
        public CreateImageCommandValidator() 
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .NotNull()
                .WithMessage("File name cannot be empty.");
            RuleFor(x => x.FileStream)
                .NotNull()
                .NotEmpty()
                .WithMessage("File cannot be empty");
        }
    }
}
