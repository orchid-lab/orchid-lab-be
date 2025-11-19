using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Element.GetElementInfor
{
    public class GetElementInforQueryValidator : AbstractValidator<GetElementInforQuery>
    {
        private readonly IElementRepositoty _elementRepository;
        public GetElementInforQueryValidator(IElementRepositoty elementRepositoty)
        {
            _elementRepository = elementRepositoty;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("Element ID can not null.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellation) => await IsExist(id))
                .WithMessage("Element with the specified ID does not exist.");
        }
        private async Task<bool> IsExist(string id)
        {
            return await _elementRepository.AnyAsync(x => x.ID.Equals(id), CancellationToken.None);
        }
    }
}
