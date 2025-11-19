using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ReportAttribute.UpdateReportAttribute
{
    public class UpdateReportAttributeCommand : IRequest
    {
        public string ReportAttributeID { get; set; }
        public string? Name { get; set; } 
        public decimal? Value { get; set; }
    }

    internal class UpdateReportAttributeCommandHandler(IReportAttributeRepository reportAttributeRepository) : IRequestHandler<UpdateReportAttributeCommand>
    {
        public async Task Handle(UpdateReportAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var reportAttribute = await reportAttributeRepository.FindAsync(attribute => attribute.ID.Equals(request.ReportAttributeID), cancellationToken);
                reportAttribute.Name = request.Name ?? reportAttribute.Name;
                reportAttribute.Value = request.Value ?? reportAttribute.Value;
                reportAttributeRepository.Update(reportAttribute);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating report attribute: {ex.Message}");
            }
        }
    }
}
