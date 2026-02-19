using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events.TaskEvents;

namespace orchid_backend_net.Domain.Entities
{
    public class Tasks : AuditableEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        //StageId để xác định là cái task này có phải là template hay không
        public int? StageId { get; set; }
        public string? ResearcherId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public Domain.Common.Enum.TaskStatus Status { get; set; }
        public virtual TaskAssignment TaskAssignment { get; set; } = null!;
        public virtual List<TaskAttributes> TaskAttributes { get; set; } = new();
        public virtual TaskCheckList? CheckList { get; set; }

        // ===== Task Assignment =====
        public void AddTaskAssignment(
            string technicianId,
            TaskTargetType targetType,
            string targetId,
            DateTime expectedEndDate,
            DateTime startDate,
            bool isSeeding = false)
        {
            if (string.IsNullOrWhiteSpace(ResearcherId))
                throw new DomainException("Task không biết ai giao cho technician nào.");
            if (TaskAssignment != null)
                throw new DomainException("Task đã được assign.");
            if (string.IsNullOrWhiteSpace(targetId))
                throw new DomainException("TargetId không được để trống.");
            if (string.IsNullOrEmpty(technicianId))
                throw new DomainException("Người nhận task không được để trống.");

            if (isSeeding)
                Status = Common.Enum.TaskStatus.InProgress;
            else
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


            var checkList = CheckList ?? throw new DomainException("Checklist chưa được tạo.");
            if (checkList.HasAnyItemIncomplete())
                throw new DomainException("Checklist còn mục bắt buộc chưa hoàn thành.");

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
            CheckList?.ResetAllItemsForRework();
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

            bool IsTemplate = StageId is not null;
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

        // ==== Task Attributes =====
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


        // ===== Checklist =====
        private TaskCheckList EnsureChecklist()
        {
            CheckList ??= new TaskCheckList()
            {
                TaskId = this.ID
            };
            return CheckList;
        }

        public void StartChecklist(string technicianId, string checkListItemId)
        {
            if (TaskAssignment?.TechnicianId != technicianId)
                throw new DomainException("Không phải task của bạn.");
            if (Status != Common.Enum.TaskStatus.InProgress)
                throw new DomainException("Task chưa được thực hiện.");
            _ = CheckList ?? throw new DomainException("Checklist chưa được tạo.");
            var item = CheckList.GetItem(checkListItemId);
            item.Start();
        }

        /// <summary>
        /// Researcher use this to add one more item into checklist. If checklist is not created yet, it will be created first then add item into it.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <param name="expectedUnit"></param>
        /// <param name="expectedMinValue"></param>
        /// <param name="expectedMaxValue"></param>
        public void AddSingleCheckListItem(
            string name,
            string? description,
            int order,
            string? expectedUnit,
            decimal? expectedMinValue,
            decimal? expectedMaxValue)
        {
            var checklist = EnsureChecklist();
            checklist.AddItem(name, description, order, expectedUnit, expectedMinValue, expectedMaxValue);
        }

        /// <summary>
        /// researcher remove the checklist item only when the checklist item is not required or the checklist item is required but not completed yet. If the checklist item is required and already completed, researcher have to request rework to technician, then after technician rework and complete the task again, researcher can remove the checklist item.
        /// </summary>
        /// <param name="checklistItemId"></param>
        public void RemoveCheckListItem(string checklistItemId)
        {
            var checklist = CheckList ?? throw new DomainException("Checklist chưa được tạo.");
            checklist.RemoveItem(checklistItemId);
        }

        /// <summary>
        /// update checklist item only use when the item is not started to change the information
        /// </summary>
        /// <param name="checklistItemId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="expectedUnit"></param>
        /// <param name="expectedMinValue"></param>
        /// <param name="expectedMaxValue"></param>
        public void UpdateCheckListItem(string checklistItemId, string? name, string? description, string? expectedUnit, decimal? expectedMinValue, decimal? expectedMaxValue)
        {
            var checklist = CheckList ?? throw new DomainException("Checklist chưa được tạo.");
            checklist.UpdateItem(checklistItemId, name, description, expectedUnit, expectedMinValue, expectedMaxValue);
        }

        /// <summary>
        /// Submits the result for a specific checklist item as performed by a technician.
        /// </summary>
        /// <param name="checklistItemId">The unique identifier of the checklist item for which the result is being submitted.</param>
        /// <param name="technicianId">The unique identifier of the technician submitting the result. Must match the technician assigned to the
        /// task.</param>
        /// <param name="actualValue">The measured or observed value to record for the checklist item.</param>
        /// <param name="actualUnit">The unit of measurement associated with the actual value.</param>
        /// <exception cref="DomainException">Thrown if the specified technician is not assigned to the current task.</exception>
        public void SubmitCheckListItemResult(
            string technicianId,
            string itemId,
            string? measurementUnit,
            decimal? measuredValue)
        {
            if (TaskAssignment?.TechnicianId != technicianId)
                throw new DomainException("Không phải task của bạn.");

            if (Status != Common.Enum.TaskStatus.InProgress && Status != Common.Enum.TaskStatus.ReworkRequired)
                throw new DomainException("Task chưa được thực hiện.");
            _ = CheckList ?? throw new DomainException("Checklist chưa được tạo.");

            var item = CheckList.GetItem(itemId);

            item.SubmitByTechnician(measuredValue, measurementUnit);
        }

        public void EvaluateCheckListItem(
            string researcherId,
            string itemId,
            bool isPass)
        {
            if (ResearcherId != researcherId)
                throw new DomainException("Không có quyền đánh giá task này");
            if (Status != Common.Enum.TaskStatus.WaitingForApproval)
                throw new DomainException("Task chưa chờ duyệt.");

            _ = CheckList ?? throw new DomainException("Checklist chưa được tạo.");

            var item = CheckList.GetItem(itemId);
            item.EvaluateByResearcher(isPass);
        }
    }
}
