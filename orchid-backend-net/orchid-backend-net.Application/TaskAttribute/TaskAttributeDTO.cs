using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.TaskAttribute
{
    public class TaskAttributeDTO : IMapFrom<TaskAttributes>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MeasurementUnit { get; set; }
        public double Value { get; set; }
        public bool Status { get; set; }

        public TaskAttributeDTO(string id, string name, string description, string measurementUnit, double value, bool status)
        {
            ID = id;
            Name = name;
            Description = description;
            Value = value;
            Status = status;
            MeasurementUnit = measurementUnit;
        }
        public TaskAttributeDTO() { }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaskAttributes, TaskAttributeDTO>()
                .ReverseMap();
        }
    }
}
