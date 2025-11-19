using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Referent.DeleteReferent
{
    public class DeleteReferentCommand(string id) : IRequest<string>
    {
        public string Id { get; set; } = id;
    }
    internal class DeleteReferentCommandHandler(IReferentRepository referentRepository) : IRequestHandler<DeleteReferentCommand, string>
    {
        public async Task<string> Handle(DeleteReferentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var referent = await referentRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken);
                referent.Status = false;
                referentRepository.Update(referent);
                return await referentRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                    ? referent.ID
                    : throw new Exception("Failed to delete referent");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
