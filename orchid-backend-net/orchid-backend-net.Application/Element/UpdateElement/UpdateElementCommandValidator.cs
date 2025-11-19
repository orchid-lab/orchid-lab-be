using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Element.UpdateElement
{
    public class UpdateElementCommandValidator : AbstractValidator<UpdateElementCommand>
    {
        private readonly IElementRepositoty _elementRepositoty;
        public UpdateElementCommandValidator(IElementRepositoty elementRepositoty)
        {
            _elementRepositoty = elementRepositoty;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(250)
                .WithMessage("Element description is too long.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await ElementExists(id, cancellationToken))
                .WithMessage("Element with specified ID does not exist.");
        }
        private async Task<bool> ElementExists(string id, CancellationToken cancellationToken)
        {
            return await _elementRepositoty.AnyAsync(x => x.ID.Equals(id) && x.Status == true, cancellationToken);
        }
    }
}
