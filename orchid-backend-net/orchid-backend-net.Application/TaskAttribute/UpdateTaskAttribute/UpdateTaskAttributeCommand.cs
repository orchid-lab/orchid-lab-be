using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskAttribute.UpdateTaskAttribute
{
    public class UpdateTaskAttributeCommand(string id, string? name, string? measurementUnit, double? value) : IRequest, ICommand
    {
        public string Id { get; set; } = id;
        public string? Name { get; set; } = name;
        public string? MeasurementUnit { get; set; } = measurementUnit;
        public double? Value { get; set; } = value;
    }

    internal class UpdateTaskAttributeCommandHandler(ITaskAttributeRepository taskAttributeRepository) : IRequestHandler<UpdateTaskAttributeCommand>
    {
        public async Task Handle(UpdateTaskAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskAttribute = await taskAttributeRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken);
                taskAttribute.Name = request.Name ?? taskAttribute.Name;
                taskAttribute.MeasurementUnit = request.MeasurementUnit ?? taskAttribute.MeasurementUnit;
                taskAttribute.Value = request.Value ?? taskAttribute.Value;
                taskAttributeRepository.Update(taskAttribute);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
