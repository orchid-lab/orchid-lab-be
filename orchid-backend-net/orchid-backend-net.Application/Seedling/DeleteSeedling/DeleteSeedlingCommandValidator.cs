using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.DeleteSeedling
{
    public class DeleteSeedlingCommandValidator : AbstractValidator<DeleteSeedlingCommand>
    {
        private readonly ISeedlingRepository _seedlingRepository;
        public DeleteSeedlingCommandValidator(ISeedlingRepository seedlingRepository)
        {
            _seedlingRepository = seedlingRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.ID)
                .NotNull().NotEmpty().WithMessage("Seedling ID is required.")
                //Please fucking remember, when the function is false, it will throw out the message
                //which means, when the function returns false, it will throw out the message
                .MustAsync(async (id, cancellation) => await IsExist(id))
                .WithMessage("Seedling with the specified ID does not exist.");
        }

        private async Task<bool> IsExist(string id)
        {
            return await _seedlingRepository.AnyAsync(x => x.ID.Equals(id), CancellationToken.None);
        } 
    }
}
