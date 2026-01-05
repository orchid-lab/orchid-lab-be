using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events;

namespace orchid_backend_net.Domain.Entities
{
    public class Tasks : BaseGuidEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        //StageId để xác định là cái task này có phải là template hay không
        public string? StageId { get; set; }
        public string? ResearcherId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public Domain.Common.Enum.TaskStatus Status { get; set; }
        public virtual TaskAssignment TaskAssignment { get; set; }
        public virtual List<TaskAttributes> TaskAttributes { get; set; } = [];

        public void AddTaskAssignment(
            string technicianId,
            TaskTargetType targetType,
            string targetId,
            DateTime expectedEndDate,
            DateTime startDate)
        {
            if (string.IsNullOrWhiteSpace(ResearcherId))
                throw new DomainException("Task không biết ai giao cho technician nào.");
            if (TaskAssignment != null)
                throw new DomainException("Task đã được assign.");
            if (string.IsNullOrWhiteSpace(targetId))
                throw new DomainException("TargetId không được để trống.");
            if (string.IsNullOrEmpty(technicianId))
                throw new DomainException("Người nhận task không được để trống.");

            Status = Common.Enum.TaskStatus.Assigned;

            TaskAssignment = new TaskAssignment
            {
                TaskId = this.ID,
                TechnicianId = technicianId,
                TargetType = targetType,
                TargetId = targetId,
                StartDate = startDate,
                ExpectedEndDate = expectedEndDate
            };
            AddDomainEvent(new TaskAssignedToTechnicianEvent(this.ID, technicianId, this.ResearcherId));
        }

        public void AcceptTask(string technicianId)
        {
            if (TaskAssignment == null)
                throw new DomainException("Task chưa được assign.");

            if (TaskAssignment.TechnicianId != technicianId)
                throw new DomainException("Không phải task của bạn.");

            if (Status != Common.Enum.TaskStatus.Assigned)
                throw new DomainException("Task không ở trạng thái có thể nhận.");

            if (string.IsNullOrWhiteSpace(ResearcherId))
                throw new DomainException("Task hiện tại đang lỗi.");

            Status = Common.Enum.TaskStatus.InProgress;

            AddDomainEvent(new TaskAcceptedByTechnicianEvent(
                ID,
                technicianId,
                this.ResearcherId
            ));
        }

        public void DeclineTask(string technicianId, string reason)
        {
            if (TaskAssignment?.TechnicianId != technicianId)
                throw new DomainException("Không phải task của bạn.");

            if (Status != Common.Enum.TaskStatus.InProgress)
                throw new DomainException("Không thể từ chối task.");

            if (string.IsNullOrWhiteSpace(ResearcherId))
                throw new DomainException("Task hiện tại đang lỗi.");
            Status = Common.Enum.TaskStatus.DeclinedByTechnician;

            AddDomainEvent(new TaskDeclineByTechnicianEvent(
                ID,
                technicianId,
                ResearcherId,
                reason
            ));
        }

        public void ReportTask(string technicianId)
        {
            if (TaskAssignment?.TechnicianId != technicianId)
                throw new DomainException("Không phải task của bạn.");

            if (Status != Common.Enum.TaskStatus.InProgress)
                throw new DomainException("Task chưa được thực hiện.");
            if (string.IsNullOrWhiteSpace(ResearcherId))
                throw new DomainException("Task hiện tại đang lỗi.");


            Status = Common.Enum.TaskStatus.WaitingForApproval;

            AddDomainEvent(new TaskReportedByTechnicianEvent(
                ID,
                technicianId,
                this.ResearcherId
            ));
        }

        public void Complete(string researcherId, DateTime completedAt)
        {
            if (Status != Common.Enum.TaskStatus.WaitingForApproval)
                throw new DomainException("Task chưa chờ duyệt.");

            var isInTime = completedAt <= TaskAssignment.ExpectedEndDate;

            Status = isInTime
                ? Common.Enum.TaskStatus.CompletedInTime
                : Common.Enum.TaskStatus.CompletedOutTime;

            AddDomainEvent(new TaskCompletedEvent(
                ID,
                researcherId,
                TaskAssignment!.TechnicianId,
                isInTime
            ));
        }

        public void RequestRedo(string researcherId, string reason)
        {
            if (ResearcherId != researcherId)
                throw new DomainException("Không có quyền.");

            if (Status != Common.Enum.TaskStatus.WaitingForApproval)
                throw new DomainException("Task chưa chờ duyệt.");

            Status = Common.Enum.TaskStatus.ReworkRequired;

            AddDomainEvent(new TaskRedoRequestedEvent(
                ID,
                researcherId,
                TaskAssignment!.TechnicianId,
                reason
            ));
        }



        public void ReassignTaskTarget(
            TaskTargetType? newTargetType,
            string? newTargetId,
            DateTime? expectedEndDate,
            DateTime? endDate)
        {

            bool IsTemplate = string.IsNullOrWhiteSpace(StageId);
            bool WantToChangeToToDoTask = string.IsNullOrWhiteSpace(newTargetId);
            if (TaskAssignment == null)
                throw new DomainException("Task chưa được assign, vui lòng hãy tạo task trước khi assign như thế này.");

            if (IsTemplate == WantToChangeToToDoTask)
                throw new DomainException("Không thể chuyển template task thành to-do task.");

            TaskAssignment.TargetType = newTargetType ?? TaskAssignment.TargetType;
            TaskAssignment.TargetId = newTargetId ?? TaskAssignment.TargetId;
            TaskAssignment.ExpectedEndDate = expectedEndDate ?? TaskAssignment.ExpectedEndDate;
            TaskAssignment.EndDate = endDate ?? TaskAssignment.EndDate;
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
