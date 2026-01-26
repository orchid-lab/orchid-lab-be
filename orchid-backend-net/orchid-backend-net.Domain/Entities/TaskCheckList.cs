using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskCheckList : BaseGuidEntity
    {
        public required string TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public virtual Tasks Task { get; set; } = null!;
        public virtual List<TaskCheckListItem> Items { get; set; } = new();
        public bool HasAnyRequiredItemIncomplete()
        {
            return Items.
                Where(i => i.IsRequired)
                .Any(i => i.Status == TaskCheckListItemStatus.Pending || i.Status == TaskCheckListItemStatus.InProgress);
        }
    }
}
