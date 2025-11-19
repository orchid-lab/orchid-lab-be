using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.TaskTemplate
{
    public class TaskTemplateDetailsDTO : IMapFrom<TaskTemplateDetails>
    {
        public string ID { get; set; }
        public string Element { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal ExpectedValue { get; set; }
        public string Unit { get; set; }
        public bool IsRequired { get; set; }
        public bool Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaskTemplateDetails, TaskTemplateDetailsDTO>();
        }
    }
}
