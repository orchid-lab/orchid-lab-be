using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.ReportAttribute.CreateReportAttribute;
using orchid_backend_net.Application.ReportAttribute.UpdateReportAttribute;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Report.UpdateReport
{
    public class UpdateReportCommand(string? name, string? description, List<CreateReportAttributeCommand>? createCommands, List<UpdateReportAttributeCommand>? updateCommands) : IRequest<string>, ICommand
    {

        public string? Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public List<CreateReportAttributeCommand>? CreateCommands { get; set; } = createCommands;
        public List<UpdateReportAttributeCommand>? UpdateCommands { get; set; } = updateCommands;
    }

    internal class UpdateReportCommandHandler(IReportRepository reportRepository, ISender sender, ICurrentUserService currentUserService) : IRequestHandler<UpdateReportCommand, string>
    {
        public async Task<string> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var report = await reportRepository.FindAsync(r => r.Name.Equals(request.Name), cancellationToken);
                report.Name = request.Name ?? report.Name;
                report.Description = request.Description ?? report.Description;
                report.Update_by = currentUserService.UserId;
                report.Update_date = DateTime.UtcNow;
                reportRepository.Update(report);

                if(request.CreateCommands != null && request.CreateCommands.Count > 0)
                {
                    foreach (var createCommand in request.CreateCommands)
                    {
                        createCommand.ReportID = report.ID; // Set the ReportID for each attribute
                        await sender.Send(createCommand, cancellationToken); // Send the command to create report attributes
                    }
                }

                if(request.UpdateCommands != null && request.UpdateCommands.Count > 0)
                {
                    foreach (var updateCommand in request.UpdateCommands)
                    {
                        await sender.Send(updateCommand, cancellationToken); // Send the command to update report attributes
                    }
                }

                return await reportRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Updated report." : "Failed to update report.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
