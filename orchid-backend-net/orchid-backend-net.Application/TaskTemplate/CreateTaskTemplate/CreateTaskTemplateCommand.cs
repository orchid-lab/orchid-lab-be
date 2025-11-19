using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.TaskTemplate.CreateTaskTemplate
{
    public class CreateTaskTemplateCommand : IRequest<string>, ICommand
    {
        public string Name { get; set; }
        public string StageID { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public List<TaskTemplateDetailsDTO> Details { get; set; }
        public CreateTaskTemplateCommand() { }
        public CreateTaskTemplateCommand(string name, string stageID, string description, bool status, List<TaskTemplateDetailsDTO> details)
        {
            Name = name;
            StageID = stageID;
            Description = description;
            Status = status;
            Details = details;
        }
    }
    internal class CreateTaskTemplateCommandHandler : IRequestHandler<CreateTaskTemplateCommand, string>
    {
        private readonly ITaskTemplatesRepository _tasksRepository;
        private readonly ITaskTemplateDetailsRepository _templateDetailsRepository;
        public CreateTaskTemplateCommandHandler(ITaskTemplateDetailsRepository taskTemplateDetails, ITaskTemplatesRepository taskTemplates)
        {
            _tasksRepository = taskTemplates;
            _templateDetailsRepository = taskTemplateDetails;
        }
        public async Task<string> Handle(CreateTaskTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.StageID == null)
                    throw new ArgumentNullException(nameof(request.StageID), "Stage must be set before creating task templates.");
                TaskTemplates obj = new TaskTemplates()
                {
                    Name = request.Name,
                    StageID = request.StageID,
                    Description = request.Description,
                    Status = request.Status,
                };
                _tasksRepository.Add(obj);
                foreach (var item in request.Details) 
                {
                    TaskTemplateDetails detail = new TaskTemplateDetails()
                    {
                        TaskTemplateID = obj.ID,
                        Name = item.Name,
                        Description = item.Description,
                        Status = item.Status,
                        Element = item.Element,
                        Unit = item.Unit,
                        IsRequired = item.IsRequired,
                        ExpectedValue = item.ExpectedValue,
                    };
                    _templateDetailsRepository.Add(detail);
                }
                return (await _tasksRepository.UnitOfWork.SaveChangesAsync(cancellationToken)) > 0 ? $"Created task template ID {obj.ID}" : "Failed to create task template";
            }
            catch (Exception ex) 
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
