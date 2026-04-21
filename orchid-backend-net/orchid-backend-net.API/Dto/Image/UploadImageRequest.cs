namespace orchid_backend_net.API.Dto.Image
{
    /// <summary>
    /// Request model for uploading images with polymorphic association.
    /// </summary>
    public class UploadImageRequest
    {
        /// <summary>
        /// Image file to upload (max 5MB recommended)
        /// </summary>
        public required IFormFile Image { get; set; }

        /// <summary>
        /// Target entity type. Allowed values:
        /// <ul>
        /// <li>MonitoringLog (0)</li>
        /// <li>Task (1)</li>
        /// </ul>
        /// </summary>
        public required string TargetType { get; set; }

        /// <summary>
        /// Target entity ID (GUID format)
        /// </summary>
        public required string TargetId { get; set; }

        /// <summary>
        /// Description/caption for the image
        /// </summary>
        public string? Description { get; set; }
    }
}
