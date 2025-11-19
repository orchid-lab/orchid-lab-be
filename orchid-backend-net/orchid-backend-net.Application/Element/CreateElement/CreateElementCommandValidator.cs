using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Element.CreateElement
{
    public class CreateElementCommandValidator : AbstractValidator<CreateElementCommand>
    {
        private readonly IElementRepositoty elementRepositoty;
        public CreateElementCommandValidator(IElementRepositoty elementRepositoty)
        {
            this.elementRepositoty = elementRepositoty;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .WithMessage("Element description can not be null.");
            RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(250)
                .WithMessage("Element description is too long.");
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Element name can not be null.");
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellation) => !await IsDuplicatedName(name))
                .WithMessage("An element with this name already exists.");
        }
        private async Task<bool> IsDuplicatedName(string name)
        {
            return await this.elementRepositoty.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()));
        }
    }
}
