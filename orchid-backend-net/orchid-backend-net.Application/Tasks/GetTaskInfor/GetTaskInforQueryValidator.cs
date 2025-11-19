using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.GetTaskInfor
{
    public class GetTaskInforQueryValidator : AbstractValidator<GetTaskInforQuery>
    {
        private readonly ITaskRepository _taskRepository;
        public GetTaskInforQueryValidator(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not null.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsTaskExist(id, cancellationToken))
                .WithMessage(x => $"Not found Task with id {x.ID}");
        }
        private async Task<bool> IsTaskExist(string id, CancellationToken cancellationToken) 
            => await _taskRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken); 
    }
}
