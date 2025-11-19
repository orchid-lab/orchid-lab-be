using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Referent.CreateReferent
{
    public class CreateReferentCommand(string name, string stageId,
        decimal valueFrom, decimal valueTo, string unit) : IRequest<string>, ICommand
    {
        public string Name { get; set; } = name;
        public string StageId { get; set; } = stageId;
        public decimal ValueFrom { get; set; } = valueFrom;
        public decimal ValueTo { get; set; } = valueTo;
        public string Unit { get; set; } = unit;
    }

    internal class CreateReferentCommandHandler(IReferentRepository referentRepository) : IRequestHandler<CreateReferentCommand, string>
    {
        public async Task<string> Handle(CreateReferentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var referents = new Referents()
                {
                    Name = request.Name,
                    StageID = request.StageId,
                    ValueFrom = request.ValueFrom,
                    ValueTo = request.ValueTo,
                    Status = true
                };
                referentRepository.Add(referents);
                return await referentRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                    $"Create referent with id: {referents.ID}" 
                    : "Failed to created";
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
