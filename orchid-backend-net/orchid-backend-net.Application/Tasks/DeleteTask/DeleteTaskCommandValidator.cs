using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.DeleteTask
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        private readonly ITaskRepository _taskRepository;
        public DeleteTaskCommandValidator(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            Configuration();
        }

        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not be empty.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsTaskExist(id, cancellationToken))
                .WithMessage(x => $"Not found task with ID: {x.ID}");
        }

        private async Task<bool> IsTaskExist(string id, CancellationToken cancellationToken)
            => await _taskRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
    }
}
