namespace orchid_backend_net.Application.Tasks.Dto.TaskCheckListItem
{
    public class CreateTaskCheckListItemDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public string? ExpectedUnit { get; set; }
        public decimal? ExpectedMinValue { get; set; }
        public decimal? ExpectedMaxValue { get; set; }
    }
}
