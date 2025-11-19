using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskAssign.CreateTaskAssign
{
    public class CreateTaskAssignCommandValidator : AbstractValidator<CreateTaskAssignCommand>
    {
        private readonly IUserRepository _userRepository;
        public CreateTaskAssignCommandValidator(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.TechnicianId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Technician cannot be missing.");
            RuleFor(x => x.TechnicianId)
                .MustAsync(async (id, cancellationToken) => await IsTechnicianExist(id, cancellationToken))
                .WithMessage(x => $"Cannot find technician with id: {x.TechnicianId}");
            RuleFor(x => x.TechnicianId)
                .MustAsync(async (id, cancellationToken) => await IsTechnicianRole(id, cancellationToken))
                .WithMessage(x => $"This user is not a technician");
        }

        private async Task<bool> IsTechnicianExist(string technicianId, CancellationToken cancellationToken) 
            => await _userRepository.AnyAsync(x => x.ID.Equals(technicianId), cancellationToken);

        private async Task<bool> IsTechnicianRole(string technicianId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.ID.Equals(technicianId), cancellationToken);
            if(user == null) return false;
            return user.RoleID.Equals(3);
        }
    }
}
