namespace orchid_backend_net.API.Dto.Task
{
    /// <summary>
    /// use to transfer data when update task checklist item information, all properties are optional, if a property is null, it will not be updated
    /// </summary>
    public class UpdateTaskCheckListItemInformationDto
    {
        /// <summary>
        /// name of checklist item
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// description of checklist item
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// expected measurement unit of the item
        /// </summary>
        public string? ExpectedMeasureUnit { get; set; }
        /// <summary>
        /// min value of the item, if the item is a checklist item that requires a value, this property will be used to validate the value, if the value is less than the expected min value, it will be considered as not completed
        /// </summary>
        public decimal? ExpectedMinValue { get; set; }
        /// <summary>
        /// max value of the item
        /// </summary>
        public decimal? ExpectedMaxValue { get; set; }
    }
}
