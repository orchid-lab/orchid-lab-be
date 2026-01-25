using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.DestroyBecauseOfDisease
{
    public record DestroySampleBecauseOfDiseaseCommand(string Id, string? Reason) : IRequest<string>;
    internal class DestroySampleBecauseOfDiseaseCommandHandler(ISampleRepository sampleRepository) : IRequestHandler<DestroySampleBecauseOfDiseaseCommand, string>
    {
        public async Task<string> Handle(DestroySampleBecauseOfDiseaseCommand request, CancellationToken cancellationToken)
        {
            var sample = await sampleRepository.FindAsync(s => s.ID.Equals(request.Id), cancellationToken)
                ?? throw new NotFoundException("Không thấy sample này");

            sample.CancelBecauseOfDisease(request.Reason);
            return await sampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? sample.ID.ToString()
                : "Xóa thất bại";
        }
    }
}
