using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskAttribute.UpdateTaskAttribute
{
    public class UpdateTaskAttributeCommandValidator : AbstractValidator<UpdateTaskAttributeCommand>
    {
        private readonly ITaskAttributeRepository _taskAttributeRepository;

        public UpdateTaskAttributeCommandValidator(ITaskAttributeRepository taskAttributeRepository)
        {
            _taskAttributeRepository = taskAttributeRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) => await IsTaskAttributeExist(id, cancellationToken))
                .WithMessage(x => $"Cannot find attribute with id: {x.Id}.");
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellationToken) => !await IsDuplicatedName(name, cancellationToken))
                .WithMessage(x => $"Attribute with name {x.Name} has been duplicated");
        }

        private async Task<bool> IsTaskAttributeExist(string id, CancellationToken cancellationToken)
            => await _taskAttributeRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);

        private async Task<bool> IsDuplicatedName(string? name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            return await _taskAttributeRepository.AnyAsync(x => x.Name.Equals(name), cancellationToken);
        }
    }
}
