using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Referent.UpdateReferent
{
    public class UpdateReferentCommand(string id, string? name,
        decimal? valueFrom, decimal? valueTo, string? unit) : IRequest<string>
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public decimal? ValueFrom { get; set; }
        public decimal? ValueTo { get; set; }
        public string? Unit { get; set; }
    }

    internal class UpdateReferentCommandHandler(IReferentRepository referentRepository) : IRequestHandler<UpdateReferentCommand, string>
    {
        public async Task<string> Handle(UpdateReferentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var referents = await referentRepository.FindAsync(x => x.ID.Equals(request.Id) && x.Status, cancellationToken);
                referents.Name = request.Name ?? referents.Name;
                referents.ValueFrom = request.ValueFrom ?? referents.ValueFrom;
                referents.ValueTo = request.ValueTo ?? referents.ValueTo;
                referents.MeasurementUnit = request.Unit ?? referents.MeasurementUnit;
                referentRepository.Update(referents);
                return await referentRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                        $"Update referent with id: {referents.ID}"
                        : "Failed to updated";
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
