using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.CreateLabRoom
{
    public class CreateLabRoomCommandValidator : AbstractValidator<CreateLabRoomCommand>
    {
        private readonly ILabRoomRepository _lapRoomRepository;
        public CreateLabRoomCommandValidator(ILabRoomRepository labRoomRepository)
        {
            _lapRoomRepository = labRoomRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name can not be null.");
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellationToken) => !await IsLabRoomNameDuplicated(name, cancellationToken))
                .WithMessage(x => $"Extis LabRoom name :{x.Name}");
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .WithMessage("Description can not be null.");
            RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(200)
                .WithMessage("Description is too long.");
        }
        private async Task<bool> IsLabRoomNameDuplicated(string name, CancellationToken cancellationToken)
            => await _lapRoomRepository.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()), cancellationToken);
    }
}
