using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.DeleteLabRoom
{
    public class DeleteLabRoomCommandValidator : AbstractValidator<DeleteLabRoomCommand>
    {
        private readonly ILabRoomRepository _labRoomRepository;
        public DeleteLabRoomCommandValidator(ILabRoomRepository labRoomRepository)
        {
            _labRoomRepository = labRoomRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not be null.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsLabRoomExist(id, cancellationToken))
                .WithMessage(x => $"Not found LabRoom with ID : {x.ID}.");
        }
        private async Task<bool> IsLabRoomExist(string id, CancellationToken cancellationToken)
            => await _labRoomRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
    }
}
