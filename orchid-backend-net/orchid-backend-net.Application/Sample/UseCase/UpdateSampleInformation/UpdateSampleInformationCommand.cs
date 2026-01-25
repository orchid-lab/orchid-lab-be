using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.UpdateSampleInformation
{
    public record UpdateSampleInformationCommand(string Id, string? Name, string? Description, string? Notes) : IRequest<string>;
    internal class UpdateSampleInformationCommandHandler(ISampleRepository sampleRepository) : IRequestHandler<UpdateSampleInformationCommand, string>
    {
        public async Task<string> Handle(UpdateSampleInformationCommand request, CancellationToken cancellationToken)
        {
            var sample = await sampleRepository.FindAsync(s => s.ID.Equals(request.Id), cancellationToken)
                ?? throw new NotFoundException("Không thấy sample này");
            sample.UpdateSampleInformation(request.Name, request.Notes, request.Description);
            sampleRepository.Update(sample);
            return await sampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? sample.ID.ToString()
                : "Cập nhật thất bại";
        }
    }
}
