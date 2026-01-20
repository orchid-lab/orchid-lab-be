using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.UseCase.CreateSeedlings
{
    public class CreateSeedlingsCommandValidator : AbstractValidator<CreateSeedlingsCommand>
    {
        private readonly ICharacteristicRepository _characteristicRepository;
        public CreateSeedlingsCommandValidator(ICharacteristicRepository characteristicRepository)
        {
            _characteristicRepository = characteristicRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.LocalName)
                .NotEmpty().WithMessage("Tên địa phương không được để trống.")
                .MaximumLength(100).WithMessage("Tên địa phương không được vượt quá 100 ký tự.");
            RuleFor(x => x.ScientificName)
                .NotEmpty().WithMessage("Tên khoa học không được để trống.")
                .MaximumLength(100).WithMessage("Tên khoa học không được vượt quá 100 ký tự.");
            RuleForEach(x => x.SeedlingsTraits).ChildRules(trait =>
            {
                trait.RuleFor(t => t.CharacteristicId)
                    .NotEmpty().WithMessage("ID đặc điểm không được để trống.");

                trait.RuleFor(t => t.CharacteristicId)
                    .MustAsync(async (id, CancellationToken) => await IsCharacteristicExist(id, CancellationToken))
                    .WithMessage("Đặc điểm này không tồn tại trong hệ thống.");

                trait.RuleFor(t => t.Value)
                    .GreaterThan(0).WithMessage("Giá trị đặc điểm phải lớn hơn 0.");
            });
        }

        private async Task<bool> IsCharacteristicExist(string id, CancellationToken cancellationToken)
            => await _characteristicRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
    }
}
