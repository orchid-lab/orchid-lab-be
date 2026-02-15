using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Tasks.Helper
{
    public static class TaskCheckListHelper
    {
        public static void AddCheckListItemsToTask(
            Domain.Entities.Tasks task,
            List<Dto.TaskCheckListItem.CreateTaskCheckListItemDto> createTaskCheckListItemDtos)
        {
            if (createTaskCheckListItemDtos is null || !createTaskCheckListItemDtos.Any())
                return;
            foreach (var item in createTaskCheckListItemDtos)
            {
                task.AddSingleCheckListItem(
                    item.Name,
                    item.Description ?? string.Empty,
                    item.Order,
                    item.ExpectedUnit,
                    item.ExpectedMinValue,
                    item.ExpectedMaxValue
                );
            }
        }
    }
}
