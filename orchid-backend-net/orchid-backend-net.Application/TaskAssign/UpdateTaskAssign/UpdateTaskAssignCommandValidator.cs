using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskAssign.UpdateTaskAssign
{
    public class UpdateTaskAssignCommandValidator : AbstractValidator<UpdateTaskAssignCommand> 
    {
        private readonly ITaskAssignRepository _taskAssignRepository;
        private readonly IUserRepository _userRepository;

        public UpdateTaskAssignCommandValidator(ITaskAssignRepository taskAssignRepository, IUserRepository userRepository)
        {
            _taskAssignRepository = taskAssignRepository;
            _userRepository = userRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(assign => assign.Id)
                .MustAsync(async (id, cancellationToken) => await IsTaskAssignExist(id, cancellationToken))
                .WithMessage(assign => $"Cannot find person working in this task with id: {assign.Id}.");
            RuleFor(x => x.TechnicianId)
                .MustAsync(async (id, cancellationToken) => await IsUserExist(id, cancellationToken))
                .WithMessage(x => $"Cannot find person with id: {x.TechnicianId}.");
        }

        private async Task<bool> IsTaskAssignExist(string id, CancellationToken cancellationToken)
            => await _taskAssignRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        private async Task<bool> IsUserExist(string? id, CancellationToken cancellationToken)
        {
            if (id == null) return true;
            return await _userRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        }
    }
}
