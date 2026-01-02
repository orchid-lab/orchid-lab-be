namespace orchid_backend_net.Application.Tasks.Helper
{
    public static class TaskAttributeHelper
    {
        public static void AddAttributesToTask(Domain.Entities.Tasks task, List<Dto.CreateTaskAttributeDto>? createTaskAttributes)
        {
            if (createTaskAttributes is null)
                return;
            if (createTaskAttributes is not null)
            {
                foreach (var attr in createTaskAttributes)
                    task.AddTaskAttribute(attr.ChemicalId, attr.MaterialId, attr.Unit, attr.Value);
            }
        }

        public static void UpdateAttributesOfTask(Domain.Entities.Tasks task, List<Dto.UpdateTaskAttributeDto>? updateTaskAttributes)
        {
            if (updateTaskAttributes is null)
                return;
            if (updateTaskAttributes is not null)
            {
                foreach (var attr in updateTaskAttributes)
                    task.UpdateTaskAttributes(attr.TaskAttributesId, attr.Unit, attr.Value, attr.ChemicalId, attr.MaterialId);
            }
        }
    }
}
