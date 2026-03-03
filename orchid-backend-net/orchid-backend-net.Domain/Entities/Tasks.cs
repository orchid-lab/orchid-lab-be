using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events.TaskEvents;

namespace orchid_backend_net.Domain.Entities
{
    /// <summary>
    /// Represents a task that a technician must complete as part of an experiment log.
    /// A task can be a template task (linked to a method stage) or a regular task assigned to an experiment log or sample.
    /// </summary>
    /// <remarks>
    /// <ul>
    /// <li>Tasks have a lifecycle: Assigned → InProgress → WaitingForApproval → Completed (InTime/OutTime)</li>
    /// <li>Tasks can be declined by technician or require rework by researcher</li>
    /// <li>Each task has a checklist with items that technician must complete and report</li>
    /// <li>Researcher must evaluate and approve the task before completion</li>
    /// </ul>
    /// </remarks>
    public class Tasks : AuditableEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        /// <summary>
        /// Stage ID to determine if this is a template task or not. Null = regular task, Not null = template task.
        /// </summary>
        public int? StageId { get; set; }
        public string? ResearcherId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public Domain.Common.Enum.TaskStatus Status { get; set; }
        public virtual TaskAssignment TaskAssignment { get; set; } = null!;
        public virtual List<TaskAttributes> TaskAttributes { get; set; } = new();
        public virtual TaskCheckList? CheckList { get; set; }

        #region Task Assignment
        // ===== Task Assignment =====

        /// <summary>
        /// Assigns this task to a specific technician with target information (experiment log or sample).
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Sets task status to Assigned (or InProgress if seeding)</li>
        /// <li>Creates a TaskAssignment entity with technician and target details</li>
        /// <li>Triggers TaskAssignedToTechnicianEvent domain event</li>
        /// </ul>
        /// </remarks>
        /// <param name="technicianId">The ID of the technician to assign the task to. Cannot be null or empty.</param>
        /// <param name="targetType">The type of target the task is assigned to (ExperimentLog or Sample).</param>
        /// <param name="targetId">The ID of the target entity (experiment log ID or sample ID). Cannot be null or empty.</param>
        /// <param name="expectedEndDate">The deadline for task completion.</param>
        /// <param name="startDate">The date when the task starts.</param>
        /// <param name="isSeeding">If true, task status will be set to InProgress immediately. Default is false (Assigned).</param>
        /// <exception cref="DomainException">Thrown when:
        /// - ResearcherId is null or empty (task creator not found)
        /// - Task is already assigned
        /// - TargetId is null or empty
        /// - TechnicianId is null or empty
        /// </exception>
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

        /// <summary>
        /// Technician accepts the assigned task and begins working on it.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Transitions task status from Assigned to InProgress</li>
        /// <li>Only assigned tasks can be accepted</li>
        /// <li>Triggers TaskAcceptedByTechnicianEvent domain event</li>
        /// </ul>
        /// </remarks>
        /// <param name="technicianId">The ID of the technician accepting the task. Must match the assigned technician.</param>
        /// <exception cref="DomainException">Thrown when:
        /// - Task is not yet assigned
        /// - TechnicianId does not match the assigned technician
        /// - ResearcherId is null or empty
        /// </exception>
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

        /// <summary>
        /// Technician declines the task without completing it.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Sets task status to DeclinedByTechnician</li>
        /// <li>Only InProgress tasks can be declined</li>
        /// <li>Triggers TaskDeclineByTechnicianEvent with reason for researcher notification</li>
        /// </ul>
        /// </remarks>
        /// <param name="technicianId">The ID of the technician declining the task. Must match the assigned technician.</param>
        /// <param name="reason">The reason why the technician is declining the task.</param>
        /// <exception cref="DomainException">Thrown when:
        /// - TechnicianId does not match the assigned technician
        /// - Task is not in InProgress status
        /// - ResearcherId is null or empty
        /// </exception>
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

