using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using System.Text.Json.Serialization;

namespace orchid_backend_net.Application.TaskAttribute.CreateTaskAttribute
{
    public class CreateTaskAttributeCommand(string name, string measurementUnit, double value, string description) : IRequest, ICommand
    {
        [JsonIgnore]
        public string? TaskId { get; set; }
        public string Name { get; set; } = name;
        public string MeasurementUnit { get; set; } = measurementUnit;
        public double Value { get; set; } = value;
        public string Description { get; set; } = description;
    }

    internal class CreateTaskAttributeCommandHandler(ITaskAttributeRepository taskAttributeRepository) : IRequestHandler<CreateTaskAttributeCommand>
    {
        public async Task Handle(CreateTaskAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                TaskAttributes attributes = new()
                {
                    TaskID = request.TaskId,
                    Name = request.Name,
                    Description = request.Description,
                    MeasurementUnit = request.MeasurementUnit,
                    Value = request.Value,
                    Status = true,
                };
                taskAttributeRepository.Add(attributes);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
