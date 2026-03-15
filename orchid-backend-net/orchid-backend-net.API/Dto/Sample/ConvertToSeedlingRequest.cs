namespace orchid_backend_net.API.Dto.Sample
{
    /// <summary>
    /// Request body for converting a completed sample into a new seedling.
    /// </summary>
    public class ConvertToSeedlingRequest
    {
        /// <summary>Tên địa phương / thương mại của cây giống mới</summary>
        public required string LocalName { get; set; }

        /// <summary>Tên khoa học của cây giống mới</summary>
        public required string ScientificName { get; set; }

        /// <summary>Mô tả thêm (tùy chọn)</summary>
        public string? Description { get; set; }
    }
}
