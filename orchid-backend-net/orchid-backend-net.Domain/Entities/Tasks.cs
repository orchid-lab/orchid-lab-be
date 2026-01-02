using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Tasks : BaseGuidEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        //StageId để xác định là cái task này có phải là template hay không
        public string? StageId { get; set; }
        public string? ResearcherId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public Domain.Common.Enum.TaskStatus Status { get; set; }
        public virtual List<TaskAssignment> TaskAssignments { get; set; } = [];
        public virtual List<TaskAttributes> TaskAttributes { get; set; } = [];

        public void AddTaskAssignment(string technicianId, string? sampleId, bool isForWholeExperimentLog)
        {
            if (isForWholeExperimentLog && !string.IsNullOrWhiteSpace(sampleId))
            {
                throw new DomainException("Không thể vừa chọn giao việc cho toàn bộ experiment log vừa chọn sample cụ thể.");
            }
            var taskAssignment = new TaskAssignment
            {
                TaskId = this.ID,
                TechnicianId = technicianId,
                SampleId = sampleId,
                IsForWholeExperimentLog = isForWholeExperimentLog
            };
            TaskAssignments.Add(taskAssignment);
        }

        public void UpdateTaskAssignment(string taskAssignmentId, string? sampleId, bool isForWholeExperimentLog)
        {
            var taskAssignment = TaskAssignments.FirstOrDefault(ta => ta.ID == taskAssignmentId);
            if (isForWholeExperimentLog && !string.IsNullOrWhiteSpace(sampleId))
            {
                throw new DomainException("Không thể vừa chọn giao việc cho toàn bộ experiment log vừa chọn sample cụ thể.");
            }
            if (taskAssignment != null)
            {
                taskAssignment.SampleId = sampleId;
                taskAssignment.IsForWholeExperimentLog = isForWholeExperimentLog;
            }
        }

        public void AddTaskAttribute(int? chemicalId, int? materialId, string unit, decimal value)
        {
            var isDuplicatedAttributes = TaskAttributes.Any(x =>
            (chemicalId != null && x.ChemicalId == chemicalId) ||
            (materialId != null && x.MaterialId == materialId));
            if (isDuplicatedAttributes)
            {
                throw new DuplicateException("Bị trùng attributes.");
            }
            if (chemicalId is not null && materialId is not null)
                throw new DomainException("Không thể cùng lúc thêm cả chemical và material cho một attribute.");
            var taskAttribute = new TaskAttributes
            {
                ChemicalId = chemicalId,
                MaterialId = materialId,
                Unit = unit,
                Value = value
            };
            TaskAttributes.Add(taskAttribute);
        }

        public void UpdateTaskAttributes(string taskAttributesId, string unit, decimal value, int? chemicalId, int? materialId)
        {
            var taskAttribute = TaskAttributes.FirstOrDefault(ta => ta.ID == taskAttributesId);
            if (taskAttribute is null)
                throw new NotFoundException("Không tìm thấy task attribute.");
            if (chemicalId is not null && materialId is not null)
            {
                throw new DomainException("Không thể cùng lúc thêm cả chemical và material cho một attribute.");
            }
            taskAttribute.MaterialId = materialId;
            taskAttribute.ChemicalId = chemicalId;
            taskAttribute.Unit = unit;
            taskAttribute.Value = value;
        }
    }
}
