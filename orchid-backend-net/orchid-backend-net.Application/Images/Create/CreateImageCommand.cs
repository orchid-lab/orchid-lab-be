using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Images.Create
{
    public class CreateImageCommand(Stream fileStream, string fileName, string reportId) : IRequest<bool>, ICommand
    {
        public Stream FileStream { get; set; } = fileStream;
        public string FileName { get; set; } = fileName;
        public string ReportId { get; set; } = reportId;
    }

    internal class CreateImageCommandHandler(IImageUploaderService uploaderService, IImageRepository imageRepository) : IRequestHandler<CreateImageCommand, bool>
    {
        public async Task<bool> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            var url = await uploaderService.UpdloadImageAsync(request.FileStream, request.FileName, "report-image");
            Imgs imgs = new()
            {
                ReportID = request.ReportId,
                Url = url,
                Status = true,
            };
            imageRepository.Add(imgs);
            return await imageRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? true : false;
        }
    }
}