        /// <summary>
        /// Technician submits the completed task for researcher approval.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Validates that all required checklist items are completed</li>
        /// <li>Transitions task status from InProgress to WaitingForApproval</li>
        /// <li>Triggers TaskReportedByTechnicianEvent for researcher notification</li>
        /// </ul>
        /// </remarks>
        /// <param name="technicianId">The ID of the technician submitting the report. Must match the assigned technician.</param>
        /// <exception cref="DomainException">Thrown when:
        /// - TechnicianId does not match the assigned technician
        /// - Task is not in InProgress status
        /// - Checklist has not been created yet
        /// - Checklist has incomplete required items
        /// - ResearcherId is null or empty
        /// </exception>
        public void ReportTask(string technicianId)
        {
            if (TaskAssignment?.TechnicianId != technicianId)
                throw new DomainException("Không phải task của bạn.");

            if (Status != Common.Enum.TaskStatus.InProgress && Status != Common.Enum.TaskStatus.ReworkRequired)
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

        /// <summary>
        /// Researcher approves and completes the task after reviewing technician's work.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Determines if task was completed within deadline (InTime) or late (OutTime)</li>
        /// <li>Transitions task status from WaitingForApproval to CompletedInTime or CompletedOutTime</li>
        /// <li>Triggers TaskCompletedEvent for notification and audit tracking</li>
        /// </ul>
        /// </remarks>
        /// <param name="researcherId">The ID of the researcher approving the task.</param>
        /// <param name="completedAt">The timestamp when the task is being approved (compared with expected end date).</param>
        /// <exception cref="DomainException">Thrown when task is not in WaitingForApproval status.</exception>
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

        /// <summary>
        /// Researcher requests technician to redo the task after review.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Transitions task status from WaitingForApproval to ReworkRequired</li>
        /// <li>Resets all checklist items to allow technician to resubmit</li>
        /// <li>Triggers TaskRedoRequestedEvent with reason for technician notification</li>
        /// </ul>
        /// </remarks>
        /// <param name="researcherId">The ID of the researcher requesting rework. Must match task creator.</param>
        /// <param name="reason">The reason why the researcher is requesting rework.</param>
        /// <exception cref="DomainException">Thrown when:
        /// - ResearcherId does not match task creator
        /// - Task is not in WaitingForApproval status
        /// </exception>
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

        /// <summary>
        /// Reassigns the task to a different target (experiment log or sample) or updates the expected end date.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Cannot convert between template and regular tasks</li>
        /// <li>Updates target type, target ID, expected end date, or end date</li>
        /// <li>Does not trigger domain events; use for administrative changes only</li>
        /// </ul>
        /// </remarks>
        /// <param name="newTargetType">The new target type. If null, keeps existing target type.</param>
        /// <param name="newTargetId">The new target ID. If null, keeps existing target ID.</param>
        /// <param name="expectedEndDate">The new expected end date. If null, keeps existing date.</param>
        /// <param name="endDate">The new end date. If null, keeps existing date.</param>
        /// <exception cref="DomainException">Thrown when:
        /// - Task has not been assigned yet
        /// - Attempting to convert between template and regular task
        /// </exception>
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

        #endregion Task Assignment

        #region Task Attributes
        // ==== Task Attributes =====

        /// <summary>
        /// Adds a new attribute (chemical or material usage) required for this task.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Each attribute represents a chemical or material needed for the task execution</li>
        /// <li>Cannot have duplicate attributes (same chemical or material)</li>
        /// <li>Must specify either chemical OR material, not both</li>
        /// </ul>
        /// </remarks>
        /// <param name="chemicalId">The ID of the chemical required. Null if using material instead.</param>
        /// <param name="materialId">The ID of the material required. Null if using chemical instead.</param>
        /// <param name="unit">The unit of measurement (e.g., "ml", "mg", "unit").</param>
        /// <param name="value">The quantity required in the specified unit.</param>
        /// <exception cref="DuplicateException">Thrown when the same chemical or material is already in the attributes list.</exception>
        /// <exception cref="DomainException">Thrown when both chemical and material IDs are provided.</exception>
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

        /// <summary>
        /// Updates an existing task attribute with new unit and value information.
        /// </summary>
        /// <param name="taskAttributesId">The ID of the attribute to update.</param>
        /// <param name="unit">The new unit of measurement.</param>
        /// <param name="value">The new quantity value.</param>
        /// <param name="chemicalId">The new chemical ID. Null to keep existing.</param>
        /// <param name="materialId">The new material ID. Null to keep existing.</param>
        /// <exception cref="NotFoundException">Thrown when the attribute is not found.</exception>
        /// <exception cref="DomainException">Thrown when both chemical and material IDs are provided.</exception>
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

        #endregion Task Attributes

        #region Task Checklist
        // ===== Checklist =====

        /// <summary>
        /// Ensures that a checklist exists, creating one if necessary.
        /// </summary>
        /// <returns>The existing or newly created TaskCheckList entity.</returns>
        private TaskCheckList EnsureChecklist()
        {
            CheckList ??= new TaskCheckList()
            {
                TaskId = this.ID
            };
            return CheckList;
        }

        /// <summary>
        /// Technician starts working on a specific checklist item.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Item status transitions from NotStarted to InProgress</li>
        /// <li>Only InProgress tasks allow starting checklist items</li>
        /// </ul>
        /// </remarks>
        /// <param name="technicianId">The ID of the technician starting the item. Must match the assigned technician.</param>
        /// <param name="checkListItemId">The ID of the checklist item to start.</param>
        /// <exception cref="DomainException">Thrown when:
        /// - TechnicianId does not match the assigned technician
        /// - Task is not in InProgress status
        /// - Checklist has not been created
        /// </exception>
        public void StartChecklist(string technicianId, string checkListItemId)
        {
            if (TaskAssignment?.TechnicianId != technicianId)
                throw new DomainException("Không phải task của bạn.");
            if (Status != Common.Enum.TaskStatus.InProgress && Status != Common.Enum.TaskStatus.ReworkRequired)
                throw new DomainException("Task chưa được thực hiện.");
            _ = CheckList ?? throw new DomainException("Checklist chưa được tạo.");
            var item = CheckList.GetItem(checkListItemId);
            item.Start();
        }

        /// <summary>
        /// Researcher adds a new item to the task checklist.
        /// If checklist doesn't exist yet, it will be created first.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>New items can be marked as required or optional</li>
        /// <li>Expected values (min/max) are used to validate technician's measurements</li>
        /// <li>Order determines the sequence checklist items should be completed</li>
        /// </ul>
        /// </remarks>
        /// <param name="name">The name of the checklist item (e.g., "pH measurement", "Temperature check").</param>
        /// <param name="description">Detailed description of what needs to be done. Optional.</param>
        /// <param name="order">The order/sequence in which this item should be completed.</param>
        /// <param name="expectedUnit">The expected unit of measurement (e.g., "pH", "°C"). Optional.</param>
        /// <param name="expectedMinValue">The minimum acceptable value for validation. Optional.</param>
        /// <param name="expectedMaxValue">The maximum acceptable value for validation. Optional.</param>
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
        /// Researcher removes a checklist item from the task.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Can remove items that are not required, or required but not yet completed</li>
        /// <li>Required and completed items require technician rework before removal</li>
        /// </ul>
        /// </remarks>
        /// <param name="checklistItemId">The ID of the checklist item to remove.</param>
        /// <exception cref="DomainException">Thrown when checklist has not been created.</exception>
        public void RemoveCheckListItem(string checklistItemId)
        {
            var checklist = CheckList ?? throw new DomainException("Checklist chưa được tạo.");
            checklist.RemoveItem(checklistItemId);
        }

        /// <summary>
        /// Researcher updates an existing checklist item information.
        /// Can only be used when the item has not been started yet by technician.
        /// </summary>
        /// <param name="checklistItemId">The ID of the checklist item to update.</param>
        /// <param name="name">The new item name. Null to keep existing.</param>
        /// <param name="description">The new item description. Null to keep existing.</param>
        /// <param name="expectedUnit">The new expected unit. Null to keep existing.</param>
        /// <param name="expectedMinValue">The new minimum acceptable value. Null to keep existing.</param>
        /// <param name="expectedMaxValue">The new maximum acceptable value. Null to keep existing.</param>
        /// <exception cref="DomainException">Thrown when checklist has not been created.</exception>
        public void UpdateCheckListItem(string checklistItemId, string? name, string? description, string? expectedUnit, decimal? expectedMinValue, decimal? expectedMaxValue)
        {
            var checklist = CheckList ?? throw new DomainException("Checklist chưa được tạo.");
            checklist.UpdateItem(checklistItemId, name, description, expectedUnit, expectedMinValue, expectedMaxValue);
        }

        /// <summary>
        /// Technician submits measurement results for a specific checklist item.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Can submit results only when task is InProgress or ReworkRequired</li>
        /// <li>Measurements are validated against expected min/max values</li>
        /// <li>Required items must have valid measurements before task can be reported</li>
        /// </ul>
        /// </remarks>
        /// <param name="technicianId">The ID of the technician submitting the result. Must match the assigned technician.</param>
        /// <param name="itemId">The ID of the checklist item being completed.</param>
        /// <param name="measurementUnit">The unit of the actual measurement performed.</param>
        /// <param name="measuredValue">The actual measured or observed value.</param>
        /// <exception cref="DomainException">Thrown when:
        /// - TechnicianId does not match the assigned technician
        /// - Task is not in InProgress or ReworkRequired status
        /// - Checklist has not been created
        /// </exception>
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

        /// <summary>
        /// Researcher evaluates and approves/rejects a technician's result for a specific checklist item.
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li>Can only evaluate when task is waiting for approval</li>
        /// <li>Pass (true) - accepts the measurement as valid</li>
        /// <li>Fail (false) - rejects the measurement; requires technician rework</li>
        /// </ul>
        /// </remarks>
        /// <param name="researcherId">The ID of the researcher evaluating. Must match the task creator.</param>
        /// <param name="itemId">The ID of the checklist item to evaluate.</param>
        /// <param name="isPass">True if the measurement is accepted, false if it needs to be redone.</param>
        /// <exception cref="DomainException">Thrown when:
        /// - ResearcherId does not match task creator
        /// - Task is not in WaitingForApproval status
        /// - Checklist has not been created
        /// </exception>
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

        #endregion Task Checklist
    }
}
