using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskCheckList : BaseGuidEntity
    {
        public string TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public virtual Tasks Task { get; set; } = null!;
        public virtual List<TaskCheckListItem> Items { get; set; } = new();
        
        // use the for validation
        public bool HasAnyItemIncomplete()
            => Items.Any(i => i.Status == TaskCheckListItemStatus.InProgress 
            || i.Status == TaskCheckListItemStatus.Pending);

        public bool HasAllItemsEvaluated()
            => Items.All(i => i.IsPass.HasValue);  

        public bool HasAnyItemFailed()
            => Items.Any(i => i.IsPass == false);

        //use for command like add, update, delete
        internal void AddItem(
            string name, 
            string? description,
            int order,
            string? expectedUnit,
            decimal? expectedMinvalue,
            decimal? expectedMaxValue)
        {
            var isDuplicated = Items.Any(Items => Items.Name.Equals(name));
            if (isDuplicated)
                throw new DuplicateException($"Mục {name} đã tồn tại");

            var item = new TaskCheckListItem
            {
                Name = name,
                Description = description,
                Order = order,
                ExpectedUnit = expectedUnit,
                ExpectedMinValue = expectedMinvalue,
                ExpectedMaxValue = expectedMaxValue,
                Status = TaskCheckListItemStatus.Pending
            };

            Items.Add(item);
        }

        internal void UpdateItem(
            string id, 
            string? name,
            string? description,
            string? expectedUnit, 
            decimal? expectedMinValue,
            decimal? expectedMaxValue)
        {
            var itemExist = Items.SingleOrDefault(i => i.ID.Equals(id) 
            && i.Status == TaskCheckListItemStatus.Pending)
                ?? throw new DomainException("Item này không tồn tại");

            itemExist.Name = name ?? itemExist.Name;
            itemExist.Description = description ?? itemExist.Description;
            itemExist.ExpectedUnit = expectedUnit ?? itemExist.ExpectedUnit;
            itemExist.ExpectedMinValue = expectedMinValue ?? itemExist.ExpectedMinValue;
            itemExist.ExpectedMaxValue = expectedMaxValue ?? itemExist.ExpectedMaxValue;
        }

        internal void RemoveItem(string itemId) 
        {
            var item = Items.SingleOrDefault(i => i.ID.Equals(itemId))
                ?? throw new DomainException("Item này không tồn tại");
            if (item.Status != TaskCheckListItemStatus.Pending)
                throw new DomainException("Không thể xóa mục đã bắt đầu công việc");
            Items.Remove(item);
        }

        internal TaskCheckListItem GetItem(string itemId)
            => Items.SingleOrDefault(i => i.ID.Equals(itemId))
                ?? throw new DomainException("Item này không tồn tại");

        internal void ResetAllItemsForRework()
        {
            foreach (var item in Items.Where(i => i.IsPass == false))
            {
                item.ResetForRework();
            }
        }
    }
}
