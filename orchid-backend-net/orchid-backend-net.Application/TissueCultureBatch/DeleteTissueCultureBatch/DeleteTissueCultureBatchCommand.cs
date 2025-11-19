using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TissueCultureBatch.DeleteTissueCultureBatch
{
    public class DeleteTissueCultureBatchCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public DeleteTissueCultureBatchCommand(string ID)
        {
            this.ID = ID;
        }
    }

    internal class DeleteTissueCultureBatchCommandHandler(ITissueCultureBatchRepository _tissueCultureBatchRepository) : IRequestHandler<DeleteTissueCultureBatchCommand, string>
    {
        public async Task<string> Handle(DeleteTissueCultureBatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tissue = await _tissueCultureBatchRepository.FindAsync(x => x.ID.Equals(request.ID), cancellationToken);
                if (tissue == null)
                    throw new NotFoundException($"Not found tissue culture batch with ID {request.ID}");
                _tissueCultureBatchRepository.Remove(tissue);
                return (await _tissueCultureBatchRepository.UnitOfWork.SaveChangesAsync(cancellationToken)) > 0 ? $"Deleted tissue culture batch Name {tissue.Name}" : "";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
