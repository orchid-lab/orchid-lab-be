using FluentValidation;
using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabRoom.UpdateLabRoom
{
    public class UpdateLabRoomCommandValidator : AbstractValidator<UpdateLabRoomCommand>
    {
        private readonly ILabRoomRepository _labRoomRepository;
        public UpdateLabRoomCommandValidator(ILabRoomRepository labRoomRepository)
        {
            _labRoomRepository = labRoomRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(200)
                .WithMessage("Description is too long.");

            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellationToken) => !await IsDuplicatedName(name, cancellationToken))
                .WithMessage(x => $"Duplicated name with {x.Name}");

            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsLabRoomExist(id, cancellationToken))
                .WithMessage(x => $"Not found labroom with ID : {x.ID}.");
        }
        private async Task<bool> IsLabRoomExist(string id, CancellationToken cancellationToken)
            => await _labRoomRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        private async Task<bool> IsDuplicatedName(string? name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                return true;
            //true => validator pass
            //false => validator catch
            return await _labRoomRepository.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()), cancellationToken);
        }
    }
}
