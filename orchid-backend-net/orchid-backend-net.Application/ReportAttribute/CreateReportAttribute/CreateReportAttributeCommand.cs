using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;
using System.Text.Json.Serialization;

namespace orchid_backend_net.Application.ReportAttribute.CreateReportAttribute
{
    public class CreateReportAttributeCommand(string referentId, string name, decimal value) : IRequest, ICommand
    {
        [JsonIgnore]
        public string? ReportID { get; set; } // ID of the report to which this attribute belongs
        public string ReferentID { get; set; } = referentId; // ID of the referent (if applicable)
        public string Name { get; set; } = name; // Name of the attribute
        public decimal Value { get; set; } = value; // Value of the attribute
    }

    internal class CreateReportAttributCommandeHandler(IReportAttributeRepository reportAttributeRepository) : IRequestHandler<CreateReportAttributeCommand>
    {
        public async Task Handle(CreateReportAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var reportAttribute = new Domain.Entities.ReportAttributes
                {
                    ID = Guid.NewGuid().ToString(),
                    ReportID = request.ReportID,
                    ReferentID = request.ReferentID,
                    Name = request.Name,
                    Value = request.Value,
                    Status = 1
                };
                reportAttributeRepository.Add(reportAttribute);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating report attribute: {ex.Message}");
            }
        }
    }
}
