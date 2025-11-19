using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Images.GetInfor
{
    public class GetImagesInforQueryValidator : AbstractValidator<GetImagesInforQuery>
    {
        private readonly IImageRepository _imageRepository;
        public GetImagesInforQueryValidator(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not be null.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await ImageExists(id,cancellationToken))
                .WithMessage("Image with the specified ID does not exist or is not active.");
        }

        private async Task<bool> ImageExists(string id, CancellationToken cancellationToken)
        {
            return await _imageRepository.AnyAsync(x => x.ID == id && x.Status, cancellationToken: cancellationToken);
        }
    }
}
