using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Element.DeleteElement
{
    public class DeleteElementCommandValidator : AbstractValidator<DeleteElementCommand>
    {
        private readonly IElementRepositoty _elementRepositoty;
        public DeleteElementCommandValidator(IElementRepositoty elementRepositoty)
        {
            _elementRepositoty = elementRepositoty;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not be null.");
            RuleFor(x => x.ID)
                .MustAsync(async(id, cancellation) => await IsElementExists(id))
                .WithMessage("Element with the specified ID does not exist.");
        }
        private async Task<bool> IsElementExists(string id)
        {
            return await _elementRepositoty.AnyAsync(x => x.ID.Equals(id), CancellationToken.None);
        }
    }
}
