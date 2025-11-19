using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.TaskTemplate.UpdateTaskTemplate
{
    public class UpdateTaskTemplateCommand : IRequest<string>, ICommand
    {
        public string ID {  get; set; }
        public string Name { get; set; }
        public string StageID { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public List<TaskTemplateDetailsDTO> Details { get; set; }
        public UpdateTaskTemplateCommand() { }
        public UpdateTaskTemplateCommand(string id, string name, string stageID, string description, bool status, List<TaskTemplateDetailsDTO> details)
        {
            ID = id;
            Name = name;
            StageID = stageID;
            Description = description;
            Status = status;
            Details = details;
        }
    }
    internal class UpdateTaskTemplateCommandHandler : IRequestHandler<UpdateTaskTemplateCommand, string>
    {
        private readonly ITaskTemplatesRepository _tasksRepository;
        private readonly ITaskTemplateDetailsRepository _detailsRepository;
        public UpdateTaskTemplateCommandHandler(ITaskTemplateDetailsRepository detailsRepository, ITaskTemplatesRepository tasksRepository)
        {
            _detailsRepository = detailsRepository;
            _tasksRepository = tasksRepository;
        }
        public async Task<string> Handle(UpdateTaskTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var template = await this._tasksRepository.FindAsync(x => x.ID.Equals(request.ID), cancellationToken);
                if (template == null)
                    throw new DirectoryNotFoundException($" not found task template with ID {request.ID}");
                template.Name = request.Name;
                template.StageID = request.StageID;
                template.Description = request.Description;
                _tasksRepository.Update(template);
                foreach (var item in request.Details) 
                {
                    var detail = await this._detailsRepository.FindAsync(x => x.ID.Equals(item.ID), cancellationToken);
                    if(detail != null)
                    {
                        detail.Name = item.Name;
                        detail.IsRequired = item.IsRequired;
                        detail.Description = item.Description;
                        detail.Unit = item.Unit;
                        detail.Status = item.Status;
                        detail.Element = item.Element;  
                        detail.ExpectedValue = item.ExpectedValue;
                        _detailsRepository.Update(detail);
                    }
                    else
                    {
                        TaskTemplateDetails newdetail = new TaskTemplateDetails()
                        {
                            Name = item.Name,
                            Description = item.Description,
                            IsRequired = item.IsRequired,
                            ExpectedValue = item.ExpectedValue,
                            Unit = item.Unit,
                            Status = true,
                            Element = item.Element,
                            TaskTemplateID = request.ID
                        };
                        _detailsRepository.Add(newdetail);
                    }
                }
                return (await _tasksRepository.UnitOfWork.SaveChangesAsync(cancellationToken)) > 0 ? $"Updated task template ID: {request.ID} /n Name: {request.Name}" : "Failed to update task template.";

            }
            catch (Exception ex) 
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
